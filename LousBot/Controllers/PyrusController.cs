using LousBot.Service;
using Microsoft.AspNetCore.Mvc;

namespace LousBot.Controllers;

[ApiController]
[Route("[controller]")]
public class PyrusController : ControllerBase
{
    private readonly LoopService _loopService;

    public PyrusController(LoopService loopService)
    {
        _loopService = loopService;
    }

    public string Get()
    {
        return "2";
    }
}