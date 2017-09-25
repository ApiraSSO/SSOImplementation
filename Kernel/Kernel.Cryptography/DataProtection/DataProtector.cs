using System.Security.Cryptography;

namespace Kernel.Cryptography.DataProtection
{
    public sealed class DpapiDataProtector : IDataProtector
    {
        private readonly System.Security.Cryptography.DpapiDataProtector _protector;
        
        public DpapiDataProtector(string appName, string primaryPurpose, params string[] specificPurpose)
        {
            this._protector = new System.Security.Cryptography.DpapiDataProtector(appName, primaryPurpose, specificPurpose)
            {
                Scope = DataProtectionScope.CurrentUser
            };
        }
        public byte[] Protect(byte[] userData)
        {
            return this._protector.Protect(userData);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return this._protector.Unprotect(protectedData);
        }
    }
}