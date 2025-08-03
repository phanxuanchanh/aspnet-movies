using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using MovieCDN.AppCodes;
using MovieCDN.Database;
using MovieCDN.Queues;

namespace MovieCDN.Controllers;

[ApiController]
[Route("api/video")]
public class VideoController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IVideoProcessingQueue _videoProcessingQueue;
    private readonly ILogger<VideoController> _logger;
    private readonly MovieCdnContext _context;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public VideoController(ILogger<VideoController> logger, StoragePath storagePath, IWebHostEnvironment webHostEnvironment, IVideoProcessingQueue videoProcessingQueue, MovieCdnContext context)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.VideoStoragePath);
        _webHostEnvironment = webHostEnvironment;
        _videoProcessingQueue = videoProcessingQueue;
        _context = context;
    }

    [Route("default")]
    [HttpGet]
    public IActionResult GetDefault()
    {
        string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DefaultFiles/default.mp4");
        return PhysicalFile(filePath, "video/mp4");
    }

    [Route("get/{fileId}.m3u8")]
    [HttpGet]
    public IActionResult GetM3u8(string fileId)
    {
        Database.File? file = _context.Files.Find(fileId);
        if (file is null)
            return NotFound();

        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string path = _fileManager.GetFilePath($"{file.PartitionKey}/{file.FileName.Replace(extension, string.Empty)}/", "master-edited.m3u8");
        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if(!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }

    [Route("get/{fileId}.m3u8/{subIndex}/{subName}.m3u8")]
    [HttpGet]
    public IActionResult GetSubM3u8(string fileId, string subIndex, string subName)
    {
        Database.File? file = _context.Files.Find(fileId);
        if (file is null)
            return NotFound();

        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string path = _fileManager.GetFilePath($"{file.PartitionKey}/{file.FileName.Replace(extension, string.Empty)}/", $"{subIndex}/{subName}.m3u8");

        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }

    [Route("get/{fileId}.m3u8/{subIndex}/{segment}.ts")]
    [HttpGet]
    public IActionResult GetSegmentTs(string fileId, string subIndex, string segment)
    {
        Database.File? file = _context.Files.Find(fileId);
        if (file is null)
            return NotFound();

        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string path = _fileManager.GetFilePath($"{file.PartitionKey}/{file.FileName.Replace(extension, string.Empty)}/", $"{subIndex}/{segment}.ts");

        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(path, contentType);
    }

    [Route("get/{fileId}")]
    [HttpGet]
    public IActionResult Get(string fileId)
    {
        Database.File? file = _context.Files.Find(fileId);
        if (file is null)
            return NotFound();

        string path = _fileManager.GetFilePath(file.PartitionKey, file.FileName);
        if (!_fileManager.FileExists(path))
            return NotFound();

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
            contentType = "application/octet-stream";

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(stream, contentType, enableRangeProcessing: true);
    }

    [Route("get-meta/{fileId}")]
    [HttpGet]
    public async Task<IActionResult> GetMeta(string fileId)
    {
        Database.File? file = await _context.Files.FindAsync(fileId);
        if (file is null)
            return NotFound();

        FileMetaDto fileMeta = new FileMetaDto
        {
            Id = file.Id,
            PartitionKey = file.PartitionKey,
            FileName = file.FileName,
            Title = file.Title,
            Description = file.Description
        };

        return Ok(fileMeta);
    }

    [Route("get-list")]
    [HttpGet]
    public async Task<IActionResult> GetList(int maxRecords = 20, string search = "")
    {
        IQueryable<Database.File> query = _context.Files.Where(f => f.Type == "video");
        if (!string.IsNullOrEmpty(search))
        {
            query.Where(f => f.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                || (string.IsNullOrEmpty(f.Title) && f.FileName.Contains(search, StringComparison.OrdinalIgnoreCase))
            );
        }

        query.Take(maxRecords).Select(s => new FileMetaDto
        {
            Id = s.Id,
            PartitionKey = s.PartitionKey,
            FileName = s.FileName,
            Title = s.Title,
            Description = s.Description,
        });

        return Ok(await query.ToListAsync());
    }

    [Route("upload")]
    [HttpPost]
    //[Authorize]
    public async Task<IActionResult> Upload([FromForm]FileMetaInput metaInput, IFormFile formFile)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

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

        Database.File file = new Database.File
        {
            Id = fileName.Replace(extension, string.Empty),
            PartitionKey = partitionKey,
            FileName = fileName,
            Type = "video",
            Title = metaInput.Title,
            Description = metaInput.Description,
        };

        await _context.Files.AddAsync(file);
        await _context.SaveChangesAsync();

        _videoProcessingQueue.Enqueue(new FileLocation { 
            PartitionKey = partitionKey, FilePath = filePath, FileName = fileName.Replace(extension, string.Empty)
        });

        return Ok(new { file.Id });
    }

    [Route("delete/{fileId}")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string fileId)
    {
        Database.File? file = await _context.Files.FindAsync(fileId);
        if (file is null)
            return NotFound();

        string filePath = _fileManager.GetFilePath(file.PartitionKey, file.FileName);
        if (_fileManager.FileExists(filePath))
        {
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file {FilePath}", filePath);
                return StatusCode(500, "Failed to delete the file.");
            }
        }

        _context.Files.Remove(file);
        await _context.SaveChangesAsync();

        return Ok();
    }
}