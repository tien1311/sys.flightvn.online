using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ConfigPhiXuatModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; } = 0;
        public double ExchangeRate { get; set; } = 0;
        public double Amount { get; set; } = 0;
    }
}
