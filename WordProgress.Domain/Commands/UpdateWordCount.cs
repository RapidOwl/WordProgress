namespace WordProgress.Domain.Commands
{
    public class UpdateWordCount : BaseCommand
    {
         public uint NewTotalWordCount { get; set; }
    }
}