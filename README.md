# NativoPlusStudio.AuthToken.SymmetricEncryption

NativoPlusStudio.AuthToken.SymmetricEncryption is part of the NativoPlusStudio.AuthToken set of libraries that can be used to encrypt and decrypt the auth token.

### Usage

First create your own implementation and use the IEncryption interface:

```csharp
public class ExampleTokenProvider : BaseTokenProvider<BaseOptions>, IAuthTokenProvider
{
    public ExampleTokenProvider(IEncryption symmetricEncryption = null,
        IAuthTokenCacheService tokenCacheService = null,
        ILogger logger = null,
        IOptions<BaseOptions> options = null)
        :base(symmetricEncryption, tokenCacheService, logger, options)
    {

    }

    public async Task<ITokenResponse> GetTokenAsync()
    {
        var token = "thisismytoken";
        var tokenResponse = new TokenResponse
        {
            Token = token,
            TokenType = "Bearer",
            EncryptedToken = _options.IncludeEncryptedTokenInResponse && _symmetricEncryption != null
                    ? _symmetricEncryption.Encrypt(token)
                    : null,
            ExpiryDateUtc = DateTime.MaxValue
        };
        //add optional code to get the token from a cache
        //add optional code to encrypt the token
        return tokenResponse;
    }
}
```
Next to be able to use the extension method called AddSymmetricEncryption you will need to extend the class AuthTokenBuilder. Here's an example:
```csharp
public static class ServicesExtension
{
    public static IServiceCollection AddExampleAuthTokenProvider(this IServiceCollection services,
        string protectedResourceName,
        //add an a delagate where you pass the AuthTokenServicesBuilder from which you can extend using the AddSymmetricEncryption extension method.
        Action<BaseOptions, AuthTokenServicesBuilder> actions
        )
    {
        var options = new BaseOptions();
        var servicesBuilder = new AuthTokenServicesBuilder() { Services = services };

        actions.Invoke(options, servicesBuilder);

        services.AddTokenProviderHelper(protectedResourceName, () =>
        {
            services.Configure<BaseOptions>(f =>
            {
                f.IncludeEncryptedTokenInResponse = options.IncludeEncryptedTokenInResponse;
            });

            services
            .AddSingleton<IAuthTokenProvider, ExampleTokenProvider>();

            services.AddTransient(implementationFactory => servicesBuilder.EncryptionService);
            services.AddTransient(implementationFactory => servicesBuilder.TokenCacheService);
        });

        return services;
    }
}
```
Next you can register it in a Console app or api using the AddTokenProviderHelper extension method:

```csharp
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
                //extend builder by invoking AddSymmetricEncryption()
                builder.AddSymmetricEncryption(configuration["Options:PrivateKey"]);
            }
        );

        serviceProvider = services.BuildServiceProvider();

        authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

        var token = authTokenGenerator.GetTokenAsync(protectedResource: configuration["Options:ProtectedResourceName"]).GetAwaiter().GetResult();
            
        Console.WriteLine(JsonConvert.SerializeObject(token));
    }
}
```
Visit the following repositories for examples on how to use other auth token nuget packages

https://github.com/nativoplus/NativoPlusStudio.AuthToken.Core
https://github.com/nativoplus/NativoPlusStudio.AuthToken.SqlServerCaching
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Ficoso
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Fis
