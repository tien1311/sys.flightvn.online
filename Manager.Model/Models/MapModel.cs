using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class MapModel
    {
        public List<Map_QN> Map_QN { get; set; }
        public List<Map_QT> Map_QT { get; set; }
    }
    public class Map_QN
    {
        public string ID { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string Loai { get; set; }
        public int IDAirport { get; set; }
        public string AirportName { get; set; }
        public List<Airport> ListAirport { get; set; }
    }
    public class Map_QT
    {
        public string ID { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string Loai { get; set; }
        public int IDAirport { get; set; }
        public string AirportName { get; set; }
        public List<Airport> ListAirport { get; set; }
    }
}
