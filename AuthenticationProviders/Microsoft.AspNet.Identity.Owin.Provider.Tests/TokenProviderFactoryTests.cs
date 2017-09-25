using AspNet.EntityFramework.IdentityProvider.Models;
using Microsoft.AspNet.Identity.Owin.Provider.Factories;
using Microsoft.Owin.Security.DataProtection;
using NUnit.Framework;

namespace Microsoft.AspNet.Identity.Owin.Provider.Tests
{
    [TestFixture]
    [Ignore("")]
    public class TokenProviderFactoryTests
    {
        [Test]
        public void TokenProviderDelegate_test()
        {
            ////ARRANGE
            //var dataProtector = new DpapiDataProtectionProvider().Create("OwinIdentity");
            ////ACT
            //var del = UserTokenProviderFactory.GetTokenProviderDelegate(typeof(ApplicationUser));
            //var r = del(dataProtector);
            ////ASSERT
            //Assert.IsInstanceOf(typeof(DataProtectorTokenProvider<,>).MakeGenericType(typeof(ApplicationUser), typeof(string)), r);
        }
    }
}