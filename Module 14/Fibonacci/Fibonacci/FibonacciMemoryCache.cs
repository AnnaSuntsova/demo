using System.Runtime.Caching;

namespace Fibonacci
{
    class FibonacciMemoryCache: ICache
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_fibonacci";          


        public int Get(int key)
        {
            var storedValue = cache.Get(prefix + key);
            if (storedValue == null)
            {
                return default(int);
            }

            return (int)storedValue;
        }                

        public void Set(int key, int value)
        {
            cache.Set(prefix + key, value, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}
