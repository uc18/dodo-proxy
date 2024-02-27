using System.Threading.Tasks;
using LousBot.Models.Loop;

namespace LousBot.Service.Interfaces;

public interface ILoopService
{
    bool IsValidRequest(string text);

    Task<bool> SendForm(IncomeAccessRequest request);

    Task SendHelpMessage(IncomeAccessRequest request);
}