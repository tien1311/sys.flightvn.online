namespace Manager.Model.Models.Other.RedisCache
{
    using Microsoft.Extensions.Options;
    using StackExchange.Redis;
    using System;

    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        /// <summary>
        ///     The _connection.
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connection;


        private readonly IOptions<RedisConfiguration> redis;

        public RedisConnectionFactory(IOptions<RedisConfiguration> redis)
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redis.Value.Host));
        }

        public ConnectionMultiplexer Connection()
        {
            return _connection.Value;
        }
    }
}