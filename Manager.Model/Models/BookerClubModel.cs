using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ThongTinBookerClubModel
    {
        public List<BookerClubDetail> ListBookerClubDetail { get; set; }
        public List<BookerClub> ListBookerClub { get; set; }
    }
    [Serializable]
    public class BookerClubDetail
    {
        public int ID { get; set; }
        public int ID_BookerClub { get; set; }
        public string MaKH { get; set; }
        public int SoLuong { get; set; }
        public string CreateEmployee { get; set; }
        public DateTime CreateDate { get; set; }
        public string ID_Booker { get; set; }
        public string TicketNumber { get; set; }
    }
    public class BookerClub
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public bool Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public int SoLuong { get; set; }

    }
}
