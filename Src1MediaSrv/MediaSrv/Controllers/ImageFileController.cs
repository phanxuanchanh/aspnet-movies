using MediaSrv.AppCodes;
using MediaSrv.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaSrv.Controllers
{
    [Route("image")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ImageFileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MovieCdnContext _context;
        private readonly StoragePath _storagePath;
        private readonly FileManager _fileManager;

        public ImageFileController(IWebHostEnvironment webHostEnvironment, MovieCdnContext context, StoragePath storagePath)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _storagePath = storagePath;
            _fileManager = new FileManager(storagePath.ImageStoragePath);
        }

        [Route("default")]
        [HttpGet]
        public IActionResult GetDefault()
        {
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DefaultFiles/default.png");
            return PhysicalFile(filePath, "image/png");
        }

        [Route("{fileId}")]
        [HttpGet]
        public async Task<IActionResult> Get(string fileId)
        {
            Database.File? file = await _context.Files.FindAsync(fileId);
            if (file is null)
                return NotFound();

            string path = _fileManager.GetFilePath(file.PartitionKey, file.FileName);
            if (!_fileManager.FileExists(path))
                return NotFound();

            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
                contentType = "application/octet-stream";

            return PhysicalFile(path, contentType);
        }
    }
}
