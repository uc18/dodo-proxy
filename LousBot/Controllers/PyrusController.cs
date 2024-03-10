using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PyrusController : ControllerBase
{
    private IPyrusService _pyrusService;

    public PyrusController(IPyrusService pyrusService)
    {
        _pyrusService = pyrusService;
    }

    [HttpPost("access/service")]
    public async Task<IActionResult> PostAccessService(IncomeAccessServiceRequest request)
    {
        await _pyrusService.CreateAccessForm(request);
        return Ok();
    }

    [HttpPost("software/buy")]
    public async Task<IActionResult> PostBuySoftware(IncomeSoftwareBuyRequest request)
    {
        await _pyrusService.CreateBuySoftwareForm(request);
        return Ok();
    }
}