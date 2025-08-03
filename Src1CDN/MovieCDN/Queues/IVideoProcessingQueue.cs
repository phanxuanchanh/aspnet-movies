using MovieCDN.AppCodes;

namespace MovieCDN.Queues;

public interface IVideoProcessingQueue
{
    void Enqueue(FileLocation fileLocation);
    bool TryDequeue(out FileLocation? fileLocation);
}