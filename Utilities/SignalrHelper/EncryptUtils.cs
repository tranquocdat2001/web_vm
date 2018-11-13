using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.SignalrHelper
{
    public class EncryptUtils
    {
        private static readonly byte[] PrivateKey = GetPrivateKey();

        private static byte[] GetPrivateKey()
        {
            var configKey = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ChatEncryptKey"])
                ? ConfigurationManager.AppSettings["ChatEncryptKey"]
                : "supper-chat.batdongsan.com.vn";

            return Encoding.ASCII.GetBytes(configKey);
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the original string is null or empty.</exception>
        public static string Encrypt(string originalString)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(PrivateKey, PrivateKey), CryptoStreamMode.Write);

                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(originalString);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
            }
            return null;
        }

        public static string EncryptObject<T>(T original)
        {
            var sOriginal = Convert.ToString(original);
            return Encrypt(sOriginal);
        }

        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the crypted string is null or empty.</exception>
        public static string Decrypt(string cryptedString)
        {
            try
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(PrivateKey, PrivateKey), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0}-{1}", cryptedString, ex));
            }
            return null;
        }
    }
}
