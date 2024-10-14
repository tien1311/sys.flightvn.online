using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.Other
{
    public interface ICacheManager<T>
    {
        T Get(string key);

        void Save(string key, T obj);

        void Delete(string key);


    }
}
