using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AspNet.EntityFramework.IdentityProvider.Managers;
using AspNet.EntityFramework.IdentityProvider.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;

namespace AspNet.EntityFramework.IdentityProvider.Tests
{
    [TestFixture]
    [Ignore("Infrastruture test")]
    public class ManagerTests
    {
        [Test]
        public void GetUser()
        {
            //ARRANGE
            DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan _timestep = TimeSpan.FromMinutes(3.0);
            var r = (long)((DateTime.UtcNow - _unixEpoch).Ticks / _timestep.Ticks);
            var r1 = (long)((DateTime.UtcNow.AddMinutes(6) - _unixEpoch).Ticks / _timestep.Ticks);
            var t = TimeSpan.FromTicks(r+2);
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var passwordValidator = new PasswordValidator();
            //var userValidator = new UserValidator<ApplicationUser>();
            var tokenProvider = new TotpSecurityStampBasedTokenProvider<ApplicationUser, string>();
            var manager = new ApplicationUserManager(store, tokenProvider, passwordValidator, null);
            //ACT
            var token = manager.GenerateUserToken("Any", "f2fc1e53-de75-4ca9-9453-fb6183754562");
            //ASSERT
        }

        [Test]
        public void TokenTest()
        {
            //ARRANGE
            var userId = "f2fc1e53-de75-4ca9-9453-fb6183754562";
            //DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //TimeSpan _timestep = TimeSpan.FromMinutes(3.0);
            //var r = (long)((DateTime.UtcNow - _unixEpoch).Ticks / _timestep.Ticks);
            //var r1 = (long)((DateTime.UtcNow.AddMinutes(6) - _unixEpoch).Ticks / _timestep.Ticks);
            //var t = TimeSpan.FromTicks(r + 2);
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var passwordValidator = new PasswordValidator();
            //var userValidator = new UserValidator<ApplicationUser>();
            var tokenProvider = new TotpSecurityStampBasedTokenProvider<ApplicationUser, string>();
            var manager = new ApplicationUserManager(store, tokenProvider, passwordValidator, null);
            var securityToken = new SecurityToken(Encoding.Unicode.GetBytes(manager.GetSecurityStamp(userId)));
            var utcNow = new DateTime(2017, 08, 19, 11, 57, 00, DateTimeKind.Utc);
            //var utcRondom = new DateTime(2017, 08, 19, 12, 03, 00, DateTimeKind.Utc);
            var code = Rfc6238AuthenticationService.GenerateCode(securityToken, utcNow);

            //ACT
            for (var i = 0; i < 600; i++)
            {
                var timeSimulated = utcNow.AddSeconds(i);
                var isValid = Rfc6238AuthenticationService.ValidateCode(securityToken, code, timeSimulated);
                if(!isValid)
                {
                    
                }
            }
            //var now = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow);
            //var rondom = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcRondom);
            //var after_3_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(3));
            //var after_6_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(6));
            //var after_10_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(10));

            //var after_T3_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(-3));
            //var after_T6_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(-6));
            //var after_T10_min = Rfc6238AuthenticationService.ValidateCode(securityToken, code, utcNow.AddMinutes(-10));
            ////ASSERT
            //Assert.IsTrue(now);
            //Assert.IsTrue(after_3_min);
            //Assert.IsTrue(after_6_min);
            //Assert.IsFalse(after_10_min);
            //Assert.IsTrue(after_T3_min);
            //Assert.IsTrue(after_T6_min);
            //Assert.IsFalse(after_T10_min);
        }

    }
    internal static class Rfc6238AuthenticationService
    {
        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3.0);
        private static readonly Encoding _encoding = (Encoding)new UTF8Encoding(false, true);

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            byte[] hash = hashAlgorithm.ComputeHash(Rfc6238AuthenticationService.ApplyModifier(bytes, modifier));
            int index = (int)hash[hash.Length - 1] & 15;
            return (((int)hash[index] & (int)sbyte.MaxValue) << 24 | ((int)hash[index + 1] & (int)byte.MaxValue) << 16 | ((int)hash[index + 2] & (int)byte.MaxValue) << 8 | (int)hash[index + 3] & (int)byte.MaxValue) % 1000000;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (string.IsNullOrEmpty(modifier))
                return input;
            byte[] bytes = Rfc6238AuthenticationService._encoding.GetBytes(modifier);
            byte[] numArray = new byte[checked(input.Length + bytes.Length)];
            Buffer.BlockCopy((Array)input, 0, (Array)numArray, 0, input.Length);
            Buffer.BlockCopy((Array)bytes, 0, (Array)numArray, input.Length, bytes.Length);
            return numArray;
        }

        private static ulong GetCurrentTimeStepNumber(DateTime time)
        {
            return (ulong)((time - Rfc6238AuthenticationService._unixEpoch).Ticks / Rfc6238AuthenticationService._timestep.Ticks);
        }

        public static int GenerateCode(SecurityToken securityToken, DateTime time, string modifier = null)
        {
            if (securityToken == null)
                throw new ArgumentNullException("securityToken");
            ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber(time);
            using (HMACSHA1 hmacshA1 = new HMACSHA1(securityToken.GetDataNoClone()))
                return Rfc6238AuthenticationService.ComputeTotp((HashAlgorithm)hmacshA1, currentTimeStepNumber, modifier);
        }

        public static bool ValidateCode(SecurityToken securityToken, int code, DateTime time, string modifier = null)
        {
            if (securityToken == null)
                throw new ArgumentNullException("securityToken");
            ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber(time);
            using (HMACSHA1 hmacshA1 = new HMACSHA1(securityToken.GetDataNoClone()))
            {
                for (int index = -2; index <= 2; ++index)
                {
                    if (Rfc6238AuthenticationService.ComputeTotp((HashAlgorithm)hmacshA1, currentTimeStepNumber + (ulong)index, modifier) == code)
                        return true;
                }
            }
            return false;
        }
    }
    internal sealed class SecurityToken
    {
        private readonly byte[] _data;

        public SecurityToken(byte[] data)
        {
            this._data = (byte[])data.Clone();
        }

        internal byte[] GetDataNoClone()
        {
            return this._data;
        }
    }
}
