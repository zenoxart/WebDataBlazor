using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Utilities
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class EncryptionHandler
    {
        private readonly byte[] _key; // 256-bit key provided by the user

        public EncryptionHandler(string key)
        {
            if (key == null) throw new ArgumentNullException();

            if (key.Length == 0) throw new ArgumentException();

            if (key.Length < 32)
            {
                key = string.Concat(Enumerable.Repeat(key, 32 / key.Length + 1)).Substring(0, 32);
            }

            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }



            if (key.Length != 32)
            {
                throw new ArgumentException("Key must be 32 characters long, which corresponds to 256 bits.");
            }
            _key = System.Text.Encoding.UTF8.GetBytes(key);
        }



        public string Encrypt(string plainText, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = _key;

            var encryptor = aes.CreateEncryptor(aes.Key, iv);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using var sw = new StreamWriter(cs);
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public byte[] GenerateIV()
        {

            using var aes = Aes.Create();
            aes.GenerateIV();

            return aes.IV;

        }
    }

}
