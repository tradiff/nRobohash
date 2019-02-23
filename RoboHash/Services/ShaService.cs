using System.Security.Cryptography;
using System.Text;

namespace RoboHash.Services
{
    public class ShaService
    {
        public string Sha512HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA512.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);
 
            return HexStringFromBytes(hashBytes);
        }

        private string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
}
}
