using System;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace Utilities.FileStorage
{
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private static readonly string[] HttpCode = { HttpStatusCode.Conflict.ToString(), HttpStatusCode.RequestTimeout.ToString(), HttpStatusCode.UnsupportedMediaType.ToString(), HttpStatusCode.InternalServerError.ToString(), HttpStatusCode.GatewayTimeout.ToString() };
        private static readonly string[] HttpMeaning = { "File này đã tồn tại", "Truy vấn TimeOut", "Server không hỗ trợ định dạng file", "Lỗi server", "Gateway Timeout" };

        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string filename)
        {
            try
            {
                string formDataBoundary = "-----------------------------28947758029299";
                string contentType = "multipart/form-data; boundary=" + formDataBoundary;

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

                return PostForm(postUrl, userAgent, contentType, formData, filename);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return null;
            }
        }

        public static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string filename)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            string strFormat = "-Upload file \"{0}\" {1}";
            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }
            // Set up the request properties
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;  // We need to count how many bytes we're sending. 
            HttpWebResponse res = null;
            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    // Push it out there
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }

                try
                {
                    res = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex);
                    strFormat += " : {2}";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                strFormat += " : {2}";
            }
            return res;// request.GetResponse() as HttpWebResponse;
        }

        public static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();

            foreach (KeyValuePair<string, object> param in postParameters)
            {
                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        //"application/octet-stream");
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, header.Length);

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    // Thanks to feedback from commenters, add a CRLF to allow multiple files to be uploaded
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, 2);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
                }
            }

            // Add the end of the request
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            private byte[] _File = new byte[] { };
            private string _FileName = string.Empty;
            private string _ContentType = string.Empty;

            public byte[] File
            {
                get { return _File; }
                set { _File = value; }
            }
            public string FileName
            {
                get { return _FileName; }
                set { _FileName = value; }
            }

            public string ContentType
            {
                get { return _ContentType; }
                set { _ContentType = value; }
            }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
        private static string GetMeaning(string str)
        {
            for (int i = 0; i < HttpCode.Length; i++)
            {
                if (str.ToLower().Contains(HttpCode[i].ToLower()))
                    return HttpMeaning[i];
            }
            return "Lỗi không xác định";
        }
        private static string GetErrorCode(string str)
        {
            for (int i = 0; i < HttpCode.Length; i++)
            {
                if (str.ToLower().Contains(HttpCode[i].ToLower()))
                    return HttpCode[i];
            }
            return string.Empty;
        }
    }
}
