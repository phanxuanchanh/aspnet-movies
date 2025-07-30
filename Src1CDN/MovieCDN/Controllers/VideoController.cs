using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieCDN.Controllers;

[ApiController]
[Route("api/video")]
public class VideoController : ControllerBase
{
    private readonly ILogger<VideoController> _logger;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public VideoController(ILogger<VideoController> logger, StoragePath storagePath)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.VideoStoragePath);
    }

    [HttpGet("get/{partitionKey}/{filename}")]
    //[Authorize]
    public IActionResult Get(string partitionKey, string filename)
    {
        string path = _fileManager.GetFilePath(partitionKey, filename);
        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if(!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }

    [Route("upload")]
    [HttpPost]
    [Authorize]
    public IActionResult Upload()
    {
        return Ok();
    }
}