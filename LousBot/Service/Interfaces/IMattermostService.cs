using System.Threading.Tasks;
using LousBot.Models.Loop;

namespace LousBot.Service.Interfaces;

public interface IMattermostService
{
    Task SendPrivateMessage(string channelId, string message);
    Task SendForm(ModalForm form);
    Task<DirectChannelResponse?> GetDirectChannel(string userId);
    Task<string> GetUserEmail(string userId);
}