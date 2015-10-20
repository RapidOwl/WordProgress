using System;

namespace WordProgress.Domain.Events
{
    public class WordCountUpdateDeleted : BaseEvent
    {
        public Guid WordCountUpdateId { get; set; }
    }
}
