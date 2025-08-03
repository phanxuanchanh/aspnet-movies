using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCDN.AppCodes;

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

    [Route("upload")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Upload(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("File is empty.");

        string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
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

        return Ok(new { partitionKey, fileName });
    }
}