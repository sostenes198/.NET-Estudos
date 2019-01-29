using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Estudos.Redis.Domain.Entities;
using Estudos.Redis.Domain.Serializer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Estudos.Redis.Domain
{
    public class PubSubRedis
    {
        private IConnectionMultiplexer _redis;

        public PubSubRedis(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
            var subscriber = _redis.GetSubscriber();


            subscriber.Subscribe("__keyevent@0__:del", (channel, value) => { });

            // subscriber.Subscribe("__keyevent@0__:expired", (channel, value) =>
            // {
            //     
            // });

            subscriber.Subscribe("__keyevent@0__:expired", (channel, value) => { });

            subscriber.Subscribe(new RedisChannel("__keyevent@0__:expired *", RedisChannel.PatternMode.Pattern), (channel, value) => { });

            subscriber.Subscribe("test", (channel, value) =>
            {
                var database = _redis.GetDatabase();

                database.StringSet("test", JsonConvert.SerializeObject(new Test {Id = 99, Name = "Testando"}));
                
                var keys = new List<RedisKey>();
                foreach (var endPoint in _redis.GetEndPoints())
                {
                    var server = _redis.GetServer(endPoint);
                    if (server.IsConnected)
                        keys.AddRange(server.Keys(0, "Estudos-Redisestudos.redis.test.*").ToList());
                }

                var values = database.StringGet(keys.ToArray());

                var testKey = database.StringGet("test");
                var result = JsonConvert.DeserializeObject<Test>(testKey);

                var x = new CacheSerializer();

                var results = keys.Select(qv => x.Deserialize<Test>(qv)).ToList();


                // var y = database.StringGet("Estudos-Redisestudos.redis.test.11");
            });
        }

        public Task SendTest()
        {
            ISubscriber subscriber = _redis.GetSubscriber();

            return subscriber.PublishAsync("test", "HASUHSA");
        }
    }
}