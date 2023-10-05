using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middleware
{
    public class HideHiddenFilesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] HiddenFileNames = { ".darcs", ".bzr", ".hg", "BitKeeper", ".bitkeeper", };

        public HideHiddenFilesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;

            // Check if the requested path contains any hidden file/directory names
            if (HiddenFileNames.Any(fileName => requestPath.Contains(fileName)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access to hidden files and directories is forbidden.");
                return;
            }

            // Continue processing the request pipeline
            await _next(context);
        }
    }
}
