using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class LoopBackgroundService : BackgroundService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly IServiceProvider _services;

    public LoopBackgroundService(IServiceProvider services, IOptions<ServiceDeskOptions> options)
    {
        _services = services;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var uri = new Uri(_options.Value.LoopBotOptions.WebsocketUrl);
        using var webSocketClient = new ClientWebSocket();
        webSocketClient.Options.SetRequestHeader("Authorization",
            $"Bearer {_options.Value.LoopBotOptions.BearerToken}");
        await webSocketClient.ConnectAsync(uri, CancellationToken.None);
        try
        {
            await ReceiveUpdateFromLoopWebSocket(webSocketClient);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private async Task ReceiveUpdateFromLoopWebSocket(ClientWebSocket webSocketClient)
    {
        while (webSocketClient.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            var buffer = new ArraySegment<byte>(new byte[1024]);
            using var ms = new MemoryStream();
            do
            {
                result = await webSocketClient.ReceiveAsync(buffer, CancellationToken.None);
                await ms.WriteAsync(buffer.Array.AsMemory(buffer.Offset, result.Count));
            } while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);

            using var rt = new StreamReader(ms, Encoding.UTF8);

            using var scope = _services.CreateScope();

            var webSocketUpdate = await rt.ReadToEndAsync();
            var scopedService = scope.ServiceProvider.GetRequiredService<IPyrusService>();
            await scopedService.SendComment(webSocketUpdate);
        }
    }
}