using System.Security.Cryptography;

namespace Kernel.Cryptography.DataProtection
{
    public class RSADataProtection
    {
        public static byte[] SignDataSHA1(RSA provider, byte[] data)
        {
            using (provider)
            {
                return provider.SignData(data, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
        }

        public static bool VerifyDataSHA1Signed(RSA provider, byte[] data, byte[]signedData)
        {
            using (provider)
            {
                return provider.VerifyData(data, signedData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
        }
    }
}