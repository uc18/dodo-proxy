using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LousBot.Models.Pyrus;
using LousBot.Options;
using Microsoft.Extensions.Options;
using Pyrus.ApiClient.Requests.Builders;
using PyrusApiClient;

namespace LousBot.Service;

public class PyrusBotService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly PyrusClient _client;

    public PyrusBotService(IOptions<ServiceDeskOptions> options)
    {
        _options = options;
        _client = new PyrusClient();
    }

    public async Task<List<ServiceResponse>> GetForms()
    {
        await _client.Auth(_options.Value.PyrusBotOptions.Login,
            _options.Value.PyrusBotOptions.Token);
        var form = await RequestBuilder.GetForm(1383275).Process(_client);
        var accessFields = form.Fields.FirstOrDefault(t => t.Id == 2);
        var i = accessFields.Info.Options.Select(t => new ServiceResponse
        {
            ChoiceId = t.ChoiceId,
            ServiceName = t.ChoiceValue
        }).ToList();

        return i;
    }

    public async System.Threading.Tasks.Task CreateFormRequest()
    {
        await _client.Auth(_options.Value.PyrusBotOptions.Login,
            _options.Value.PyrusBotOptions.Token);

        await RequestBuilder
            .CreateFormTask(1383275)
            .Fields
            .Add(FormField.Create<FormFieldText>("Email").WithValue("ababab@yandex.ru"))
            .Add(FormField.Create<FormFieldMultipleChoice>("Что нужно сделать?").WithChoice(1))
            .Add(FormField.Create<FormFieldMultipleChoice>("Выбор").WithChoice(2))
            .Process(_client);
    }
}