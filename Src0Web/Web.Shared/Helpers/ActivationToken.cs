using System.Security.Cryptography;

namespace Web.Shared.Helpers
{
    public class ActivationToken
    {
        public static string Generate()
        {
            byte[] raw = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(raw);
            }

            return Base64.UrlEncode(raw);

        }
    }
}
