using Cdn;
using System.Net.Http;

namespace CdnService
{
    public class MediaServiceWrapper : CdnSerivce
    {
        private readonly HttpClient _httpClient;

        private MediaServiceWrapper(HttpClient httpClient)
            :base(httpClient)
        {
        }

        public static MediaServiceWrapper Init(string host, string clientId, string secretKey)
        {
            HttpClient httpClient = new HttpClient();
            return new MediaServiceWrapper(httpClient);
        }
    }
}
