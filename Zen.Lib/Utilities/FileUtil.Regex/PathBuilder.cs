using System.Collections.Generic;
using System.IO;

namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Builds full path from short file name and search directories
    /// </summary>
    public sealed class PathBuilder
    {
        /// <summary>
        /// Return null if can not find this file
        /// </summary>
        public static string BuildFullPath(string fileName, List<string> searchDirs)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            if (FileValidator.IsValid(fileName))
                return fileName;

            if (searchDirs == null || searchDirs.Count < 1)
                return null;

            string fullPath = null;
            foreach (string baseDir in searchDirs)
            {
                fullPath = Path.Combine(baseDir, fileName);
                if (FileValidator.IsValid(fullPath))
                    return fullPath;
            }
            return null;
        }
    }
}
