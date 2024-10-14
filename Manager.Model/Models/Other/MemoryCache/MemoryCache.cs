using Manager.Model.Models.Other;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.Other.MemoryCache
{
    public class MemoryCache<T> : ICacheManager<T>
    {

        private readonly IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }



        public void Delete(string key)
        {
            throw new NotImplementedException();
        }



        public T Get(string key)
        {
            throw new NotImplementedException();
        }



        public void Save(string key, T obj)
        {
            throw new NotImplementedException();
        }
    }
}



