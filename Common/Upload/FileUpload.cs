using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Common.Upload
{
    public class FileUpload
    {
        public static string FilePath = "~/FileUpload";
        public static string ImageFilePath = string.Format("{0}/Images", FilePath);
        public static string VideoFilePath = string.Format("{0}/Videos", FilePath);

        public enum UploadState { Success, Failed, Failed_AlreadyExist, Failed_InvalidFile };
        public FileUpload()
        {

        }

        public bool CheckExists(string filePath)
        {
            if (File.Exists(filePath))
                return true;
            return false;
        }

        public UploadState UploadImage(HttpPostedFile file, ref string filePath)
        {
            if (file == null)
                throw new Exception("@'file' must be not null");
            if (!IsValidImage(file))
                return UploadState.Failed_InvalidFile;
            string path = string.Format("{0}/{1}", GenerateImageFolder(), file.FileName);
            filePath = path.Replace(ImageFilePath, "");
            string fileMapPath = HttpContext.Current.Server.MapPath(path);
            if (CheckExists(fileMapPath))
                return UploadState.Failed_AlreadyExist;

            file.SaveAs(fileMapPath);
            return UploadState.Success;
        }

        public UploadState UploadVideo(HttpPostedFile file, ref string filePath)
        {
            if (file == null)
                throw new Exception("@'file' must be not null");
            if (!IsValidVideo(file))
                return UploadState.Failed_InvalidFile;
            string path = string.Format("{0}/{1}", GenerateVideoFolder(), file.FileName);
            filePath = path.Replace(VideoFilePath, "");
            string fileMapPath = HttpContext.Current.Server.MapPath(path);
            if (CheckExists(fileMapPath))
                return UploadState.Failed_AlreadyExist;

            file.SaveAs(fileMapPath);
            return UploadState.Success;
        }

        public string GenerateImageFolder()
        {
            string path1 = string.Format("{0}/{1}", ImageFilePath, DateTime.Now.Year);
            string mapPath = HttpContext.Current.Server.MapPath(path1);
            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }
            string path2 = string.Format("{0}/{1}", path1, DateTime.Now.Month);
            mapPath = HttpContext.Current.Server.MapPath(path2);
            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }
            return path2;
        }

        public string GenerateVideoFolder()
        {
            string path1 = string.Format("{0}/{1}", VideoFilePath, DateTime.Now.Year);
            string mapPath = HttpContext.Current.Server.MapPath(path1);
            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }
            string path2 = string.Format("{0}/{1}", path1, DateTime.Now.Month);
            mapPath = HttpContext.Current.Server.MapPath(path2);
            if (!Directory.Exists(mapPath))
            {
                Directory.CreateDirectory(mapPath);
            }
            return path2;
        }

        public bool IsValidImage(HttpPostedFile file)
        {
            List<string> contentType = new List<string>();
            contentType.Add("image/jpeg");
            contentType.Add("image/png");
            contentType.Add("image/gif");
            string mime = file.ContentType;

            if (contentType.Contains(mime))
                return true;
            return false;
        }

        public bool IsValidVideo(HttpPostedFile file)
        {
            List<string> contentType = new List<string>();
            contentType.Add("video/mp4");
            contentType.Add("video/mpeg");
            contentType.Add("video/ogg");
            contentType.Add("video/webm");
            contentType.Add("video/3gpp");
            string mime = file.ContentType;

            if (contentType.Contains(mime))
                return true;
            return false;
        }

        public bool RemoveImage(string filePath)
        {
            string fullFilePath = string.Format("{0}/{1}", ImageFilePath, filePath);
            string fullFileMapPath = HttpContext.Current.Server.MapPath(fullFilePath);
            if (File.Exists(fullFileMapPath))
            {
                File.Delete(fullFileMapPath);
                return true;
            }
            return false;
        }

        public bool RemoveVideo(string filePath)
        {
            string fullFilePath = string.Format("{0}/{1}", VideoFilePath, filePath);
            string fullFileMapPath = HttpContext.Current.Server.MapPath(fullFilePath);
            if (File.Exists(fullFileMapPath))
            {
                File.Delete(fullFileMapPath);
                return true;
            }
            return false;
        }
    }
}
