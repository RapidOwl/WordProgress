using System;

namespace WordProgress.Domain.Events
{
    public class WordCountUpdated : BaseEvent
    {
        public Guid WordCountUpdateId { get; set; }
        public uint NewTotalWordCount { get; set; }
    }
}
