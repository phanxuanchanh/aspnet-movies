using MediaSrv.AppCodes;

namespace MediaSrv.Queues;

public interface IVideoProcessingQueue
{
    void Enqueue(FileLocation fileLocation);
    bool TryDequeue(out FileLocation? fileLocation);
}