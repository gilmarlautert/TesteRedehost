using System.Collections.Generic;
using StackExchange.Redis;

namespace ProjetoRedehost.Services.tld.cache
{
    public class TldCacheService : ITldCache
    { 
        private readonly IDatabase _cache;

        private readonly string _key;
        public TldCacheService(string connection, string key)
        {
            var cnn = ConnectionMultiplexer.Connect(connection);
            _cache = cnn.GetDatabase();
            _key = key;
        }
        public void Add(string extension)
        {
            _cache.SortedSetAdd(_key, extension, 0);
        }

        public void Remove(string extension)
        {
            _cache.SortedSetRemove(_key,extension);
        }

        public void Edit(string extensionOrigin, string extensionDestination)
        {
            Remove(extensionOrigin);
            Add(extensionDestination);
        }

        public string Find(string extension)
        {
            var res2 = _cache.SortedSetScan(_key, extension).GetEnumerator();
            if(res2.MoveNext())
            {
                return res2.Current.Element;
            }
            return null;
        }

        public IEnumerable<string> ListAll()
        {
            var entries = new List<string>();
            foreach (var res2 in _cache.SortedSetScan(_key, ""))
            {
                entries.Add(res2.Element);
            };
            return entries;
        }
    }
}