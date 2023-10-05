using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middleware
{
    public class HideHiddenFilesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] HiddenFileNames = { ".darcs", ".bzr", ".hg", "BitKeeper", ".bitkeeper", "._darcs"};
        
        private ILogger<HideHiddenFilesMiddleware> _logger;
        private readonly ICurrentUserService _currentUserService;
        public HideHiddenFilesMiddleware(RequestDelegate next, ILogger<HideHiddenFilesMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
        {
            var requestPath = context.Request.Path.Value;

            // Check if the requested path contains any hidden file/directory names
            if (HiddenFileNames.Any(fileName => requestPath.Contains(fileName)))
            {
                
                
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                _logger.LogWarning($"Someone with Userid: { currentUserService.UserId } tried to access a forbidden file");
                await context.Response.WriteAsync("Access to hidden files and directories is forbidden.");
                return;
            }

            // Continue processing the request pipeline
            await _next(context);
        }
    }
}
