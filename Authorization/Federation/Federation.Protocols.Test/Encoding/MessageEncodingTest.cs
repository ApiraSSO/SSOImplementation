using System.Threading.Tasks;
using DeflateCompression;
using Federation.Protocols.Endocing;
using NUnit.Framework;

namespace Federation.Protocols.Test.Encoding
{
    [TestFixture]
    internal class MessageEncodingTest
    {
        [Test]
        public async Task MessageEncoding_test()
        {
            //ARRANGE
            var source = "Text to encode";
            var compressor = new DeflateCompressor();
            var encoder = new MessageEncoding(compressor);
            var expected = await Helper.DeflateEncode(source.ToString(), compressor);
            //ACT
            var encoded = await encoder.EncodeMessage(source);
            
            //ASSERT
            Assert.AreEqual(expected, encoded);
        }

        [Test]
        public async Task MessageDecoding_test()
        {
            //ARRANGE
            var source = "Text to encode";
            var compressor = new DeflateCompressor();
            var encoder = new MessageEncoding(compressor);
            var encoded = await Helper.DeflateEncode(source.ToString(), compressor);
            //ACT
            var decoded = await encoder.DecodeMessage(encoded);

            //ASSERT
            Assert.AreEqual(decoded, source);
        }

        [Test]
        public async Task MessageEncodingDecodingTest_test()
        {
            //ARRANGE
            var source = "Text to encode";
            var compressor = new DeflateCompressor();
            var encoder = new MessageEncoding(compressor);
            //ACT
            var encoded = await encoder.EncodeMessage(source);
            var decoded = await encoder.DecodeMessage(encoded);
            //ASSERT
            Assert.AreEqual(decoded, source);
        }
    }
}
