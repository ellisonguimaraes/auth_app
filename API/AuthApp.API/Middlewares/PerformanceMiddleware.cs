using System;
using System.Diagnostics;

namespace AuthApp.API.Middlewares;

/// <summary>
/// Performance Middleware Monitoring
/// </summary>
public class PerformanceMiddleware
{
    #region Constants
    private const string TRACE_ID_NAME = "TraceId";
    private const string DURATION_NAME = "Duration";
    private const string EXCEEDED_ROUTE_PERFORMANCE = "Exceeded route performance";
    private const long MAXIMUM_REQUEST_TIME_MS = 7000;
    #endregion

    private RequestDelegate _next;
    private ILogger<PerformanceMiddleware> _logger;

    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
	{
        _next = next;
        _logger = logger;
	}

    /// <summary>
    /// Route performance monitoring
    /// </summary>
    /// <param name="context">Http Context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var timer = new Stopwatch();

        timer.Start();
        await _next(context);
        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > MAXIMUM_REQUEST_TIME_MS)
            _logger.LogWarning($"[{context.Request.Method} {context.Request.Path}] {EXCEEDED_ROUTE_PERFORMANCE}, {DURATION_NAME}: {elapsedMilliseconds}ms, {TRACE_ID_NAME}: {Activity.Current?.Id}");
    }
}

