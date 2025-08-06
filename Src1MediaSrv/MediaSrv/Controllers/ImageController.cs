using MediaSrv.AppCodes;
using MediaSrv.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MediaSrv.Controllers;

[ApiController]
[Route("api/image")]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly ILogger<VideoController> _logger;
    private readonly MovieCdnContext _context;
    private readonly StoragePath _storagePath;
    private readonly FileManager _fileManager;

    public ImageController(ILogger<VideoController> logger, StoragePath storagePath, MovieCdnContext context)
    {
        _logger = logger;
        _storagePath = storagePath;
        _fileManager = new FileManager(_storagePath.ImageStoragePath);
        _context = context;
    }

    [Route("get-default-image")]
    [HttpGet]
    public IActionResult GetDefault()
    {
        HttpRequest request = HttpContext.Request;
        string hostUrl = $"{request.Scheme}://{request.Host}";
        string? endpoint = Url.Action("GetDefault", "ImageFile");

        return Ok(new { Url = $"{hostUrl}{endpoint}" });
    }

    [Route("get-image/{fileId}")]
    [HttpGet]
    [ProducesResponseType(typeof(FileMetaDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string fileId)
    {
        Database.File? file = await _context.Files.FindAsync(fileId);
        if (file is null)
            return NotFound();

        HttpRequest request = HttpContext.Request;
        string hostUrl = $"{request.Scheme}://{request.Host}";
        string? endpoint = Url.Action("Get", "ImageFile", new { fileId = file.Id });

        return Ok(new FileMetaDto
        {
            Id = file.Id,
            PartitionKey = file.PartitionKey,
            FileName = file.FileName,
            Title = file.Title,
            Description = file.Description,
            Url = $"{hostUrl}{endpoint}"
        });
    }

    [Route("get-image-list")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FileMetaDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetList(int maxRecords = 20, string search = "")
    {
        IQueryable<Database.File> query = _context.Files.Where(f => f.Type == "image");
        if (!string.IsNullOrEmpty(search))
        {
            query.Where(f => f.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(f.Title) && f.FileName.Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        }

        HttpRequest request = HttpContext.Request;
        string hostUrl = $"{request.Scheme}://{request.Host}";

        IEnumerable<FileMetaDto> files = (await query.Take(maxRecords).ToListAsync())
            .Select(s =>
            {
                string? endpoint = Url.Action("Get", "ImageFile", new { fileId = s.Id });

                return new FileMetaDto
                {
                    Id = s.Id,
                    PartitionKey = s.PartitionKey,
                    FileName = s.FileName,
                    Title = s.Title,
                    Description = s.Description,
                    Url = $"{hostUrl}{endpoint}"
                };
            });

        return Ok(files);
    }

    [Route("upload-image")]
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] FileMetaInput metaInput, IFormFile formFile)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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

        Database.File file = new Database.File
        {
            Id = fileName.Replace(extension, string.Empty),
            PartitionKey = partitionKey,
            FileName = fileName,
            Type = "image",
            Title = metaInput.Title,
            Description = metaInput.Description,
        };

        await _context.Files.AddAsync(file);
        await _context.SaveChangesAsync();

        return Ok(new { file.Id });
    }

    [Route("delete-image/{fileId}")]
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