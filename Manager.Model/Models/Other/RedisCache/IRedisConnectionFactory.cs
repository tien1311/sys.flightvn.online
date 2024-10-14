using StackExchange.Redis;

namespace Manager.Model.Models.Other.RedisCache
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}