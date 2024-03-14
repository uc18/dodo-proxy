using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus.Request;

namespace LousBot.Service.Interfaces;

public interface ILoopService
{
    bool IsValidRequest(string text);

    Task<bool> SendForm(IncomeAccessRequest request);

    Task SendHelpMessage(IncomeAccessRequest request);

    Task SendUpdateMessage(UpdateLoopThread updateRequest);
}