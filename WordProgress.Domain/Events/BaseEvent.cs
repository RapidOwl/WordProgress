using System;

namespace WordProgress.Domain.Events
{
    public class BaseEvent
    {
        public Guid Id { get; set; }
    }
}
