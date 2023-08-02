using System;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

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

        protected ControllerBase([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid UserId
        {
            get
            {
                var nameIdentifier = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(nameIdentifier, out var result))
                    return result;

                return Guid.Empty;
            }
        }

        protected HttpContext Context => _httpContextAccessor.HttpContext;
        protected string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        private string _userAgent => _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
        protected bool IsMobileDevice => mobileDevices.Any(device => _userAgent.ToLower().Contains(device));
    }
}
