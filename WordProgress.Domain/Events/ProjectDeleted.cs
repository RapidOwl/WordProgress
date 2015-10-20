using System;

namespace WordProgress.Domain.Events
{
    public class ProjectDeleted : BaseEvent
    {
        public Guid ProjectId { get; set; }
    }
}
