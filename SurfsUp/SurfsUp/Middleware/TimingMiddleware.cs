using static System.Net.Mime.MediaTypeNames;

namespace SurfsUp.Middleware;

public class TimingMiddleware
{
	private readonly ILogger<TimingMiddleware> _logger;
	private readonly RequestDelegate _next;
	public TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
	{
		_logger = logger;
		_next = next;
	}

	public async Task Invoke(HttpContext ctx)
	{
        var start = DateTime.Now;
        await _next.Invoke(ctx);
        _logger.LogInformation($"Timing: {ctx.Request.Path}: {(DateTime.Now - start).TotalMilliseconds}ms");
    }
}
