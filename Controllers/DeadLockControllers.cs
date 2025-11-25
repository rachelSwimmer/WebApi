using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeadlockController : ControllerBase
{
    private static readonly object CoffeeTool = new object();
    private static readonly object MilkTool = new object();

    [HttpGet("deadlock-demo")]
    public ActionResult<string> DeadlockDemo()
    {
        var results = new List<string>();
        results.Add("Coffee Shop is opening!");

        Thread baristaAlice = new Thread(() => MakeCoffee("Alice", results));
        Thread baristaBob = new Thread(() => MakeCoffee("Bob", results));

        baristaAlice.Start();
        baristaBob.Start();

        bool aliceDone = baristaAlice.Join(TimeSpan.FromSeconds(5));
        bool bobDone = baristaBob.Join(TimeSpan.FromSeconds(5));

        if (!aliceDone || !bobDone)
        {
            results.Add("DEADLOCK DETECTED! Threads are stuck waiting for each other.");
            baristaAlice.Interrupt();
            baristaBob.Interrupt();
        }
        else
        {
            results.Add("Coffee Shop is closing!");
        }

        return Ok(string.Join("\n", results));
    }

    [HttpGet("deadlock-fixed")]
    public ActionResult<string> DeadlockFixed()
    {
        var results = new List<string>();
        results.Add("Coffee Shop is opening!");

        Thread baristaAlice = new Thread(() => MakeCoffeeFixed("Alice", results));
        Thread baristaBob = new Thread(() => MakeCoffeeFixed("Bob", results));

        baristaAlice.Start();
        baristaBob.Start();

        baristaAlice.Join();
        baristaBob.Join();

        results.Add("Coffee Shop is closing!");

        return Ok(string.Join("\n", results));
    }

    static void MakeCoffee(string baristaName, List<string> results)
    {
        try
        {
            if (baristaName == "Alice")
            {
                lock (CoffeeTool)
                {
                    results.Add($"{baristaName} took Coffee Tool");
                    Thread.Sleep(1000);
                    lock (MilkTool)
                    {
                        results.Add($"{baristaName} took Milk Tool");
                        results.Add($"{baristaName} made the coffee!");
                    }
                }
            }
            else
            {
                lock (MilkTool)
                {
                    results.Add($"{baristaName} took Milk Tool");
                    Thread.Sleep(1000);
                    lock (CoffeeTool)
                    {
                        results.Add($"{baristaName} took Coffee Tool");
                        results.Add($"{baristaName} made the coffee!");
                    }
                }
            }
        }
        catch (ThreadInterruptedException)
        {
            results.Add($"{baristaName} was interrupted due to deadlock");
        }
    }

    static void MakeCoffeeFixed(string baristaName, List<string> results)
    {
        lock (CoffeeTool)
        {
            results.Add($"{baristaName} took Coffee Tool");
            Thread.Sleep(500);
            lock (MilkTool)
            {
                results.Add($"{baristaName} took Milk Tool");
                results.Add($"{baristaName} made the coffee!");
            }
        }
    }
}
