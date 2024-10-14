using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models
{

    public class GuiMailHangModel
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string ToAddress { get; set; }
        public string CCAddress { get; set; }
        public string NoiDung { get; set; }
        [DataType(DataType.MultilineText)]
        public string FileDinhKem { get; set; }
        public string ChuDe { get; set; }
        [DataType(DataType.MultilineText)]
        public string NgayGui { get; set; }
        public string NhanVienGui { get; set; }

        public string HtmlNoiDung { get; set; }

        public string TinhTrang { get; set; }
        public string NoiDungPhanHoi { get; set; }
        public string NgayPhanHoi { get; set; }
        public string OtherCCAddress { get; set; }
        public string TinhTrangGuiMail { get; set; }

    }
    public class DanhsachTiêude
    {
        public int RowID { get; set; }

        public string TieuDe { get; set; }

        public string NoiDungTimKiem { get; set; }
    }
    public class DanhsachTO
    {
        public int RowID { get; set; }

        public string Ten { get; set; }

        public string Hang { get; set; }
    }
    public class GuiMailHang
    {
        public List<GuiMailHangModel> Guimailhang { get; set; }
        public List<DanhsachTO> ListTo { get; set; }
        public List<DanhsachTiêude> ListTieuDe { get; set; }
        public List<DanhsachEV> ListEV { get; set; }

    }

    public class DanhsachEV
    {
        public int RowID { get; set; }

        public string Ten { get; set; }

        public string CCEV { get; set; }
    }
}
