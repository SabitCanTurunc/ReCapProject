using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    internal class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HashSet<string> _cacheKeys;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _cacheKeys = new HashSet<string>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
            lock (_cacheKeys)  // Thread-safety for concurrent operations
            {
                _cacheKeys.Add(key);
            }
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            lock (_cacheKeys)  // Thread-safety for concurrent operations
            {
                _cacheKeys.Remove(key);
            }
        }

        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<string> keysToRemove;

            lock (_cacheKeys)  // Thread-safety for concurrent operations
            {
                keysToRemove = _cacheKeys.Where(key => regex.IsMatch(key)).ToList();
            }

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                lock (_cacheKeys)  // Thread-safety for concurrent operations
                {
                    _cacheKeys.Remove(key);
                }
            }
        }
    }
}
