using System;

namespace WordProgress.Domain.Events
{
    public class ProjectDeletedForWriter : BaseEvent
    {
        public Guid ProjectId { get; set; }
    }
}
