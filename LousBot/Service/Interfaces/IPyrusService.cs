using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus.Request;

namespace LousBot.Service.Interfaces;

public interface IPyrusService
{
    Task CreateAccessForm(IncomeAccessServiceRequest request);
    Task CreateBuySoftwareForm(IncomeSoftwareBuyRequest request);
    UpdateLoopThread GetMessageFromPyrusRequest(PyrusTask pyrusTask);
}