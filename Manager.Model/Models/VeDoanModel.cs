using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class VeDoanModel
    {
        public string ID { get; set; }
        public string Hang { get; set; }
        public string HanhTrinh { get; set; }
        public string Code { get; set; }
        public double Gia { get; set; }
        public string FindID { get; set; }
        public string SoLuongKhach { get; set; }
        public string DieuKien { get; set; }
        public double Tongsotien { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public string Ghichu { get; set; }

        public string mabay { get; set; }
        public string ngaybay { get; set; }
        public string giobay { get; set; }



    }
    public class VeDoanDetail
    {
        public string MaCB { get; set; }
        public string NgayBay { get; set; }
        public string GioBay { get; set; }

    }
    public class Luuthongtin
    {
        public string ID { get; set; }

        public string Makh { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public string Hang { get; set; }
        public string Note { get; set; }
        public string SoLuongKhach { get; set; }
        public string Code { get; set; }
        public string Ngaydi { get; set; }
        public string Ngayve { get; set; }
        public string Noidi { get; set; }
        public string Noiden { get; set; }

        public string CreateDate { get; set; }

    }
    public class VeDoanAll
    {
        public List<Luuthongtin> yeucaudoan { get; set; }
        public List<VeDoanModel> vedoan { get; set; }
    }
    public class ChitietBooking
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Makh { get; set; }
        public string FindID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public string Ghichu { get; set; }
        public string Ngaygui { get; set; }
        public string Soluong { get; set; }
        public double Tongtien { get; set; }
        public string Hang { get; set; }
        public string Hanhtrinh { get; set; }

        public string FlightID { get; set; }

        public List<BookingDetail> bookingdetail { get; set; }
    }
    public class BookingDetail
    {
        public string ID { get; set; }

        public string Mabay { get; set; }
        public string Ngaybay { get; set; }
        public string giobay { get; set; }

    }
}
