using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Service;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PyrusController : ControllerBase
{
    private readonly LoopService _loopService;

    public PyrusController(LoopService loopService)
    {
        _loopService = loopService;
    }

    [HttpGet]
    public string Get()
    {
        return "2";
    }

    [HttpPost("access/service")]
    public async Task<IActionResult> PostPyrus(IncomeFormRequest request)
    {
        return Ok();
    }

    [HttpPost("software/buy")]
    public async Task<IActionResult> PostPyrus2(IncomeFormRequest request)
    {
        return Ok();
    }
}