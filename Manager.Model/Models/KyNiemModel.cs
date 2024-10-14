using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class KyNiemModel
    {
        public string nam { get; set; } = "";
        public string event_name { get; set; } = "";
        public List<EventModel> list_event_name { get; set; }
        public List<NamModel> listnam { get; set; }
        public string hinh { get; set; }
    }
    public class NamModel
    {
        public string So_Nam { get; set; }
        public string Event_ID { get; set; }
    }
    public class EventModel
    {
        public string Event_Name { get; set; }
        public string Event_ID { get; set; }
    }

}
