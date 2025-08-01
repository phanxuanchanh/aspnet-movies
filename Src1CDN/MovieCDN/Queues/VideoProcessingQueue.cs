using System.Collections.Concurrent;

namespace MovieCDN.Queues;

public class VideoProcessingQueue : IVideoProcessingQueue
{
    private readonly ConcurrentQueue<(string VideoPath, string VideoName)> _queue = new();

    public void Enqueue((string VideoPath, string VideoName) pathAndName)
    {
        _queue.Enqueue(pathAndName);
    }

    public bool TryDequeue(out (string VideoPath, string VideoName) pathAndName)
    {
        return _queue.TryDequeue(out pathAndName);
    }
}