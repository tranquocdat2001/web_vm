using System.Configuration;

namespace Utilities
{
    public class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object ObjLocked = new object();
        protected AppSettings()
        {
        }

        public static AppSettings Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (ObjLocked)
                    {
                        if (null == _instance)
                            _instance = new AppSettings();
                    }
                }
                return _instance;
            }
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            try
            {
                return (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) && ConfigurationManager.AppSettings[key].ToBool());
            }
            catch
            {
                return defaultValue;
            }
        }

        public string GetConnection(string key, string defaultValue = "")
        {
            try
            {
                return (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[key].ConnectionString) ? ConfigurationManager.ConnectionStrings[key].ConnectionString : defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public int GetInt32(string key, int defaultValue = 0)
        {
            try
            {
                return (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? ConfigurationManager.AppSettings[key].ToInt() : defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public long GetInt64(string key, long defaultValue = 0L)
        {
            try
            {
                return (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? ConfigurationManager.AppSettings[key].ToLong() : defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public string GetString(string key, string defaultValue = "")
        {
            try
            {
                return (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? ConfigurationManager.AppSettings[key].Trim() : defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

