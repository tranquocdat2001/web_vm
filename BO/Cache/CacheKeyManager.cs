using Utilities;
using System.Linq;

namespace BO
{
    public class KeyCachedManager
    {
        private int _cacheType;
        private string _prefixKey = string.Empty;

        public struct ObjectKey
        {
            public object Input { get; set; }
            public bool IsSerialize { get; set; }
        }

        public KeyCachedManager()
        {
            _cacheType = (int)CachedEnum.CachedTypes.IIS;
            _prefixKey = AppSettings.Instance.GetString("PreCacheKey");
        }

        public KeyCachedManager(int cacheType)
        {
            this._cacheType = cacheType;
            this._prefixKey = AppSettings.Instance.GetString("PreCacheKey");
        }

        public KeyCachedManager(int cacheType, string prefixKey)
        {
            this._cacheType = cacheType;
            this._prefixKey = prefixKey;
        }

        public string GetCacheKey(string className, string functionName, params object[] args)
        {
            string separator = _cacheType == (int)CachedEnum.CachedTypes.Redis ? ":" : "_";
            if (args != null && args.Length > 0)
            {
                string cacheKey = string.Format("{0}{1}{2}{1}{3}", _prefixKey, separator, className, functionName);
                return args.Aggregate(cacheKey, (current, param) => current + (separator + param));
            }
            else
            {
                return string.Format("{0}{1}{2}{1}{3}", _prefixKey, separator, className, functionName);
            }
        }

        public string GenCacheKey(string cacheName, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                string keyReturn = string.Empty;
                string separator = _cacheType == (int)CachedEnum.CachedTypes.Redis ? ":" : "_";
                string cacheKey = string.Format("{0}{1}{2}", _prefixKey, separator, cacheName);
                keyReturn = args.Aggregate(cacheKey, (current, param) => current + (separator + param));
                return keyReturn;

            }
            else return cacheName;
        }

        public string GenCacheKeySerialize(string cacheName, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                string separator = _cacheType == (int)CachedEnum.CachedTypes.Redis ? ":" : "_";
                string cacheKey = string.Format("{0}{1}{2}", _prefixKey, separator, cacheName);
                return args.Aggregate(cacheKey, (current, param) => current + (separator + EncodeObject(param)));
            }
            else return cacheName;
        }

        public string GenCacheKeyDynamic(string cacheName, params ObjectKey[] args)
        {
            if (args != null && args.Length > 0)
            {
                string separator = _cacheType == (int)CachedEnum.CachedTypes.Redis ? ":" : "_";
                string cacheKey = string.Format("{0}{1}{2}", _prefixKey, separator, cacheName);
                return args.Aggregate(cacheKey, (current, param) => current + (separator + EncodeObject(param)));
            }
            else return cacheName;
        }

        private string EncodeObject(object obj)
        {
            if (obj == null) return string.Empty;
            return StringUtils.CalculateMD5Hash(NewtonJson.Serialize(obj));
        }

        private object EncodeObject(ObjectKey obj)
        {
            if (obj.Input == null) return string.Empty;
            if (obj.IsSerialize)
                return StringUtils.CalculateMD5Hash(NewtonJson.Serialize(obj));

            return obj.Input;
        }
    }
}
