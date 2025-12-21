using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.Dlna
{
/// <summary>
/// Middleware to fix encoded URLs for DLNA requests.
/// </summary>
/// 
public class DlnaUrlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DlnaUrlMiddleware> _logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="DlnaUrlMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public DlnaUrlMiddleware(RequestDelegate next, ILogger<DlnaUrlMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

  /// <summary>
  /// Invokes the middleware.
  /// </summary>
  /// <param name="context">The http context.</param>
  /// <returns>A task representing the operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Before Processing DLNA URL: {Path}", context.Request.Path);

        // Process only DLNA video requests
        if (context.Request.Path.StartsWithSegments("/Dlna/Videos", System.StringComparison.OrdinalIgnoreCase))
        {
            string rawUrl = context.Request.Path;
            var fullUrl = rawUrl.Replace("%3f", "?", System.StringComparison.OrdinalIgnoreCase);
            var split = fullUrl.Split('?', 2);
        
            context.Request.Path = new PathString(split[0]);
            if (split.Length > 1)
            {
                context.Request.QueryString = new QueryString("?" + split[1]);
            }

        }

        await _next(context).ConfigureAwait(false);;
    }
}

}