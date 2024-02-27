using System.Threading.Tasks;

namespace LousBot.Service.Interfaces;

public interface IPyrusService
{
    Task CreateTicketRequest();

    Task GetAllServiceName();
}