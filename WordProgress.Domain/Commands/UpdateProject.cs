using System;

namespace WordProgress.Domain.Commands
{
    public class UpdateProject : BaseCommand
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public int TargetWordCount { get; set; }
    }
}