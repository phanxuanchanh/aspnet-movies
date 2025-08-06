namespace MediaSrv.AppCodes;

public class FileManager
{
    public static string GenerateFileName(string extension = "")
    {
        var fileName = $"{Guid.NewGuid():N}_{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        return string.IsNullOrEmpty(extension) ? fileName : $"{fileName}{extension}";
    }

    public static string GenerateParitionKey()
    {
        return DateTime.UtcNow.ToString("yyyyMMdd");
    }


    private readonly string _basePath;

    public FileManager(string basePath)
    {
        _basePath = basePath;
        EnsureDirectoryExists(_basePath);
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public string GetFilePath(string partitionKey, string filename)
    {
        string folderPath = Path.Combine(_basePath, partitionKey);
        EnsureDirectoryExists(folderPath);

        return Path.Combine(folderPath, filename);
    }

    public bool FileExists(string filePath)
    {
        string fullFilePath = Path.Combine(_basePath, filePath);
        return File.Exists(fullFilePath);
    }

    public string GetFolderPath(string folderName)
    {
        string folderPath = Path.Combine(_basePath, folderName);
        EnsureDirectoryExists(folderPath);
        return folderPath;
    }
}
