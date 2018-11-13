using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities
{
    public class GetQueryString
    {
        public enum RequestType
        {
            Query = 1,
            Form = 2,
            Files = 3,
            Params = 4
        }
        public enum CultureInfoType
        {
            Vi = 1,
            En = 3,
        }
        public static string GetVariable(string name, string val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.ServerVariables[name]) ? context.Request.ServerVariables[name].Trim() : val;
        }

        private static object GetParam(string paramName, RequestType method)
        {
            var result = new object();
            var context = HttpContext.Current;
            if (context != null)
            {
                switch (method)
                {
                    case RequestType.Query:
                        result = context.Request.QueryString[paramName];
                        break;
                    case RequestType.Form:
                        result = context.Request.Form[paramName];
                        break;
                    case RequestType.Files:
                        result = context.Request.Files[paramName];
                        break;
                }
            }
            return result;
        }

        public static string GetHostRefer(bool noPort)
        {
            var context = HttpContext.Current;
            var host = Regex.Replace(context.Request.ServerVariables["HTTP_ORIGIN"], "http://|https://", string.Empty);
            if (host.IndexOf(":") != -1 && noPort) host = host.Substring(0, host.LastIndexOf(":"));
            return host;
        }

        public static string GetRemoteServer(bool noPort)
        {
            var context = HttpContext.Current;
            var host = context.Request.ServerVariables["REMOTE_HOST"];
            var port = context.Request.ServerVariables["REMOTE_PORT"];
            if (!noPort)
                host = host + (!string.IsNullOrEmpty(port) ? ":" + port : "");
            return host;
        }

        #region Get Post Form

        /// <summary>
        /// Gets post data parse sting type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetPost(string name, string val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? context.Request.Form[name].Trim() : val;
        }

        /// <summary>
        /// Gets post data parse Int32 type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int GetPost(string name, int val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? int.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse Long (Int64) type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long GetPost(string name, long val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? long.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse decimal type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal GetPost(string name, decimal val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? decimal.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse Boolean type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool GetPost(string name, bool val)
        {
            var context = HttpContext.Current;
            var postValue = val;
            if (!string.IsNullOrEmpty(context.Request.Form[name]))
            {
                if (context.Request.Form[name].IndexOf("false") != -1)
                    return false;
                if (context.Request.Form[name].IndexOf("true") != -1)
                    return true;
                if (context.Request.Form[name] == "1")
                    return true;
                if (context.Request.Form[name] == "0")
                    return false;
            }
            return postValue;
        }

        /// <summary>
        /// Gets post data parse short (Int16) type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static short GetPost(string name, short val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? short.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse byte type. 8 bits
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte GetPost(string name, byte val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? byte.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse Boolean type from checkbox
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="isCheckBox"></param>
        /// <returns></returns>
        public static bool GetPost(string name, bool val, bool isCheckBox)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name])
                ? (isCheckBox ? context.Request.Form[name] == "on" : bool.Parse(context.Request.Form[name]))
                : val;
        }

        /// <summary>
        /// Gets post data parse Double (Float) type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double GetPost(string name, double val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? double.Parse(context.Request.Form[name].Trim()) : val;
        }
        
        /// <summary>
        /// Gets post data parse Float type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float GetPost(string name, float val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.Form[name]) ? float.Parse(context.Request.Form[name].Trim()) : val;
        }

        /// <summary>
        /// Gets post data parse DateTime type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime GetPost(string name, DateTime val)
        {
            var context = HttpContext.Current;
            var outputDate = new DateTime();
            if (!string.IsNullOrEmpty(context.Request.Form[name]))
                outputDate = Utils.ConvertToDateTime(context.Request.Form[name]);
            return outputDate;
        }

        /// <summary>
        /// Gets post data parse DateTime type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="formatInputDate">formatInputDate</param>
        /// <param name="formatInputTime">formatInputTime</param>
        /// <returns></returns>
        public static DateTime GetPost(string name, DateTime val, string formatInputDate, string formatInputTime)
        {
            var ci = new CultureInfo("en-US");
            var dtf = new DateTimeFormatInfo
                          {
                              ShortDatePattern = !string.IsNullOrEmpty(formatInputDate) ? formatInputDate : "dd/MM/yyyy",
                              ShortTimePattern = !string.IsNullOrEmpty(formatInputTime) ? formatInputTime : "HH:mm:ss"
                          };
            ci.DateTimeFormat = dtf;
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            var context = HttpContext.Current;
            var outputDate = new DateTime();
            if (!string.IsNullOrEmpty(context.Request.Form[name]))
                outputDate = Utils.ConvertToDateTime(context.Request.Form[name]);
            return outputDate;
        }
        #endregion

        #region Get Request Files

        /// <summary>
        /// Gets post data parse HttpPostedFile type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpPostedFile GetPost(string name)
        {
            var context = HttpContext.Current;
            return context.Request.Files[name];
        }

        #endregion

        #region Get QueryString

        public static string GetQuery(string name, string val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.QueryString[name]) ? context.Request.QueryString[name].Trim() : val;
        }
        public static int GetQuery(string name, int val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.QueryString[name]) ? int.Parse(context.Request.QueryString[name].Trim()) : val;
        }
        public static byte GetQuery(string name, byte val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.QueryString[name]) ? byte.Parse(context.Request.QueryString[name].Trim()) : val;
        }
        public static long GetQuery(string name, long val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.QueryString[name]) ? long.Parse(context.Request.QueryString[name].Trim()) : val;
        }
        public static bool GetQuery(string name, bool val)
        {
            var context = HttpContext.Current;
            return !string.IsNullOrEmpty(context.Request.QueryString[name]) ? bool.Parse(context.Request.QueryString[name].Trim()) : val;
        }

        #endregion
    }
}
