using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DeflateCompression.Test
{
    [TestFixture]
    public class DeflateCompressorTests
    {
        [Test]
        public async Task CompressDecompressString()
        {
            //ARRANGE
            var str = "String to compress";
            var compressor = new DeflateCompressor();

            //ACT
            var strArr = Encoding.UTF8.GetBytes(str);
            var source = new MemoryStream(strArr);
            source.Position = 0;
            var compressed = new MemoryStream();
            await compressor.Compress(source, compressed);
            compressed.Position = 0;

            var decompressed = new MemoryStream();
            await compressor.Decompress(compressed, decompressed);
            decompressed.Position = 0;
            var result = new StreamReader(decompressed).ReadToEnd();
            //ASSERT
            Assert.AreEqual(str, result);
        }
    }
}