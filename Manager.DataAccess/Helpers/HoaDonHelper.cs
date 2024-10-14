using EasyInvoice.Client;
using EasyInvoice.Client.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Manager.Model.Models.SharedModels;
using Manager.Common.Helpers;

namespace Manager.DataAccess.Helpers
{

    public static class HoaDonHelper
    {
        #region Các biến khởi tạo
        static DBase db = new DBase();
        public static string apiHost = Settings.Get("easyInvoice_apiHost");
        public static string apiUname = Settings.Get("easyInvoice_apiUsername");
        public static string apiPwd = Settings.Get("easyInvoice_apiPassword");

        static EasyService easyService = new EasyService();

        public static SelectList lst_HinhThucThanhToan = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem {Text="TM/CK",Value = "3"},
                new SelectListItem {Text="Chuyển khoản",Value = "2"}

            }, "Value", "Text", 1
            );
        #endregion

        //Kiểm tra phần mềm có bị khóa ko
        public static bool CheckKhoaHD(string phanMem)
        {
            bool res = false;
            try
            {
                string sql = $@"SELECT 1 as 'Khoa' FROM KHOA_XUATHOADON WHERE KHOA='1' OR KHOA{phanMem}='1'";
                DataTable table = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (table.Rows.Count > 0)
                {
                    res = true;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }





        //Kiểm tra số vé trùng
        public static string CheckTrung(string sove)
        {
            string result = "";
            string sql_trung = $"EXEC SP_CHECK_TRUNG_SOVE @SoVe={sove}";
            DataTable tb = db.ExecuteDataSet(sql_trung, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                result = "Số vé: " + tb.Rows[0]["SOVE"].ToString() + " đã tồn tại trong số hóa đơn: " + tb.Rows[0]["SOCT"].ToString() + " KHHĐ: " + tb.Rows[0]["KHHOADON"].ToString();
                goto Finish;
            }
            string sql_trungYC = $"EXEC SP_CHECK_TRUNG_YC_SOVE @SoVe={sove}";
            tb = db.ExecuteDataSet(sql_trungYC, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                result = "Số vé: " + tb.Rows[0]["TENHH"].ToString() + " đã tồn tại trong mã yêu cầu: " + tb.Rows[0]["MAYEUCAU"].ToString();
                goto Finish;
            }
        Finish:
            return result;
        }

        //Kiểm tra xem đại lý có bị khóa xuất hóa đơn ko
        public static bool CheckDaiLyKhoaXuatHD(string maKH)
        {
            bool res = false;
            DataTable tbl = db.ExecuteDataSet($"EXEC SP_GET_KHOA_DAILY_XUATHD @MaKH='{maKH}',@LoaiKhoa='0'", CommandType.Text, "server37", null).Tables[0];
            res = Convert.ToBoolean(int.Parse(tbl.Rows[0]["Result"].ToString()));
            return res;
        }

        //Kiểm tra xem đại lý có bị khóa gửi yêu cầu xuất HD ko
        public static bool CheckDaiLyKhoaYeuCau(string maKH)
        {
            bool res = false;
            DataTable tbl = db.ExecuteDataSet($"EXEC SP_GET_KHOA_DAILY_XUATHD @MaKH='{maKH}',@LoaiKhoa='1'", CommandType.Text, "server37", null).Tables[0];
            res = Convert.ToBoolean(int.Parse(tbl.Rows[0]["Result"].ToString()));
            return res;
        }

        /// <summary>
        /// Lấy dữ liệu vé từ server
        /// </summary>
        /// <param name="sp_name">Tên SP cần gọi đến</param>
        /// <param name="sove">Số vé</param>
        /// <returns></returns>


        /// <summary>
        /// Tạo file PDF, lấy từ bên easyInvoice
        /// </summary>
        /// <param name="xxx"></param>
        /// <param name="xxx2"></param>
        public static EasyResponse GeneratePDF(string iKey, string pattern, int option = 0)
        {
            Request req = new Request() { Ikey = iKey, Option = option, Pattern = pattern };
            //Response resP = easyService.GetInvoicePdf(req, path, apiHost, apiUname, apiPwd);

            EasyResponse resP = easyService.PostJsonObject(req, "api/publish/getInvoicePdf", apiHost, apiUname, apiPwd);
            return resP;
        }

        public static decimal TongDonGia(List<ChiTietVeHoaDon> list_ve)
        {
            decimal result = 0;
            foreach (var item in list_ve)
            {
                result += item.DonGia;
            }
            return result;
        }

        public static decimal TongTienVat(List<ChiTietVeHoaDon> list_ve)
        {
            decimal result = 0;
            foreach (var item in list_ve)
            {
                result += item.Thue;
            }
            return result;
        }

        public static decimal TongTienPhiKhac(List<ChiTietVeHoaDon> list_ve)
        {
            decimal result = 0;
            foreach (var item in list_ve)
            {
                result += item.ThueKhac;
            }
            return result;
        }

        public static decimal TongTienPhiDoi(List<ChiTietVeHoaDon> list_ve)
        {
            decimal result = 0;
            foreach (var item in list_ve)
            {
                result += item.PhiDoi;
            }
            return result;
        }

        #region Easy Invoice
        /// <summary>
        /// Lấy IKey mới
        /// </summary>
        /// <param name="loai"></param>
        /// <param name="kyhieu"></param>
        /// <returns></returns>
        public static string GetIKey(string loai, string kyhieu)
        {
            string result = "";
            try
            {
                List<DBase.AddParameters> Para = new List<DBase.AddParameters>();
                Para.Add(new DBase.AddParameters("@Loai", loai));
                Para.Add(new DBase.AddParameters("@KH", kyhieu));
                result = db.ExecuteDataSet("SP_GET_IKEY", CommandType.StoredProcedure, "server37", Para).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra trạng thái HDDT
        /// </summary>
        /// <param name="iKey"></param>
        /// <returns></returns>
        public static int CheckStatusHD(string iKey)
        {
            try
            {
                string json = "{\"Ikeys\":[\"" + iKey + "\"]}";
                EasyResponse<InvoiceStatus_Response> resP = easyService.PostJsonObject<InvoiceStatus_Response>(json, "api/publish/checkInvoiceState", apiHost, apiUname, apiPwd);
                int stats = -1;
                resP.Data.Data.KeyInvoiceMsg.TryGetValue(iKey, out stats);
                return stats;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static Response GetInvoicesKey(string[] iKeys)
        {

            Request req1 = new Request() { Ikeys = iKeys };
            Response result = easyService.GetInvoicesByIkeys(req1, apiHost, apiUname, apiPwd);
            return result;
        }

        public class InvoiceStatus_Response
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public Data Data { get; set; }
        }
        public class Data
        {
            public IDictionary<string, int> KeyInvoiceMsg { get; set; }
        }

        #endregion


        #region Đọc số tiền
        public static string DocSoTien(decimal gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod 
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            try
            {

                if (tach3.Equals("000"))
                    return "";
                if (tach3.Length == 3)
                {
                    string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                    string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                    string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                    if (tr.Equals("0") && ch.Equals("0"))
                        Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                    if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                        Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                    if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                        Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                    if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                        Ktach = " không trăm mười ";
                    if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                        Ktach = " không trăm mười lăm ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

                }
            }
            catch (Exception ex)
            {

                new DBase().LogErrorData(Session.TenDangNhap, "EVManager", "frmHoaDonDienTu", tach3 + Environment.NewLine + ex.ToString());
                Ktach = " Âm ";
            }

            return Ktach;

        }
        #endregion

        public static string GetConfig(string config_Key, string maKH)
        {

            string res = "";
            string sp = "SP_LOAD_CAUHINH_HDDL";
            List<DBase.AddParameters> paras = new List<DBase.AddParameters>();
            paras.Add(new DBase.AddParameters(@"Key", config_Key));
            DataTable tbl = db.ExecuteDataSet(sp, CommandType.StoredProcedure, "server37", paras).Tables[0];
            foreach (DataRow row in tbl.Rows)
            {
                if (row["MaKH"].ToString().Contains(maKH))
                {
                    res = row["Value"].ToString();
                    return res;
                }
                if (string.IsNullOrEmpty(row["MaKH"].ToString()))
                {
                    res = row["Value"].ToString();
                }
            }
            return res;
        }


    }

}
