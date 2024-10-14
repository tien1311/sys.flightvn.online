namespace Manager.Model.Models.Other.RedisCache
{
    using System;
    using System.Linq;
    using System.Reflection;
    using StackExchange.Redis;

    public abstract class BaseService<T>
    {
        protected string Name => Type.Name;

        protected PropertyInfo[] Properties => Type.GetProperties();

        protected Type Type => typeof(T);

        /// <summary>
        /// Generates a key for a Redis Entry  , follows the Redis Name Convention of inserting a colon : to identify values
        /// </summary>
        /// <param name="key">Redis identifier key</param>
        /// <returns>concatenates the key with the name of the type</returns>
        protected string GenerateKey(string key)
        {
            return string.Concat(key.ToLower(), ":", Name.ToLower());
        }

        protected HashEntry[] GenerateHash(T obj)
        {
            var props = Properties;
            var hash = new HashEntry[props.Count()];

            for (var i = 0; i < props.Count(); i++)
                hash[i] = new HashEntry(props[i].Name, props[i].GetValue(obj).ToString());

            return hash;
        }

        protected T MapFromHash(HashEntry[] hash)
        {
            var obj = (T)Activator.CreateInstance(Type); // new instance of T
            var props = Properties;

            for (var i = 0; i < props.Count(); i++)
            {
                for (var j = 0; j < hash.Count(); j++)
                {
                    if (props[i].Name == hash[j].Name)
                    {
                        var val = hash[j].Value;
                        var type = props[i].PropertyType;

                        if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            if (string.IsNullOrEmpty(val))
                            {
                                props[i].SetValue(obj, null);
                            }
                        props[i].SetValue(obj, Convert.ChangeType(val, type));
                    }
                }
            }
            return obj;
        }
    }
}