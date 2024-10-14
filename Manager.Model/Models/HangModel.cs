using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Manager.Model.Models
{

    public class SoDuHangViewModel
    {
        public SoDuHangModel SoDuHangModel { get; set; }
        public List<ChiTietSoDuHangModel> chiTietHangModel { get; set; }
    }

    public class SoDuHangModel
    {
        public int ID { get; set; }
        public DateTime NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public int Status { get; set; }

    }

    public class ChiTietSoDuHangModel
    {
        public int ID { get; set; }
        public int IDSoDuHang { get; set; }
        public string Hang { get; set; }
        public string SoTien { get; set; }
        public string NgayLap { get; set; }
        public decimal WarningAmount { get; set; }
    }

    public class AuthenticateModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string client_id { get; set; }
        public string username { get; set; }
    }

    public class RequestAirlineSystem
    {
        public string AirlineSystem { get; set; }

    }
    public class RequestAirlineSystem_MT
    {
        public string AirlineSystem { get; set; }
        public string BookingRegion { get; set; } = "";
    }
    public class RequestVNA
    {
        public string iata { get; set; }
    }
    public class ResponseRetrieveAgencyCredit
    {
        public decimal AgencyCreditAmount { get; set; }
        public string Result { get; set; }
        public string[] Message { get; set; }
    }
    public class ResponseRetrieveAgencyCreditVNA
    {
        public string IataCode { get; set; }
        public string creditCode { get; set; }
        public bool success { get; set; }
        public int active { get; set; }
        public decimal balance { get; set; }
        public decimal creditLimit { get; set; }
        public string Currency { get; set; }
    }
    public class AirlineAirasia
    {
        public string AgentID { get; set; }
        public decimal AvailableCredits { get; set; }
    }
    public class BalanceResponse
    {
        public string username { get; set; }
        public double balance { get; set; }
        public string currency { get; set; }
    }
    public class Config_Balance
    {
        public string Airline { get; set; }
        public decimal Amount { get; set; }
    }

}
