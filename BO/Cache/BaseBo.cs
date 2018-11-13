using Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BO
{
    public abstract class BaseBo
    {
        private BaseCache _cacheClient;
        private KeyCachedManager _keyManager;

        public BaseBo()
        {
            _cacheClient = new BaseCache();
            _keyManager = new KeyCachedManager();
        }

        public T Execute<T>(Func<T> func, string funcName, int cachedInMinutes, bool isSerialize = false, params object[] args)
        {
            string cacheName = isSerialize ? _keyManager.GenCacheKeySerialize(funcName, args) : _keyManager.GenCacheKey(funcName, args);

            T obj = _cacheClient.GetT<T>(cacheName);

            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    _cacheClient.AddT<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        public T Execute<T>(Func<T> func, int cachedInMinutes, bool isSerialize = false, params object[] args)
        {
            string cacheName = _keyManager.GenCacheKey(func.GetType().Name, isSerialize, args);

            T obj = _cacheClient.GetT<T>(cacheName);

            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    _cacheClient.AddT<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        public T Execute<T>(Func<T> func, int cachedInMinutes, bool isSerialize = false, bool? allowCached = null, params object[] args)
        {
            string cacheName = _keyManager.GenCacheKey(func.GetType().Name, isSerialize, args);

            T obj = _cacheClient.GetT<T>(cacheName);

            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    _cacheClient.AddT<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        public T Execute<T>(Func<T> func, int cachedInMinutes, string cacheName, bool? allowCached = null)
        {
            T obj = _cacheClient.GetT<T>(cacheName);

            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    _cacheClient.AddT<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        public T Execute<T>(Func<T> func, int cachedInMinutes, Func<string> genCacheName)
        {
            string cacheName = genCacheName.Invoke();

            T obj = _cacheClient.GetT<T>(cacheName);

            if (EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    _cacheClient.AddT<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }
    }
}