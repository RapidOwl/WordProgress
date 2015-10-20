using System;

namespace WordProgress.Domain.Commands
{
    public class UpdateWordCount : BaseCommand
    {
        public Guid WordCountUpdateId { get; set; }
         public uint NewTotalWordCount { get; set; }
    }
}