﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace CachingSolutionsSamples.Regions
{
	class RegionsRedisCache : IRegionsCache
    {
		private ConnectionMultiplexer _redisConnection;
		string prefix = "Cache_Regions";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Region>));

		public RegionsRedisCache(string hostName)
		{
            var options = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { hostName },
                Ssl = false
            };
            _redisConnection = ConnectionMultiplexer.Connect(options);
        }

		public IEnumerable<Region> Get(string forUser)
		{
			var db = _redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<Region>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set(string forUser, IEnumerable<Region> regions, DateTimeOffset expirationDate)
		{
			var db = _redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (regions == null)
			{
				db.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, regions);
				db.StringSet(key, stream.ToArray(), expirationDate - DateTimeOffset.Now);
			}
		}
	}
}
