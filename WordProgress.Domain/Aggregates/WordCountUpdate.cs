using WordProgress.Edument;

namespace WordProgress.Domain.Aggregates
{
    public class WordCountUpdate : Aggregate
    {
        public uint WordsAdded { get; set; }
    }
}
