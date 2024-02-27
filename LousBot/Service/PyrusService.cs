using System;
using System.Threading.Tasks;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class PyrusService : IPyrusService
{
    private readonly IOptions<ServiceDeskOptions> _options;

    public PyrusService(IOptions<ServiceDeskOptions> options)
    {
        _options = options;
    }
    public Task CreateTicketRequest()
    {
        throw new NotImplementedException();
    }

    public Task GetAllServiceName()
    {
        throw new NotImplementedException();
    }
}