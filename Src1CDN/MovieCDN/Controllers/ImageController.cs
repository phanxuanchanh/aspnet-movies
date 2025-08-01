using Microsoft.AspNetCore.Mvc;

namespace MovieCDN.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<VideoController> _logger;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public ImageController(ILogger<VideoController> logger, StoragePath storagePath, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.ImageStoragePath);
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("default")]
    [HttpGet]
    public IActionResult GetDefault()
    {
        string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DefaultFiles/default.png");
        return PhysicalFile(filePath, "image/png");
    }

    [Route("get/{partitionKey}/{filename}")]
    [HttpGet]
    //[Authorize]
    public IActionResult Get(string partitionKey, string filename)
    {
        string path = _fileManager.GetFilePath(partitionKey, filename);
        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }
}