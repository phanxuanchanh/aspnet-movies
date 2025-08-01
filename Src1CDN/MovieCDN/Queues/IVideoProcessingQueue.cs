namespace MovieCDN.Queues;

public interface IVideoProcessingQueue
{
    void Enqueue((string VideoPath, string VideoName) pathAndName);
    bool TryDequeue(out (string VideoPath, string VideoName) pathAndName);
}