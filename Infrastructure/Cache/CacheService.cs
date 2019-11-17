using System;
using System.Collections.Concurrent;

namespace MAS.Payments.Infrastructure.Cache
{
    public static class CacheService
    {
        private static ConcurrentDictionary<string, object> Cache { get; } =
            new ConcurrentDictionary<string, object>();

        public static void Save<T>(string key, T @object)
        {
            Cache.AddOrUpdate(key, @object, (recordKey, value) =>
            {
                value = @object;
                return @object;
            });
        }

        public static T Get<T>(string key)
        {
            if (Cache.IsEmpty)
            {
                return default(T);
            }

            if (!Cache.ContainsKey(key))
            {
                return default(T);
            }

            object objectResult = null;

            var isSuccess = Cache.TryGetValue(key, out objectResult);

            if (isSuccess && objectResult != null)
            {
                T value = (T)Convert.ChangeType(objectResult, typeof(T));

                return value;
            }

            throw new Exception($"Cache record isn't accessible or cannot be converted into {typeof(T).Name} type.");
        }

        public static void Remove(string key)
        {
            var isSuccess = Cache.TryRemove(key, out object objectResult);

            if (!isSuccess)
            {
                throw new Exception($"Cache record {key} cannot be removed.");
            }
        }
    }
}