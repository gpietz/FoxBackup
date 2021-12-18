namespace FoxBackup;

internal sealed class LogDistributor : ILogger
{
    private readonly Type _type;
    private readonly NLog.Logger _logger;
    
    public LogDistributor(Type type)
    {
        _type = type ?? throw new ArgumentNullException(nameof(type));
        _logger = NLog.LogManager.GetLogger(type.Name, type);
    }
    
    /// <summary>
    /// Writes a log entry.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">Id of the event.</param>
    /// <param name="state">The entry to be written. Can be also an object.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="formatter">Function to create a String message of the state and exception.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                _logger.Trace(state);
                break;
            
            case LogLevel.Debug:
                _logger.Debug(state);
                break;
            
            case LogLevel.Information:
                _logger.Info(state);
                break;
            
            case LogLevel.Warning:
                _logger.Warn(state);
                break;
            
            case LogLevel.Error:
                _logger.Error(exception, state?.ToString());
                break;
            
            case LogLevel.Critical:
                _logger.Fatal(exception, state?.ToString());
                break;
            
            case LogLevel.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Checks if the given logLevel is enabled.
    /// </summary>
    /// <param name="logLevel">level to be checked.</param>
    /// <returns><b>true</b> if enabled; <b>false</b> otherwise.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Begins a logical operation scope. (not yet implemented)
    /// </summary>
    /// <param name="state">The type of the state to begin scope for</param>
    /// <typeparam name="TState"></typeparam>
    /// <returns>The identifier for the scope.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}
