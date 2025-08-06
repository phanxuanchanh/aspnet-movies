using MediaSrv.AppCodes;
using MediaSrv.Database;
using Microsoft.AspNetCore.Mvc;

namespace MediaSrv.Controllers
{
    [Route("video")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VideoFileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MovieCdnContext _context;
        private readonly StoragePath _storagePath;
        private readonly FileManager _fileManager;

        public VideoFileController(IWebHostEnvironment webHostEnvironment, MovieCdnContext context, StoragePath storagePath)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _storagePath = storagePath;
            _fileManager = new FileManager(storagePath.VideoStoragePath);
        }

        [Route("default")]
        [HttpGet]
        public IActionResult GetDefault()
        {
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "DefaultFiles/default.mp4");
            return PhysicalFile(filePath, "video/mp4");
        }

        [Route("{fileId}")]
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

        [Route("{fileId}.m3u8")]
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
            if (!provider.TryGetContentType(path, out var contentType))
                contentType = "application/octet-stream";

            return PhysicalFile(path, contentType);
        }

        [Route("{fileId}.m3u8/{subIndex}/{subName}.m3u8")]
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

        [Route("{fileId}.m3u8/{subIndex}/{segment}.ts")]
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
    }
}
