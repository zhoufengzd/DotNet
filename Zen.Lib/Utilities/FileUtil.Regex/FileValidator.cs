using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Validate file path, access permission, etc
    /// </summary>
    public class FileValidator
    {
        public static bool IsValid(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool IsValid(string filePath, long length)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return (fileInfo.Exists && fileInfo.Length >= length);
        }

        public static bool IsValid(string filePath, FileAccess access)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, access))
                {
                    fs.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(string filePath, FileAccess access, long length)
        {
            if (!IsValid(filePath, length))
                return false;

            return IsValid(filePath, access);
        }
    }
}
