using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class PyrusService : IPyrusService
{
    private readonly IMattermostService _mattermostService;
    private readonly IPyrusApi _apiService;
    private IOptions<ServiceDeskOptions> _options;

    public PyrusService(IMattermostService mattermostService, IPyrusApi apiService,
        IOptions<ServiceDeskOptions> options)
    {
        _mattermostService = mattermostService;
        _apiService = apiService;
        _options = options;
    }

    public async Task CreateAccessForm(IncomeAccessServiceRequest request)
    {
        if (int.TryParse(request.Submission.Service, out var serviceId))
        {
            var userEmail = await _mattermostService.GetUserEmail(request.UserId);
            var directId = await _mattermostService.GetDirectChannel(request.UserId);
            if (directId != null)
            {
                var ticketRequest = new CreateTicketRequest(userEmail, serviceId, request.Submission.Justification,
                    request.Submission.AnotherNameService, directId.Id);
                var numberTicketRequest = await _apiService.CreateRequestToAccess(ticketRequest);
                var message =
                    $"Был создан тикет в Пайрусе {_options.Value.PyrusBotOptions.PyrusUrl}id{numberTicketRequest}";
                await _mattermostService.SendPrivateMessage(directId.Id, message);
            }
        }
    }

    public async Task CreateBuySoftwareForm(IncomeSoftwareBuyRequest request)
    {
        if (int.TryParse(request.Submission.BuyServiceName, out var serviceId))
        {
            var userEmail = await _mattermostService.GetUserEmail(request.UserId);
            var directId = await _mattermostService.GetDirectChannel(request.UserId);
            if (directId != null)
            {
                var t = new CreateTicketRequest(userEmail, serviceId, request.Submission.Justification,
                    request.Submission.AnotherNameService, directId.Id);
                var s = await _apiService.CreateRequestToBuySoftware(t);
                var message = $"Был создан тикет в Пайрусе {_options.Value.PyrusBotOptions.PyrusUrl}id{s}";
                await _mattermostService.SendPrivateMessage(directId.Id, message);
            }
        }
    }
}