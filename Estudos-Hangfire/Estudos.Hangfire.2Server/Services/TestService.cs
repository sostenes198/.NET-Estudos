using Hangfire;

namespace Estudos.Hangfire._2Server.Services;

public class TestService : ITestService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TestService> _logger;
    private readonly IRepo _repo;

    public TestService(IHttpContextAccessor httpContextAccessor, ILogger<TestService> logger, IRepo repo)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _repo = repo;
    }
    public Task WriteServer2()
    {
        
        _repo.SaveAsync("Postando Mensagem  App Server2");
        return Task.CompletedTask;
    }
}