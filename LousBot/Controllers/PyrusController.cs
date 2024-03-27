using System.Threading;
using System.Threading.Tasks;
using LousBot.Models.Loop;
using LousBot.Models.Pyrus;
using LousBot.Models.Pyrus.Request;
using LousBot.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PyrusController : ControllerBase
{
    private IPyrusService _pyrusService;
    private readonly ILoopService _loopService;

    public PyrusController(IPyrusService pyrusService, ILoopService loopService)
    {
        _pyrusService = pyrusService;
        _loopService = loopService;
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

    [HttpPost]
    public async Task<IActionResult> CaptureRequest(PyrusTask pyrusTask, CancellationToken ct)
    {
        var messageForLoop = _pyrusService.GetMessageFromPyrusRequest(pyrusTask);

        if (messageForLoop != null)
        {
            await _loopService.SendUpdateMessage(messageForLoop);
            return Ok(new CaptureResponse
            {
                Text = string.Empty,
                ApprovalChoice = "approved"
            });
        }

        return Ok();
    }

    [HttpGet("tet")]
    public async Task<IActionResult> TestRequest()
    {
        return Ok(new CaptureResponse
        {
            Text = string.Empty,
            ApprovalChoice = "approved"
        });
    }
}