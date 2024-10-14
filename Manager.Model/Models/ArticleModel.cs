using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class ArticleModel
    {
        public int ID { get; set; }
        public int IDMenuChild { get; set; }
        public string Title { get; set; }
        public string Content_Article { get; set; }
        public string CreateDate { get; set; }
        public string CreateEmployee { get; set; }
        public string UrlImage { get; set; }
        public string DanhMuc_Name { get; set; }
        public string DanhMuc_ID { get; set; }
        public List<SideMenu_ChildModel> ListSideMenu_Child { get; set; }
    }
}
