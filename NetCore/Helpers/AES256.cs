using System;
using System.Security.Cryptography;
using System.Text;

namespace NetCore.Helpers
{
    public class AES256
    {
        /// <summary>
        /// default key
        /// </summary>
        private const string cryptoString = "Key";

        /// <summary>
        /// string encrypt
        /// </summary>
        /// <param name="plainText">unencrypt string</param>
        /// <param name="cryptoKey">encrypt key</param>
        /// <returns>encrypt result</returns>
        public static string EncryptString(string plainText, string cryptoKey = cryptoString)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            byte[] key = default(byte[]);
            byte[] iv = default(byte[]);
            byte[] cipherBytes;

            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            }

            using (var sha384 = new SHA384CryptoServiceProvider())
            {
                iv = sha384.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            }

            var source = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[iv.Length - 32];

                using (var encryptor = aes.CreateEncryptor())
                {
                    cipherBytes = encryptor.TransformFinalBlock(source, 0, source.Length);
                }

            }

            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// string decrypt
        /// </summary>
        /// <param name="cipherText">encrypted Base64String</param>
        /// <param name="cryptoKey">encrypt key</param>
        /// <returns>decrypted result</returns>
        public static string DecryptString(string cipherText, string cryptoKey = cryptoString)
        {
            try
            {

                byte[] key = default(byte[]);
                byte[] iv = default(byte[]);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                }

                using (var sha384 = new SHA384CryptoServiceProvider())
                {
                    iv = sha384.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                }

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = new byte[iv.Length - 32];

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length));
                    }
                }

            }
            catch (Exception ex)
            {
                return cipherText;
            }
        }

        /// <summary>
        /// string encrypt
        /// </summary>
        /// <param name="source">unencrypt string</param>
        /// <param name="cryptoKey">encrypt key</param>
        /// <param name="salt">encrypt salt</param>
        /// <returns>encrypt result</returns>
        public static string EncryptStringWithSalt(string source, string cryptoKey, string salt)
        {
            if (string.IsNullOrEmpty(source)) return source;

            using (var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                var encryptedBytes = AES256.EncryptStreamWithSalt(sourceStream, cryptoKey, salt);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// string decrypt
        /// </summary>
        /// <param name="source">encrypted Base64String</param>
        /// <param name="cryptoKey">encrypt key</param>
        /// <param name="salt">encrypt salt</param>
        /// <returns>encrypt result</returns>
        public static string DecryptStringWithSalt(string source, string cryptoKey, string salt)
        {
            try
            {
                using (var sourceStream = new MemoryStream(Convert.FromBase64String(source)))
                {
                    var decryptedBytes = AES256.DecryptStreamWithSalt(sourceStream, cryptoKey, salt);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception)
            {
                return source;
            }
        }

        /// <summary>
        /// data encrypt + Salt
        /// </summary>
        /// <param name="stream">unencrypted source</param>
        /// <param name="cryptoKey">encrypt key</param>
        /// <param name="salt">encrypt salt</param>
        /// <returns>encrypt result</returns>
        public static byte[] EncryptStreamWithSalt(Stream stream, string cryptoKey, string salt)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(cryptoKey, Encoding.UTF8.GetBytes(salt), 100000))
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var encryptedStream = new MemoryStream())
                using (var cstream = new CryptoStream(stream, encryptor, CryptoStreamMode.Read))
                {
                    int length = 0;

                    var buffer = new byte[1024 * 256];
                    while ((length = cstream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        encryptedStream.Write(buffer, 0, length);
                    }

                    return encryptedStream.ToArray();
                }
            };
        }

        /// <summary>
        /// data decrypt + Salt
        /// </summary>
        /// <param name="stream">encrypted source</param>
        /// <param name="cryptoKey">decrypt key</param>
        /// <param name="salt">decrypt salt</param>
        /// <returns>decrypt result</returns>
        public static byte[] DecryptStreamWithSalt(Stream stream, string cryptoKey, string salt)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(cryptoKey, Encoding.UTF8.GetBytes(salt), 100000))
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (var decryptedStream = new MemoryStream())
                using (var decstream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                {
                    int length = 0;

                    var buffer = new byte[1024 * 256];
                    while ((length = decstream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        decryptedStream.Write(buffer, 0, length);
                    }

                    return decryptedStream.ToArray();
                }
            };
        }
    }
}

