using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCDN.AppCodes;
using MovieCDN.Queues;

namespace MovieCDN.Controllers;

[ApiController]
[Route("api/video")]
public class VideoController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IVideoProcessingQueue _videoProcessingQueue;
    private readonly ILogger<VideoController> _logger;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public VideoController(ILogger<VideoController> logger, StoragePath storagePath, IWebHostEnvironment webHostEnvironment, IVideoProcessingQueue videoProcessingQueue)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.VideoStoragePath);
        _webHostEnvironment = webHostEnvironment;
        _videoProcessingQueue = videoProcessingQueue;
    }

    [Route("default")]
    [HttpGet]
    public IActionResult GetDefault()
    {
        string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DefaultFiles/default.mp4");
        return PhysicalFile(filePath, "video/mp4");
    }

    [Route("get/{partitionKey}/{filename}.m3u8")]
    [HttpGet]
    public IActionResult GetM3u8(string partitionKey, string filename)
    {
        if(filename.EndsWith(".m3u8"))
            return BadRequest("Filename should not end with .m3u8");
        
        string path = _fileManager.GetFilePath($"{partitionKey}/{filename}", "master.m3u8");
        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if(!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }

    [Route("get-original-video/{partitionKey}/{fileName}")]
    [HttpGet]
    public IActionResult GetOriginalVideo(string partitionKey, string filename)
    {
        string path = _fileManager.GetFilePath(partitionKey, filename);
        if (!_fileManager.FileExists(path))
            return NotFound();

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, "video/mp4", enableRangeProcessing: true);
    }

    [Route("upload")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Upload(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("File is empty.");

        string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
        string[] allowedExtensions = new[] { ".mp4", ".avi", ".mkv", ".webm", ".mov", ".mpeg" };
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Unsupported file extension.");

        string partitionKey = FileManager.GenerateParitionKey();
        string fileName = FileManager.GenerateFileName(extension);
        string filePath = _fileManager.GetFilePath(partitionKey, fileName);
        while (true)
        {
            if (!_fileManager.FileExists(filePath))
                break;

            fileName = FileManager.GenerateFileName(extension);
            filePath = _fileManager.GetFilePath(partitionKey, fileName);
        }

        using FileStream stream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(stream);

        _videoProcessingQueue.Enqueue(new FileLocation { 
            PartitionKey = partitionKey, FilePath = filePath, FileName = fileName.Replace(extension, string.Empty)
        });

        return Ok(new { partitionKey, fileName });
    }
}