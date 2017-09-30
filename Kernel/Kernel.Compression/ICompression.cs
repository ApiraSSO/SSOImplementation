using System.IO;
using System.Threading.Tasks;

namespace Kernel.Compression
{
    public interface ICompression
    {
        Task Compress(Stream streamToCompress, Stream compressed);
        Task Decompress(Stream streamToDecompress, Stream decompressed);
    }
}