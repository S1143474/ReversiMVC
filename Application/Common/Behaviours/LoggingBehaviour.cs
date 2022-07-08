using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        private readonly IReversiDbContext reversiDbContext;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            string uniqueId = Guid.NewGuid().ToString();

            var userId = _currentUserService.UserId ?? string.Empty;
            string userName = string.Empty;

            /*if (!string.IsNullOrEmpty(userId))
            {
                userName = (await reversiDbContext.Spelers.FindAsync(userId))!.Naam;
            }*/

            /*_logger.LogInformation($"Begin Request Id: {uniqueId}, request name: {requestName}, requested by user: {userName}, request: {request}");*/
            _logger.LogInformation($"Begin Request Id: {uniqueId}, request name: {requestName}, requested by user: {userId}, request: {request}");
            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();

            _logger.LogInformation($"End Request Id: {uniqueId}, request name: {requestName}, requested by user: {userId}, total request time: {timer.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
