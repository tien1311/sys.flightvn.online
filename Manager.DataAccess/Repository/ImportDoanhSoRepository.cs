using EasyInvoice.Json;
using Manager.Model.Models;
using Manager.Model.Models.ViewModel;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class ImportDoanhSoRepository
    {
        DBase db = new DBase();
        //Hiển thị thông tin cá nhân và thông tin hợp đồng
        public List<ImportDoanhSoViewModel> GetListDoanhSo(string filename, string thang, string nam)
        {
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int tam = 0, result = 0;
                    string MaKH = "", Tong = "", VN = "", VJ = "", QH = "", IATA = "", Khac = "";
                    while (reader.Read()) //Each row of the file
                    {
                        if (tam != 0)
                        {
                            listDoanhSo.Add(new ImportDoanhSoViewModel
                            {
                                MaKH = reader.GetValue(0).ToString(),
                                Tong = double.Parse(reader.GetValue(1).ToString()),
                                VN = double.Parse(reader.GetValue(2).ToString()),
                                VJ = double.Parse(reader.GetValue(3).ToString()),
                                QH = double.Parse(reader.GetValue(4).ToString()),
                                VU = double.Parse(reader.GetValue(5).ToString()),
                                IATA = double.Parse(reader.GetValue(6).ToString()),
                                Khac = double.Parse(reader.GetValue(7).ToString()),
                                Thang = thang,
                                Nam = nam
                            });
                        }
                        tam++;
                    }
                }
            }
            return listDoanhSo;
        }
        public bool CheckUploadDoanhSo(List<ImportDoanhSoViewModel> listDoanhSo)
        {

            string thang = "", nam = "";
            try
            {
                bool result = false;
                thang = listDoanhSo[0].Thang;
                nam = listDoanhSo[0].Nam;
                string sql = @"select Thang, Nam from DoanhThuDaiLy group by Thang, Nam";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i]["Thang"].ToString() == thang && tb.Rows[i]["Nam"].ToString() == nam)
                    {
                        result = false;
                        break;
                    }
                    else
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateDoanhSo(List<ImportDoanhSoViewModel> listDoanhSo)
        {
            int result = 0, resultDel = 0;
            string SqlInsert = "", SqlDel = "";

            try
            {
                SqlDel = @"DELETE FROM DoanhThuDaiLy WHERE Thang = '" + listDoanhSo[0].Thang + "' and Nam = '" + listDoanhSo[0].Nam + "' ";
                resultDel = db.ExecuteNoneQuery(SqlDel, CommandType.Text, "server18", null);

                for (int i = 0; i < listDoanhSo.Count; i++)
                {
                    ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();
                    doanhso.MaKH = listDoanhSo[i].MaKH;
                    doanhso.Tong = listDoanhSo[i].Tong;
                    doanhso.VN = listDoanhSo[i].VN;
                    doanhso.VJ = listDoanhSo[i].VJ;
                    doanhso.QH = listDoanhSo[i].QH;
                    doanhso.VU = listDoanhSo[i].VU;
                    doanhso.IATA = listDoanhSo[i].IATA;
                    doanhso.Khac = listDoanhSo[i].Khac;
                    doanhso.Thang = listDoanhSo[i].Thang;
                    doanhso.Nam = listDoanhSo[i].Nam;
                    SqlInsert += @"INSERT INTO DoanhThuDaiLy (MaKH, Tong, VNA, VJ, QH, IATA, KHAC, Thang, Nam, VU, NgayUp) VALUES('" + doanhso.MaKH + "', '" + doanhso.Tong + "', '" + doanhso.VN + "', '" + doanhso.VJ + "', '" + doanhso.QH + "', '" + doanhso.IATA + "', '" + doanhso.Khac + "', '" + doanhso.Thang + "', '" + doanhso.Nam + "', '" + doanhso.VU + "', GETDATE())";
                }
                result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool InsertDoanhSo(List<ImportDoanhSoViewModel> listDoanhSo)
        {
            int result = 0;
            string SqlInsert = "";
            try
            {

                for (int i = 0; i < listDoanhSo.Count; i++)
                {
                    ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();
                    doanhso.MaKH = listDoanhSo[i].MaKH;
                    doanhso.Tong = listDoanhSo[i].Tong;
                    doanhso.VN = listDoanhSo[i].VN;
                    doanhso.VJ = listDoanhSo[i].VJ;
                    doanhso.QH = listDoanhSo[i].QH;
                    doanhso.VU = listDoanhSo[i].VU;
                    doanhso.IATA = listDoanhSo[i].IATA;
                    doanhso.Khac = listDoanhSo[i].Khac;
                    doanhso.Thang = listDoanhSo[i].Thang;
                    doanhso.Nam = listDoanhSo[i].Nam;
                    SqlInsert += @"INSERT INTO DoanhThuDaiLy (MaKH, Tong, VNA, VJ, QH, IATA, KHAC, Thang, Nam, VU, NgayUp) VALUES('" + doanhso.MaKH + "', '" + doanhso.Tong + "', '" + doanhso.VN + "', '" + doanhso.VJ + "', '" + doanhso.QH + "', '" + doanhso.IATA + "', '" + doanhso.Khac + "', '" + doanhso.Thang + "', '" + doanhso.Nam + "', '" + doanhso.VU + "', GETDATE())";
                }
                result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ImportDoanhSoViewModel> TraCuuDoanhSo(string Thang, string Nam, string MaKH)
        {
            int stt = 1;
            string sql = "";
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            if (Thang != null && MaKH == null)
            {
                sql = "select * from DoanhThuDaiLy where Thang = '" + Thang + "' and Nam = '" + Nam + "' order by Thang";
            }
            if (Thang == null && MaKH != null)
            {
                sql = "select * from DoanhThuDaiLy where MaKH = '" + MaKH + "' and Nam = '" + Nam + "' order by Thang";
            }
            if (Thang != null && MaKH != null)
            {
                sql = "select * from DoanhThuDaiLy where MaKH = '" + MaKH + "' and Thang = '" + Thang + "' and Nam = '" + Nam + "' order by Thang";
            }
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();
                doanhso.STT = stt.ToString();
                doanhso.MaKH = dt.Rows[i]["MaKH"].ToString();
                doanhso.Tong = double.Parse(dt.Rows[i]["Tong"].ToString());
                doanhso.VN = double.Parse(dt.Rows[i]["VNA"].ToString().Replace(",", ""));
                doanhso.VJ = double.Parse(dt.Rows[i]["VJ"].ToString().Replace(",", ""));
                doanhso.IATA = double.Parse(dt.Rows[i]["IATA"].ToString().Replace(",", ""));
                doanhso.QH = double.Parse(dt.Rows[i]["QH"].ToString().Replace(",", ""));
                doanhso.VU = double.Parse(dt.Rows[i]["VU"].ToString().Replace(",", ""));
                doanhso.Khac = double.Parse(dt.Rows[i]["KHAC"].ToString().Replace(",", ""));
                doanhso.Thang = dt.Rows[i]["Thang"].ToString();
                doanhso.Nam = dt.Rows[i]["Nam"].ToString();
                listDoanhSo.Add(doanhso);
                stt++;
            }
            return listDoanhSo;
        }
        public DoanhSoViewModel DoanhSo(string MaNV, string Thang, string Nam)
        {
            DoanhSoViewModel DoanhSo = new DoanhSoViewModel();

            List<KinhDoanh> ListKD = new List<KinhDoanh>();
            string sqlTenKD = @"select MaNV,Ten,Yahoo from DM_NV where NVKD = '1' and TinhTrang = '1'";
            DataTable tbTenKD = db.ExecuteDataSet(sqlTenKD, CommandType.Text, "server37", null).Tables[0];
            if (tbTenKD != null)
            {
                if (tbTenKD.Rows.Count > 0)
                {
                    for (int i = 0; i < tbTenKD.Rows.Count; i++)
                    {
                        KinhDoanh KD = new KinhDoanh();
                        KD.TenNV = tbTenKD.Rows[i]["Ten"].ToString();
                        KD.MaNV = tbTenKD.Rows[i]["Yahoo"].ToString();

                        ListKD.Add(KD);
                    }
                    DoanhSo.ListKinhDoanh = ListKD;
                }
            }
            double TongCong = 0, TongVN = 0, TongVJ = 0, TongVU = 0, TongQH = 0, TongIATA = 0, TongKhac = 0;
            int stt = 1;
            string sql = "", TenKD = "";
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            if (MaNV == "ALL")
            {
                sql = @"select  KH.TENCONGTY,DS.MaKH,DS.Tong,DS.VNA,DS.VJ,DS.VU,DS.QH,DS.IATA,DS.Khac,DS.Thang, DS.Nam , NV.Ten from DM_NV NV  
                        left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MaNV 
                        left join Server37.Agent.dbo.DoanhThuDaiLy DS on DS.MaKH COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                        where NV.NVKD = '1' and NV.TinhTrang = '1' and KH.TENTINHTRANGAGENT <> 2 and DS.Thang = '" + Thang + "' and DS.Nam = '" + Nam + "' and DS.ID is not null group by KH.TENCONGTY,DS.MaKH,DS.Tong,DS.VNA,DS.VJ,DS.VU,DS.QH,DS.IATA,DS.Khac,DS.Thang, DS.Nam, NV.Ten order by DS.Tong desc ";
            }
            else
            {
                sql = @"select KH.TENCONGTY, DS.MaKH,DS.Tong,DS.VNA,DS.VJ,DS.VU,DS.QH,DS.IATA,DS.Khac,DS.Thang, DS.Nam , NV.Ten from DM_NV NV  
                        left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MaNV 
                        left join Server37.Agent.dbo.DoanhThuDaiLy DS on DS.MaKH COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                        where NV.Yahoo = '" + MaNV + "' and KH.TENTINHTRANGAGENT <> 2 and DS.Thang = '" + Thang + "' and DS.Nam = '" + Nam + "' and DS.ID is not null group by KH.TENCONGTY,DS.MaKH,DS.Tong,DS.VNA,DS.VJ,DS.VU,DS.QH,DS.IATA,DS.Khac,DS.Thang, DS.Nam, NV.Ten order by DS.Tong desc ";
            }

            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();
                doanhso.STT = stt.ToString();
                doanhso.MaKH = dt.Rows[i]["MaKH"].ToString();
                doanhso.TenCongTy = dt.Rows[i]["TENCONGTY"].ToString();
                doanhso.Tong = double.Parse(dt.Rows[i]["Tong"].ToString());
                doanhso.VN = double.Parse(dt.Rows[i]["VNA"].ToString().Replace(",", ""));
                doanhso.VJ = double.Parse(dt.Rows[i]["VJ"].ToString().Replace(",", ""));
                doanhso.IATA = double.Parse(dt.Rows[i]["IATA"].ToString().Replace(",", ""));
                doanhso.QH = double.Parse(dt.Rows[i]["QH"].ToString().Replace(",", ""));
                doanhso.VU = double.Parse(dt.Rows[i]["VU"].ToString().Replace(",", ""));
                doanhso.Khac = double.Parse(dt.Rows[i]["KHAC"].ToString().Replace(",", ""));
                doanhso.Thang = dt.Rows[i]["Thang"].ToString();
                doanhso.Nam = dt.Rows[i]["Nam"].ToString();
                TongVU += doanhso.VU;
                TongVN += doanhso.VN;
                TongVJ += doanhso.VJ;
                TongQH += doanhso.QH;
                TongIATA += doanhso.IATA;
                TongKhac += doanhso.Khac;
                TongCong += doanhso.Tong;
                if (MaNV == "ALL")
                {
                    TenKD = MaNV;
                }
                else
                {
                    TenKD = dt.Rows[0]["Ten"].ToString();
                }
                listDoanhSo.Add(doanhso);
                stt++;
            }
            DoanhSo.ListDoanhSo = listDoanhSo;
            DoanhSo.TenKD = TenKD;
            DoanhSo.TongCong = TongCong;
            DoanhSo.TongIATA = TongIATA;
            DoanhSo.TongKhac = TongKhac;
            DoanhSo.TongQH = TongQH;
            DoanhSo.TongVJ = TongVJ;
            DoanhSo.TongVN = TongVN;
            DoanhSo.TongVU = TongVU;
            return DoanhSo;
        }
        public List<CongNoViewModel> LayCongNoDaiLy(string maKH, string thang, string nam)
        {
            try
            {

                List<CongNoViewModel> result = new List<CongNoViewModel>();
                CongNoViewModel congNo = new CongNoViewModel();
                string sql_congno = "Select * from CONGNO_DAILY_CONFIRM where IsConfirmed=0 and Ngay is not null and month(DATEADD(month, -1, Ngay))='" + thang + "' and year(Ngay)='" + nam + "'";

                DataTable tbl_congno = db.ExecuteDataSet(sql_congno, CommandType.Text, "server37", null).Tables[0];
                if (tbl_congno != null && tbl_congno.Rows.Count > 0)
                {
                    for (int i = 0; i < tbl_congno.Rows.Count; i++)
                    {
                        congNo = new CongNoViewModel();
                        congNo.RowID = int.Parse(tbl_congno.Rows[i]["RowID"].ToString());
                        congNo.MaKH = tbl_congno.Rows[i]["MaKH"].ToString();
                        congNo.Ngay = DateTime.Parse(tbl_congno.Rows[i]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                        congNo.NoiDung = tbl_congno.Rows[i]["NoiDung"].ToString();
                        congNo.SoDuDau = double.Parse(tbl_congno.Rows[i]["SoDuDau"].ToString());
                        congNo.SoDuCuoi = double.Parse(tbl_congno.Rows[i]["SoDuCuoi"].ToString());
                        congNo.Attachment = tbl_congno.Rows[i]["Attachment"].ToString();
                        congNo.IsConfirmed = bool.Parse(tbl_congno.Rows[i]["IsConfirmed"].ToString());

                        result.Add(congNo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public CongNoViewModel LayChiTietCongNo(int RowID)
        {
            try
            {

                CongNoViewModel result = new CongNoViewModel();
                List<ChiTietCongNoViewModel> List_CT_CongNo = new List<ChiTietCongNoViewModel>();
                ChiTietCongNoViewModel chitietcongNo = new ChiTietCongNoViewModel();

                //Thông tin công nợ
                string sql = @"select * from CONGNO_DAILY_CONFIRM where RowID = '" + RowID + "'";
                DataTable tbl_congno = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tbl_congno != null && tbl_congno.Rows.Count > 0)
                {
                    string sql_tendaily = @"select * from member where member_isactive = 1 and member_kh='" + tbl_congno.Rows[0]["MaKH"].ToString() + "'";
                    result.RowID = int.Parse(tbl_congno.Rows[0]["RowID"].ToString());
                    result.MaKH = tbl_congno.Rows[0]["MaKH"].ToString();
                    result.Ngay = DateTime.Parse(tbl_congno.Rows[0]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                    result.NoiDung = tbl_congno.Rows[0]["NoiDung"].ToString();
                    result.SoDuDau = double.Parse(tbl_congno.Rows[0]["SoDuDau"].ToString());
                    result.SoDuCuoi = double.Parse(tbl_congno.Rows[0]["SoDuCuoi"].ToString());
                    result.Attachment = tbl_congno.Rows[0]["Attachment"].ToString();
                    //     result.NguoiDaiDien = NguoiDaiDien;
                    result.ConfirmID = RowID.ToString();
                    result.NguoiLapBieu = tbl_congno.Rows[0]["KeToanQuanLy"].ToString();

                    result.TuNgay = DateTime.Parse(tbl_congno.Rows[0]["Ngay"].ToString()).AddMonths(-1).ToString("dd/MM/yyyy");
                }


                //Chi tiết công nợ
                string sql_congno = $@"EXEC SP_GET_CHITIETCONGNODAILY @ConfirmID='{RowID}'";
                DataTable tbl_chitietcongno = db.ExecuteDataSet(sql_congno, CommandType.Text, "server37", null).Tables[0];
                if (tbl_chitietcongno != null && tbl_chitietcongno.Rows.Count > 0)
                {
                    for (int i = 0; i < tbl_chitietcongno.Rows.Count; i++)
                    {
                        chitietcongNo = new ChiTietCongNoViewModel();
                        chitietcongNo.RowID = int.Parse(tbl_chitietcongno.Rows[i]["RowID"].ToString());
                        chitietcongNo.Ngay = DateTime.Parse(tbl_chitietcongno.Rows[i]["Ngay"].ToString()).ToString("dd/MM/yyyy");
                        chitietcongNo.SoCT = tbl_chitietcongno.Rows[i]["SoCT"].ToString();
                        chitietcongNo.DienGiai = tbl_chitietcongno.Rows[i]["DienGiai"].ToString();
                        chitietcongNo.TK131 = tbl_chitietcongno.Rows[i]["TK131"].ToString();
                        chitietcongNo.GiaVe = double.Parse(tbl_chitietcongno.Rows[i]["GiaVe"].ToString());
                        chitietcongNo.ChietKhau = double.Parse(tbl_chitietcongno.Rows[i]["ChietKhau"].ToString());
                        chitietcongNo.PhatSinh_No = double.Parse(tbl_chitietcongno.Rows[i]["PhatSinh_No"].ToString());
                        chitietcongNo.PhatSinh_Co = double.Parse(tbl_chitietcongno.Rows[i]["PhatSinh_Co"].ToString());
                        chitietcongNo.SoDu_No = double.Parse(tbl_chitietcongno.Rows[i]["SoDu_No"].ToString());
                        chitietcongNo.SoDu_Co = double.Parse(tbl_chitietcongno.Rows[i]["SoDu_Co"].ToString());
                        chitietcongNo.NghiepVu = tbl_chitietcongno.Rows[i]["NghiepVu"].ToString();
                        chitietcongNo.NgayMua = DateTime.Parse(tbl_chitietcongno.Rows[i]["NgayMua"].ToString()).ToString("dd/MM/yyyy");
                        List_CT_CongNo.Add(chitietcongNo);
                    }
                    result.List_ChiTietCongNo = List_CT_CongNo;
                }




                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool UpdateTinhTrang(int rowID, string tenFile)
        {
            string sql = @"update CONGNO_DAILY_CONFIRM set IsConfirmed = 1,ConfrimDate=GetDate(),Auto='1',Attachment='" + tenFile + "'where RowID = " + rowID;
            int result = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
            if (result > 0)
            {
                return true;
            }
            return false;

        }

        public string GetTenFile(int rowID)
        {
            string sql = @"select top 1 thang=Month(DateAdd(mm,-1,Ngay)),nam=Year(DateAdd(mm,-1,Ngay)),MaKH from CONGNO_DAILY_CONFIRM where RowID = " + rowID;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

            return "CN" + tb.Rows[0]["thang"].ToString() + tb.Rows[0]["nam"].ToString() + "-" + tb.Rows[0]["MaKH"].ToString() + ".pdf";
        }
        public ListTieuDe GetNoiDung(int id)
        {
            ListTieuDe result = new ListTieuDe();
            string sql_NoiDung = " SELECT * FROM QLTHONGBAO WHERE RowID = " + id;
            // string sql_NoiDung = " SELECT * FROM QLTHONGBAO WHERE PHANMEM ='Sys.GuiMailKinhDoanh'";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    result.NoiDungTimKiem = dt.Rows[0]["NoiDung"].ToString();
                    result.TieuDe = dt.Rows[0]["TieuDe"].ToString();

                }
            }



            return result;
        }
        public GuiMailDaiLyModel DanhSachMail(string MaNV, string dafr, string dato)
        {
            try
            {

                GuiMailDaiLyModel result = new GuiMailDaiLyModel();
                List<GuiMailDaiLyKDModel> CV = new List<GuiMailDaiLyKDModel>();
                GuiMailDaiLyKDModel guimail = new GuiMailDaiLyKDModel();
                string sql = @"select * from SendMailAgent_KD  where NHANVIEN='" + MaNV + "' and NGAYGUI > '" + dafr + "' and NGAYGUI <='" + dato + " 23:59:59' ORDER BY NGAYGUI DESC ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        guimail = new GuiMailDaiLyKDModel();
                        guimail.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                        guimail.MAKH = tb.Rows[i]["MAKH"].ToString();
                        guimail.DAILY = tb.Rows[i]["TENDAILY"].ToString();
                        guimail.MAIL = tb.Rows[i]["MAIL"].ToString();
                        guimail.MAILCC = tb.Rows[i]["MAILCC"].ToString();
                        guimail.NGAYGUI = DateTime.Parse(tb.Rows[i]["NGAYGUI"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        guimail.NGUOIGUI = tb.Rows[i]["NGUOIGUI"].ToString();
                        CV.Add(guimail);
                    }
                    result.Guimailkinhdoanh = CV;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public GuiMailDaiLyModel DanhSachMailAll(string dafr, string dato)
        {
            try
            {

                GuiMailDaiLyModel result = new GuiMailDaiLyModel();
                List<GuiMailDaiLyKDModel> CV = new List<GuiMailDaiLyKDModel>();
                GuiMailDaiLyKDModel guimail = new GuiMailDaiLyKDModel();
                string sql = @"select * from SendMailAgent_KD  where NGAYGUI > '" + dafr + "' and NGAYGUI <='" + dato + " 23:59:59' ORDER BY NGAYGUI DESC ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        guimail = new GuiMailDaiLyKDModel();
                        guimail.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                        guimail.MAKH = tb.Rows[i]["MAKH"].ToString();
                        guimail.DAILY = tb.Rows[i]["TENDAILY"].ToString();
                        guimail.MAIL = tb.Rows[i]["MAIL"].ToString();
                        guimail.MAILCC = tb.Rows[i]["MAILCC"].ToString();
                        guimail.NGAYGUI = DateTime.Parse(tb.Rows[i]["NGAYGUI"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        guimail.NGUOIGUI = tb.Rows[i]["NGUOIGUI"].ToString();
                        CV.Add(guimail);
                    }
                    result.Guimailkinhdoanh = CV;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public GuiMailHang DanhSachMailHang(string dafr, string dato)
        {
            try
            {

                GuiMailHang result = new GuiMailHang();
                List<GuiMailHangModel> CV = new List<GuiMailHangModel>();
                GuiMailHangModel guimail = new GuiMailHangModel();
                string sql = @"select * from GuiMailSC  where NgayGui > '" + dafr + "' and NgayGui <='" + dato + " 23:59:59' ORDER BY NgayGui DESC ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null && tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        guimail = new GuiMailHangModel();
                        guimail.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                        guimail.ToAddress = tb.Rows[i]["ToAddress"].ToString();
                        guimail.CCAddress = tb.Rows[i]["CCAddress"].ToString();
                        guimail.OtherCCAddress = tb.Rows[i]["OtherCCAddress"].ToString();
                        guimail.NoiDung = tb.Rows[i]["NoiDung"].ToString();
                        guimail.ChuDe = tb.Rows[i]["ChuDe"].ToString();
                        guimail.NgayGui = DateTime.Parse(tb.Rows[i]["NgayGui"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        guimail.NhanVienGui = tb.Rows[i]["NhanVienGui"].ToString();
                        CV.Add(guimail);
                    }
                    result.Guimailhang = CV;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
