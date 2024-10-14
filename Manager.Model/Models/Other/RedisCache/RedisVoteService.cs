namespace Manager.Model.Models.Other.RedisCache
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Manager.Model.Models.Other;
    using StackExchange.Redis;

    public class RedisVoteService<T> : BaseService<T>, ICacheManager<T>
    {
        internal readonly IDatabase Db;
        protected readonly IRedisConnectionFactory ConnectionFactory;

        public RedisVoteService(IRedisConnectionFactory connectionFactory)
        {

            ConnectionFactory = connectionFactory;
            Db = ConnectionFactory.Connection().GetDatabase();
        }

        public void Delete(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");
            key = GenerateKey(key);
            Db.KeyDelete(key);

        }

        public T Get(string key)
        {
            key = GenerateKey(key);
            var hash = Db.HashGetAll(key);
            return MapFromHash(hash);
        }

        public void Save(string key, T obj)
        {
            if (obj != null)
            {
                var hash = GenerateHash(obj);
                key = GenerateKey(key);

                if (Db.HashLength(key) == 0)
                {
                    Db.HashSet(key, hash);
                }
                else
                {
                    var props = Properties;
                    foreach (var item in props)
                    {
                        if (Db.HashExists(key, item.Name))
                        {
                            Db.HashIncrement(key, item.Name, Convert.ToInt32(item.GetValue(obj)));
                        }
                    }
                }

            }
        }
    }
}