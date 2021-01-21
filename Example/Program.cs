using Microsoft.Extensions.DependencyInjection;
using System;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.SymmetricEncryption.Extensions;
using Newtonsoft.Json;
using ExampleLib;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Example
{
    class Program
    {
        public static IServiceProvider serviceProvider;
        public static IAuthTokenGenerator authTokenGenerator;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
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

            authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

            var token = authTokenGenerator.GetTokenAsync(protectedResource: configuration["Options:ProtectedResourceName"]).GetAwaiter().GetResult();
            
            Console.WriteLine(JsonConvert.SerializeObject(token));
        }
    }
}
