using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.AuthToken.SymmetricEncryption.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace NativoPlusStudio.AuthToken.EncryptTest
{
    [TestClass]
    public class EncryptTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            new AuthTokenServicesBuilder() { Services = new ServiceCollection() }.AddSymmetricEncryption("mykey");
            Assert.IsTrue(true);
        }
    }
}
