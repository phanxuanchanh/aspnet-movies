using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieCDN.Controllers;

[ApiController]
[Route("api/video")]
public class VideoController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<VideoController> _logger;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public VideoController(ILogger<VideoController> logger, StoragePath storagePath, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.VideoStoragePath);
        _webHostEnvironment = webHostEnvironment;
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
    //[Authorize]
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

    [Route("get-mp4/{partitionKey}/{fileName}.mp4")]
    [HttpGet]
    public IActionResult GetMp4(string partitionKey, string filename)
    {
        filename = filename.EndsWith(".mp4") ? filename : $"{filename}.mp4";

        string path = _fileManager.GetFilePath(partitionKey, filename);
        if (!_fileManager.FileExists(path))
            return NotFound();

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, "video/mp4", enableRangeProcessing: true);
    }

    [Route("upload")]
    [HttpPost]
    [Authorize]
    public IActionResult Upload()
    {
        return Ok();
    }
}