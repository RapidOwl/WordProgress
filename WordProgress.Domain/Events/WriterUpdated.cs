namespace WordProgress.Domain.Events
{
    public class WriterUpdated : BaseEvent
    {
        public string Name { get; set; }
        public string Bio { get; set; }
    }
}
