using System;

namespace Utilities
{
    public class EncryptUtility
    {
        private static int expiredInMinute = AppSettings.Instance.GetInt32("EncryptIDExpiredInMinute", 60);

        private const string rpl1 = "=";
        private const string rpl11 = "_______";
        private const string rpl2 = "+";
        private const string rpl21 = "@@@";
        private const string rpl3 = "/";
        private const string rpl31 = "$$$";

        public static string EncryptId(string id)
        {
            try
            {
                string key = GenarateKey();

                string encrypt = CryptorEngine.Encrypt(id, true, key);

                encrypt = encrypt.Replace(rpl1, rpl11).Replace(rpl2, rpl21).Replace(rpl3, rpl31);

                return encrypt;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                return string.Empty;
            }
        }
        public static string EncryptId(long id)
        {
            try
            {
                string key = GenarateKey();

                string encrypt = CryptorEngine.Encrypt(id.ToString(), true, key);
                
                encrypt = encrypt.Replace(rpl1, rpl11).Replace(rpl2, rpl21).Replace(rpl3, rpl31);

                return encrypt;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
                return string.Empty;
            }
        }

        public static int DecryptId(string encrypt)
        {
            try
            {
                string key = GenarateKey();

                encrypt = encrypt.Replace(rpl11, rpl1).Replace(rpl21, rpl2).Replace(rpl31, rpl3);

                string decrypt = CryptorEngine.Decrypt(encrypt, true, key);

                return decrypt.ToInt(0);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

                return 0;
            }
        }

        public static long DecryptIdToLong(string encrypt)
        {
            try
            {
                string key = GenarateKey();

                encrypt = encrypt.Replace(rpl11, rpl1).Replace(rpl21, rpl2).Replace(rpl31, rpl3);

                string decrypt = CryptorEngine.Decrypt(encrypt, true, key);
                
                return decrypt.ToLong(0);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

                return 0L;
            }
        }

        public static int DecryptIdToInt(string encrypt)
        {
            try
            {
                string key = GenarateKey();

                encrypt = encrypt.Replace(rpl11, rpl1).Replace(rpl21, rpl2).Replace(rpl31, rpl3);

                string decrypt = CryptorEngine.Decrypt(encrypt, true, key);


                return decrypt.ToInt(0);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

                return 0;
            }
        }

        private static string GenarateKey()
        {
            //TimeSpan dateTimeSub = DateTime.Now - DateTime.Now.AddMinutes(expiredInMinute);
            //string dateToString = new DateTime(dateTimeSub).ToString("");
            string dateToString = DateTime.Now.ToString("yyyyMMdd");
            string key = string.Concat("tinxe_", dateToString);

            return key;
        }
    }
}