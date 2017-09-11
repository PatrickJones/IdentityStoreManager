using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NuUserStoreManager
{
    internal static class DataEncryption
    {
        /// <summary>
        /// Encrypts a byte[] using AES algorithm.
        /// </summary>
        /// <param name="bytesToBeEncrypted">byte[] to be encrypted</param>
        /// <returns>Encrypted byte[]</returns>
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted)
        {
            byte[] encryptedBytes = null;

            // Set salt 
            byte[] saltBytes = new byte[16];

            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(new byte[16], saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        /// <summary>
        /// Decrypts a byte[] using AES algorithm.
        /// </summary>
        /// <param name="bytesToBeDecrypted">byte[] to be dncrypted</param>
        /// <returns>Decrypted byte[]</returns>
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted)
        {
            byte[] decryptedBytes = null;
            // Set salt
            byte[] saltBytes = new byte[16];

            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(new byte[16], saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        /// <summary>
        /// Encrypts text string.
        /// </summary>
        /// <param name="text">String data to encrypt</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string text)
        {
            //// ensure proper Base64
            //if (text.Length % 4 > 0)
            //    text = text.PadRight(text.Length + 4 - text.Length % 4, '=');

            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;

            // Hash the password with SHA256
            var salt = SHA256.Create().ComputeHash(new byte[16]);

            // Getting the salt size
            int saltSize = GetSaltSize(salt);
            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted);

            return Convert.ToBase64String(encryptedBytes);
        }
        /// <summary>
        /// Decrypts text string
        /// </summary>
        /// <param name="decryptedText"></param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string decryptedText)
        {
            // ensure proper Base64
            if (decryptedText.Length % 4 > 0)
                decryptedText = decryptedText.PadRight(decryptedText.Length + 4 - decryptedText.Length % 4, '=');

            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);

            // Hash the password with SHA256
            var salt = SHA256.Create().ComputeHash(new byte[16]);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted);

            // Getting the size of salt
            int saltSize = GetSaltSize(salt);

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }
        /// <summary>
        /// Gets the size of the salted hash.
        /// </summary>
        /// <param name="salt">Hashed array of bytes</param>
        /// <returns>Int32 of hash size </returns>
        public static int GetSaltSize(byte[] salt)
        {
            var key = new Rfc2898DeriveBytes(salt, salt, 1000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize += intc;
            }

            return saltSize;
        }
        /// <summary>
        /// Fills an array of bytes with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="length">byte[] length</param>
        /// <returns>byte[] filled random sequecnce of values</returns>
        public static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }
    }
}
