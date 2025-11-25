using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExamplesController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpGet("async")]
    public async Task<ActionResult<string>> GetAsync()
    {
        var result = await _exampleService.GetDataAsync();
        return Ok(result);
    }

    [HttpGet("sequential")]
    public async Task<ActionResult<string>> GetSequential()
    {
        var result = await _exampleService.ProcessSequentiallyAsync();
        return Ok(result);
    }

    [HttpGet("parallel")]
    public async Task<ActionResult<string>> GetParallel()
    {
        var result = await _exampleService.ProcessInParallelAsync();
        return Ok(result);
    }

    [HttpGet("complex")]
    public async Task<ActionResult<string>> GetComplex()
    {
        var result = await _exampleService.ProcessMultipleTasksAsync();
        return Ok(result);
    }

    [HttpGet("sync-bad")]
    public ActionResult<string> GetSyncBad()
    {
        var result = _exampleService.GetDataSync();
        return Ok(result + " - This BLOCKED the thread!");
    }

    [HttpGet("sync-worse")]
    public ActionResult<string> GetSyncWorse()
    {
        var result = _exampleService.ProcessMultipleTasksSync();
        return Ok(result + " - This BLOCKED the thread multiple times!");
    }

    [HttpGet("compare")]
    public async Task<ActionResult<object>> Compare()
    {
        var asyncStart = DateTime.Now;
        var asyncResult = await _exampleService.ProcessInParallelAsync();
        var asyncTime = (DateTime.Now - asyncStart).TotalSeconds;

        var syncStart = DateTime.Now;
        var syncResult = _exampleService.ProcessMultipleTasksSync();
        var syncTime = (DateTime.Now - syncStart).TotalSeconds;

        return Ok(new
        {
            Async = new { Result = asyncResult, TimeSeconds = asyncTime, ThreadBlocked = false },
            Sync = new { Result = syncResult, TimeSeconds = syncTime, ThreadBlocked = true },
            Difference = $"Async is {syncTime / asyncTime:F1}x faster and doesn't block threads!"
        });
    }
}
