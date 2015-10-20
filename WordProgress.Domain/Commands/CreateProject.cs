using System;

namespace WordProgress.Domain.Commands
{
    public class CreateProject : BaseCommand
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }

        public uint TargetWordCount { get; set; }
    }
}