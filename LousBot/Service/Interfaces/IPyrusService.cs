using System.Threading.Tasks;
using LousBot.Models.Loop;

namespace LousBot.Service.Interfaces;

public interface IPyrusService
{
    Task CreateAccessForm(IncomeAccessServiceRequest request);
    Task CreateBuySoftwareForm(IncomeSoftwareBuyRequest request);
}