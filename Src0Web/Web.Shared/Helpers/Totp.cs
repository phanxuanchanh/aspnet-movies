using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
    public class Totp
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly byte[] _secretKey;
        private readonly int _digits;
        private readonly int _step; // seconds
        private readonly HashAlgorithmName _hashAlgorithm;

        // Constructor dùng secret base32
        public Totp(string base32Secret, int digits = 6, int step = 30, HashAlgorithmName? hashAlgorithm = null)
        {
            _secretKey = Base32Decode(base32Secret);
            _digits = digits;
            _step = step;
            _hashAlgorithm = hashAlgorithm ?? HashAlgorithmName.SHA1;
        }

        // Tạo secret base32 ngẫu nhiên, mặc định 160-bit (20 bytes)
        public static string GenerateSecret(int bytesLength = 20)
        {
            byte[] bytes = new byte[bytesLength];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Base32Encode(bytes);
            }
        }

        public string GenerateTotp(DateTime utcNow)
        {
            long counter = GetCurrentCounter(utcNow);
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);

            using (HMAC hmac = HMAC.Create("HMAC" + _hashAlgorithm.Name))
            {
                hmac.Key = _secretKey;
                byte[] hash = hmac.ComputeHash(counterBytes);

                int offset = hash[hash.Length - 1] & 0x0F;
                int binaryCode = ((hash[offset] & 0x7F) << 24)
                               | ((hash[offset + 1] & 0xFF) << 16)
                               | ((hash[offset + 2] & 0xFF) << 8)
                               | (hash[offset + 3] & 0xFF);


                int otp = binaryCode % (int)Math.Pow(10, _digits);
                return otp.ToString(new string('0', _digits));
            }
        }

        public bool VerifyTotp(string code, DateTime utcNow, int allowedDriftSteps = 1)
        {
            for (int i = -allowedDriftSteps; i <= allowedDriftSteps; i++)
            {
                string generated = GenerateTotp(utcNow.AddSeconds(i * _step));
                if (generated == code)
                    return true;
            }
            return false;
        }

        private long GetCurrentCounter(DateTime utcNow)
        {
            var unixTime = (long)(utcNow - UnixEpoch).TotalSeconds;
            return unixTime / _step;
        }

        // Base32 Decode (RFC 4648)
        private static byte[] Base32Decode(string base32)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            base32 = base32.TrimEnd('=').ToUpperInvariant();
            int byteCount = base32.Length * 5 / 8;
            byte[] bytes = new byte[byteCount];

            int bitBuffer = 0, bitsLeft = 0, byteIndex = 0;

            foreach (char c in base32)
            {
                int val = alphabet.IndexOf(c);
                if (val < 0) throw new FormatException("Invalid base32 character.");

                bitBuffer = (bitBuffer << 5) | val;
                bitsLeft += 5;

                if (bitsLeft >= 8)
                {
                    bitsLeft -= 8;
                    bytes[byteIndex++] = (byte)(bitBuffer >> bitsLeft);
                    bitBuffer &= (1 << bitsLeft) - 1;
                }
            }
            return bytes;
        }

        // Base32 Encode (RFC 4648)
        private static string Base32Encode(byte[] data)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            StringBuilder result = new StringBuilder();
            int buffer = data[0];
            int next = 1;
            int bitsLeft = 8;

            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < 5)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xFF;
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = 5 - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                int index = (buffer >> (bitsLeft - 5)) & 0x1F;
                bitsLeft -= 5;
                result.Append(alphabet[index]);
            }

            return result.ToString();
        }
    }
}
