using System.Linq;
using System.Security.Claims;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Hubs
{
    public class BaseHub : Hub<IReversiHub>
    {

        private ISender _mediator = null!;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private static string[] mobileDevices = new string[]
        {
            // TODO: if necessary add value android to array.
            "iphone",
            "ppc",
            "windows ce",
            "blackberry",
            "opera mini",
            "mobile",
            "palm",
            "portable",
            "opera mobi"
        };

        public BaseHub([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected ISender Mediator => _mediator ??= _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISender>();

        protected string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        protected string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        private string _userAgent => _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
        protected bool IsMobileDevice => mobileDevices.Any(device => _userAgent.ToLower().Contains(device));
    }
}
