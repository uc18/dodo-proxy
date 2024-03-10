using System.Collections.Generic;
using System.Threading.Tasks;
using LousBot.Models.Pyrus;

namespace LousBot.Service.Interfaces;

public interface IPyrusApi
{
    Task<List<ServiceResponse>> GetForms();
    Task<int> CreateRequestToAccess(CreateTicketRequest request);
    Task<int> CreateRequestToBuySoftware(CreateTicketRequest request);
}