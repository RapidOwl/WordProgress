using WordProgress.Domain.Aggregates;
using WordProgress.Edument;
using WordProgress.ReadModels;

namespace WordProgress.Web
{
    public static class WPDomain
    {
        public static MessageDispatcher Dispatcher;
        public static IWriterReader WriterReaderQueries;

        public static void Setup()
        {
            Dispatcher = new MessageDispatcher(new InMemoryEventStore());

            Dispatcher.ScanInstance(new Writer());

            WriterReaderQueries = new WriterReader();
            Dispatcher.ScanInstance(WriterReaderQueries);
        }
    }
}