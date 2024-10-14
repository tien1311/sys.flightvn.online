using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Model.Models.ViewModel
{
    public class CongNoViewModel
    {
        public string MaKH { get; set; }

        public string TenDaiLy { get; set; }
        public string Ngay { get; set; }
        public string NoiDung { get; set; }
        public double SoDuDau { get; set; }
        public double SoDuCuoi { get; set; }

        public bool IsConfirmed { get; set; }

        public string Attachment { get; set; }

        public DateTime ConfrimDate { get; set; }

        public int RowID { get; set; }

        public string TuNgay { get; set; }

        public string NguoiLapBieu { get; set; }

        public string NguoiDaiDien { get; set; }

        public string ConfirmID { get; set; }

        public List<ChiTietCongNoViewModel> List_ChiTietCongNo { get; set; }
    }
    public class ActionRequest
    {
        public string rowID { get; set; }
        public string dulieu { get; set; }

    }
    public class ChiTietCongNoViewModel
    {
        public int RowID { get; set; }
        public int ConfirmID { get; set; }
        public string Ngay { get; set; }
        public string SoCT { get; set; }
        public string DienGiai { get; set; }

        public string TK131 { get; set; }

        public double GiaVe { get; set; }
        public double ChietKhau { get; set; }
        public double PhatSinh_No { get; set; }
        public double PhatSinh_Co { get; set; }

        public double SoDu_No { get; set; }
        public double SoDu_Co { get; set; }

        public string NghiepVu { get; set; }


        public string NgayMua { get; set; }






    }
    public class ReturnObjectLogin
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }

    }

    public class DuNoDaiLy
    {
        public string AgentId { get; set; }
        public string Balance { get; set; }
        public string Status { get; set; }
        public string AllowedCredit { get; set; }

        public string Allowance { get; set; }
    }

    public class Agent
    {
        public string AgentId { get; set; }

    }


}
