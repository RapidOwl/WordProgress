using WordProgress.Domain.Events;
using WordProgress.Edument;
using WordProgress.ReadModels.DTOs;

namespace WordProgress.ReadModels
{
    public class WriterReader : IWriterReader,
        ISubscribeTo<WriterRegistered>,
        ISubscribeTo<WriterUpdated>
    {
        // TODO Database persistence and reading.
        // This isn't the event store. The events are persisted somewhere else?
        // Is this how we avoid using event sourcing, while still using CQRS?

        // This is an in memory version of the writer object.
        // This and any other objects stored in memory won't technically know about each other.
        // So, any events coming in need to know the IDs of the objects they're referring to.
        // e.g. "Add Project" events need to have the writer's ID as well.
        private WriterDto _writer = new WriterDto();

        public void Handle(WriterRegistered e)
        {
            // This would write the changes to the read store.
            _writer.Id = e.Id;
            _writer.UserName = e.UserName;
            _writer.Name = e.Name;
        }

        public void Handle(WriterUpdated e)
        {
            // This would write the changes to the read store.
            _writer.Name = e.Name;
            _writer.Bio = e.Bio;
        }

        // Needs a user name parameter.
        public WriterDto GetWriterDetails(string userName)
        {
            // This would involve reading the project from the database?
            // Yeah, we need to keep the username parameter because we won't always be getting the writer from memory.
            return _writer;
        }
    }
}
