using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;

namespace NativoPlusStudio.AuthToken.EncryptTest
{
    [TestClass]
    public class EncryptTests : BaseConfiguration
    {
        [TestMethod]
        public void TestMethod1()
        {
            IAuthTokenGenerator authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

            var token = authTokenGenerator?.GetTokenAsync(protectedResource: configuration["Options:ProtectedResourceName"]).GetAwaiter().GetResult();
            Assert.IsTrue(token.EncryptedToken != null);
        }
    }
}
