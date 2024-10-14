using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class SideMenuModel
    {
        public List<SideMenu_ParentModel> ListSideMenu_Parent { get; set; }
        public List<SideMenu_ChildModel> ListSideMenu_Child { get; set; }
        public List<Airport> ListAirport { get; set; }
    }
    public class SideMenu_ParentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public class SideMenu_ChildModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int IDParent { get; set; }
        public string Name_Parent { get; set; }
        public bool Status { get; set; }
        public List<SideMenu_ParentModel> ListSideMenu_Parent { get; set; }
    }
    public class Airport
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
