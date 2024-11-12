﻿namespace KhangNghi_BE.Utils
{
    public static class FileUtils
    {
        public static async Task<bool> UploadFileAsync(IFormFile file, string path)
        {
            if(!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, file.FileName);

            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
        public static async Task<bool> UploadFileAsync(IFormFile file, string path, string fileName)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, fileName);

            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }


        public static bool DeleteFile(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}