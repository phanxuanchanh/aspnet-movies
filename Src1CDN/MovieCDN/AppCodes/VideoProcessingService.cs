using MovieCDN.Queues;
using System.Diagnostics;

namespace MovieCDN.AppCodes;

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
            if (_queue.TryDequeue(out var fileLocation))
                await ProcessVideo(fileLocation!, stoppingToken);
            else
                await Task.Delay(1000, stoppingToken);
        }
    }

    public async Task ProcessVideo(FileLocation fileLocation, CancellationToken stoppingToken)
    {
        string videoPath = fileLocation.FilePath;
        string outputDir = _fileManager.GetFolderPath($"{fileLocation.PartitionKey}/{fileLocation.FileName}/");

        string normalizedVideoPath = videoPath.Replace("\\", "/");
        string normalizedOutputDir = outputDir.Replace("\\", "/");

        // Chạy ffmpeg (multi resolution HLS)
        string args = $"-i \"{normalizedVideoPath}\""
            + " -filter_complex \"[0:v]split=3[v1080][v720][v480];[v1080]scale=w=1920:h=1080[v1080out];[v720]scale=w=1280:h=720[v720out];[v480]scale=w=854:h=480[v480out]\""
            + " -map \"[v1080out]\" -map 0:a:0 -c:v:0 libx264 -b:v:0 5000k -c:a:0 copy"
            + " -map \"[v720out]\" -map 0:a:0 -c:v:1 libx264 -b:v:1 3000k -c:a:1 copy"
            + " -map \"[v480out]\" -map 0:a:0 -c:v:2 libx264 -b:v:2 1000k -c:a:2 copy"
            + " -f hls -hls_time 6 -hls_playlist_type vod"
            + " -master_pl_name master.m3u8"
            + $" -hls_segment_filename \"{normalizedOutputDir}%v/segment%d.ts\""
            + " -var_stream_map \"v:0,a:0 v:1,a:1 v:2,a:2\""
            + $" \"{normalizedOutputDir}%v/index.m3u8\"";

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
