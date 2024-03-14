using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LousBot.Service;

public class LoopBackgroundService : BackgroundService
{
    private readonly HttpClient _client;

    public LoopBackgroundService(HttpClient client)
    {
        _client = client;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new System.NotImplementedException();
    }
}