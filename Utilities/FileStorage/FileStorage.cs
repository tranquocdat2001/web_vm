using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Utilities.FileStorage
{
    public class FileStorage
    {
        public static string _UploadDomain = ConfigurationManager.AppSettings["Upload-Domain"];
        public static string _UploadProject = ConfigurationManager.AppSettings["Upload-Project"];
        public static string _UploadHandler = ConfigurationManager.AppSettings["Upload-Handler"];
        public static string _ViewDomain = ConfigurationManager.AppSettings["DomainAvatar"];
        public static string _NoImage = ConfigurationManager.AppSettings["NoImage"];
        public static string AES_Key = ConfigurationManager.AppSettings["AES-Key"];
        public static string AES_IV = ConfigurationManager.AppSettings["AES-IV"];
        public static string VideoAES_Key = ConfigurationManager.AppSettings["Video-AES-Key"];
        public static string VideoAES_IV = ConfigurationManager.AppSettings["Video-AES-IV"];
        public static string _VideoUploadDomain = ConfigurationManager.AppSettings["Video-Upload-Domain"];
        public static string _VideoUploadHandler = ConfigurationManager.AppSettings["Video-Upload-Handler"];

        public static class UploadType
        {
            public static string Upload = "upload";
            public static string Copy = "copy";
            public static string Download = "downloadFileFromUrl";
        }

        public static string AESEncrypt(string Input) // "{username}|2014-12-17 10:15" 
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AES_Key);
            aes.IV = Convert.FromBase64String(AES_IV);

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }

        public static string VideoAESEncrypt(string Input) // "{username}|2014-12-17 10:15" 
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(VideoAES_Key);
            aes.IV = Convert.FromBase64String(VideoAES_IV);

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }

        public string AESDecrypt(string Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AES_Key);
            aes.IV = Convert.FromBase64String(AES_IV);

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Encoding.UTF8.GetString(xBuff);
        }
        public static string EncriptUsername(string username)
        {
            var key = AESEncrypt(username);
            key = key.Replace("/", "")
                     .Replace("\\", "")
                     .Replace("?", "")
                     .Replace("+", "")
                     .Replace("=", "");
            return key.Substring(0, 8);
        }
        public static string SendRequestWithParram(string url, NameValueCollection nvc)
        {
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;
                var strResult = string.Empty;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    strResult = reader2.ReadToEnd();
                }
                catch (Exception ex)
                {
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }

                return strResult;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public static string HttpUploadFile(string url, HttpPostedFile file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            var fileStream = file.InputStream;
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            var strResult = string.Empty;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                strResult = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return strResult;
        }

        public static string HttpUploadFile(string url, string fileName, Stream inputStream, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, fileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            var fileStream = inputStream;
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            var strResult = string.Empty;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                strResult = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return strResult;
        }

        //public static string HttpUploadFile(string url, string fileName, Stream inputStream, string paramName, string contentType, NameValueCollection nvc)
        //{
        //    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        //    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        //    wr.ContentType = "multipart/form-data; boundary=" + boundary;
        //    wr.Method = "POST";
        //    wr.KeepAlive = true;
        //    wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

        //    Stream rs = wr.GetRequestStream();

        //    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //    foreach (string key in nvc.Keys)
        //    {
        //        rs.Write(boundarybytes, 0, boundarybytes.Length);
        //        string formitem = string.Format(formdataTemplate, key, nvc[key]);
        //        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //        rs.Write(formitembytes, 0, formitembytes.Length);
        //    }
        //    rs.Write(boundarybytes, 0, boundarybytes.Length);

        //    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //    //NhatHD: 26-12-2014
        //    //string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
        //    string header = string.Format(headerTemplate, paramName, fileName, contentType);
        //    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        //    rs.Write(headerbytes, 0, headerbytes.Length);

        //    //NhatHD 26-12-2014
        //    //var fileStream = file.InputStream;
        //    var fileStream = inputStream;
        //    byte[] buffer = new byte[4096];
        //    int bytesRead = 0;
        //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //    {
        //        rs.Write(buffer, 0, bytesRead);
        //    }
        //    fileStream.Close();

        //    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //    rs.Write(trailer, 0, trailer.Length);
        //    rs.Close();

        //    WebResponse wresp = null;
        //    var strResult = string.Empty;
        //    try
        //    {
        //        wresp = wr.GetResponse();
        //        Stream stream2 = wresp.GetResponseStream();
        //        StreamReader reader2 = new StreamReader(stream2);
        //        strResult = reader2.ReadToEnd();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (wresp != null)
        //        {
        //            wresp.Close();
        //            wresp = null;
        //        }
        //    }
        //    finally
        //    {
        //        wr = null;
        //    }
        //    return strResult;
        //}

        public static bool CopyOutFromTempFolder(string fileName, out string newImages)
        {
            newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("FileTemp", fileName);
                postParameters.Add("project", _UploadProject);
                postParameters.Add("UploadType", UploadType.Copy);
                var eKey = FileStorage.AESEncrypt("temp" + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("param", "end");

                string postURL = _UploadDomain + _UploadHandler;
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    //return readImageHost + baseFolder + "/" + fu.FileName;                    
                    return GetImageURLFromResult(fullResponse, out newImages);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return false;
            }
        }

        public static bool CopyVideoOutFromTempFolder(string fileName, out string newVideo)
        {
            newVideo = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("FileTemp", fileName);
                postParameters.Add("project", _UploadProject);
                postParameters.Add("UploadType", UploadType.Copy);
                var eKey = FileStorage.VideoAESEncrypt("|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("param", "end");

                string postURL = _VideoUploadDomain + _VideoUploadHandler;
                if (postURL.Contains("https"))
                {
                    postURL = postURL.Replace("https", "http");
                }
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    //return readImageHost + baseFolder + "/" + fu.FileName;                    
                    return GetVideoURLFromResult(fullResponse, out newVideo);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return false;
            }
        }

        public static string UploadToServer(string fileName)
        {
            string newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("downloadUrl", fileName);
                postParameters.Add("project", _UploadProject);
                postParameters.Add("UploadType", UploadType.Download);
                var eKey = AESEncrypt(string.Concat("|", DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("submit", "Upload Image");
                postParameters.Add("param", "end");

                string postURL = string.Concat(_UploadDomain, _UploadHandler);
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();

                    GetImageURLViewDomain(fullResponse, out newImages);
                }
                return newImages;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return newImages;
            }
        }

        private static bool GetVideoURLFromResult(string fullResponse, out string newImages)
        {
            try
            {
                if (!string.IsNullOrEmpty(fullResponse) && fullResponse.IndexOf("OK") != -1)
                {
                    newImages = fullResponse;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
            }

            newImages = string.Empty;
            return false;

        }

        private static bool GetImageURLFromResult(string fullResponse, out string newImages)
        {
            try
            {
                if (fullResponse.IndexOf("OK") != -1)
                {
                    newImages = fullResponse.Replace("{\"OK\": \"", string.Empty).Replace("\"}", string.Empty);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
            }

            newImages = string.Empty;
            return false;

        }

        private static bool GetImageURLViewDomain(string fullResponse, out string newImages)
        {
            if (fullResponse.IndexOf("OK") != -1)
            {
                newImages = fullResponse.Replace("{\"OK\": \"", string.Empty).Replace("\"}", string.Empty);
                newImages = _ViewDomain + "/" + newImages;
                return true;
            }

            newImages = string.Empty;
            return false;
        }

        public static bool DownloadFromRotateFolder(string fileName, out string newImages)
        {
            newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("downloadUrl", ConfigurationManager.AppSettings["serverupload"] + "/" + fileName);
                postParameters.Add("project", ConfigurationManager.AppSettings["UploadProject"]);
                postParameters.Add("UploadType", "downloadFileFromUrl");
                var eKey = FileStorage.AESEncrypt("" + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("WaterMark", "true");
                postParameters.Add("param", "end");

                string postURL = ConfigurationManager.AppSettings["UploadDomain"] + ConfigurationManager.AppSettings["UploadHandler"];
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    //return readImageHost + baseFolder + "/" + fu.FileName;                    
                    return GetImageURLFromResult(fullResponse, out newImages);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static Tuple<MemoryStream, string> SaveImage(string fromUrl)
        {
            MemoryStream ms = null;
            string mess = string.Empty;
            try
            {
                HttpWebResponse imageResponse = null;
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(fromUrl);
                imageRequest.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                try
                {
                    imageResponse = (HttpWebResponse)imageRequest.GetResponse();
                }
                catch
                {
                    imageRequest = (HttpWebRequest)WebRequest.Create(fromUrl);
                    imageRequest.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                    imageResponse = (HttpWebResponse)imageRequest.GetResponse();
                }

                Stream responseStream = imageResponse.GetResponseStream();
                ms = new MemoryStream();
                responseStream.CopyTo(ms);
                responseStream.Close();
                imageResponse.Close();
            }
            catch (Exception ex)
            {
                mess = ex.Message;
            }
            return new Tuple<MemoryStream, string> (ms, mess);
        }

        //public string UploadImage(MemoryStream ms, string imageName, ref string md5, int domainId)
        //{
        //    string newPath = string.Empty;
        //    try
        //    {
        //        byte[] buff = null;
        //        UploadResult uResult = null;


        //        WVTStorage.AES_IV = Bds.Thailand.AutoCrawler.PushData.Properties.Settings.Default.AES_IV;
        //        WVTStorage.AES_Key = Bds.Thailand.AutoCrawler.PushData.Properties.Settings.Default.AES_Key;
        //        string uploadAPI = Bds.Thailand.AutoCrawler.PushData.Properties.Settings.Default.uploadAPI;
        //        string projectName = Bds.Thailand.AutoCrawler.PushData.Properties.Settings.Default.projectName;
        //        string eKey = WVTStorage.AES_encrypt("|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

        //        System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();

        //        switch (domainId)
        //        {
        //            case (int)EnumEntity.DomainSite.One2CarCOM:
        //                buff = Utils.Images_RemoveWM(ms, "one2car");
        //                break;
        //            case (int)EnumEntity.DomainSite.TaladrodCOM:
        //                buff = Utils.Images_RemoveWM(ms, "taladrod");
        //                break;
        //            case (int)EnumEntity.DomainSite.Unseencar:
        //                buff = Utils.Images_RemoveWM(ms, "unseencar.com");
        //                break;
        //            case (int)EnumEntity.DomainSite.Xn_72c3a7ag1brb1f:
        //                buff = Utils.Images_RemoveWM(ms, "second_hand_car_thai");
        //                break;
        //            default:
        //                buff = ms.ToArray();
        //                break;
        //        }
        //        ms = new System.IO.MemoryStream(buff);
        //        nvc.Add("id", "TTR");
        //        nvc.Add("project", projectName);
        //        nvc.Add("UploadType", "upload");
        //        nvc.Add("StringDecypt", eKey);
        //        nvc.Add("submit", "Upload Image");

        //        string resultFileBds = FileStorage.HttpUploadFile(uploadAPI, imageName, ms, "fileToUpload", "image/jpeg", nvc);

        //        //N?u upload ?nh thành công.
        //        if (resultFileBds.Contains("OK"))
        //        {
        //            if (!string.IsNullOrEmpty(resultFileBds))
        //            {
        //                uResult = JsonConvert.DeserializeObject<UploadResult>(resultFileBds);
        //                if (uResult != null)
        //                {
        //                    if (!string.IsNullOrEmpty(uResult.OK))
        //                    {
        //                        newPath = uResult.OK;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //N?u upload ?nh l?i, thì tr? v? thông báo l?i.
        //            md5 = uResult.OK;
        //        }
        //    }
        //    catch (Exception ex) { md5 = "UploadImage::" + ex.Message; }
        //    return newPath;
        //}
    }
}
