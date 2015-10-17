using System;

namespace WordProgress.Domain.Events
{
    public class ProjectUpdated : BaseEvent
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public int TargetWordCount { get; set; }
    }
}
