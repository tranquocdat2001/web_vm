using System;
using System.Web;

namespace Utilities
{
    public interface ICookieManager
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, int expireSeconds = 0, string domain = "", string path = "/");
        void Remove(string key);
    }

    public class CookieManager : ICookieManager
    {
        private static CookieManager _instance;
        private static object lockObject = new object();
        CookieManager() { }

        public static CookieManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (lockObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new CookieManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    string value = string.Empty;
                    HttpCookie aCookie = HttpContext.Current.Request.Cookies[key];
                    if (aCookie != null)
                    {
                        value = aCookie.Value;
                        if (typeof (T) == typeof (string) || typeof (T) == typeof (String))
                            return (T) Convert.ChangeType(value, typeof (T));
                        else
                            return !string.IsNullOrEmpty(value) ? NewtonJson.Deserialize<T>(value) : default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Warning, ex.ToString());
            }
            return default(T);
        }

        public void Set<T>(string key, T value, int expireSeconds = 0, string domain = "", string path = "/")
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    var httpCookies = HttpContext.Current.Response.Cookies;
                    var httpRequestCookies = HttpContext.Current.Request.Cookies;

                    string inputValue;

                    if (typeof(T) == typeof(string) || typeof(T) == typeof(String))
                        inputValue = (string)Convert.ChangeType(value, typeof(string));
                    else
                        inputValue = NewtonJson.Serialize(value);

                    HttpCookie cookie = new HttpCookie(key, inputValue)
                    {
                        HttpOnly = true,
                        Domain = domain,
                        Path = path
                    };

                    if (expireSeconds > 0)
                    {
                        cookie.Expires = DateTime.Now.AddSeconds(expireSeconds);
                    }

                    if ((httpRequestCookies[key] != null) &&
                        !string.IsNullOrEmpty(httpRequestCookies[key].Value))
                    {
                        httpCookies.Remove(key);
                    }

                    httpCookies.Add(cookie);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Warning, ex.ToString());
            }
        }

        public void Remove(string key)
        {
            try
            {
                if (HttpContext.Current != null)
                {

                    var httpCookies = HttpContext.Current.Request.Cookies;

                    httpCookies.Remove(key);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Warning, ex.ToString());
            }
        }
    }
}
