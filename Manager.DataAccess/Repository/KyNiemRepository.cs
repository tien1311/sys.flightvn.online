using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class KyNiemRepository
    {
        DBase db = new DBase();

        public KyNiemModel DSkyniem()
        {
            KyNiemModel Kyniem = new KyNiemModel();
            List<NamModel> ds_Nam = new List<NamModel>();
            string SqlKyNiem_nam = " SELECT * FROM Nam order by So_Nam desc";
            DataTable dl_nam = db.ExecuteDataSet(SqlKyNiem_nam, CommandType.Text, "serverkyniem", null).Tables[0];
            if (dl_nam.Rows.Count > 0 && dl_nam != null)
            {
                for (int i = 0; i < dl_nam.Rows.Count; i++)
                {
                    NamModel Nam = new NamModel();
                    Nam.So_Nam = dl_nam.Rows[i]["So_Nam"].ToString();
                    Nam.Event_ID = dl_nam.Rows[i]["Event_ID"].ToString();
                    ds_Nam.Add(Nam);
                }
            }
            List<EventModel> ds_Event = new List<EventModel>();
            string SqlKyNiem_event = " SELECT * FROM Event ";
            DataTable dl_envent = db.ExecuteDataSet(SqlKyNiem_event, CommandType.Text, "serverkyniem", null).Tables[0];
            if (dl_envent.Rows.Count > 0 && dl_envent != null)
            {
                for (int i = 0; i < dl_envent.Rows.Count; i++)
                {
                    EventModel Event = new EventModel();
                    Event.Event_ID = dl_envent.Rows[i]["Event_ID"].ToString();
                    Event.Event_Name = dl_envent.Rows[i]["Event_Name"].ToString();
                    ds_Event.Add(Event);
                }
            }
            Kyniem.listnam = ds_Nam;
            Kyniem.list_event_name = ds_Event;

            return Kyniem;
        }

        public KyNiemModel kyniem(string event_id, string nam)
        {
            KyNiemModel Kyniem = new KyNiemModel();
            Kyniem = DSkyniem();
            if (event_id == null && nam == null)
            {
                var local_nam = Kyniem.listnam[0].So_Nam;

                var local_event = "Famtrip";
                Kyniem.hinh = "~/images/kyniem/" + local_event + "/" + local_nam;
                Kyniem.nam = local_nam.ToString();
                Kyniem.event_name = local_event;
            }
            else
            {
                var local_nam = nam;
                var local_event = "";
                foreach (var item in Kyniem.list_event_name)
                {
                    if (item.Event_ID == event_id) { local_event = item.Event_Name; }
                }
                Kyniem.hinh = "~/images/kyniem/" + local_event + "/" + local_nam;
                Kyniem.nam = local_nam.ToString();
                Kyniem.event_name = local_event;
            }
            return Kyniem;
        }
    }
}
