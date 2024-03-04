using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Service;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PyrusController : ControllerBase
{
    private readonly ILoopService _loopService;
    private readonly PyrusBotService _botService;

    public PyrusController(ILoopService loopService, PyrusBotService botService)
    {
        _loopService = loopService;
        _botService = botService;
    }

    [HttpPost("access/service")]
    public async Task<IActionResult> PostPyrus(IncomeAccessServiceRequest request)
    {
        await _botService.CreateFormRequest();
        return Ok();
    }

    [HttpPost("software/buy")]
    public async Task<IActionResult> PostPyrus2(IncomeSoftwareBuyRequest request)
    {
        return Ok();
    }
}