using System.IO;
using System.Linq;

namespace Federation.Protocols.Test.Mock
{
    internal class FileHelper
    {
        internal static string GetLastesFile(string path)
        {
            var directory = new DirectoryInfo(path);
            var lastFile = directory.GetFiles().OrderByDescending(f => f.CreationTimeUtc)
                .First();
            return lastFile.FullName;
        }
    }
}