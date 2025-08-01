using System.Collections.Concurrent;

namespace MovieCDN;

public class FileManager
{
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
