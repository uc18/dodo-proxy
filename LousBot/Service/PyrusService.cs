using System.Linq;
using System.Text;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus;
using LousBot.Models.Pyrus.Request;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pyrus.ApiClient.JsonConverters;
using PyrusApiClient;
using Task = System.Threading.Tasks.Task;

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
                var emptyMessage = "Был создан тикет в Пайрусе";
                var response = await _mattermostService.SendPrivateMessage(directId.Id, emptyMessage);
                var ticketRequest = new CreateTicketRequest(userEmail, serviceId, request.Submission.Justification,
                    request.Submission.AnotherNameService, response.Id);
                var numberTicketRequest = await _apiService.CreateRequestToAccess(ticketRequest);
                var message =
                    $"Был создан тикет в Пайрусе {_options.Value.PyrusBotOptions.PyrusUrl}id{numberTicketRequest}";
                await _mattermostService.UpdateMessage(message, response.Id);
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
                var emptyMessage = "Был создан тикет в Пайрусе";
                var response = await _mattermostService.SendPrivateMessage(directId.Id, emptyMessage);
                var t = new CreateTicketRequest(userEmail, serviceId, request.Submission.Justification,
                    request.Submission.AnotherNameService, response.Id);
                var s = await _apiService.CreateRequestToBuySoftware(t);
                var message = $"Был создан тикет в Пайрусе {_options.Value.PyrusBotOptions.PyrusUrl}id{s}";
                await _mattermostService.SendPrivateMessage(directId.Id, message);
            }
        }
    }

    public UpdateLoopThread GetMessageFromPyrusRequest(PyrusTask pyrusTask)
    {
        var requestBody =
            JsonConvert.DeserializeObject<TaskWithComments>(
                pyrusTask.Task.ToString(), new FormFieldJsonConverter());

        if (requestBody == null) return null;

        var threadIdField = requestBody.FlatFields.FirstOrDefault(ff => ff.Name.ToLower() == "threadid");
        if (requestBody.Comments.Count > 0 && requestBody.Comments[0].Text != null && threadIdField != null)
        {
            var sb = new StringBuilder();

            var commentWords = requestBody.Comments[0].Text.Split(" ");

            foreach (var word in commentWords)
            {
                if (word.ToLower() == "it-office_bot")
                {
                    continue;
                }

                sb.Append(word + " ");
            }

            var threadId = (FormFieldText)threadIdField;
            return new UpdateLoopThread
            {
                Comment = sb.ToString().TrimEnd(),
                ThreadId = threadId.Value
            };
        }

        return null;
    }
}