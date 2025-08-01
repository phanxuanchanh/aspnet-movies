using MovieCDN.Queues;
using System.Diagnostics;

namespace MovieCDN;

public class VideoProcessingService : BackgroundService
{
    private readonly IVideoProcessingQueue _queue;
    private readonly FileManager _fileManager;

    public VideoProcessingService(IVideoProcessingQueue queue, StoragePath storagePath)
    {
        _queue = queue;
        _fileManager = new FileManager(storagePath.VideoStoragePath);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var pathAndName))
                await ProcessVideo(pathAndName, stoppingToken);
            else
                await Task.Delay(1000, stoppingToken);
        }
    }

    public async Task ProcessVideo((string VideoPath, string VideoName) pathAndName, CancellationToken stoppingToken)
    {
        string videoPath = pathAndName.VideoPath;
        string outputDir = _fileManager.GetFolderPath(pathAndName.VideoName);

        Directory.CreateDirectory(outputDir);

        // Chạy ffmpeg (multi resolution HLS)
        var args = $"-i \"{videoPath}\" " +
                   "-filter:v:0 scale=w=1920:h=1080 -map 0 -c:v:0 libx264 -b:v:0 5000k " +
                   "-filter:v:1 scale=w=1280:h=720  -map 0 -c:v:1 libx264 -b:v:1 3000k " +
                   "-filter:v:2 scale=w=854:h=480   -map 0 -c:v:2 libx264 -b:v:2 1000k " +
                   "-map a:0 -c:a aac -ac 2 -ar 48000 -b:a 128k " +
                   "-f hls -hls_time 6 -hls_playlist_type vod " +
                   "-var_stream_map \"v:0,a:0 v:1,a:0 v:2,a:0\" " +
                   $"-master_pl_name master.m3u8 " +
                   $"-hls_segment_filename \"{outputDir}/%v/segment%d.ts\" " +
                   $"\"{outputDir}/%v/index.m3u8\"";

        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        string errorLog = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync(stoppingToken);
    }
}
