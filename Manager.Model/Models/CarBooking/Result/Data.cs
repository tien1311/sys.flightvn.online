using System.Collections.Generic;

namespace Manager.Model.Models.CarBooking.Result
{
    public class Data
    {
        public int id { get; set; }
        public List<Record> records { get; set; }
        public Pagination pagination { get; set; }
    }
}
