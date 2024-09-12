using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WebData.Objects.PageContext.Utilities
{
    /// <summary>
    /// Stellt eine Verschlüsselungs-Manager für AES-256
    /// </summary>
    public class EncryptionHandler
    {
        /// <summary>
        /// Definiert den Verschlüsselungs-Key (256-bit)
        /// </summary>
        private readonly byte[] _key;

        public EncryptionHandler(string key)
        {

            if (key == null || key.Length == 0) throw new ArgumentException();

            key = key.Length switch
            {
                < 32 => string.Concat(Enumerable.Repeat(key, 32 / key.Length + 1)).Substring(0, 32),
                > 32 => key.Substring(0, 32),
                _ => key
            };

            if (key.Length != 32)
            {
                throw new ArgumentException("Key must be 32 characters long, which corresponds to 256 bits.");
            }

            _key = System.Text.Encoding.UTF8.GetBytes(key);
        }


        /// <summary>
        /// Verschlüsselt den Text mithilfe der Salt-IV-Werte
        /// </summary>
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

        /// <summary>
        /// Generiert neue Salt-IV-Werte
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateIV()
        {
            using var aes = Aes.Create();
            aes.GenerateIV();

            return aes.IV;
        }

        /// <summary>
        /// Überprüft die Passwort-Stärke
        /// </summary>
        public static IEnumerable<string> PasswordStrength(string pw)
        {
            yield return pw switch
            {
                _ when string.IsNullOrWhiteSpace(pw)    => "Password is required!",
                _ when pw.Length < 16                   => "Password must be at least of length 16",
                _ when !Regex.IsMatch(pw, @"[A-Z]")     => "Password must contain at least one capital letter",
                _ when !Regex.IsMatch(pw, @"[a-z]")     => "Password must contain at least one lowercase letter",
                _ when !Regex.IsMatch(pw, @"[0-9]")     => "Password must contain at least one digit",
                _ => string.Empty // No error
            };

        }
    }

}
