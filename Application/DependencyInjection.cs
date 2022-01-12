using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Application.Hubs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<BaseHub, ReversiHub>();
            return services;
        }
    }
}
