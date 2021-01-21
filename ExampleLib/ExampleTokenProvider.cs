using Microsoft.Extensions.Options;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.AuthToken.Core.DTOs;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.DTOs;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ExampleLib
{
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
}
