using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LousBot.Extension;
using LousBot.Models.Pyrus;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;
using Pyrus.ApiClient.Requests.Builders;
using PyrusApiClient;

namespace LousBot.Service;

public class PyrusApi : IPyrusApi
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly PyrusClient _client;
    private const int FormId = 1383275;
    private const int ChoiceFieldId = 2;

    public PyrusApi(IOptions<ServiceDeskOptions> options)
    {
        _options = options;
        _client = new PyrusClient();
    }

    public async Task<List<ServiceResponse>> GetForms()
    {
        await _client.Auth(_options.Value.PyrusBotOptions.Login,
            _options.Value.PyrusBotOptions.Token);
        var form = await RequestBuilder.GetForm(FormId).Process(_client);
        var accessFields = form.Fields.FirstOrDefault(t => t.Id == ChoiceFieldId);
        var i = accessFields.Info.Options.Select(t => new ServiceResponse
        {
            ChoiceId = t.ChoiceId,
            ServiceName = t.ChoiceValue
        }).ToList();

        return i;
    }

    public async Task<int> CreateRequestToAccess(CreateTicketRequest request)
    {
        const int getAccessToService = 1;
        await _client.Auth(_options.Value.PyrusBotOptions.Login,
            _options.Value.PyrusBotOptions.Token);

        if (request.AnotherServiceName != null && request.AnotherServiceName.ToLower().Equals("другое"))
        {
            return await SendRequestWithAnother(request, getAccessToService);
        }

        return await SendRequest(request, getAccessToService);
    }

    public async Task<int> CreateRequestToBuySoftware(CreateTicketRequest request)
    {
        const int buySoftware = 2;
        await _client.Auth(_options.Value.PyrusBotOptions.Login,
            _options.Value.PyrusBotOptions.Token);

        if (request.AnotherServiceName != null && request.AnotherServiceName.ToLower().Equals("другое"))
        {
            return await SendRequestWithAnother(request, buySoftware);
        }

        return await SendRequest(request, buySoftware);
    }

    private async Task<int> SendRequest(CreateTicketRequest request, int whatDo)
    {
        var existRequestOnPyrus = await RequestBuilder
            .CreateFormTask(FormId)
            .Fields
            .Add(FormField.Create<FormFieldText>(DodoConstants.Email).WithValue($"{request.Email}"))
            .Add(FormField.Create<FormFieldMultipleChoice>(DodoConstants.WhatDo).WithChoice(whatDo))
            .Add(FormField.Create<FormFieldMultipleChoice>(DodoConstants.Choise).WithChoice(request.ServiceName))
            .Add(FormField.Create<FormFieldText>(DodoConstants.ForWhat).WithValue(request.Justification))
            .Add(FormField.Create<FormFieldText>(DodoConstants.ThreadID).WithValue(request.DirectId))
            .Process(_client);

        return existRequestOnPyrus.Task.Id;
    }

    private async Task<int> SendRequestWithAnother(CreateTicketRequest request, int whatDo)
    {
        var existRequestOnPyrus = await RequestBuilder
            .CreateFormTask(FormId)
            .Fields
            .Add(FormField.Create<FormFieldText>(DodoConstants.Email).WithValue($"{request.Email}"))
            .Add(FormField.Create<FormFieldMultipleChoice>(DodoConstants.WhatDo).WithChoice(whatDo))
            .Add(FormField.Create<FormFieldMultipleChoice>(DodoConstants.Choise).WithChoice(request.ServiceName))
            .Add(FormField.Create<FormFieldText>(DodoConstants.ForWhat).WithValue(request.Justification))
            .Add(FormField.Create<FormFieldText>(DodoConstants.ServiceName).WithValue(request.AnotherServiceName))
            .Add(FormField.Create<FormFieldText>(DodoConstants.ThreadID).WithValue(request.DirectId))
            .Process(_client);
        return existRequestOnPyrus.Task.Id;
    }
}