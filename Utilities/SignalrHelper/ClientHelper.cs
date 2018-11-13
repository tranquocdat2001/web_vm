using System;
using Newtonsoft.Json;

namespace Utilities.SignalrHelper
{
    public enum ChatClientName
    {
        Admin = 0,
        Android = 1,
        iOS = 2
    }

    public class ClientPackage
    {
        [JsonProperty("ui")]
        public long UserId { get; set; }
        [JsonProperty("sk")]
        public string SecretKey { get; set; }
        [JsonProperty("dts")]
        public long DateTimeStamp { get; set; }
    }

    public static class ClientHelper
    {
        /// <summary>
        /// To encrypt string
        /// Author: ThanhDT
        /// CreatedDate: 10/13/2014 3:30 PM
        /// </summary>
        /// <param name="clientPackage">The client package.</param>
        /// <returns></returns>
        public static string ToEncryptString(this ClientPackage clientPackage)
        {
            var jsonContent = NewtonJson.Serialize(clientPackage);
            var encrypt = EncryptUtils.Encrypt(jsonContent);

            return encrypt;
        }

        /// <summary>
        /// From encrypt string
        /// Author: ThanhDT
        /// CreatedDate: 10/13/2014 3:30 PM
        /// </summary>
        /// <param name="packageEncrypt">The package encrypt.</param>
        /// <returns></returns>
        public static ClientPackage FromEncryptString(string packageEncrypt)
        {
            try
            {
                var jsonContent = EncryptUtils.Decrypt(packageEncrypt);
                ClientPackage clientPackage = NewtonJson.Deserialize<ClientPackage>(jsonContent);

                return clientPackage;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
            }

            return null;
        }

    }
}
