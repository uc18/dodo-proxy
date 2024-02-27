using System.Threading.Tasks;
using LousBot.Models.Loop;

namespace LousBot.Service.Interfaces;

public interface IMattermostService
{
    Task SendHelpMessage(string channelId);
    Task SendForm(ModalForm form);
    Task<DirectChannelResponse?> GetDirectChannel(string userId);
}