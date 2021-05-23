
namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Get hash value for any given doc.
    /// </summary>
    public class DocHasher
    {
        public DocHasher(string fileName, long fileSize)
        {
            _fileName = fileName;
            _fileSize = fileSize;
        }

        public byte[] GetHashValue()
        {
            return null;
        }

        private string _fileName;
        private long _fileSize;
    }
}
