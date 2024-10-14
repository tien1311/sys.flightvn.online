using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class FlightGroupModel
    {
        public int ID { get; set; }
        public string Airline { get; set; }
        public string Itinerary { get; set; }
        public int NumberOfGuests { get; set; }
        public int TotalNumberOfGuests { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAgent { get; set; }
        public bool Status { get; set; }
        public string TypeOfTrip { get; set; } //0 là 1 chiều, 1 là khứ hồi, 2 là nhiều chặn
        public string active { get; set; }
        public string Condition { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Specification { get; set; }
        public string QuickSearch { get; set; }
        public string Unit { get; set; }
        public decimal Fare { get; set; } = 0;
        public decimal Charge { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public List<FlightGroupDetailModel> ListFlightDetail { get; set; }
        public List<ListAirline> ListAirline { get; set; }
    }
    public class FlightGroupDetailModel
    {
        public int ID { get; set; } = 0;
        public int FlightID { get; set; } = 0;
        public string FlightCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureHour { get; set; }
        public string KindFlight { get; set; }
        public string DepartureCode { get; set; }
        public string ArrivalCode { get; set; }
    }
    public class ListAirline
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    public class BookingFlightGroup
    {
        public int ID { get; set; }
        public int ID_Flight { get; set; }
        public string AgentCode { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneRemark { get; set; }
        public string BookType { get; set; }
        public double Fare { get; set; }
        public double Charge { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public string NumberOfGuests { get; set; }
        public double Total { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class DetailBookingFlightGroup
    {
        public List<BookingFlight> ListFlights { get; set; }
        public List<BookingPassenger> ListPassenger { get; set; }
    }
    public class BookingPassenger
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ID_Baggages { get; set; }
        public string ID_Booking { get; set; }
    }
    public class BookingFlight
    {
        public string ID { get; set; }
        public string RouteNo { get; set; }
        public string DepartureCode { get; set; }
        public string ArrivalCode { get; set; }
        public string FlightCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public string AirlineSystem { get; set; }
        public string FlightAirline { get; set; }
    }
}
