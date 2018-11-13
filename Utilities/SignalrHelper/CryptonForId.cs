
namespace Utilities.SignalrHelper
{
    public class CryptonForId
    {
        private const string EncryptKey = "encrypt_key";
        private static string CryptonKey
        {
            get
            {
                return AppSettings.Instance.GetString(EncryptKey, "financial");
            }
        }
        public static int DecryptIdToInt(string encryptId)
        {
            if (string.IsNullOrEmpty(encryptId)) return 0;
            string decrypt = Decrypt(encryptId);
            return decrypt.ToInt();
        }
        public static long DecryptIdToLong(string encryptId)
        {
            if (string.IsNullOrEmpty(encryptId)) return 0L;
            string decrypt = Decrypt(encryptId);
            return decrypt.ToLong();
        }
        public static string EncryptId(int id)
        {
            //string encrypt = Crypton.EncryptForHTML(Crypton.EncryptByKey(id.ToString(), CryptonKey));
            string encrypt = Crypton.EncryptForHTMLByKey(id.ToString(), CryptonKey);
            //return Crypton.EncryptByKey(id.ToString(), CryptonKey);
            return encrypt;
        }
        public static string EncryptId(long id)
        {
            //string encrypt = Crypton.EncryptForHTML(Crypton.EncryptByKey(id.ToString(), CryptonKey));
            string encrypt = Crypton.EncryptForHTMLByKey(id.ToString(), CryptonKey);
            //return Crypton.EncryptByKey(id.ToString(), CryptonKey);
            return encrypt;
        }

        public static string Encrypt(string input)
        {
            string encrypt = Crypton.EncryptForHTMLByKey(input.ToString(), CryptonKey);
            return encrypt;
        }
        public static string Decrypt(string encrypt)
        {
            if (string.IsNullOrEmpty(encrypt)) return string.Empty;
            /*string decrypt = Crypton.DecryptFromHTML(encrypt);
            return Crypton.DecryptByKey(decrypt, CryptonKey).ToString();*/
            return Crypton.DecryptFromHTMLByKey(encrypt, CryptonKey);
        }
    }
}
