using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Events.Spellen;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Spellen.EventHandlers
{
    public class SpelCreatedEventHandler : INotificationHandler<SpelCreatedEvent>
    {
        private readonly ILogger<SpelCreatedEventHandler> _logger;

        public SpelCreatedEventHandler(ILogger<SpelCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SpelCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reversi Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
