using System;
using UnityEngine;

public class UnityLogger : ILogger
{
    public static UnityLogger Instance { get; } = new UnityLogger();

    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }

    private UnityLogger()
    {
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
    {
        return NullScope.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        switch(logLevel)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                Debug.LogError((formatter == null) ? exception.ToString() : formatter.Invoke(state, exception));
                break;
            default:
                Debug.Log((formatter == null) ? exception.ToString() : formatter.Invoke(state, exception));
                break;
        }
    }
}
