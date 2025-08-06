using MediaSrv.AppCodes;
using System.Collections.Concurrent;

namespace MediaSrv.Queues;

public class VideoProcessingQueue : IVideoProcessingQueue
{
    private readonly ConcurrentQueue<FileLocation?> _queue = new();

    public void Enqueue(FileLocation fileLocation)
    {
        _queue.Enqueue(fileLocation);
    }

    public bool TryDequeue(out FileLocation? fileLocation)
    {
        return _queue.TryDequeue(out fileLocation);
    }
}