using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLoggerFactory : ILoggerFactory
{
    public static readonly UnityLoggerFactory Instance = new UnityLoggerFactory();

    /// <inheritdoc />
    /// <remarks>
    /// This returns a <see cref="NullLogger"/> instance which logs nothing.
    /// </remarks>
    public ILogger CreateLogger(string name)
    {
        return UnityLogger.Instance;
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method ignores the parameter and does nothing.
    /// </remarks>
    public void AddProvider(ILoggerProvider provider)
    {
    }

    public void Dispose()
    {
    }
}
