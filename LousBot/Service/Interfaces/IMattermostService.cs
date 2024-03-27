using System.Threading.Tasks;
using LousBot.Models.Loop;

namespace LousBot.Service.Interfaces;

public interface IMattermostService
{
    Task<MessageResponse?> SendPrivateMessage(string channelId, string message);
    Task<MessageResponse?> SendMessageOnThread(string channelId, string message, string rootId);
    Task<MessageResponse?> UpdateMessage(string textMessage, string messageId);
    Task SendForm(ModalForm form);
    Task<DirectChannelResponse?> GetDirectChannel(string userId);
    Task<string> GetUserEmail(string userId);

    MessageResponse GetInfoAboutMessage(string messageId);
}