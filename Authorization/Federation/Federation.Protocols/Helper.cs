using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kernel.Compression;

namespace Federation.Protocols
{
    internal class Helper
    {
        internal static async Task<string> DeflateEncode(string val, ICompression compression)
        {
            var strArr = Encoding.UTF8.GetBytes(val);
            using (var memoryStream = new MemoryStream(strArr))
            {
                using (var compressed = new MemoryStream())
                {
                    await compression.Compress(memoryStream, compressed);
                    compressed.Position = 0;
                    return Convert.ToBase64String(compressed.GetBuffer(), 0, (int)compressed.Length, Base64FormattingOptions.None);
                }
            }
        }

        internal static async Task<string> DeflateDecompress(string value, ICompression compression)
        {
            var encoded = Convert.FromBase64String(value);
            using (var compressed = new MemoryStream(encoded))
            {
                using (var decompressed = new MemoryStream())
                {
                    await compression.Decompress(compressed, decompressed);
                    decompressed.Position = 0;
                    using (var streamReader = new StreamReader(decompressed, Encoding.UTF8))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
        }

        internal static string UpperCaseUrlEncode(string value)
        {
            var result = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
            {
                if (result[i] == '%')
                {
                    result[++i] = char.ToUpper(result[i]);
                    result[++i] = char.ToUpper(result[i]);
                }
            }

            return result.ToString();
        }
    }
}