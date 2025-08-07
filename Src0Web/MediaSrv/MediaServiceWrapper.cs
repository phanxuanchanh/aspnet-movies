using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MediaSrv
{
    public class MediaServiceWrapper : MediaService, IDisposable
    {
        private static string _token = null;
        private bool disposedValue;
        private static readonly object _tokenLock = new object();
        private readonly HttpClient _httpClient;

        private MediaServiceWrapper(HttpClient httpClient)
            :base(httpClient)
        {
            _httpClient = httpClient;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _httpClient.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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
