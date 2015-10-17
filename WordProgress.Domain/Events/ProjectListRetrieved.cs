using System.Collections.Generic;
using WordProgress.Domain.DTOs;

namespace WordProgress.Domain.Events
{
    public class ProjectListRetrieved : BaseEvent
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
