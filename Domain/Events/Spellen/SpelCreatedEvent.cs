using System;
using System.Collections.Generic;
using System.Text;
using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Spellen
{
    public class SpelCreatedEvent : BaseEvent
    {
        public SpelCreatedEvent(Spel item)
        {
            Item = item;
        }

        public Spel Item { get; }
    }
}
