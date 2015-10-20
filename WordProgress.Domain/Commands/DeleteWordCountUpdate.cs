using System;

namespace WordProgress.Domain.Commands
{
    public class DeleteWordCountUpdate : BaseCommand
    {
        public Guid WordCountUpdateId { get; set; }
    }
}