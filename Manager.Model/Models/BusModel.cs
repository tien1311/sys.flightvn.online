using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class BusModel
    {
        public string ID { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public int IDAirport { get; set; }
        public string AirportName { get; set; }
        public List<Airport> ListAirport { get; set; }
    }
}
