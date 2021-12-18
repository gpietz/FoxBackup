using System.Diagnostics;
using System.Reflection;
using NLog.Config;
using NLog.Targets;

namespace FoxBackup;

internal sealed class LogManager
{
    private const string DefaultLogFileName = "FoxBackup.log";

    private readonly Dictionary<Type, ILogger> _logMap = new();
    private bool _configured;
    
    public void ConfigureLogging()
    {
        if (_configured)
            throw new InvalidOperationException("Logging can't be configured twice; Logging has been already configured!");

        _configured = true;
        _logMap.Clear();

        var config = new LoggingConfiguration();
        var fileTarget = CreateFileTarget();
        config.AddRuleForAllLevels(fileTarget);
        config.AddTarget(fileTarget);

        NLog.LogManager.Configuration = config;
    }

    private static FileTarget CreateFileTarget()
    {
        return new FileTarget
        {
            Name ="FileTarget",
            FileName = GetLoggingFileName(),
        };
    }

    private static string GetLoggingFileName()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        folder = Path.Combine(folder, "FoxBackup");
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        return Path.Combine(folder, DefaultLogFileName);
    }

    public void LogAppStart()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var log = GetLog<LogManager>();
        var textDecoration = new string('=', 5);
        var appVersion = versionInfo.FileVersion;
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        log.Log(LogLevel.Information, $"{textDecoration} FoxBackup v{appVersion} {textDecoration}");
    }

    public ILogger GetLog(Type type)
    {
        if (_logMap.TryGetValue(type, out var logger))
            return logger;

        logger = new LogDistributor(type);
        _logMap.Add(type, logger);
        return logger;
    }
    
    public static ILogger GetLog<TService>() 
        where TService : class
    {
        var logManager = Worker.Container.GetInstance<LogManager>();
        return logManager.GetLog(typeof(TService));
    }
}
