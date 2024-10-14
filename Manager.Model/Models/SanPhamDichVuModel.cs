using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class SanPhamDichVuModel
    {
        public string ID { get; set; }
        public string ID_PRODUCT { set; get; }
        public string MainImg { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Price { get; set; }
        public string PriceLogin { get; set; }
    }
    public class SanPhamChild
    {
        public string ID { get; set; }
        public string ID_PRODUCTCHILD { get; set; }
        public string ChildImg { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string PriceLogin { get; set; }
        public string IDParent { get; set; }
        public string NameParent { get; set; }
        public List<SanPhamDichVuModel> listSP { get; set; }
    }
}

