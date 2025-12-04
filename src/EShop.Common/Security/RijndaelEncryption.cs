using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace EShop.Common.Security
{
    public class RijndaelEncryption : IRijndaelEncryption, IDisposable
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private bool _disposed;
        public RijndaelEncryption(IConfiguration configuration)
        {
            var key = configuration["Rijndael:Key"];
            var iv = configuration["Rijndael:IV"];

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Rijndael:Key is not configured");

            if (string.IsNullOrEmpty(iv))
                throw new ArgumentException("Rijndael:IV is not configured");

            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);

            ValidateKeySize();
        }
        private void ValidateKeySize()
        {
            if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
                throw new ArgumentException($"Invalid key size. Expected 16, 24, or 32 bytes. Got {_key.Length} bytes.");
        }
        public string Encryption(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Value cannot be null or empty", nameof(plainText));
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new(cs, Encoding.UTF8))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Encryption failed", ex);
            }
        }
        public string Decryption(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Value cannot be null or empty", nameof(cipherText));

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream ms = new(buffer);
                using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new (cs, Encoding.UTF8);

                return sr.ReadToEnd();
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid base64 string", nameof(cipherText), ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Decryption failed", ex);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Clear sensitive data from memory
                    if (_key != null)
                        Array.Clear(_key, 0, _key.Length);
                    if (_iv != null)
                        Array.Clear(_iv, 0, _iv.Length);
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
