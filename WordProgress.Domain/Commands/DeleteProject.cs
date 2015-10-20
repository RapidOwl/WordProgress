using System;

namespace WordProgress.Domain.Commands
{
    public class DeleteProject : BaseCommand
    {
         public Guid ProjectId { get; set; }
    }
}