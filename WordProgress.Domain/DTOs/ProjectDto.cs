using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace WordProgress.Domain.DTOs
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
