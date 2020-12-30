using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.Encryption.Services;
using Serilog;

namespace NativoPlusStudio.AuthToken.SymmetricEncryption.Extensions
{
    public static class AuthTokenSymmetricEncryptionServices
    {
        public static void AddSymmetricEncryption(this AuthTokenServicesBuilder builder, string primaryPrivateKey, string secondaryPrivateKey = null)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger>();
            builder.AddAuthTokenEncryptionImplementation(new SymmetricEncryptionService(new Encryption.Configuration.EncryptionConfiguration() { PrimaryPrivateKey = primaryPrivateKey, SecondaryPrivateKey = secondaryPrivateKey }, logger));
        }
    }
}
