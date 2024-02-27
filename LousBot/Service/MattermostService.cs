using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LousBot.Extension;
using LousBot.Models.Loop;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class MattermostService : IMattermostService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly BotInfoResponse _response;
    private readonly HttpClient _client;

    public MattermostService(IOptions<ServiceDeskOptions> options, IHttpService httpService)
    {
        _options = options;
        _client = httpService.ReturnHttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", _options.Value.LoopBotOptions.BearerToken);
        _response = GetBotId();
    }

    private BotInfoResponse GetBotId()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_options.Value.LoopBotOptions.ApiUrl}/users/username/{_options.Value.LoopBotOptions.BotName}");
        var r = _client.Send(request);
        var info = JsonSerializer.Deserialize<BotInfoResponse>(r.Content.ReadAsStream());
        return info;
    }

    public async Task SendHelpMessage(string channelId)
    {
        var formRequest = JsonSerializer.Serialize(new OutgoingWebhookRequest
        {
            ChannelId = channelId,
            Message = $"Отправь {LetterExtension.SlashCommand} 1 для формы \"Получить доступ\". " +
                      $"Отправь {LetterExtension.SlashCommand} 2 для формы \"Купить ПО\" "
        });
        var content = new StringContent(formRequest, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/posts";
        using var response = await _client.PostAsync(stringUri, content);

        var rt = await response.Content.ReadAsStringAsync();

        Console.WriteLine(rt + " " + response.ReasonPhrase);
    }

    public async Task SendForm(ModalForm form)
    {
        var formRequest = JsonSerializer.Serialize(form);

        var content = new StringContent(formRequest, Encoding.UTF8,
            "application/json");

        using var response = await _client.PostAsync(
            $"{_options.Value.LoopBotOptions.ApiUrl}/actions/dialogs/open",
            content);

        var rt = await response.Content.ReadAsStringAsync();
    }

    public async Task<DirectChannelResponse?> GetDirectChannel(string userId)
    {
        var request = JsonSerializer.Serialize(new[]
        {
            userId, _response.Id
        });

        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/channels/direct";

        using var response = await _client.PostAsync(stringUri, content);

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DirectChannelResponse>(responseContent);
    }
}