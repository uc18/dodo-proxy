using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class LoopController : ControllerBase
{
    private readonly ILoopService _loopService;

    public LoopController(ILoopService loopService)
    {
        _loopService = loopService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] IncomeAccessRequest request)
    {
        if (_loopService.IsValidRequest(request.text))
        {

            var result = await _loopService.SendForm(request);
            return result ? Ok() : BadRequest();
        }
        await _loopService.SendHelpMessage(request);

        return Ok();
    }
}