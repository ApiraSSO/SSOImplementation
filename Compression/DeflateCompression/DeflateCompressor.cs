using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Compression;

namespace DeflateCompression
{
    public class DeflateCompressor : ICompression
    {
        public async Task Compress(Stream streamToCompress, Stream compressed)
        {
            using (var target = new MemoryStream())
            {
                using (var defalateStream = new DeflateStream(target, CompressionMode.Compress, true))
                {
                    await streamToCompress.CopyToAsync(defalateStream);
                }
                target.Position = 0;
                await target.CopyToAsync(compressed);
            }
        }

        public async Task Decompress(Stream streamToDecompress, Stream decompressed)
        {
            using (var defalateStream = new DeflateStream(streamToDecompress, CompressionMode.Decompress))
            {
                await defalateStream.CopyToAsync(decompressed);
            }
        }
    }
}