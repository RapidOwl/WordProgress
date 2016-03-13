using System;

namespace WordProgress.ReadModels.DTOs
{
    public class ProjectDto : BaseDto
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }

        public int TargetWordCount { get; set; }
    }
}
