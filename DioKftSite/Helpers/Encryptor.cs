using System;
using System.Security.Cryptography;
using System.Text;

namespace DioKftSite.Helpers
{
    public static class Encryptor
    {
        public static string EncrypteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException($"Cannot be null {nameof(value)}.");
            }

            using (SHA256Managed sha = new SHA256Managed())
            {
                var encodedValue = Encoding.UTF8.GetBytes(value);
                sha.ComputeHash(encodedValue);
                
                return Convert.ToBase64String(sha.Hash);
            }
        }
    }
}