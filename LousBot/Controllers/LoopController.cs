using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LousBot.Models.Loop;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class LoopController : ControllerBase
{
    private readonly IPyrusService _pyrusService;
    private readonly IOptions<ServiceDeskOptions> _options;

    public LoopController(IPyrusService pyrusService,
        IOptions<ServiceDeskOptions> options)
    {
        _pyrusService = pyrusService;
        _options = options;
    }

    [HttpGet]
    public string Get()
    {
        return $"{_options.Value.LoopBotOptions.ApiUrl}";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] IncomeAccessRequest request)
    {
        Console.WriteLine($"{request.trigger_id}");
        Console.WriteLine($"Пришел запрос {request.command}");
        using var client = new HttpClient();

        client.BaseAddress = new Uri(_options.Value.LoopBotOptions.ApiUrl);

        var form = new ModalForm
        {
            TriggerId = request.trigger_id,
            ApiUrl = _options.Value.LoopBotOptions.ReturnUrl,
            Dialog = new Dialog
            {
                CallbackId = "request",
                Title = "Получить доступ",
                Elements = new[]
                {
                    new Element
                    {
                        DisplayName = "Что нужно сделать?",
                        Name = "Options",
                        Type = "select",
                        HelpText = "Выбери опцию",
                        Options = new[]
                        {
                            new LoopModalOption
                            {
                                Text = "Получить доступ",
                                Value = "Получить доступ"
                            },
                            new LoopModalOption
                            {
                                Text = "Купить ПО",
                                Value = "Купить ПО"
                            }
                        }
                    },
                    new Element
                    {
                        DisplayName = "Сервис",
                        Name = "Service",
                        Type = "select",
                        HelpText = "Какой сервис?",
                        Options = new[]
                        {
                            new LoopModalOption
                            {
                                Text = "Сервис 1",
                                Value = "Сервис 1"
                            },
                            new LoopModalOption
                            {
                                Text = "Сервис 2",
                                Value = "Сервис 2"
                            }
                        }
                    },
                    new Element
                    {
                        DisplayName = "Для чего?",
                        Name = "Justification",
                        Type = "text",
                        HelpText = "Обоснование"
                    }
                },
                SumbitLabel = "Request"
            }
        };

        var formRequest = JsonSerializer.Serialize(form);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", _options.Value.LoopBotOptions.BearerToken);

        Console.WriteLine(formRequest);

        var content = new StringContent(formRequest, Encoding.UTF8,
            "application/json");

        using var response = await client.PostAsync("api/v4/actions/dialogs/open",
            content);

        var rt = await response.Content.ReadAsStringAsync();
        Console.WriteLine(rt + " " + response.ReasonPhrase);
        return Ok();
    }

    [HttpPost("pyrus")]
    public void PostPyrus(IncomeFormRequest request)
    {

        Console.WriteLine($"Income new request, {request.UserId}");
    }
}