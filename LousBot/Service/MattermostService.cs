using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class MattermostService : IMattermostService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly BotInfoResponse _botInfo;
    private readonly HttpClient _client;

    public MattermostService(IOptions<ServiceDeskOptions> options, IHttpService httpService)
    {
        _options = options;
        _client = httpService.ReturnHttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", _options.Value.LoopBotOptions.BearerToken);
        _botInfo = GetBotId();
    }

    private BotInfoResponse GetBotId()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_options.Value.LoopBotOptions.ApiUrl}/users/username/{_options.Value.LoopBotOptions.BotName}");
        var httpResponse = _client.Send(request);
        var botId = JsonSerializer.Deserialize<BotInfoResponse>(httpResponse.Content.ReadAsStream());

        return botId != null ? botId : new BotInfoResponse();
    }

    public async Task<MessageResponse?> SendPrivateMessage(string channelId, string message)
    {
        var formRequest = JsonSerializer.Serialize(new OutgoingWebhookRequest
        {
            ChannelId = channelId,
            Message = message
        });
        var content = new StringContent(formRequest, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/posts";
        using var response = await _client.PostAsync(stringUri, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MessageResponse>(responseContent);
    }

    public async Task<MessageResponse?> SendMessageOnThread(string channelId, string message, string rootId)
    {
        var formRequest = JsonSerializer.Serialize(new OutgoingWebhookRequest
        {
            ChannelId = channelId,
            Message = message,
            RootId = rootId
        });
        var content = new StringContent(formRequest, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/posts";
        using var response = await _client.PostAsync(stringUri, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MessageResponse>(responseContent);
    }

    public async Task<MessageResponse?> UpdateMessage(string textMessage, string messageId)
    {
        var formRequest = JsonSerializer.Serialize(new OutgoingWebhookRequest
        {
            Message = textMessage
        });
        var content = new StringContent(formRequest, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/posts/{messageId}/patch";
        using var response = await _client.PutAsync(stringUri, content);

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MessageResponse>(responseContent);
    }

    public async Task SendForm(ModalForm form)
    {
        var formRequest = JsonSerializer.Serialize(form);

        var content = new StringContent(formRequest, Encoding.UTF8,
            "application/json");
        await _client.PostAsync(
            $"{_options.Value.LoopBotOptions.ApiUrl}/actions/dialogs/open",
            content);
    }

    public async Task<DirectChannelResponse?> GetDirectChannel(string userId)
    {
        var request = JsonSerializer.Serialize(new[]
        {
            userId, _botInfo.Id
        });

        var content = new StringContent(request, Encoding.UTF8, "application/json");

        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/channels/direct";

        using var response = await _client.PostAsync(stringUri, content);

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DirectChannelResponse>(responseContent);
    }

    public async Task<string> GetUserEmail(string userId)
    {
        var stringUri = $"{_options.Value.LoopBotOptions.ApiUrl}/users/{userId}";
        using var httpResponse = await _client.GetAsync(stringUri);

        if (httpResponse.IsSuccessStatusCode)
        {
            var httpContentResponse = await httpResponse.Content.ReadAsStringAsync();

            var botInfo = JsonSerializer.Deserialize<BotInfoResponse>(httpContentResponse);

            if (botInfo != null)
            {
                return botInfo.Email;
            }
        }

        return string.Empty;
    }

    public MessageResponse GetInfoAboutMessage(string messageId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_options.Value.LoopBotOptions.ApiUrl}/posts/{messageId}");
        var httpResponse = _client.Send(request);
        var messageResponse = JsonSerializer.Deserialize<MessageResponse>(httpResponse.Content.ReadAsStream());

        if (messageResponse != null)
        {
            return messageResponse;
        }

        return null;
    }
}