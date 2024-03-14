using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LousBot.Extension;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus;
using LousBot.Models.Pyrus.Request;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class LoopService : ILoopService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly IMattermostService _mattermostService;
    private readonly IPyrusApi _pyrusApi;

    public LoopService(IOptions<ServiceDeskOptions> options,
        IMattermostService mattermostService, IPyrusApi pyrusApi)
    {
        _options = options;
        _mattermostService = mattermostService;
        _pyrusApi = pyrusApi;
    }

    public bool IsValidRequest(string text)
    {
        if (int.TryParse(text, out var accessNumber))
        {
            return accessNumber >= 1 && accessNumber <= 2;
        }

        return false;
    }

    public async Task<bool> SendForm(IncomeAccessRequest request)
    {
        var result = await _pyrusApi.GetForms();
        var form = request.text switch
        {
            "1" => PrepareModalFormAccessToService(request.trigger_id, result),
            "2" => PrepareModalFormBuySoftware(request.trigger_id, result),
            _ => null
        };

        if (form == null)
        {
            return false;
        }

        await _mattermostService.SendForm(form);
        return true;
    }

    public async Task SendHelpMessage(IncomeAccessRequest request)
    {
        var directId = await _mattermostService.GetDirectChannel(request.user_id);

        if (directId != null)
        {
            var message = $"Отправь {LetterExtension.SlashCommand} 1 для формы \"Получить доступ\". " +
                          $"Отправь {LetterExtension.SlashCommand} 2 для формы \"Купить лицензию/сервис\" ";
            await _mattermostService.SendPrivateMessage(directId.Id, message);
        }
    }

    public async Task SendUpdateMessage(UpdateLoopThread updateRequest)
    {
        var response = _mattermostService.GetChannelId(updateRequest.ThreadId);

        if (response != null)
        {
            await _mattermostService.SendMessageOnThread(response.ChannelId, updateRequest.Comment,
                updateRequest.ThreadId);
        }
    }

    private ModalForm PrepareModalFormAccessToService(string triggerId, IEnumerable<ServiceResponse> servicesName)
    {
        var elements = servicesName.Select(t => new LoopModalOption
        {
            Text = t.ServiceName,
            Value = t.ChoiceId.ToString()
        }).ToArray();

        return new ModalForm
        {
            TriggerId = triggerId,
            ApiUrl = _options.Value.LoopBotOptions.ReturnUrl + $"{RoutingExtension.AccessService}",
            Dialog = new Dialog
            {
                CallbackId = "",
                Title = "Получить доступ",
                Elements = new[]
                {
                    new Element
                    {
                        DisplayName = "Выбор",
                        Name = "Service",
                        Type = "select",
                        HelpText = "Выбери сервис",
                        Options = elements
                    },
                    new Element
                    {
                        DisplayName = "Для чего нужен доступ?",
                        Name = "Justification",
                        Type = "text",
                        HelpText = "Обоснование",
                        Optional = true
                    }
                }
            }
        };
    }

    private ModalForm PrepareModalFormBuySoftware(string triggerId, IEnumerable<ServiceResponse> servicesName)
    {
        var elements = servicesName.Select(t => new LoopModalOption
        {
            Text = t.ServiceName,
            Value = t.ChoiceId.ToString()
        }).ToArray();

        return new ModalForm
        {
            TriggerId = triggerId,
            ApiUrl = _options.Value.LoopBotOptions.ReturnUrl + $"{RoutingExtension.BuySoftware}",
            Dialog = new Dialog
            {
                CallbackId = "",
                Title = "Купить ПО",
                Elements = new[]
                {
                    new Element
                    {
                        DisplayName = "Выбор",
                        Name = "BuyServiceName",
                        Type = "select",
                        HelpText = "Выбери сервис",
                        Options = elements
                    },
                    new Element
                    {
                        DisplayName = "Для чего?",
                        Name = "Justification",
                        Type = "text",
                        HelpText = "Обоснование"
                    },
                    new Element
                    {
                        DisplayName = "Подразделение",
                        Name = "Service",
                        Type = "select",
                        HelpText = "Какое подразделение?",
                        Options = new[]
                        {
                            new LoopModalOption
                            {
                                Text = "Corporate",
                                Value = "Corporate"
                            },
                            new LoopModalOption
                            {
                                Text = "Franchise",
                                Value = "Franchise"
                            }
                        }
                    }
                }
            }
        };
    }
}