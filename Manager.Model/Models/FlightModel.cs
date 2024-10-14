using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class FlightModel
    {
        public int ID { get; set; }
        public string Airline { get; set; }
        public string Itinerary { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAgent { get; set; }
        public bool Status { get; set; }
        public string KindTrip { get; set; } //0 là 1 chiều, 1 là khứ hồi, 2 là nhiều chặn
        public string active { get; set; }
        public string Condition { get; set; }
        public string CreatedDate { get; set; }
        public string Specification { get; set; }
        public string Donvi { get; set; }
        public decimal Fare { get; set; } = 0;
        public decimal Charge { get; set; } = 0;
        public decimal Discount { get; set; } = 0;

        public List<FlightDetailModel> ListFlightDetail { get; set; }

    }
    public class FlightDetailModel
    {
        public int ID { get; set; } = 0;
        public int FlightID { get; set; } = 0;
        public string FlightNumber { get; set; }
        public DateTime FlightDate { get; set; }
        public string FlightHour { get; set; }

    }
    public class Airline
    {
        public string Name { get; set; }
        public string IMG { get; set; }


    }
}
