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

    public string GetDirectory(string quality)
    {
        string datePath = DateTime.Now.ToString("yyyy/MM/dd");
        string fullPath = Path.Combine(_basePath, quality, datePath);
        EnsureDirectoryExists(fullPath);
        return fullPath;
    }

    public string SaveFile(string quality, string filename, byte[] content)
    {
        string dir = GetDirectory(quality);
        string fullPath = Path.Combine(dir, filename);
        File.WriteAllBytes(fullPath, content);
        return fullPath;
    }



    public bool DeleteFile(string quality, string filename)
    {
        string dir = GetDirectory(quality);
        string fullPath = Path.Combine(dir, filename);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }
}
