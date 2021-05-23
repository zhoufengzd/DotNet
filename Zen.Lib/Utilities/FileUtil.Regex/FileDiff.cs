using System.IO;

namespace Zen.Utilities.FileUtil
{
    public sealed class FileComparator
    {
        /// <summary>
        /// Return true if they are the same. 
        /// </summary>
        public static bool Compare(string filePath1, string filePath2)
        {
            if (filePath1 == filePath2)
                return true;

            FileStream fs1 = new FileStream(filePath1, FileMode.Open);
            FileStream fs2 = new FileStream(filePath2, FileMode.Open);
            if (fs1.Length != fs2.Length)
            {
                fs1.Close();
                fs2.Close();

                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // files is reached.
            int file1byte = -1;
            int file2byte = -1;
            while (true)
            {
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();

                if (file1byte == file2byte || file1byte == -1 || file2byte == -1)
                    break;
            }
            fs1.Close();
            fs2.Close();

            return ((file1byte - file2byte) == 0);
        }

    }
}
