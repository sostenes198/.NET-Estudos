namespace Estudos.Hangfire._2Server.Services;

public interface IRepo
{
    Task SaveAsync(string message);
}

public class Repo : IRepo
{
    private readonly ILogger<Repo> _logger;

    public Repo(ILogger<Repo> logger)
    {
        _logger = logger;
    }

    public Task SaveAsync(string message)
    {
        _logger.LogInformation(message);
        return Task.CompletedTask;
    }
}