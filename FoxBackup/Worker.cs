using SimpleInjector;

namespace FoxBackup;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    internal static Container Container { get; private set; }

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            InitializeContainer();
            InitializeLogging();
        }, cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void InitializeContainer()
    {
        Container = new Container();
        Container.RegisterSingleton<LogManager>();
    }

    private static void InitializeLogging()
    {
        var logManager = IocProxy.Get<LogManager>();
        logManager.ConfigureLogging();
        logManager.LogAppStart();
    }
}
