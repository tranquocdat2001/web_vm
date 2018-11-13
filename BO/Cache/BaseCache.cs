using ServiceStack.Redis;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using Utilities;

namespace BO
{
    public class BaseCache
    {
        private static bool IsRefreshCache = false;
        private static readonly string RedisIP = AppSettings.Instance.GetString("RedisIP");
        private static readonly int RedisPort = AppSettings.Instance.GetInt32("RedisPort", 0) > 0 ? AppSettings.Instance.GetInt32("RedisPort") : 6379;
        private int _redisDb = AppSettings.Instance.GetInt32("RedisDB");
        public static readonly bool AllowCached = AppSettings.Instance.GetInt32("CacheType", 0) > 0 ? true : false;
        private int cacheType = AppSettings.Instance.GetInt32("CacheType", (int)CachedEnum.CachedTypes.Redis);
        private static readonly string redisNativeSlotName = "redisNativeSlotName";
        private const int MaxDayExpired = 3;

        public BaseCache()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.UserAgent != null)
            {
                if (System.Web.HttpContext.Current.Request.UserAgent.Contains("refreshcache"))
                {
                    IsRefreshCache = true;
                }
            }
        }
        public bool AddT<T>(string key, T item, int expire)
        {
            try
            {
                if (!AllowCached) return false;

                if (cacheType== (int)CachedEnum.CachedTypes.Redis)
                {
                    Redis.SetEx(key, expire * 60, Serialize(item));
                }
                else
                {
                    SetIISCache<T>(key, item, expire);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Trace, string.Format("AddT key: {0} {1} {2}", key, Environment.NewLine, ex.ToString()));
                return false;
            }
        }

        private T BaseGetCacheT<T>(string key)
        {
            try
            {
                //object obj = cm.GetData(key);
                //object obj = null;                

                if (cacheType == (int)CachedEnum.CachedTypes.Redis)
                {
                    var obj1 = Redis.Get(key);

                    return Deserialize<T>(obj1);
                }
                else
                {
                    return GetIISCache<T>(key);
                }
                //return default(T);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Trace, string.Format("BaseGetCacheT<T> key: {0} {1} {2}", key, Environment.NewLine, ex.ToString()));
                return default(T);
            }
        }

        public T GetT<T>(string key)
        {
            try
            {
                // Xoa cache ca o IIS va Redis
                //if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.UserAgent != null)
                //{
                //    if (System.Web.HttpContext.Current.Request.UserAgent.Contains("refreshcache"))
                //    {
                //        //if (IsRemoveCache(key, 120))
                //        //{
                //        Remove(key);
                //        return default(T);
                //        //}
                //    }
                //}
                if (IsRefreshCache)
                {
                    Remove(key);
                    return default(T);
                }
                return BaseGetCacheT<T>(key);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Trace, string.Format("GetT key: {0} {1} {2}", key, Environment.NewLine, ex.ToString()));
                return default(T);
            }
        }

        public bool Remove(string key)
        {
            try
            {                
                if (cacheType == (int)CachedEnum.CachedTypes.Redis)
                {
                    Redis.Del(key);
                }
                else
                {
                    RemoveIISCache(key);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] Serialize<T>(T obj)
        {
            if (obj == null) return null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public static T Deserialize<T>(byte[] arrBytes)
        {
            if (arrBytes == null) return default(T);

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var obj = (T)bf.Deserialize(ms);

                return obj;
            }
        }

        public RedisNativeClient Redis
        {
            get
            {
                try
                {
                    if (!HttpContext.Current.Items.Contains(redisNativeSlotName))
                    {
                        RedisNativeClient redis = new RedisNativeClient(RedisIP, RedisPort);
                        redis.Db = _redisDb;
                        redis.ConnectTimeout = 600;
                        HttpContext.Current.Items.Add(redisNativeSlotName, redis);
                    }

                    return (RedisNativeClient)HttpContext.Current.Items[redisNativeSlotName];
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Trace, string.Format("Khởi tạo Redis {0} {1}", Environment.NewLine, ex.ToString()));
                    RedisNativeClient redis = new RedisNativeClient(RedisIP, RedisPort);
                    redis.Db = _redisDb;
                    redis.ConnectTimeout = 600;
                    return redis;
                }
            }
        }

        private void SetIISCache<T>(string key, object item, int expire)
        {
            if (HttpContext.Current == null) return;
            if (item == null || key.Length == 0) return;
            HttpContext.Current.Cache.Insert(key, item, null, DateTime.Now.AddMinutes(expire), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        private T GetIISCache<T>(string key)
        {
            if (HttpContext.Current == null) return default(T);
            object data = HttpContext.Current.Cache[key];
            if (null != data)
            {
                try
                {
                    return (T)data;
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        private void RemoveIISCache(string key)
        {
            if (HttpContext.Current == null) return;
            object data = HttpContext.Current.Cache[key];
            if (null != data)
            {
                try
                {
                    HttpContext.Current.Cache.Remove(key);
                }
                catch
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}