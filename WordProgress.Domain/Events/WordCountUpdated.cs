namespace WordProgress.Domain.Events
{
    public class WordCountUpdated : BaseEvent
    {
        public int NewTotalWordCount { get; set; }
        public int WordsAdded { get; set; }
    }
}
