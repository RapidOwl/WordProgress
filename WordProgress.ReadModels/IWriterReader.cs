using WordProgress.ReadModels.DTOs;

namespace WordProgress.ReadModels
{
    public interface IWriterReader
    {
        WriterDto GetWriterDetails(string userName);
    }
}