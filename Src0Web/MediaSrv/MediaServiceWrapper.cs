using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MediaSrv
{
    public class MediaServiceWrapper : MediaService
    {
        private static string _token = null;
        private static readonly object _tokenLock = new object();

        private MediaServiceWrapper(HttpClient httpClient)
            :base(httpClient)
        {
        }

        public static MediaServiceWrapper Init(string host, string clientId, string secretKey)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(host)
            };

            MediaServiceWrapper serviceWrapper = new MediaServiceWrapper(httpClient);

            lock (_tokenLock)
            {
                if (_token == null || IsTokenExpired(_token))
                {
                    AuthenticationResult result = serviceWrapper
                        .AuthAsync(new ApiKey { ClientId = clientId, SecretKey = secretKey })
                        .ConfigureAwait(false).GetAwaiter().GetResult();

                    _token = result.Token;
                }

                httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _token);
            }

            return serviceWrapper;
        }

        private static bool IsTokenExpired(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return true;

            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            Claim expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim == null || !long.TryParse(expClaim.Value, out var expUnix))
                return true;

            DateTime expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

            return expDate <= DateTime.UtcNow;
        }
    }
}
