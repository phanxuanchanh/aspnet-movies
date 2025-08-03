using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;

namespace MovieCDN.AppCodes;

public class VideoProcessor
{
    public bool UseGpu { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private void DetectGpu()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = "-encoders",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = Process.Start(startInfo);
        string output = process!.StandardOutput.ReadToEnd();
        process.WaitForExit();

        UseGpu = output.Contains("h264_nvenc");
    }

    private void GetVideoResolution()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffprobe",
            Arguments = $"-v error -select_streams v:0 -show_entries stream=width,height -of csv=p=0 \"{"InputPath"}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        string? output = process?.StandardOutput.ReadLine();
        process?.WaitForExit();

        if (string.IsNullOrWhiteSpace(output))
            throw new Exception("Cannot retrieve video resolution.");

        var parts = output.Split(',');
        Width = int.Parse(parts[0]);
        Height = int.Parse(parts[1]);
    }
}
