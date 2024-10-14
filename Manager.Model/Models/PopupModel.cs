using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{
    public class Upload_ImgModel
    {
        public Detail_Img popupSys_Img { get; set; }
        public Detail_Img bannerAirline_Img { get; set; }
        public Detail_Img bannerloginEvbay { get; set; }
        public Detail_Img bannerADVEvbay { get; set; }
        public Detail_Img bannerADVVeDoanEvbay { get; set; }
        public Detail_Img bannerADSEnvietGroup { get; set; }
        public Detail_Img bannerADSDuLich { get; set; }
    }
    public class Detail_Img
    {
        public string adv_picture { get; set; } = "";
        public string adv_linkto { get; set; } = "";
        public string adv_isshow { get; set; } = "";
        public string category_id { get; set; } = "";
    }
}
