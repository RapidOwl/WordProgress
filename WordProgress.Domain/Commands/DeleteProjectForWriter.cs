using System;

namespace WordProgress.Domain.Commands
{
    public class DeleteProjectForWriter : BaseCommand
    {
         public Guid ProjectId { get; set; }
    }
}