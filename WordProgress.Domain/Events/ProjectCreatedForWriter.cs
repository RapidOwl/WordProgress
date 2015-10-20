using System;

namespace WordProgress.Domain.Events
{
    public class ProjectCreatedForWriter : BaseEvent
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }

        public uint TargetWordCount { get; set; }
    }
}
