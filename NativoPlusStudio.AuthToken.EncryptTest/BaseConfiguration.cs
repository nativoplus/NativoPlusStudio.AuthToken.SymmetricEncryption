using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using ExampleLib;
using NativoPlusStudio.AuthToken.SymmetricEncryption.Extensions;

namespace NativoPlusStudio.AuthToken.EncryptTest
{
    public abstract class BaseConfiguration
    {
        public static IServiceProvider serviceProvider;
        public static IConfiguration configuration;
        public BaseConfiguration()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var services = new ServiceCollection();
            services.AddExampleAuthTokenProvider(
                protectedResourceName: configuration["Options:ProtectedResourceName"],
                (options, builder) =>
                {
                    options.IncludeEncryptedTokenInResponse = true;
                    builder.AddSymmetricEncryption(configuration["Options:PrivateKey"]);
                }
            );

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
