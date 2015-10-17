namespace WordProgress.Domain.Events
{
    public class WriterRegistered : BaseEvent
    {
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
