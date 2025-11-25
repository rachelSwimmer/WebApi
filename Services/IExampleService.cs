namespace WebApi.Services;

public interface IExampleService
{
    // Async methods (proper way)
    Task<string> GetDataAsync();
    Task<string> ProcessMultipleTasksAsync();
    Task<string> ProcessSequentiallyAsync();
    Task<string> ProcessInParallelAsync();
    
    // Synchronous methods (blocking - NOT recommended)
    string GetDataSync();
    string ProcessMultipleTasksSync();
}
