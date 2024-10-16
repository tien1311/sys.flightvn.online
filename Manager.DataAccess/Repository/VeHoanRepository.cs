using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Web;

using System.Net;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class VeHoanRepository
    {
        DBase db = new DBase();
        Mail baoVH = new Mail("EVM_HOANVE");
        string server_Agent_Main; /*"Data Source=.;Initial Catalog=Agent;User ID=sa;Password=Ngominhtien@13;";*/

        public VeHoanRepository(IConfiguration configuration)
        {
            server_Agent_Main = configuration.GetConnectionString("SQL_Agent_MAIN");
        }

        public List<VeHoanModel> DSVeHoan(string TenDangNhap)
        {
            string SRefurnd = "", sqlVe = "";
            int stt = 1;
            List<VeHoanModel> ListVeHoan = new List<VeHoanModel>();
            SRefurnd = Refurnd(TenDangNhap);
            if (SRefurnd == "1")
            {
                sqlVe = "SELECT top 15000 code_ticket,SoVeEMD,subject_isshow,subject_ishot,subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE section_id in(SELECT section_id from subject_section where parent_id ='76')  AND  (1=1 AND subject_isshow='1') or (subject_isshow = 8  AND GETDATE() > NGAYHOAN  ) ORDER BY subject_isshow, NgayHoan DESC, subject_id DESC";
            }
            else
            {
                sqlVe = "SELECT top 15000 code_ticket,SoVeEMD,subject_isshow,subject_ishot,subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject_isshow in ('1') AND subject.section_id in (" + SRefurnd + ") AND  (1=1 AND subject_isshow='1') or (subject_isshow = 8  AND GETDATE() > NGAYHOAN  ) ORDER BY subject_isshow, NgayHoan DESC, subject_id DESC";
            }
            DataTable tbVe = db.ExecuteDataSet(sqlVe, CommandType.Text, "server18", null).Tables[0];

            if (tbVe != null)
            {
                for (int i = 0; i < tbVe.Rows.Count; i++)
                {
                    VeHoanModel VeHoan = new VeHoanModel();
                    VeHoan.STT = stt;
                    VeHoan.subject_id = tbVe.Rows[i]["subject_id"].ToString();
                    VeHoan.ID_Hoan = tbVe.Rows[i]["code_ticket"].ToString();
                    VeHoan.TenDL = FormatContentNews(tbVe.Rows[i]["subject_name"].ToString());
                    VeHoan.GhiChu = tbVe.Rows[i]["subject_picnote"].ToString();
                    VeHoan.TinhTrang = ReturnTinhTrang(tbVe.Rows[i]["subject_isnew"].ToString());
                    VeHoan.subject_isnew = tbVe.Rows[i]["subject_isnew"].ToString();
                    VeHoan.NgayGui = tbVe.Rows[i]["subject_date"].ToString();
                    VeHoan.NgayHoan = tbVe.Rows[i]["NgayHoanVe"].ToString();
                    VeHoan.NgayXuLy = tbVe.Rows[i]["subject_update"].ToString();
                    VeHoan.EMD = tbVe.Rows[i]["SoVeEMD"].ToString();
                    VeHoan.GiaTri = ReturnGiaTri(tbVe.Rows[i]["subject_isshow"].ToString(), tbVe.Rows[i]["subject_ishot"].ToString(), tbVe.Rows[i]["subject_com"].ToString());
                    VeHoan.Hang = tbVe.Rows[i]["section_name"].ToString();
                    VeHoan.NguoiXuLy = tbVe.Rows[i]["subject_updateby"].ToString();
                    ListVeHoan.Add(VeHoan);
                    stt++;
                }
            }
            return ListVeHoan;
        }
        public List<VeHoanModel> DSVeDangHoan(string TenDangNhap)
        {
            string SRefurnd = "", sqlVe = "";
            int stt = 1;
            List<VeHoanModel> ListVeHoan = new List<VeHoanModel>();
            SRefurnd = Refurnd(TenDangNhap);
            if (SRefurnd == "1")
            {
                sqlVe = "SELECT code_ticket,SoVeEMD,subject_isshow,subject_ishot,subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE section_id in(SELECT section_id from subject_section where parent_id ='76')  AND  (1=1 AND subject_isshow in (2,7)) AND Year(subject_date) >= 2023 ORDER BY subject_isshow, NgayHoan DESC, subject_id DESC";
            }
            else
            {
                sqlVe = "SELECT code_ticket,SoVeEMD,subject_isshow,subject_ishot,subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject.section_id in (" + SRefurnd + ") AND  (1=1 AND subject_isshow in (2,7)) AND Year(subject_date) >= 2023 ORDER BY subject_isshow, NgayHoan DESC, subject_id DESC";
            }
            DataTable tbVe = db.ExecuteDataSet(sqlVe, CommandType.Text, "server18", null).Tables[0];

            if (tbVe != null)
            {
                for (int i = 0; i < tbVe.Rows.Count; i++)
                {
                    VeHoanModel VeHoan = new VeHoanModel();
                    VeHoan.STT = stt;
                    VeHoan.subject_id = tbVe.Rows[i]["subject_id"].ToString();
                    VeHoan.ID_Hoan = tbVe.Rows[i]["code_ticket"].ToString();
                    VeHoan.TenDL = FormatContentNews(tbVe.Rows[i]["subject_name"].ToString());
                    VeHoan.GhiChu = tbVe.Rows[i]["subject_picnote"].ToString();
                    VeHoan.TinhTrang = ReturnTinhTrang(tbVe.Rows[i]["subject_isnew"].ToString());
                    VeHoan.subject_isnew = tbVe.Rows[i]["subject_isnew"].ToString();
                    VeHoan.NgayGui = tbVe.Rows[i]["subject_date"].ToString();
                    VeHoan.NgayHoan = tbVe.Rows[i]["NgayHoanVe"].ToString();
                    VeHoan.NgayXuLy = tbVe.Rows[i]["subject_update"].ToString();
                    VeHoan.EMD = tbVe.Rows[i]["SoVeEMD"].ToString();
                    VeHoan.GiaTri = ReturnGiaTri(tbVe.Rows[i]["subject_isshow"].ToString(), tbVe.Rows[i]["subject_ishot"].ToString(), tbVe.Rows[i]["subject_com"].ToString());
                    VeHoan.Hang = tbVe.Rows[i]["section_name"].ToString();
                    VeHoan.NguoiXuLy = tbVe.Rows[i]["subject_updateby"].ToString();
                    ListVeHoan.Add(VeHoan);
                    stt++;
                }
            }
            return ListVeHoan;
        }
        public Task<string> SoLuongVeHoanAsync()
        {
            VeHoanModel veHoan = new VeHoanModel();
            string result = "";
            try
            {
                string sql = @"SELECT count(*) as SoLuongVeHoan FROM subject WHERE section_id in(SELECT section_id from subject_section where parent_id ='76')  AND  (1=1 AND subject_isshow='1') or (subject_isshow = 8  AND GETDATE() > NGAYHOAN  ) ";
                using (var conn = new SqlConnection(server_Agent_Main))
                {
                    veHoan = conn.QueryFirst<VeHoanModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                result = veHoan.SoLuongVeHoan;
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }

        }
        public Task<string> SoLuongVeDangHoanAsync()
        {
            VeHoanModel veHoan = new VeHoanModel();
            string result = "";
            try
            {

                string sql = @"SELECT count(*) as SoLuongVeDangHoan
                            FROM subject 
                            WHERE  1=1  AND subject_isshow in (2,7) AND Year(subject_date) >= 2023  AND section_id in(SELECT section_id from subject_section where parent_id ='76')";
                using (var conn = new SqlConnection(server_Agent_Main))
                {
                    veHoan = conn.QueryFirst<VeHoanModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                result = veHoan.SoLuongVeDangHoan;
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }

        }
        public string FormatContentNews(string value)
        {
            string _value = value;
            if (_value.Length >= 40)
            {
                string ValueCut = _value.Substring(0, 40 - 3);
                string[] valuearray = ValueCut.Split(' ');
                string valuereturn = "";
                for (int i = 0; i < valuearray.Length - 1; i++)
                {
                    valuereturn = valuereturn + " " + valuearray[i];
                }
                return valuereturn + "...";
            }
            else
            {
                return _value;
            }
        }
        public List<VeHoanModel> SearchVeHoan(string loaive, string Dieukien, string Giatri, string tinhtrang, string cal_from, string cal_to, string nguoixuly, string vedenhan, string TenDangNhap)
        {
            string SRefurnd = "", sqlVe = "";
            int stt = 1;
            List<VeHoanModel> ListVeHoan = new List<VeHoanModel>();
            SRefurnd = Refurnd(TenDangNhap);
            if (vedenhan == "on")
            {
                sqlVe = @"SELECT  top 15000  code_ticket, SoVeEMD, subject_isshow, subject_ishot, subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] FROM subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update, convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject_isshow = 8  AND GETDATE() > NGAYHOAN   ORDER BY NgayHoan DESC,subject_id DESC";
            }
            else
            {
                string param = Param(loaive, Dieukien, Giatri, tinhtrang, cal_from, cal_to, nguoixuly);
                if (SRefurnd == "1")
                {
                    sqlVe = "SELECT  top 15000 code_ticket, SoVeEMD,subject_isshow,subject_ishot, subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE " + param + " AND section_id in(SELECT section_id from subject_section where parent_id ='76') ORDER BY subject_id DESC";
                }
                else
                {
                    sqlVe = "SELECT  top 15000 code_ticket,SoVeEMD,subject_isshow, subject_ishot, subject_com,subject_isnew,subject_id,subject_name,subject_number,subject_picnote,CONVERT(VARCHAR(10),subject_date,103)+ ' '+convert(varchar(10),subject_date,108) as subject_date,subject_updateby,convert(varchar(10),subject_time,103)+' '+convert(varchar(10),subject_time,108) as subject_time,(select top 1 [section_name] FROM subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as subject_update,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE " + param + " AND subject.section_id in (" + SRefurnd + ")   ORDER BY subject_id DESC";
                }
            }

            DataTable tbVe = db.ExecuteDataSet(sqlVe, CommandType.Text, "server18", null).Tables[0];

            if (tbVe != null)
            {
                for (int i = 0; i < tbVe.Rows.Count; i++)
                {
                    VeHoanModel VeHoan = new VeHoanModel();
                    VeHoan.STT = stt;
                    VeHoan.subject_id = tbVe.Rows[i]["subject_id"].ToString();
                    VeHoan.ID_Hoan = tbVe.Rows[i]["code_ticket"].ToString();
                    VeHoan.TenDL = FormatContentNews(tbVe.Rows[i]["subject_name"].ToString());
                    VeHoan.GhiChu = tbVe.Rows[i]["subject_picnote"].ToString();
                    VeHoan.TinhTrang = ReturnTinhTrang(tbVe.Rows[i]["subject_isnew"].ToString());
                    VeHoan.NgayGui = tbVe.Rows[i]["subject_date"].ToString();
                    VeHoan.NgayHoan = tbVe.Rows[i]["NgayHoanVe"].ToString();
                    VeHoan.NgayXuLy = tbVe.Rows[i]["subject_update"].ToString();
                    VeHoan.EMD = tbVe.Rows[i]["SoVeEMD"].ToString();
                    VeHoan.GiaTri = ReturnGiaTri(tbVe.Rows[i]["subject_isshow"].ToString(), tbVe.Rows[i]["subject_ishot"].ToString(), tbVe.Rows[i]["subject_com"].ToString());
                    VeHoan.Hang = tbVe.Rows[i]["section_name"].ToString();
                    VeHoan.NguoiXuLy = tbVe.Rows[i]["subject_updateby"].ToString();
                    ListVeHoan.Add(VeHoan);
                    stt++;
                }
            }
            return ListVeHoan;
        }
        public SubjectModel ChiTietVeHoan(int? id, string tendangnhap)
        {
            string Sql = " SELECT *,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as ngayxuly,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject_id='" + id + "' ";
            DataTable dt = db.ExecuteDataSet(Sql, CommandType.Text, "server18", null).Tables[0];
            SubjectModel subject = new SubjectModel();
            if (dt.Rows.Count > 0)
            {
                subject.subject_name = dt.Rows[0]["subject_name"].ToString();
                string[] subject_name_en = dt.Rows[0]["subject_name_en"].ToString().Split("-");
                subject.NguoiGui = subject_name_en[0].ToString();
                subject.SoDienThoai = subject_name_en[1].ToString();
                subject.subject_code = dt.Rows[0]["subject_code"].ToString();
                subject.subject_number = dt.Rows[0]["subject_number"].ToString();
                subject.subject_content = dt.Rows[0]["subject_content"].ToString();
                subject.subject_date = dt.Rows[0]["subject_date"].ToString();
                subject.subject_picnote = dt.Rows[0]["subject_picnote"].ToString();
                subject.subject_com = int.Parse(dt.Rows[0]["subject_com"].ToString());
                if (dt.Rows[0]["NgayBay"].ToString() != "")
                {
                    subject.NgayBay = DateTime.Parse(dt.Rows[0]["NgayBay"].ToString()).ToString("dd/MM/yyyy");
                }
                subject.SoNgayHoan = dt.Rows[0]["SoNgayHoan"].ToString();
                subject.subject_id = int.Parse(dt.Rows[0]["subject_id"].ToString());
                subject.subject_isshow = int.Parse(dt.Rows[0]["subject_isshow"].ToString());
                if (dt.Rows[0]["IsCheckEMD"].ToString() != "")
                {
                    subject.IsCheckEMD = bool.Parse(dt.Rows[0]["IsCheckEMD"].ToString());
                }
                if (dt.Rows[0]["IsCheckHoan"].ToString() != "")
                {
                    subject.IsCheckHoan = bool.Parse(dt.Rows[0]["IsCheckHoan"].ToString());
                }
                subject.subject_ishot = int.Parse(dt.Rows[0]["subject_ishot"].ToString());
                subject.subject_isnew = int.Parse(dt.Rows[0]["subject_isnew"].ToString());
                subject.subject_status_daily = dt.Rows[0]["subject_status_daily"].ToString();

                subject.subject_content = dt.Rows[0]["subject_content"].ToString();
                subject.SoVeEMD = dt.Rows[0]["SoVeEMD"].ToString();
                subject.TenKhachEMD = dt.Rows[0]["TenKhachHang"].ToString();
                subject.Remark = Remark();

                subject.code_ticket = dt.Rows[0]["code_ticket"].ToString();

                if (dt.Rows[0]["NgayHuyHanhTrinh"].ToString() != "" && dt.Rows[0]["NgayHuyHanhTrinh"].ToString() != "01/01/1900 12:00:00 SA")
                {
                    subject.NoiDungHuyHanhTrinh = "Đại lý đã xác nhận hủy hành trình, ngày giờ hủy " + DateTime.Parse(dt.Rows[0]["NgayHuyHanhTrinh"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    subject.NoiDungHuyHanhTrinh = "";
                }
                if (dt.Rows[0]["NgayHoanVe"].ToString() != "01/01/1900")
                {
                    subject.NgayHoan = dt.Rows[0]["NgayHoanVe"].ToString();
                }
                else
                {
                    subject.NgayHoan = "";
                }
                if (dt.Rows[0]["subject_isnew"].ToString() == "1")
                {
                    subject.YeuCau = "Delay";
                }
                else
                {
                    if (dt.Rows[0]["subject_isnew"].ToString() == "3")
                    {
                        subject.YeuCau = "EMD";
                    }
                    else
                    {
                        if (dt.Rows[0]["subject_isnew"].ToString() == "2")
                        {
                            subject.YeuCau = "Khẩn";
                        }
                        else
                        {
                            if (dt.Rows[0]["subject_isnew"].ToString() == "4")
                            {
                                subject.YeuCau = "Noshow";
                            }
                            else
                            {
                                subject.YeuCau = "Bình thường";
                            }
                        }
                    }
                }

                string GetLogs = " SELECT * FROM Logs WHERE ID='" + id + "' ORDER BY LogsID DESC ";
                DataTable tb_log = db.ExecuteDataSet(GetLogs, CommandType.Text, "server18", null).Tables[0];
                List<NhanKyHoanVe> ListNhatKy = new List<NhanKyHoanVe>();
                if (tb_log != null)
                {
                    for (int i = 0; i < tb_log.Rows.Count; i++)
                    {
                        string[] DtProti = tb_log.Rows[i]["LProperties"].ToString().Split('|');
                        string[] DtOvalues = tb_log.Rows[i]["OldValues"].ToString().Split('|');
                        string[] DtNValues = tb_log.Rows[i]["NewValues"].ToString().Split('|');
                        string StrUserUp = tb_log.Rows[i]["UserName"].ToString();
                        string StrDateTime = tb_log.Rows[i]["LogsDate"].ToString();

                        for (int j = 0; j < DtProti.Length; j++)
                        {
                            if (DtProti[j] != "")
                            {
                                NhanKyHoanVe nhatKy = new NhanKyHoanVe();
                                nhatKy.ThuocTinh = DtProti[j];
                                nhatKy.GiaTriCu = DtOvalues[j];
                                nhatKy.GiaTriMoi = DtNValues[j];
                                string sql1 = "select * from DM_NV where tendangnhap like N'" + StrUserUp.Trim() + "'";
                                DataTable tb1 = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
                                nhatKy.NhanVienSua = tb1.Rows[0]["Ten"].ToString();
                                nhatKy.NgaySua = StrDateTime;
                                ListNhatKy.Add(nhatKy);
                            }
                        }
                    }
                }
                string SRefurnd = "";
                SRefurnd = Refurnd(tendangnhap);
                subject.ListNhatKy = ListNhatKy;
                subject.listHang = ListHang(SRefurnd);
            }
            return subject;
        }
        public string BtnChuyen(int? IDVe, int? isshow, string loaive)
        {
            int result = 0;
            string thongbao = "";
            if (isshow == 0)
            {
                //thongbao = "Đại lý đã hủy số vé này !";
                thongbao = StaticDetailVoucher.FAIL + "1";
            }
            else
            {
                string SqlUpdate = " UPDATE [subject] SET subject_updateby = '',subject_time=null,subject_isshow='1',subject_com='0',subject_ishot='0',section_id=" + loaive + " WHERE subject_id='" + IDVe + "' ";
                result = db.ExecuteNoneQuery(SqlUpdate, CommandType.Text, "server18", null);
                if (result == 1)
                {
                    //thongbao = "Chuyển danh mục thành công ";
                    thongbao = StaticDetailVoucher.SUCCESS;
                }
                else
                {
                    //thongbao = "Chuyển danh mục không thành công ";
                    thongbao = StaticDetailVoucher.FAIL;
                }
            }
            return thongbao;
        }
        public string BtnSave(string tendangnhap, int? IDVe, int? isshow, int? ishot, int? com, string picnote, string loaive, string tinhtrang, string ghichu, string ngaybay, string songay, string checkHoan, string checkEMD, string SoVeEMD, string TenKhach, string YeuCau)
        {
            int result = 0, checkhoan = 0, checkemd = 0;
            string thongbao = "", StrParam = "", NgayBay = null, SoNgay = "0", NgayHoan = null, Strtto = "";
            if (isshow == 0)
            {
                //thongbao = "Đại lý đã hủy số vé này !";
                thongbao = StaticDetailVoucher.FAIL;
            }
            else
            {
                switch (tinhtrang)
                {
                    case "Đã hoàn - chờ Hãng chi tiền hoàn"://Đang chờ xin điện SC,Đang Chuyển Nogo,Đang chờ Hãng kiểm tra
                        StrParam = "subject_isshow='3',subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_uptime=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đang chờ xin điện SC":
                        StrParam = "subject_isshow='2',subject_ishot=1,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đang chờ chuyển NOGO":
                        StrParam = "subject_isshow='2',subject_ishot=2,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đang chờ Hãng kiểm tra":
                        StrParam = "subject_isshow='2',subject_ishot=0,subject_com=1,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Sự cố":
                        StrParam = "subject_isshow='4',subject_ishot=0,subject_com=1,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đã nhận":
                        StrParam = "subject_isshow='5',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đang chờ mở OK để hoàn":
                        StrParam = "subject_isshow='7',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Chờ đến ngày hoàn":
                        StrParam = "subject_isshow='8',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đã hoàn sang EMD-S":
                        StrParam = "subject_isshow='9',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đã báo khách đổi yêu cầu khác":
                        StrParam = "subject_isshow='10',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                    case "Đang xử lý":
                        StrParam = "subject_isshow='11',subject_ishot=0,subject_com=0,subject_picnote=N'" + ghichu + "',subject_update=GETDATE(),subject_time=GETDATE(),subject_updateby = '" + tendangnhap + "',";
                        break;
                }
                StrParam += "subject_isnew=" + YeuCau + ",";
                if (ngaybay != null)
                {
                    NgayBay = DateTime.ParseExact(ngaybay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (songay != null)
                {
                    SoNgay = songay;
                }
                if (checkHoan != null)
                {
                    checkhoan = 1;
                }
                if (checkEMD != null)
                {
                    checkemd = 1;
                }
                if (ngaybay != null)
                {
                    NgayHoan = DateTime.Parse(NgayBay).AddDays(int.Parse(songay)).ToString();
                    StrParam += "IsCheckHoan=" + checkhoan + ",NgayHoan = '" + NgayHoan + "' ,NgayBay='" + NgayBay + "', SoNgayHoan=" + SoNgay + ", IsCheckEMD=" + checkemd + ",SoVeEMD=N'" + SoVeEMD + "',TenKhachHang=N'" + TenKhach + "'";
                }
                else
                {
                    StrParam += "IsCheckHoan=" + checkhoan + ", SoNgayHoan=" + SoNgay + ", IsCheckEMD=" + checkemd + ",SoVeEMD=N'" + SoVeEMD + "',TenKhachHang=N'" + TenKhach + "'";
                }

                if (isshow == 3)
                {
                    Strtto = "Đã hoàn - chờ Hãng chi tiền hoàn";
                }
                if (ishot == 1 && isshow == 2 && com == 0)
                {
                    Strtto = "Đang chờ xin điện SC";
                }
                if (ishot == 2 && isshow == 2 && com == 0)
                {
                    Strtto = "Đang chờ chuyển NOGO";
                }
                if (ishot == 0 && isshow == 2 && com == 1)
                {
                    Strtto = "Đang chờ Hãng kiểm tra";
                }
                if (ishot == 0 && isshow == 4 && com == 1)
                {
                    Strtto = "Sự cố";
                }
                if (ishot == 0 && isshow == 5 && com == 0)
                {
                    Strtto = "Đã nhận";
                }
                if (ishot == 0 && isshow == 6 && com == 0)
                {
                    Strtto = "Đã hoàn - Hãng đã chi tiền hoàn";
                }
                if (ishot == 0 && isshow == 7 && com == 0)
                {
                    Strtto = "Đang chờ mở OK để hoàn";
                }

                if (ishot == 0 && isshow == 8 && com == 0)
                {
                    Strtto = "Chờ đến ngày hoàn";
                }
                if (ishot == 0 && isshow == 9 && com == 0)
                {
                    Strtto = "Đã hoàn sang EMD-S";
                }
                if (ishot == 0 && isshow == 10 && com == 0)
                {
                    Strtto = "Đã báo khách đổi yêu cầu khác";
                }
                if (ishot == 0 && isshow == 11 && com == 0)
                {
                    Strtto = "Đang xử lý";
                }

                string Strproper = " Tình trạng|Ghi chú ";
                string Ovalues = Strtto + "|" + picnote;
                string NewValue = tinhtrang + "|" + ghichu;

                string SqlUpdate = " UPDATE subject SET  " + StrParam + " WHERE subject_id='" + IDVe + "' ";
                result = db.ExecuteNoneQuery(SqlUpdate, CommandType.Text, "server18", null);
                if (result == 1)
                {
                    //thongbao = "Lưu vé hoàn thành công ";
                    thongbao = StaticDetailVoucher.SUCCESS;
                    int KQ_SetLogs = SetLogs(Strproper, Ovalues, NewValue, tendangnhap, IDVe);
                    if (CheckAdmin(tendangnhap) != "0")
                    {
                        string ngayxuly = "", skype = "", sdt = "", tenDL = "", email = "", hang = "", sove = "", nguoigui = "", thongtinkemtheo = "", ngaygui = "", emailCC = "hoanveev@enviet-group.com", IDVeHoan = "", ngayhuy = "", EMD = "";

                        string sql = @"SELECT *,(select top 1 [section_name] from subject_section WHERE [section_id]=subject.section_id) as section_name,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as ngayxuly,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject_id='" + IDVe + "'";
                        DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                        string sqlBookerName = @"select Ten,DienThoai,Skyper from DM_NV where TenDangNhap = '" + tendangnhap + "'";
                        DataTable dtName = db.ExecuteDataSet(sqlBookerName, CommandType.Text, "server37", null).Tables[0];
                        string[] StrNames = dt.Rows[0]["subject_name_en"].ToString().Split('-');
                        string strSender = "";//StrNames[0];
                        string strPhone = "";
                        if (StrNames.Length > 1)
                        {
                            strSender = StrNames[0];
                            strPhone = StrNames[1];
                        }
                        IDVeHoan = dt.Rows[0]["code_ticket"].ToString();
                        ngayhuy = dt.Rows[0]["NgayHuyHanhTrinh"].ToString();
                        skype = dtName.Rows[0]["Skyper"].ToString();
                        sdt = dtName.Rows[0]["DienThoai"].ToString();
                        ngayxuly = dt.Rows[0]["ngayxuly"].ToString();
                        EMD = dt.Rows[0]["SoVeEMD"].ToString();
                        tenDL = dt.Rows[0]["subject_name"].ToString();
                        email = dt.Rows[0]["subject_code"].ToString();
                        sove = dt.Rows[0]["subject_number"].ToString();
                        thongtinkemtheo = dt.Rows[0]["subject_content"].ToString();
                        ngaygui = dt.Rows[0]["subject_date"].ToString();
                        hang = dt.Rows[0]["section_name"].ToString();
                        nguoigui = dtName.Rows[0]["Ten"].ToString();

                        //SendMail(tenDL, strSender, email, strPhone, hang, tinhtrang, sove, thongtinkemtheo, ghichu, ngaygui, nguoigui, emailCC, IDVeHoan, ngayhuy, skype, sdt, ngayxuly, TenKhach, NgayHoan, EMD);
                    }
                }
                else
                {
                    //thongbao = "Lưu vé hoàn không thành công ";
                    thongbao = StaticDetailVoucher.FAIL + "1";
                }
            }
            return thongbao;
        }
        public bool SendMail(string AgentName, string StrSender, string StrEmail, string StrPhone, string StrKticket, string StrStatus, string StrSoVe, string StrInfor, string StrNoteBk, string ngaygui, string nguoigui, string StrEmaillCC, string IDVeHoan, string ngayhuy, string skype, string sdt, string ngayxuly, string TenKhach, string NgayHoan, string EMD)
        {
            string StrDatetime = DateTime.Now.ToString("dd/MM/yyyy H:m");

            MailMessage mail = new MailMessage(baoVH.username, StrEmail);
            mail.From = new MailAddress(baoVH.username, "");
            mail.Subject = "Hoàn Vé";
            SmtpClient client = new SmtpClient();
            client.EnableSsl = Convert.ToBoolean(baoVH.useSSL);
            client.Port = baoVH.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(baoVH.username, new DBase().Decrypt(baoVH.password, "vodacthe", true));
            client.Host = baoVH.host;

            try
            {
                if (!string.IsNullOrEmpty(StrEmaillCC))
                {
                    mail.CC.Add(StrEmaillCC);
                }
            }
            catch { }
            try { mail.Bcc.Add(baoVH.BCC); }
            catch { }

            ///-------- Start of mail body ------------

            string StrNgayGui = DateTime.Parse(ngaygui).ToString("dd/MM/yyyy");
            string StrNgayHuy = DateTime.Parse(ngayhuy).ToString("dd/MM/yyyy");
            //string StrNgayHoan = DateTime.Parse(NgayHoan).ToString("dd/MM/yyyy");
            string StrNgayHoan = "";
            string StrBody = @"<table style='width: 100%;'cellspacing = '0' cellpadding = '7' border = '0'>
                                <tr>
                                    <td><img style='width: 80px;' src='http://gateway.enviet-group.com/logoev-mail.png'></td>
                                    <td style='text-align: right;'> Ngày gửi: <span>" + StrNgayGui + @"</span>.Vui lòng ko reply</td>
                                </tr>
                                <tr style='color:#fff; background-color: #006886; padding:5px 10px; font-size: 15px;'>
                                     <td style='width:200px;'> Thông Tin Người Gửi </td>
                                    <td style='text-align: right;'> ID:" + IDVeHoan + @"</td>
                                </tr>
                                <tr>
                                    <td> Đại lý:</td>
                                    <td><strong>" + AgentName + @"</strong></td>
                                </tr>
                                <tr>
                                    <td> Họ tên:</td>
                                    <td><strong>" + StrSender + @" </strong></td>
                                </tr>
                                <tr>
                                    <td> Email:</td>
                                    <td><strong>" + StrEmail + @"</strong></td>
                                </tr>
                                <tr>
                                    <td> Điện thoại:</td>
                                    <td><strong>" + StrPhone + @"</strong></td>
                                </tr>
                                <tr style='color:#fff; background-color: #006886;padding:5px 10px; font-size: 15px;'>
                                    <td colspan = '2'> Thông Tin Yêu Cầu </td>
                                </tr>            
                                <tr>          
                                    <td> Hãng:</td>          
                                    <td><strong>" + StrKticket + @"</strong></td>
                                </tr>
                                <tr>
                                    <td> Tình trạng:</td>    
                                    <td><strong>" + StrStatus + @" </strong></td>
                                </tr>                                     
                                <tr>                                     
                                    <td> Số vé:</td>                        
                                    <td><strong>" + StrSoVe + @" </strong></td>       
                                </tr>                                            
                                <tr>                                            
                                    <td> Thông tin kèm theo:</td>                   
                                    <td><strong>" + StrInfor + @"</strong></td>         
                                </tr>                       
                                <tr>                                                      
                                    <td colspan = '2' style = 'color:red;'> Đại lý đã xác nhận hủy hành trình, ngày giờ hủy " + StrNgayHuy + @" </td>
                                </tr>                                                            
                                <tr style = 'color:#fff; background-color: #006886;padding:5px 10px; font-size: 15px;'>
                                    <td colspan = '2'> Thông Tin Xử Lý </td>                                 
                                </tr>                                                             
                                <tr>                                                                
                                    <td> Người xử lý:</td>                                         
                                    <td><strong>" + nguoigui + @" </strong></td>                                    
                                </tr>                                                                     
                                <tr>                                                                   
                                    <td> Skype:</td>                                                        
                                    <td><strong>" + skype + @"</strong></td>                                    
                                </tr>                                                                           
                                <tr>                                                                           
                                    <td> Điện thoại:</td>                                                       
                                    <td><strong>" + sdt + @"</strong></td>                                               
                                </tr>                                                                 
                                <tr>                                                                     
                                    <td> Ghi chú:</td>                                                                    
                                    <td><strong>" + StrNoteBk + @"</strong></td>                                                   
                                </tr>                                                                                               
                                <tr>                                                                                               
                                    <td> Số vé EMD:</td>                                                                            
                                    <td><strong>" + EMD + @"</strong></td>                                                               
                                </tr>                                                                                                 
                                <tr>                                                                                                 
                                    <td> Tên khách:</td>                                                                                        
                                    <td><strong>" + TenKhach + @" </strong></td>                                                                 
                                </tr>                                                                   
                                <tr>                                                                 
                                    <td> Ngày xử lý:</td>                                                                           
                                    <td><strong>" + ngayxuly + @" </strong></td>                                                                       
                                </tr>                                                                                                        
                                <tr>                                                                                                         
                                    <td> Ngày hoàn:</td>                                                                                            
                                    <td><strong>" + StrNgayHoan + @" </strong></td>                                                                             
                                </tr>                                                                                                                        
                                <tr>                                                                                                   
                                    <td> Tình trạng:</td>                                                                                                    
                                    <td><strong>" + StrStatus + @" </strong></td>                                                                                     
                                </tr>                                                                                         
                                <tr style = 'color:#fff; background-color: #006886; font-size: 15px; text-align: center;'>                                          
                                    <td colspan = '2'>© 2021 ENVIETGROUP - Support Team </td>                                                                         
                                </tr>                                                                                                                                    
                            </table>";

            mail.Body = StrBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }
        public int SetLogs(string StrProperties, string SOvalues, string SNvalues, string StrUsername, int? AID)
        {
            int result = 0;
            string[] StrProp = StrProperties.Split('|');
            string[] StrOvalues = SOvalues.Split('|');
            string[] StrNvalues = SNvalues.Split('|');
            string Pvalues = "";
            string Ovalues = "";
            string Nvalues = "";
            string Strpasce = "";
            if (StrOvalues.Count() == StrProp.Count() && StrProp.Count() == StrNvalues.Count())
            {

                for (int i = 0; i < StrProp.Count(); i++)
                {
                    if (i != StrProp.Count())
                    {
                        Strpasce = "|";
                    }
                    if (StrOvalues[i] != StrNvalues[i])
                    {
                        Pvalues += StrProp[i] + Strpasce;
                        Ovalues += StrOvalues[i] + Strpasce;
                        Nvalues += StrNvalues[i] + Strpasce;
                    }
                }
                string SqlInsert = " INSERT logs ([ID],[UserName],[LProperties],[OldValues],[NewValues],[LogsDate]) values ('" + AID + "','" + StrUsername + "',N'" + Pvalues + "',N'" + Ovalues + "',N'" + Nvalues + "',GETDATE()) ";
                result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server18", null);
            }
            return result;
        }
        public string ReturnGiaTri(string giaTri, string ishot, string subject_com)
        {
            string kq = "";
            switch (giaTri)
            {
                case "0":
                    kq = "<span class='cancle' style='line-height: 0;'> Hủy </span>";
                    break;
                case "1":
                    kq = "<span class='new_' style='line-height: 0;'>Mới</span>";
                    break;
                case "2":
                    if (ishot == "1")
                    {
                        kq = "<span class='waitting' style='line-height: 0;'>Đang chờ xin điện SC</span>";
                    }
                    if (ishot == "2")
                    {
                        kq = "<span class='waitting' style='line-height: 0;'>Đang chờ chuyển NOGO</span>";
                    }
                    if (subject_com == "1")
                    {
                        kq = "<span class='waitting' style='line-height: 0;'>Đang chờ Hãng kiểm tra</span>";
                    }
                    break;
                case "3":
                    kq = "<span class='export' style='line-height: 0;'>Hoàn thành -  chờ hãng chi tiền</span>";
                    break;
                case "4":
                    kq = "<span class='pending' style='line-height: 0;'>Sự cố</span>";
                    break;
                case "5":
                    kq = "<span class='waitting' style='line-height: 0;'>Đã nhận</span>";
                    break;
                case "6":
                    kq = "<span class='export' style='line-height: 0;'>Hoàn thành - hãng đã chi tiền</span>";
                    break;
                case "7":
                    kq = "<span class='waitting' style='line-height: 0;'>Đang chờ mở OK hoàn</span>";
                    break;
                case "8":
                    kq = "<span class='waitting' style='line-height: 0;'>Chờ đến ngày hoàn</span>";
                    break;
                case "9":
                    kq = "<span class='waitting' style='line-height: 0;'>Đã hoàn sang EMD-S</span>";
                    break;
                case "10":
                    kq = "<span class='waitting' style='line-height: 0;'>Đã báo khách đổi yêu cầu khác</span>";
                    break;
                case "11":
                    kq = "<span class='waitting' style='line-height: 0;'>Đang xử lý</span>";
                    break;


            }
            return kq;
        }
        public string ReturnTinhTrang(string tinhTrang)
        {
            string kq = "";
            switch (tinhTrang)
            {
                case "0":
                    kq = "<span style='line-height: 0;'> Bình thường </span>";
                    break;
                case "1":
                    kq = "<span style='color:red;line-height: 0;'>Delay</span>";
                    break;
                case "2":
                    kq = "<span style='color:red;font-weight:bold;line-height: 0;'>KHẨN</span>";
                    break;
                case "3":
                    kq = "<span style='color:blue;line-height: 0;'>EMD</span>";
                    break;
                case "4":
                    kq = "<span style='color:blue;line-height: 0;'>Noshow</span>";
                    break;
            }
            return kq;
        }
        public List<Hang> ListHang(string SRefurnd)
        {
            string sqlcate = "";
            List<Hang> listHang = new List<Hang>();
            if (SRefurnd == "1")
            {
                sqlcate = " SELECT * FROM subject_section WHERE parent_id ='76' AND position_id='3' ORDER BY section_id DESC ";
            }
            else
            {
                sqlcate = " SELECT * FROM subject_section WHERE section_id in (" + SRefurnd + ") ORDER BY section_id DESC ";
            }
            DataTable tbcate = db.ExecuteDataSet(sqlcate, CommandType.Text, "server18", null).Tables[0];
            if (tbcate != null)
            {
                for (int i = 0; i < tbcate.Rows.Count; i++)
                {
                    Hang hang = new Hang();
                    hang.RefundID = int.Parse(tbcate.Rows[i]["section_id"].ToString()).ToString();
                    hang.RefundName = tbcate.Rows[i]["section_name"].ToString();
                    listHang.Add(hang);
                }
            }
            return listHang;
        }
        public List<NguoiXuLy> ListNguoiXuLy()
        {
            List<NguoiXuLy> list = new List<NguoiXuLy>();
            string sql = "select subject_updateby from subject where subject_updateby <> '' group by subject_updateby ";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    NguoiXuLy item = new NguoiXuLy();
                    item.Name = tb.Rows[i]["subject_updateby"].ToString();
                    list.Add(item);
                }
            }
            return list;
        }
        public string Refurnd(string tendangnhap)
        {
            string SRefurnd = "";
            string sql = "SELECT * FROM RefundGroup WHERE UserID='" + tendangnhap + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                if (tb.Rows[0]["GrFull"].ToString() == "0")
                {
                    SRefurnd = "1";
                }
                else
                {
                    SRefurnd = tb.Rows[0]["RefundID"].ToString();
                }
            }
            return SRefurnd;
        }
        public string CheckAdmin(string tendangnhap)
        {
            string SRefurnd = "0";
            string sql = "SELECT * FROM RefundGroup WHERE UserID='" + tendangnhap + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb.Rows.Count > 0)
            {
                if (tb.Rows[0]["GrFull"].ToString() != "0")
                {
                    SRefurnd = "1";
                }
            }
            return SRefurnd;
        }
        public string Remark()
        {
            string sqlcate = " SELECT NOIDUNG FROM QLTHONGBAO WHERE RowID = 2";
            DataTable dt = db.ExecuteDataSet(sqlcate, CommandType.Text, "server37", null).Tables[0];
            string noidung = "";
            if (dt.Rows.Count > 0)
            {
                noidung = dt.Rows[0]["NOIDUNG"].ToString();
            }
            return noidung;
        }
        public string Param(string loaive, string Dieukien, string Giatri, string tinhtrang, string cal_from, string cal_to, string nguoixuly)
        {
            string param = " 1=1 ", tungay = "", denngay = "";
            if (cal_from != null)
            {
                tungay = DateTime.ParseExact(cal_from, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (cal_to != null)
            {
                denngay = DateTime.ParseExact(cal_to, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (Giatri != null)
            {
                if (Dieukien == "1")
                {
                    param += " AND subject_number like '%" + Giatri.Trim() + "%'";
                }
                else
                {
                    param += " AND code_ticket like '%" + Giatri.Trim() + "%'";
                }
            }
            if (cal_from != null && cal_to != null)
            {
                param += " AND subject_date >=  '" + tungay.Trim() + "' AND subject_date <= '" + denngay.Trim() + "'";
            }
            if (cal_from != null && cal_to == null)
            {
                param += " AND CONVERT(nvarchar(13),subject_date,101) = '" + tungay.Trim() + "'";
            }
            if (cal_from == null && cal_to != null)
            {
                param += " AND CONVERT(nvarchar(13),subject_date,101) = '" + denngay.Trim() + "'";
            }
            if (nguoixuly != "0" && nguoixuly != null)
            {
                param += " AND subject_updateby like N'%" + nguoixuly.Trim() + "%'";
            }
            if (loaive != "0")
            {
                param += " AND section_id='" + loaive + "'";
            }
            if (tinhtrang != "All")
            {
                if (tinhtrang == "2")
                {
                    param += " AND subject_isshow in (2,7) ";
                }
                else
                {
                    if (tinhtrang == "5")
                    {
                        param += " AND subject_isshow='4' ";
                    }
                    else
                    {
                        if (tinhtrang == "4")
                        {
                            param += " AND subject_isnew='1' ";
                        }
                        else
                        {
                            if (tinhtrang == "6")
                            {
                                param += " AND subject_isnew='3' ";
                            }
                            else
                            {
                                if (tinhtrang == "13")
                                {
                                    param += " AND subject_isshow='8' ";
                                }
                                else
                                {
                                    if (tinhtrang == "14")
                                    {
                                        param += " AND subject_isshow='9' ";
                                    }
                                    else
                                    {
                                        if (tinhtrang == "15")
                                        {
                                            param += " AND subject_isshow='10' ";
                                        }
                                        else
                                        {
                                            if (tinhtrang == "7")
                                            {
                                                param += " AND subject_isshow='5' ";
                                            }
                                            else
                                            {
                                                if (tinhtrang == "8")
                                                {
                                                    param += " AND subject_isshow='6' ";
                                                }
                                                else
                                                {
                                                    if (tinhtrang == "9")
                                                    {
                                                        param += " AND subject_isshow='2' and subject_ishot = '1' ";
                                                    }
                                                    else
                                                    {
                                                        if (tinhtrang == "10")
                                                        {
                                                            param += " AND subject_isshow='2' and subject_ishot = '2' ";
                                                        }
                                                        else
                                                        {
                                                            if (tinhtrang == "11")
                                                            {
                                                                param += " AND subject_isshow='2' and subject_com = '1' ";
                                                            }
                                                            else
                                                            {
                                                                if (tinhtrang == "12")
                                                                {
                                                                    param += " AND subject_isshow='7'";
                                                                }
                                                                else
                                                                {
                                                                    if (tinhtrang == "16")
                                                                    {
                                                                        param += " AND subject_isshow='11'";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tinhtrang == "17")
                                                                        {
                                                                            param += " AND subject_isnew='4' ";
                                                                        }
                                                                        else
                                                                        {
                                                                            param += " AND subject_isshow='" + tinhtrang + "' ";
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return param;
        }
    }

}
