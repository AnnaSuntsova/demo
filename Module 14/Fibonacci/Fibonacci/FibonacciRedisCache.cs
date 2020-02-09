using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fibonacci
{
    class FibonacciRedisCache: ICache
    {
        private ConnectionMultiplexer _redisConnection;
        string prefix = "Cache_fibonacci";
        DataContractSerializer serializer = new DataContractSerializer(typeof(int));

        public FibonacciRedisCache(string hostName)
        {
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { hostName },
                Ssl = false
            };
            _redisConnection = ConnectionMultiplexer.Connect(options);
        }
         
        public int Get(int key)
        {
            var db = _redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix+key);
            if (s == null)
                return default(int);

            return (int)serializer
                .ReadObject(new MemoryStream(s));

        }        

        public void Set(int key, int value)
        {
            var db = _redisConnection.GetDatabase();
            var rediskey = prefix + key;

            if (value == null)
            {
                db.StringSet(rediskey, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, value);
                db.StringSet(rediskey, stream.ToArray());
            }
        }
    }
}
