namespace WebApi.Services;

public class ExampleService : IExampleService
{
    public async Task<string> GetDataAsync()
    {
        await Task.Delay(2000);
        return "Data fetched asynchronously!";
    }

    public async Task<string> ProcessSequentiallyAsync()
    {
        var start = DateTime.Now;
        
        var result1 = await FetchFromDatabase();
        var result2 = await FetchFromApi();
        var result3 = await FetchFromCache();
        
        var duration = (DateTime.Now - start).TotalSeconds;
        return $"Sequential: {result1}, {result2}, {result3}. Time: {duration:F1}s";
    }

    public async Task<string> ProcessInParallelAsync()
    {
        var start = DateTime.Now;
        
        var task1 = FetchFromDatabase();
        var task2 = FetchFromApi();
        var task3 = FetchFromCache();
        
        var results = await Task.WhenAll(task1, task2, task3);
        
        var duration = (DateTime.Now - start).TotalSeconds;
        return $"Parallel: {string.Join(", ", results)}. Time: {duration:F1}s";
    }

    public async Task<string> ProcessMultipleTasksAsync()
    {
        var start = DateTime.Now;
        
        var userData = await FetchFromDatabase();
        
        var profileTask = FetchUserProfile(userData);
        var ordersTask = FetchUserOrders(userData);
        
        await Task.WhenAll(profileTask, ordersTask);
        
        var duration = (DateTime.Now - start).TotalSeconds;
        return $"Async: Got {userData}, {profileTask.Result}, {ordersTask.Result}. Time: {duration:F1}s";
    }

    public string GetDataSync()
    {
        Thread.Sleep(2000);
        return "Data fetched synchronously (BLOCKED thread for 2s)";
    }

    public string ProcessMultipleTasksSync()
    {
        var start = DateTime.Now;
        
        var result1 = FetchFromDatabase().Result;
        var result2 = FetchFromApi().Result;
        var result3 = FetchFromCache().Result;
        
        var duration = (DateTime.Now - start).TotalSeconds;
        return $"Sync (BAD): {result1}, {result2}, {result3}. Time: {duration:F1}s (thread blocked!)";
    }

    private async Task<string> FetchFromDatabase()
    {
        await Task.Delay(2000);
        return "DB data";
    }
    
    private async Task<string> FetchFromApi()
    {
        await Task.Delay(2000);
        return "API data";
    }
    
    private async Task<string> FetchFromCache()
    {
        await Task.Delay(2000);
        return "Cache data";
    }
    
    private async Task<string> FetchUserProfile(string userId)
    {
        await Task.Delay(1000);
        return $"Profile for {userId}";
    }
    
    private async Task<string> FetchUserOrders(string userId)
    {
        await Task.Delay(1000);
        return $"Orders for {userId}";
    }
}
