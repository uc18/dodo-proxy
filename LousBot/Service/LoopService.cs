using System;
using System.Threading.Tasks;
using LousBot.Extension;
using LousBot.Models.Loop;
using LousBot.Options;
using LousBot.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace LousBot.Service;

public class LoopService : ILoopService
{
    private readonly IOptions<ServiceDeskOptions> _options;
    private readonly IMattermostService _mattermostService;

    public LoopService(IOptions<ServiceDeskOptions> options,
        IMattermostService mattermostService)
    {
        _options = options;
        _mattermostService = mattermostService;
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
        var form = request.text switch
        {
            "1" => PrepareModalFormAccessToService(request.trigger_id),
            "2" => PrepareModalFormBuySoftware(request.trigger_id),
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
            Console.WriteLine(directId.Id);
            await _mattermostService.SendHelpMessage(directId.Id);
        }
    }

    private ModalForm PrepareModalFormAccessToService(string triggerId)
    {
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
                }
            }
        };
    }

    private ModalForm PrepareModalFormBuySoftware(string triggerId)
    {
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
                        DisplayName = "Какой сервис?",
                        Name = "BuyServiceName",
                        Type = "text",
                        HelpText = "Какой сервис?",
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
                    },
                }
            }
        };
    }
}