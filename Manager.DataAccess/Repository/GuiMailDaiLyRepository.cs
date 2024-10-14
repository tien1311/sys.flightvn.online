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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Dapper;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{

    public class GuiMailDaiLyRepository
    {
        DBase db = new DBase();
        Mail baoNS = new Mail("EVM_DAILY");
        Mail baoNS_KD = new Mail("EVM_DAILY_KD");
        GuiMailDaiLyModel guimailkinhdoanh = new GuiMailDaiLyModel();
        GuiMailHang guimailhang = new GuiMailHang();
        Danhsachmodel danhsachmodel = new Danhsachmodel();
        private readonly string SQL_EV_MAIN;

        public GuiMailDaiLyRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public TongQuatMail SearchMaKH(string member_kh)
        {
            TongQuatMail tongQuat = new TongQuatMail();
            List<GuiMailDaiLytModel> result = new List<GuiMailDaiLytModel>();
            string sql = "select * from member where  member_kh like '" + member_kh + "%'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    GuiMailDaiLytModel info = new GuiMailDaiLytModel();
                    info.MAKH = tb.Rows[i]["member_kh"].ToString();
                    info.DAILY = tb.Rows[i]["member_company"].ToString();
                    info.MAIL = tb.Rows[i]["member_email"].ToString();
                    result.Add(info);
                }
            }
            tongQuat.ListGuiMail = result;

            return tongQuat;
        }
        public List<ChiTietVe> ListChiTietVeOther(string MaNV, string TuNgay, string DenNgay, string SoVeSearch, string Server_EV, string Server_KH_KT, string Status)
        {
            List<ChiTietVe> ListChiTietVe = new List<ChiTietVe>();
            string where = "";
            string sql = "";
            if (MaNV != "NV00014" && MaNV != "NV00016" && MaNV != "NV00006" && MaNV != "NV00293")
            {
                where = " and NGUOISUA = N'" + MaNV + "'";
            }
            if (Status == "0")
            {
                where += " and TenNhanVien like N'bsp'";
            }
            else
            {
                if (Status == "1")
                {
                    where += " and TenNhanVien like N'onlineairticket.vn'";
                }
                else
                {
                    if (Status == "2")
                    {
                        where += " and TenNhanVien like N'appca'";
                    }
                    else
                    {
                        if (Status == "3")
                        {
                            where += " and TenNhanVien like N'test'";
                        }
                        else
                        {
                            where += " and TenNhanVien in ('test','onlineairticket.vn','appca','bsp')";
                        }
                    }

                }
            }

            if (SoVeSearch != null && SoVeSearch != "")
            {
                where += " and SOVE = '" + SoVeSearch.Trim() + "'";
                sql = @"select BC.*,EFF.field13 as MaKH_EFF, BC.HANG as MAHHK from BaoCaoVeSot BC
                            left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535]  EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BC.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT  
                            where 1=1" + where + "order by NgaySua Desc";
            }
            else
            {
                where += " and NgaySua >= '" + TuNgay + "' and NgaySua < '" + DenNgay + " 23:59:59' ";
                sql = @"select BC.*,EFF.field13 as MaKH_EFF, BC.HANG as MAHHK from BaoCaoVeSot BC
                            left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535] EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BC.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT  and EFF.dtime >=  '" + TuNgay + @"'
                            where 1=1" + where + "order by NgaySua Desc";
            }

            using (var conn = new SqlConnection(Server_EV))
            {
                ListChiTietVe = (List<ChiTietVe>)conn.Query<ChiTietVe>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            if (ListChiTietVe.Count > 0)
            {
                for (int i = 0; i < ListChiTietVe.Count; i++)
                {
                    if (ListChiTietVe[i].TenNhanVien == "bsp")
                    {
                        ListChiTietVe[i].TenNhanVien = "BSP";
                    }
                    else
                    {
                        if (ListChiTietVe[i].TenNhanVien == "appca")
                        {
                            ListChiTietVe[i].TenNhanVien = "APPCA";
                        }
                        else
                        {
                            if (ListChiTietVe[i].TenNhanVien == "test")
                            {
                                ListChiTietVe[i].TenNhanVien = "TEST";
                            }
                            else
                            {
                                ListChiTietVe[i].TenNhanVien = "OLA";
                            }
                        }

                    }
                    ListChiTietVe[i].ChietKhau = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].ChietKhau));
                    ListChiTietVe[i].GiaMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].GiaMua));
                    ListChiTietVe[i].PhiDVMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVMua));
                    ListChiTietVe[i].PhiDVBan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVBan));
                    ListChiTietVe[i].PhiHoan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiHoan));
                    string NgaySua = DateTime.Parse(ListChiTietVe[i].NGAYSUA).ToString("dd/MM/yyyy HH:mm:ss");
                    ListChiTietVe[i].NGAYSUA = NgaySua;
                }

            }
            return ListChiTietVe;
        }
        public List<ChiTietVe> ListChiTietVe(string MaNV, string TuNgay, string DenNgay, string SoVeSearch, string Server_EV, string Server_KH_KT, string Status, string Server_EV_V2)
        {
            int result_update = 0;
            List<ChiTietVe> ListChiTietVe = new List<ChiTietVe>();
            string where = "";
            string sql = "";
            string where_update = "";
            if (MaNV != "NV00014" && MaNV != "NV00016" && MaNV != "NV00293" && MaNV != "NV00006")
            {
                where = " and NGUOISUA = N'" + MaNV + "'";
                where_update = " and NGUOISUA = N'" + MaNV + "'";
            }
            if (Status == "1")
            {
                where += " and BC.Code is not null";
                where_update = " and Code is not null";
            }
            if (Status == "0")
            {
                where += " and BC.Code is null";
                where_update = " and Code is null";
            }
            string update_2023 = @" update BAOCAOVESOT_EV_2023
                                    set MaKH_EFF = (select top 1 EFF.field13 from [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535]  EFF WITH (NOLOCK) where BAOCAOVESOT_EV_2023.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BAOCAOVESOT_EV_2023.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT and EFF.dtime >= '" + TuNgay + @"' ) 
                                    where NgaySua >= '" + TuNgay + @"' and MaKH_EFF is null " + where_update;

            using (var conn1 = new SqlConnection(Server_EV_V2))
            {
                result_update = conn1.Execute(update_2023, null, commandType: CommandType.Text, commandTimeout: 90);
                conn1.Dispose();
                conn1.Close();
            }
            string update = @" update BAOCAOVESOT
                                    set MaKH_EFF = (select top 1 EFF.field13 from [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535]  EFF WITH (NOLOCK) where BAOCAOVESOT.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BAOCAOVESOT.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT and EFF.dtime >= '" + TuNgay + @"' ) 
                                    where NgaySua >= '" + TuNgay + @"' and MaKH_EFF is null " + where_update;
            using (var conn2 = new SqlConnection(Server_EV))
            {
                result_update = conn2.Execute(update, null, commandType: CommandType.Text, commandTimeout: 90);
                conn2.Dispose();
                conn2.Close();
            }
            if (SoVeSearch != null && SoVeSearch != "")
            {
                where += " and BC.SOVE = '" + SoVeSearch.Trim() + "'";
                sql = @"(select BC.*, BC.HANG as MAHHK from [SERVER37].[Manager_V2].[dbo].[BAOCAOVESOT_EV_2023] BC
                            where 1=1  " + where + @" and not exists (select * from [SERVER37].[Manager_V2].[dbo].[BAOCAOVESOT_EV_2023] BCT
                            where  BCT.ID = BC.ID and  BCT.Code is null and BCT.TENNHANVIEN in ('onlineairticket.vn','test')))
                            UNION ALL";


                sql += @"(select BC.*, BC.HANG as MAHHK from BaoCaoVeSot BC
                            where 1=1  " + where + @" and not exists (select * from BaoCaoVeSot BCT
                            where  BCT.ID = BC.ID and  BCT.Code is null and BCT.TENNHANVIEN in ('onlineairticket.vn','test')))";
            }
            else
            {
                where += " and NgaySua >= '" + TuNgay + "' and NgaySua < '" + DenNgay + " 23:59:59' ";
                sql = @"(select BC.*, BC.HANG as MAHHK from [SERVER37].[Manager_V2].[dbo].[BAOCAOVESOT_EV_2023] BC
                          
                            where 1=1  " + where + @" and not exists (select * from [SERVER37].[Manager_V2].[dbo].[BAOCAOVESOT_EV_2023] BCT
                            where  BCT.ID = BC.ID and  BCT.Code is null and BCT.TENNHANVIEN in ('onlineairticket.vn','test')))
                            UNION ALL
                ";

                sql += @"(select BC.*, BC.HANG as MAHHK from BaoCaoVeSot BC
                            where 1=1  " + where + @" and not exists (select * from BaoCaoVeSot BCT
                            where  BCT.ID = BC.ID and  BCT.Code is null and BCT.TENNHANVIEN in ('onlineairticket.vn','test')))";
            }

            using (var conn = new SqlConnection(Server_EV))
            {
                ListChiTietVe = conn.Query<ChiTietVe>(sql, null, commandType: CommandType.Text, commandTimeout: 90).OrderByDescending(n => n.NGAYSUA).ToList();
                conn.Dispose();
                conn.Close();
            }

            if (ListChiTietVe.Count > 0)
            {
                for (int i = 0; i < ListChiTietVe.Count; i++)
                {
                    string maNV = "";
                    if (ListChiTietVe[i].TenNhanVien == "")
                    {
                        maNV = ExistsMaKH(ListChiTietVe[i].NGUOISUA, Server_KH_KT);
                        string sql_update = "Update BaoCaoVeSot set TENNHANVIEN = N'" + maNV + "' where ID = " + ListChiTietVe[i].ID;
                        db.ExecuteNoneQuery(sql_update, CommandType.Text, "server37", null);
                    }
                    else
                    {
                        maNV = ListChiTietVe[i].TenNhanVien;
                    }
                    ListChiTietVe[i].TenNhanVien = maNV;
                    ListChiTietVe[i].ChietKhau = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].ChietKhau));
                    ListChiTietVe[i].GiaMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].GiaMua));
                    ListChiTietVe[i].PhiDVMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVMua));
                    ListChiTietVe[i].PhiDVBan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVBan));
                    ListChiTietVe[i].PhiHoan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiHoan));
                    string NgaySua = DateTime.Parse(ListChiTietVe[i].NGAYSUA).ToString("dd/MM/yyyy HH:mm:ss");
                    ListChiTietVe[i].NGAYSUA = NgaySua;
                }

            }
            return ListChiTietVe;
        }
        public List<ChiTietVe> ListChiTietVeWeb(string MaNV, string TuNgay, string DenNgay, string SoVeSearch, string Server_EV, string Server_KH_KT, string Status)
        {
            List<ChiTietVe> ListChiTietVe = new List<ChiTietVe>();
            string where = "";
            string sql = "";
            if (MaNV != "")
            {
                where = " and MAKH in ('" + MaNV + "','Manager')";
                //where = " and NGUOISUA = N'" + MaNV + "'";
            }
            if (Status == "1")
            {
                where += " and Code is not null";
            }
            if (Status == "0")
            {
                where += " and Code is null";
            }
            if (SoVeSearch != null && SoVeSearch != "")
            {
                where += " and SOVE = '" + SoVeSearch.Trim() + "'";
                sql = @"select BC.*,EFF.field13 as MaKH_EFF, BC.HANG as MAHHK from BaoCaoVeSot_NV BC
                            left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535]  EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BC.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT  
                            where 1=1 and TenNhanVien in ('onlineairticket.vn','test') " + where + "order by NgaySua Desc";
            }
            else
            {
                where += " and NgaySua >= '" + TuNgay + "' and NgaySua < '" + DenNgay + " 23:59:59' ";
                sql = @"select BC.*,EFF.field13 as MaKH_EFF, BC.HANG as MAHHK from BaoCaoVeSot_NV BC
                            left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535] EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT  = EFF.field1 COLLATE DATABASE_DEFAULT  and BC.PNR COLLATE DATABASE_DEFAULT  = EFF.field9 COLLATE DATABASE_DEFAULT  and EFF.dtime >=  '" + TuNgay + @"'
                            where 1=1 and TenNhanVien in ('onlineairticket.vn','test') and EFF.field13 is null" + where + "order by NgaySua Desc";
            }

            using (var conn = new SqlConnection(Server_EV))
            {
                ListChiTietVe = (List<ChiTietVe>)conn.Query<ChiTietVe>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            if (ListChiTietVe.Count > 0)
            {
                for (int i = 0; i < ListChiTietVe.Count; i++)
                {
                    string maNV = "";
                    if (ListChiTietVe[i].TenNhanVien == "")
                    {
                        maNV = ExistsMaKH(ListChiTietVe[i].NGUOISUA, Server_KH_KT);
                        string sql_update = "Update BaoCaoVeSot_NV set TENNHANVIEN = N'" + maNV + "' where ID = " + ListChiTietVe[i].ID;
                        db.ExecuteNoneQuery(sql_update, CommandType.Text, "server37", null);
                    }
                    else
                    {
                        if (ListChiTietVe[i].TenNhanVien == "test")
                        {
                            ListChiTietVe[i].TenNhanVien = "BSP";
                        }
                        else
                        {
                            ListChiTietVe[i].TenNhanVien = "OLA";
                        }
                    }
                    ListChiTietVe[i].ChietKhau = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].ChietKhau));
                    ListChiTietVe[i].GiaMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].GiaMua));
                    ListChiTietVe[i].PhiDVMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVMua));
                    ListChiTietVe[i].PhiDVBan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVBan));
                    ListChiTietVe[i].PhiHoan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiHoan));
                    string NgaySua = DateTime.Parse(ListChiTietVe[i].NGAYSUA).ToString("dd/MM/yyyy HH:mm:ss");
                    ListChiTietVe[i].NGAYSUA = NgaySua;
                }

            }
            return ListChiTietVe;
        }
        public List<ChiTietVe> ListVeSotBCCTC(string MaNV, string Server_EV, string Server_KH_KT)
        {
            List<ChiTietVe> ListChiTietVe = new List<ChiTietVe>();
            string where = "";
            string sql = "";
            if (MaNV != "NV00014" && MaNV != "NV00016" && MaNV != "NV00006" && MaNV != "NV00293")
            {
                where = " and NGUOISUA = N'" + MaNV + "'";
            }
            sql = @"select BC.*,EFF.field13 as MaKH_EFF, BC.HANG as MAHHK from BaoCaoVeSot BC
                    left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535] EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT = EFF.field1 COLLATE DATABASE_DEFAULT and BC.PNR COLLATE DATABASE_DEFAULT = EFF.field9 COLLATE DATABASE_DEFAULT 
                    where BC.NGUOISUA = N'" + MaNV + "' and EFF.field13 = '' and isnull(BC.TinhTrangEFF,0) = 0";

            using (var conn = new SqlConnection(Server_EV))
            {
                ListChiTietVe = (List<ChiTietVe>)conn.Query<ChiTietVe>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            if (ListChiTietVe.Count > 0)
            {
                for (int i = 0; i < ListChiTietVe.Count; i++)
                {
                    string maNV = "";
                    if (ListChiTietVe[i].TenNhanVien == "")
                    {
                        maNV = ExistsMaKH(ListChiTietVe[i].NGUOISUA, Server_KH_KT);
                        string sql_update = "Update BaoCaoVeSot set TENNHANVIEN = N'" + maNV + "' where ID = " + ListChiTietVe[i].ID;
                        db.ExecuteNoneQuery(sql_update, CommandType.Text, "server37", null);
                    }
                    else
                    {
                        maNV = ListChiTietVe[i].TenNhanVien;
                    }
                    ListChiTietVe[i].TenNhanVien = maNV;
                    ListChiTietVe[i].ChietKhau = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].ChietKhau));
                    ListChiTietVe[i].GiaMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].GiaMua));
                    ListChiTietVe[i].PhiDVMua = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVMua));
                    ListChiTietVe[i].PhiDVBan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiDVBan));
                    ListChiTietVe[i].PhiHoan = string.Format("{0:0,0}", double.Parse(ListChiTietVe[i].PhiHoan));
                    string NgaySua = DateTime.Parse(ListChiTietVe[i].NGAYSUA).ToString("dd/MM/yyyy HH:mm:ss");
                    ListChiTietVe[i].NGAYSUA = NgaySua;
                }

            }
            return ListChiTietVe;
        }
        public Task<string> SLVeSotBCCTCAsync(string MaNV)
        {
            string result = "";
            try
            {

                string sql = @"select COUNT(NGUOISUA) as SLVeSotBCCTC from BaoCaoVeSot BC
                            left join [KETOAN].[ELV_KETOAN].[dbo].[Life_obj3535] EFF WITH (NOLOCK) on BC.SOVE COLLATE DATABASE_DEFAULT = EFF.field1 COLLATE DATABASE_DEFAULT and BC.PNR COLLATE DATABASE_DEFAULT = EFF.field9 COLLATE DATABASE_DEFAULT 
                            where BC.NGUOISUA = N'" + MaNV + "' and EFF.field13 = '' and isnull(BC.TinhTrangEFF,0) = 0";
                using (var conn = new SqlConnection(SQL_EV_MAIN))
                {
                    result = conn.QueryFirst<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                return Task.FromResult(result);

            }
            catch (Exception)
            {
                return Task.FromResult(result);

            }
        }
        public GuiMailDaiLytModel ChiTietXuatDoiVe(string idkhoachinh)
        {
            GuiMailDaiLytModel result = new GuiMailDaiLytModel();
            string sql = "select * from SendMailAgent where ID = " + idkhoachinh;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];

            result.NOIDUNG = tb.Rows[0]["NOIDUNG"].ToString();
            result.DIEUKIEN = tb.Rows[0]["DIEUKIEN"].ToString();
            return result;
        }
        public string CheckTrung(string SoVe, string PNR, string MAHHK)
        {
            string Ngay = DateTime.Now.ToString("dd/MM/yyyy");
            string result = "";
            string sql = "select top 1 * from BaoCaoVeSot where isnull(TenNhanVien,'') not in ('onlineairticket.vn','BSP') and SOVE = '" + SoVe + "' and PNR = '" + PNR + "' and HANG = '" + MAHHK + "' and Convert(nvarchar(10),NGAYSUA,103) = '" + Ngay + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    result = SoVe;
                }
            }
            return result;
        }
        public string NoiDungLuuY()
        {
            string result = "";
            string sql = @"select top 1 NOIDUNG from QLTHONGBAO where PHANMEM = 'Sys.BaoCaoVe'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    result = tb.Rows[0][0].ToString();
                }
            }
            return result;
        }
        public string NoiDungLuuYCongNo()
        {
            string result = "";
            string sql = @"select top 1 NOIDUNG from QLTHONGBAO where PHANMEM = 'Sys.BaoCaoVe' order by ROWID DESC";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    result = tb.Rows[0][0].ToString();
                }
            }
            return result;
        }
        //Get lấy text nội dung tỷ giá
        public string TextTyGia(string Server)
        {

            string text = "<p>Số tiền nhập mặc định là VNĐ ( Tỷ giá 6E : 01 USD = {0} VND, WEBBSP : Theo tỷ giá hiện tại 1A, còn lại 01 USD = {1} VND)</p>";
            List<LoaiPhiXuatModel> ListLoaiPhi = new List<LoaiPhiXuatModel>();
            string sql = "select * from BAOCAOVESOT_CONFIG_PHIDICHVU";
            using (var conn = new SqlConnection(Server))
            {
                ListLoaiPhi = (List<LoaiPhiXuatModel>)conn.Query<LoaiPhiXuatModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            string result = string.Format(text, string.Format("{0:0,0}", ListLoaiPhi[0].ExchangeRate), string.Format("{0:0,0}", ListLoaiPhi[3].ExchangeRate));
            return result;
        }
        public List<LoaiPhiXuatModel> ListPhiXuat(string Server)
        {
            List<LoaiPhiXuatModel> result = new List<LoaiPhiXuatModel>();
            string sql = "select * from BAOCAOVESOT_CONFIG_PHIDICHVU";
            using (var conn = new SqlConnection(Server))
            {
                result = (List<LoaiPhiXuatModel>)conn.Query<LoaiPhiXuatModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            LoaiPhiXuatModel detail = new LoaiPhiXuatModel();
            detail.Name = " ";
            detail.Amount = -1;
            result.Insert(0, detail);
            return result;
        }
        public ChiTietVe VeDetail(string Server, string PNR, string SoVe)
        {
            ChiTietVe result = new ChiTietVe();
            try
            {
                string sql = "select top 1 * from BaoCaoVeSot where PNR = '" + PNR + "' and SoVe = '" + SoVe + "' and DATEDIFF(day, GetDate(), NGAYSUA) < 2";
                using (var conn = new SqlConnection(Server))
                {
                    result = conn.QueryFirst<ChiTietVe>(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                    conn.Dispose();
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public string ExistsMaKH(string MaKH, string Server_KH_KT)
        {
            string result = "";
            try
            {
                string sql = @"select Expr1 from VIEW_khachhang WITH (NOLOCK) where ma = '" + MaKH.Trim() + "'";
                using (var conn = new SqlConnection(Server_KH_KT))
                {
                    result = conn.QueryFirst<string>(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }

        }
        //public string LuuDetailTicket(string MANV, List<ChiTietVe> chiTietVe)
        //{
        //    try
        //    {
        //        string sql = "";
        //        string sql_EV = "";
        //        DBase db = new DBase();
        //        string result = "";
        //        if (MANV == null || MANV == "")
        //        {
        //            result = "Hết thời gian đăng nhập, xin vui lòng thoát ra đăng nhập lại";
        //            return result;
        //        }
        //        for (int i = 0; i < chiTietVe.Count; i++)
        //        {
        //            if (chiTietVe[i].MAHHK.Trim() == "QH")
        //            {
        //                chiTietVe[i].SoVe = chiTietVe[i].PNR;
        //            }
        //            if (CheckTrung(chiTietVe[i].SoVe, chiTietVe[i].PNR, chiTietVe[i].MAHHK) != "")
        //            {
        //                result = chiTietVe[i].SoVe + " đã được báo cáo rồi";
        //                return result;
        //            }
        //            if (ExistsMaKH(chiTietVe[i].MAKH) == "")
        //            {
        //                result = chiTietVe[i].MAKH + " không tồn tại";
        //                return result;
        //            }


        //            sql = @"Insert into Life_obj3535(ngay_thc,field12,field13,field14,field1,field6,field8,field4,field11,field5,field9,field3,field15,field16) 
        //                       Values('" + DateTime.Now + "','" + MANV.Trim() + "','" + chiTietVe[i].MAKH.Trim() + "','" + chiTietVe[i].MAHHK.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "'," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + "," + chiTietVe[i].ChietKhau + ",'" + chiTietVe[i].PNR.Trim() + "',N'" + chiTietVe[i].GHICHU + "',N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "')";


        //            int result_sql = db.ExecuteNoneQuery(sql, CommandType.Text, "serverMoiKeToan", null);
        //            if (result_sql > 0)
        //            {
        //                sql_EV = @"Insert into BAOCAOVESOT(PNR,SOVE,NGUOISUA,NGAYSUA,MAKH,CHIETKHAU,GIAMUA,PHIDVMUA,PHIDVBAN,PHIHOAN,HANG,GHICHU,THUOCTINH,MAGIOITHIEU,NGUOIGIOITHIEU) Values('" + chiTietVe[i].PNR.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "','" + MANV.Trim() + "','" + DateTime.Now + "','" + chiTietVe[i].MAKH.Trim() + "'," + chiTietVe[i].ChietKhau + "," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + ",'" + chiTietVe[i].MAHHK.Trim() + "',N'" + chiTietVe[i].GHICHU + "',1,N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "')";
        //                int result_sql_EV = db.ExecuteNoneQuery(sql_EV, CommandType.Text, "server37", null);
        //                if (result_sql_EV > 0)
        //                {
        //                    string sql_update = "updatethongtinbookernhap";
        //                    db.ExecuteNoneQuery(sql_update, CommandType.StoredProcedure, "serverMoiKeToan", null);
        //                    result = "";
        //                }
        //                else
        //                {
        //                    string sqldel = @"delete Life_obj3535  where field1 = '" + chiTietVe[i].SoVe.Trim() + "' and field9 = '" + chiTietVe[i].PNR.Trim() + "'";
        //                    int result2 = db.ExecuteNoneQuery(sqldel, CommandType.Text, "serverMoiKeToan", null);
        //                    result = "Báo cáo vé không thành công số vé " + chiTietVe[i].SoVe.Trim();
        //                    return result;
        //                }
        //            }
        //            else
        //            {
        //                result = "Báo cáo vé không thành công";
        //            }
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        public string LuuDetailTicket(string MANV, List<ChiTietVe> chiTietVe, string server_KT, string server_KH_KT)
        {
            string result = "";
            try
            {
                string sql = "";
                string sql_EV = "";
                DBase db = new DBase();

                if (MANV == null || MANV == "")
                {
                    result = "Hết thời gian đăng nhập, xin vui lòng thoát ra đăng nhập lại";
                    return result;
                }
                for (int i = 0; i < chiTietVe.Count; i++)
                {
                    if (chiTietVe[i].MAHHK.Trim() == "QH")
                    {
                        chiTietVe[i].SoVe = chiTietVe[i].PNR;
                    }
                    if (CheckTrung(chiTietVe[i].SoVe, chiTietVe[i].PNR, chiTietVe[i].MAHHK) != "")
                    {
                        result = chiTietVe[i].SoVe + " đã được báo cáo rồi";
                        return result;
                    }
                    if (ExistsMaKH(chiTietVe[i].MAKH, server_KH_KT) == "")
                    {
                        result = chiTietVe[i].MAKH + " không tồn tại";
                        return result;
                    }

                    int result_sql = 0;
                    sql = @"Insert into Life_obj3535(ngay_thc,field12,field13,field14,field1,field6,field8,field4,field11,field5,field9,field3,field15,field16) 
                               Values('" + DateTime.Now + "','" + MANV.Trim() + "','" + chiTietVe[i].MAKH.Trim() + "','" + chiTietVe[i].MAHHK.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "'," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + "," + chiTietVe[i].ChietKhau + ",'" + chiTietVe[i].PNR.Trim() + "',N'" + chiTietVe[i].GHICHU + "',N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "')";
                    using (var conn = new SqlConnection(server_KT))
                    {
                        result_sql = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Dispose();
                    }

                    if (result_sql > 0)
                    {
                        sql_EV = @"Insert into BAOCAOVESOT(PNR,SOVE,NGUOISUA,NGAYSUA,MAKH,CHIETKHAU,GIAMUA,PHIDVMUA,PHIDVBAN,PHIHOAN,HANG,GHICHU,THUOCTINH,MAGIOITHIEU,NGUOIGIOITHIEU) Values('" + chiTietVe[i].PNR.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "','" + MANV.Trim() + "','" + DateTime.Now + "','" + chiTietVe[i].MAKH.Trim() + "'," + chiTietVe[i].ChietKhau + "," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + ",'" + chiTietVe[i].MAHHK.Trim() + "',N'" + chiTietVe[i].GHICHU + "',1,N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "')";
                        int result_sql_EV = db.ExecuteNoneQuery(sql_EV, CommandType.Text, "server37", null);
                        if (result_sql_EV > 0)
                        {
                            string sql_update = "updatethongtinbookernhap";
                            using (var conn = new SqlConnection(server_KT))
                            {
                                conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                conn.Dispose();
                                result = "";
                            }
                        }
                        else
                        {
                            string sqldel = @"delete Life_obj3535  where field1 = '" + chiTietVe[i].SoVe.Trim() + "' and field9 = '" + chiTietVe[i].PNR.Trim() + "'";
                            using (var conn = new SqlConnection(server_KT))
                            {
                                conn.Execute(sqldel, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                conn.Dispose();
                            }
                            result = "Báo cáo vé không thành công số vé " + chiTietVe[i].SoVe.Trim();
                            return result;
                        }
                    }
                    else
                    {
                        result = "Báo cáo vé không thành công";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = "Báo cáo vé không thành công";
                return result;
                throw ex;
            }

        }
        public string LuuDetailTicketVoQuy(string MANV, List<ChiTietVe> chiTietVe, string server_KT, string server_KH_KT, string server_EV)
        {
            string result = "";
            try
            {
                string sql = "";
                string sql_EV = "";
                DBase db = new DBase();

                if (MANV == null || MANV == "")
                {
                    result = "Hết thời gian đăng nhập, xin vui lòng thoát ra đăng nhập lại";
                    return result;
                }
                for (int i = 0; i < chiTietVe.Count; i++)
                {
                    if (chiTietVe[i].MAHHK.Trim() == "QH")
                    {
                        chiTietVe[i].SoVe = chiTietVe[i].PNR;
                    }
                    if (CheckTrung(chiTietVe[i].SoVe, chiTietVe[i].PNR, chiTietVe[i].MAHHK) != "")
                    {
                        result = chiTietVe[i].SoVe + " đã được báo cáo rồi";
                        return result;
                    }
                    if (ExistsMaKH(chiTietVe[i].MAKH, server_KH_KT) == "")
                    {
                        result = chiTietVe[i].MAKH + " không tồn tại";
                        return result;
                    }
                    string sql1 = sql_EV = @"Insert into BAOCAOVESOT(PNR,SOVE,NGUOISUA,NGAYSUA,MAKH,CHIETKHAU,GIAMUA,PHIDVMUA,PHIXUATVE,PHIDVBAN,PHIHOAN,HANG,GHICHU,THUOCTINH,MAGIOITHIEU,NGUOIGIOITHIEU, LOAIPHI, PHIXUATVE, SOLUONG) OUTPUT Inserted.ID Values('" + chiTietVe[i].PNR.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "','" + MANV.Trim() + "','" + DateTime.Now + "','" + chiTietVe[i].MAKH.Trim() + "'," + chiTietVe[i].ChietKhau + "," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiXuatVe + ",," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + ",'" + chiTietVe[i].MAHHK.Trim() + "',N'" + chiTietVe[i].GHICHU + "',1,N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "',N'" + chiTietVe[i].LoaiPhi + "'," + chiTietVe[i].PhiXuatVe + "," + chiTietVe[i].SoLuong + ")";
                    int result_sql = 0;
                    sql = @"Insert into Life_obj3535(ngay_thc,field12,field13,field14,field1,field6,field8,field17,field11,field5,field9,field3,field15,field16, field4) 
                               Values('" + DateTime.Now + "','" + MANV.Trim() + "','" + chiTietVe[i].MAKH.Trim() + "','" + chiTietVe[i].MAHHK.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "'," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiXuatVe + "," + chiTietVe[i].PhiHoan + "," + chiTietVe[i].ChietKhau + ",'" + chiTietVe[i].PNR.Trim() + "',N'" + chiTietVe[i].GHICHU + "',N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "', " + chiTietVe[i].PhiDVBan + ")";
                    using (var conn = new SqlConnection(server_KT))
                    {
                        result_sql = conn.Execute(sql, null, null, commandType: CommandType.Text, commandTimeout: 30);
                        conn.Dispose();
                    }

                    if (result_sql > 0)
                    {
                        sql_EV = @"Insert into BAOCAOVESOT(PNR,SOVE,NGUOISUA,NGAYSUA,MAKH,CHIETKHAU,GIAMUA,PHIDVMUA,PHIXUATVE,PHIDVBAN,PHIHOAN,HANG,GHICHU,THUOCTINH,MAGIOITHIEU,NGUOIGIOITHIEU, LOAIPHI, SOLUONG) OUTPUT Inserted.ID Values('" + chiTietVe[i].PNR.Trim() + "','" + chiTietVe[i].SoVe.Trim() + "','" + MANV.Trim() + "','" + DateTime.Now + "','" + chiTietVe[i].MAKH.Trim() + "'," + chiTietVe[i].ChietKhau + "," + chiTietVe[i].GiaMua + "," + chiTietVe[i].PhiDVMua + "," + chiTietVe[i].PhiXuatVe + "," + chiTietVe[i].PhiDVBan + "," + chiTietVe[i].PhiHoan + ",'" + chiTietVe[i].MAHHK.Trim() + "',N'" + chiTietVe[i].GHICHU + "',1,N'" + chiTietVe[i].MAGIOITHIEU + "',N'" + chiTietVe[i].NGUOIGIOITHIEU + "',N'" + chiTietVe[i].LoaiPhi + "'," + chiTietVe[i].SoLuong + ")";
                        int result_sql_EV = int.Parse(db.ExecuteDataSet(sql_EV, CommandType.Text, "server37", null).Tables[0].Rows[0][0].ToString());
                        if (result_sql_EV > 0)
                        {
                            int result_update = 0;
                            string sql_update = "updatethongtinbookernhap";
                            using (var conn = new SqlConnection(server_KT))
                            {
                                result_update = conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                conn.Dispose();
                                result = "";
                            }
                            string sql_update_tien = "SP_INSERT_BIENDONGSODU_TM";
                            double soTien = double.Parse(chiTietVe[i].GiaMua.Trim()) + double.Parse(chiTietVe[i].PhiDVBan.Trim()) + double.Parse(chiTietVe[i].PhiXuatVe.Trim());
                            string noiDung = chiTietVe[i].MAHHK + "-" + chiTietVe[i].PNR + "-" + chiTietVe[i].SoVe;
                            if (chiTietVe[i].PNR == chiTietVe[i].SoVe)
                            {
                                noiDung = chiTietVe[i].MAHHK + "-" + chiTietVe[i].PNR;
                            }
                            var param = new
                            {
                                MAKH = chiTietVe[i].MAKH.Trim(),
                                SOTIEN = soTien,
                                NOCO = "Nợ",
                                NOIDUNG = noiDung,
                                NGAYCK = DateTime.Now,
                                NGAYNHAN = DateTime.Now,
                                NGAYGUI = DateTime.Now,
                                IDBAOCAO = result_sql_EV,
                                HANG = chiTietVe[i].MAHHK.Trim()
                            };
                            using (var conn = new SqlConnection(server_EV))
                            {
                                result_update = conn.Execute(sql_update_tien, param, null, commandType: CommandType.StoredProcedure, commandTimeout: 60);
                                conn.Dispose();
                                result = "";
                            }
                            if (result_update > 0)
                            {
                                sql_update = "UpdateThongTinTM";
                                using (var conn = new SqlConnection(server_KT))
                                {
                                    result_update = conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                    conn.Dispose();
                                    result = "";
                                }
                            }
                        }
                        else
                        {
                            string sqldel = @"delete Life_obj3535  where field1 = '" + chiTietVe[i].SoVe.Trim() + "' and field9 = '" + chiTietVe[i].PNR.Trim() + "'";
                            using (var conn = new SqlConnection(server_KT))
                            {
                                conn.Execute(sqldel, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                conn.Dispose();
                            }
                            result = "Báo cáo vé không thành công số vé " + chiTietVe[i].SoVe.Trim();
                            return result;
                        }
                    }
                    else
                    {
                        result = "Báo cáo vé không thành công";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = "Báo cáo vé không thành công";
                return result;
                throw ex;
            }

        }
        public List<NhanVien> ListNhanVien()
        {
            List<NhanVien> list = new List<NhanVien>();
            string sql = @"select * from DM_NV where TinhTrang = 1 and MaPhongBan = 'PV'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    NhanVien nv = new NhanVien();
                    nv.TenNhanVien = tb.Rows[i]["Ten"].ToString();
                    nv.TenDangNhap = tb.Rows[i]["TenDangNhap"].ToString();
                    list.Add(nv);
                }

            }
            return list;
        }
        public DanhSachTongNhanVienXuatDoiVe SearchTongXuatDoiNhanVien(string TuNgay, string DenNgay, string NHANVIEN, string NGAYTIMKIEM, bool MORONG, string search)
        {
            DanhSachTongNhanVienXuatDoiVe DanhSachTong = new DanhSachTongNhanVienXuatDoiVe();
            List<NhanVienXuatDoiVe> result = new List<NhanVienXuatDoiVe>();
            int STT = 0;
            string where = "";
            string sql = "";

            if (NHANVIEN != "All")
            {
                where += " and A.NHANVIEN = '" + NHANVIEN + "'";
            }

            if (search == "Search")
            {
                sql = @"select  NV.MaNV,
                                NV.Ten,
                                SLXUATVE=(select COUNT(*) from [SERVER18].[Agent].[dbo].[SendMailAgent] A where A.XuatVe = 1 and NV.TenDangNhap = A.NhanVien and A.NgayGui >= '" + TuNgay + "'  and NgayGui < '" + DenNgay + "' " + where + @"),
                                SLDOIVE =(select COUNT(*) from [SERVER18].[Agent].[dbo].[SendMailAgent] A where A.DoiVe = 1 and NV.TenDangNhap = A.NhanVien and A.NgayGui >= '" + TuNgay + "'  and NgayGui < '" + DenNgay + "' " + where + @")
                                from DM_NV NV";
            }
            else
            {
                sql = @"select  NV.MaNV,
                                NV.Ten,
                                SLXUATVE=(select COUNT(*) from [SERVER18].[Agent].[dbo].[SendMailAgent] A where A.XuatVe = 1 and NV.TenDangNhap = A.NhanVien and A.NgayGui >= '" + TuNgay + "'  and NgayGui < '" + DenNgay + " 23:59:59' " + where + @"),
                                SLDOIVE =(select COUNT(*) from [SERVER18].[Agent].[dbo].[SendMailAgent] A where A.DoiVe = 1 and NV.TenDangNhap = A.NhanVien and A.NgayGui >= '" + TuNgay + "'  and NgayGui< '" + DenNgay + " 23:59:59' " + where + @")
                                from DM_NV NV";
            }


            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    int SLXuatVe = int.Parse(tb.Rows[i]["SLXUATVE"].ToString());
                    int SLDoiVe = int.Parse(tb.Rows[i]["SLDOIVE"].ToString());
                    if (SLXuatVe != 0 || SLDoiVe != 0)
                    {
                        STT = STT + 1;
                        NhanVienXuatDoiVe info = new NhanVienXuatDoiVe();
                        info.STT = STT;
                        info.MaNV = tb.Rows[i]["MaNV"].ToString();
                        info.TenNV = tb.Rows[i]["Ten"].ToString();
                        info.SLXuatVe = SLXuatVe.ToString();
                        info.SLDoiVe = SLDoiVe.ToString();

                        result.Add(info);
                    }
                }
            }
            DanhSachTong.ListNhanVienXuatDoiVe = result;
            DanhSachTong.ListNhanVien = ListNhanVien();

            return DanhSachTong;
        }
        public DanhSachXuatDoiVe SearchGuiXuatDoi(string TuNgay, string DenNgay, string PNR, string MAKH, string TINHTRANG, string NHANVIEN)
        {
            DanhSachXuatDoiVe DanhSachTong = new DanhSachXuatDoiVe();
            List<GuiMailDaiLytModel> result = new List<GuiMailDaiLytModel>();

            string where = "";
            if (PNR != "" && PNR != null)
            {
                where += " and PNR = '" + PNR.ToUpper() + "'";
            }
            if (MAKH != "" && MAKH != null)
            {
                where += " and MAKH = '" + MAKH + "'";
            }
            if (TINHTRANG != "All")
            {
                if (TINHTRANG == "1")
                {

                    where += " and XUATVE = 1";
                }
                if (TINHTRANG == "2")
                {

                    where += " and DOIVE = 1";
                }

            }
            if (NHANVIEN != "All")
            {
                where += " and NHANVIEN = '" + NHANVIEN + "'";


            }
            string sql = "select STT=Row_Number() over (order by ID), * from SendMailAgent where NgayGui >= '" + TuNgay + "' and NgayGui < '" + DenNgay + " 23:59:59' " + where + " order by ID desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    GuiMailDaiLytModel info = new GuiMailDaiLytModel();
                    info.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                    info.STT = int.Parse(tb.Rows[i]["STT"].ToString());
                    info.PNR = tb.Rows[i]["PNR"].ToString();
                    info.MAKH = tb.Rows[i]["MAKH"].ToString();
                    info.DAILY = tb.Rows[i]["TENDAILY"].ToString();
                    info.MAIL = tb.Rows[i]["MAIL"].ToString();
                    info.MAILCC = tb.Rows[i]["MAILCC"].ToString();
                    info.NGUOIGUI = tb.Rows[i]["NGUOIGUI"].ToString();
                    if (tb.Rows[i]["XUATVE"].ToString() == "True")
                    {
                        info.TINHTRANG = "Xuất vé";
                    }
                    else
                    {
                        info.TINHTRANG = "Đổi vé";
                    }
                    info.HANG = tb.Rows[i]["HANG"].ToString();
                    info.URL = tb.Rows[i]["URL"].ToString();
                    info.SOPHIEU = tb.Rows[i]["SOPHIEU"].ToString();
                    info.PHIDICHVU = string.Format("{0:0,0}", tb.Rows[i]["PHIDICHVU"]);
                    info.NGAYGUI = DateTime.Parse(tb.Rows[i]["NgayGui"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    result.Add(info);
                }
            }
            DanhSachTong.ListXuatDoiVe = result;
            DanhSachTong.ListNhanVien = ListNhanVien();

            return DanhSachTong;
        }
        public bool SendMailAgent(string NGUOIGUI, string DIENTHOAI_NV, string TENDANGNHAP, string MAKH, string DAILY, string MAIL, string MAILCC, string PNR, string HANG, string TINHTRANG, string NOIDUNG, string DIEUKIEN, IFormFile[] files, string WebRootPath, decimal PHIDICHVU, string NHANVIEN)
        {
            try
            {
                string sql_NV = @"select * from DM_NV where TenDangNhap =  '" + TENDANGNHAP + "'";
                string EMAIL_NV = db.ExecuteDataSet(sql_NV, CommandType.Text, "server37", null).Tables[0].Rows[0]["Email"].ToString();
                string CCMAIL = EMAIL_NV;
                if (MAILCC != "" && MAILCC != null)
                {
                    CCMAIL += "," + MAILCC;
                }
                string sql = "SP_INSERT_SENDMAILAGENT_BOOKER";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@MAKH", MAKH));
                Param.Add(new DBase.AddParameters("@TENDAILY", DAILY));
                Param.Add(new DBase.AddParameters("@PNR", PNR.ToUpper().Trim()));
                Param.Add(new DBase.AddParameters("@HANG", HANG));
                Param.Add(new DBase.AddParameters("@MAIL", MAIL));
                Param.Add(new DBase.AddParameters("@MAILCC", CCMAIL));
                if (TINHTRANG == "XUATVE")
                {
                    Param.Add(new DBase.AddParameters("@XUATVE", "1"));
                    Param.Add(new DBase.AddParameters("@DOIVE", "0"));
                }
                else
                {
                    Param.Add(new DBase.AddParameters("@XUATVE", "0"));
                    Param.Add(new DBase.AddParameters("@DOIVE", "1"));
                }
                Param.Add(new DBase.AddParameters("@NGUOIGUI", NGUOIGUI));
                Param.Add(new DBase.AddParameters("@NOIDUNG", NOIDUNG));
                Param.Add(new DBase.AddParameters("@DIEUKIEN", DIEUKIEN));
                Param.Add(new DBase.AddParameters("@PHIDICHVU", PHIDICHVU));
                Param.Add(new DBase.AddParameters("@NHANVIEN", NHANVIEN));
                string url = "";
                if (files.Length > 0)
                {
                    url = "http://sys.airline24h.com/UploadFile/" + files[0].FileName;
                }

                Param.Add(new DBase.AddParameters("@URL", url));


                string result = db.ExecuteDataSet(sql, CommandType.StoredProcedure, "server18", Param).Tables[0].Rows[0][0].ToString();

                if (result != "")
                {

                    string uploads = Path.Combine(WebRootPath, "UploadFile");
                    if (files.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, files[0].FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            files[0].CopyToAsync(fileStream);
                        }
                    }
                    bool resut_sendmail = SendMail(NGUOIGUI, DIENTHOAI_NV, TENDANGNHAP, MAKH, DAILY, MAIL, CCMAIL, PNR, HANG, TINHTRANG, NOIDUNG, DIEUKIEN, result, files, EMAIL_NV);
                    if (resut_sendmail == false)
                    {
                        string sql_delete = "delete SendMailAgent where SOPHIEU = '" + result + "'";
                        db.ExecuteNoneQuery(sql_delete, CommandType.Text, "server18", null);

                        return false;
                    }
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {
                throw ex;


            }

        }
        public bool SendMailKinhdoanhEV(string NGUOIGUI, string EMailNV, string DIENTHOAI_NV, string TENDANGNHAP, string MAKH, string DAILY, string MAIL, string MAILCC, string SOPHIEU, IFormFile[] files, string WebRootPath, string NV, string noiDungTong)
        {
            try
            {
                string sql_NV = @"select * from DM_NV where TenDangNhap =  '" + TENDANGNHAP + "'";
                string EMAIL = db.ExecuteDataSet(sql_NV, CommandType.Text, "server37", null).Tables[0].Rows[0]["Email"].ToString();
                string CCMAIL = EMAIL + ",toantt@enviet-group.com";
                if (MAILCC != "" && MAILCC != null)
                {
                    CCMAIL += "," + MAILCC;
                }
                string sql = "INSERT INTO [SendMailAgent_KD] ([MAKH] ,[TENDAILY],[MAIL] ,[MAILCC],[NOIDUNG],[URL],[NGUOIGUI],[NGAYGUI],[NHANVIEN]) VALUES ( @MAKH,@TENDAILY,@MAIL,@MAILCC,@NOIDUNG,@URL,@NGUOIGUI,@NGAYGUI,@NHANVIEN)";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@MAKH", MAKH));
                Param.Add(new DBase.AddParameters("@TENDAILY", DAILY));
                Param.Add(new DBase.AddParameters("@MAIL", MAIL));
                Param.Add(new DBase.AddParameters("@MAILCC", CCMAIL));
                Param.Add(new DBase.AddParameters("@NGUOIGUI", NGUOIGUI));
                Param.Add(new DBase.AddParameters("@NGAYGUI", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                Param.Add(new DBase.AddParameters("@NOIDUNG", noiDungTong));
                Param.Add(new DBase.AddParameters("@NHANVIEN", NV));

                //toantt@enviet-group.com
                string url = "";
                if (files.Length > 0)
                {
                    url = "http://sys.airline24h.com/UploadFile/" + files[0].FileName;
                }

                Param.Add(new DBase.AddParameters("@URL", url));

                int abc = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
                string result = "";

                if (result == "")
                {

                    string uploads = Path.Combine(WebRootPath, "UploadFile");
                    if (files.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, files[0].FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            files[0].CopyToAsync(fileStream);
                        }
                    }
                    bool resut_sendmail = SendMailKinhDoanh(NGUOIGUI, EMailNV, DIENTHOAI_NV, TENDANGNHAP, MAKH, DAILY, MAIL, CCMAIL, noiDungTong, SOPHIEU, files, NV);
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {
                throw ex;


            }

        }
        // Gửi mail Kinh Doanh
        public bool SendMailKinhDoanh(string NGUOIGUI, string EmailNV, string DIENTHOAI_NV, string TENDANGNHAP, string MAKH, string DAILY, string MAIL, string MAILCC, string noiDungTong, string SOPHIEU, IFormFile[] files, string NV)
        {

            MailMessage mail = new MailMessage(baoNS_KD.username, MAIL);
            mail.From = new MailAddress(baoNS_KD.username, "ENVIET SALES");
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.Port = baoNS_KD.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(baoNS_KD.username, baoNS_KD.password);
            client.Host = baoNS_KD.host;

            string subject = "[KINH DOANH] Thông báo thông tin tài khoản";
            try
            {
                string mailCC = MAILCC;
                mail.CC.Add(mailCC);

            }
            catch { }
            try {/* mail.Bcc.Add(baoNS.BCC);*/ }
            catch { }

            if (files.Length > 0)
            {
                foreach (IFormFile file in files)
                {
                    string FileName = Path.GetFullPath(file.FileName);
                    mail.Attachments.Add(new Attachment(file.OpenReadStream(), FileName));
                }
            }

            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = WebRequest.Create(baoNS_KD.templateUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            mailBody = mailBody.Replace("$_NGAYGUI", DateTime.Now.ToString("dd/MM/yyyy"));
            mailBody = mailBody.Replace("$_DAILY", DAILY);
            mailBody = mailBody.Replace("$_MAKH", MAKH);
            mailBody = mailBody.Replace("$_NGUOIGUI", NGUOIGUI);
            mailBody = mailBody.Replace("$_NOIDUNG", noiDungTong);
            mailBody = mailBody.Replace("$_DIENTHOAI", DIENTHOAI_NV);
            mailBody = mailBody.Replace("$_EMAIL", EmailNV);


            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }
        // Gửi mail đại lý
        public bool SendMail(string NGUOIGUI, string DIENTHOAI_NV, string TENDANGNHAP, string MAKH, string DAILY, string MAIL, string MAILCC, string PNR, string HANG, string TINHTRANG, string NOIDUNG, string DIEUKIEN, string SOPHIEU, IFormFile[] files, string EMAIL_NV)
        {
            MailMessage mail = new MailMessage(baoNS.username, MAIL);
            mail.From = new MailAddress(baoNS.username, "ENVIET AIR");
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.Port = baoNS.port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(baoNS.username, baoNS.password);
            client.Host = baoNS.host;
            string subject = "";
            if (TINHTRANG == "XUATVE")
            {
                TINHTRANG = "XUẤT VÉ";
                subject = "Yêu cầu xuất vé cho PNR: " + PNR + " thành công";
            }
            else
            {
                TINHTRANG = "ĐỔI VÉ";
                subject = "Yêu cầu đổi vé cho PNR: " + PNR + " thành công";
            }

            try
            {
                string mailCC = MAILCC;
                mail.CC.Add(mailCC);
            }
            catch { }
            try {/* mail.Bcc.Add(baoNS.BCC);*/ }
            catch { }

            if (files.Length > 0)
            {
                foreach (IFormFile file in files)
                {

                    string FileName = Path.GetFullPath(file.FileName);

                    mail.Attachments.Add(new Attachment(file.OpenReadStream(), FileName));
                }

            }

            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = WebRequest.Create(baoNS.templateUrl);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            mailBody = mailBody.Replace("$_NGAYGUI", DateTime.Now.ToString("dd/MM/yyyy"));
            mailBody = mailBody.Replace("$_DAILY", DAILY);
            mailBody = mailBody.Replace("$_MAKH", MAKH);
            mailBody = mailBody.Replace("$_NGUOIGUI", NGUOIGUI);
            mailBody = mailBody.Replace("$_IDXUATVE", SOPHIEU);
            mailBody = mailBody.Replace("$_TINHTRANG", TINHTRANG);
            mailBody = mailBody.Replace("$_PNR", PNR.ToUpper());
            mailBody = mailBody.Replace("$_NOIDUNG", NOIDUNG);
            mailBody = mailBody.Replace("$_DIEUKIEN", DIEUKIEN);
            mailBody = mailBody.Replace("$_DIENTHOAI", DIENTHOAI_NV);
            mailBody = mailBody.Replace("$_EMAIL", EMAIL_NV);

            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;
        }
        public GuiMailDaiLyModel SearchMaKH_KD(string member_kh)
        {
            List<GuiMailDaiLyKDModel> result = new List<GuiMailDaiLyKDModel>();
            string sql = "select * from member where  member_kh like '" + member_kh + "%'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null && tb.Rows.Count > 0)
            {
                GuiMailDaiLyKDModel info = new GuiMailDaiLyKDModel();
                info.MAKH = tb.Rows[0]["member_kh"].ToString();
                info.DAILY = tb.Rows[0]["member_company"].ToString();
                info.MAIL = tb.Rows[0]["member_email"].ToString();
                result.Add(info);
            }
            guimailkinhdoanh.ListNoiDung = DSTieuDe();
            guimailkinhdoanh.Guimailkinhdoanh = Guimaildaili().Guimailkinhdoanh;
            guimailkinhdoanh.Guimailkinhdoanh = result;
            return guimailkinhdoanh;
        }
        public List<ListTieuDe> DSTieuDe()
        {
            List<ListTieuDe> result = new List<ListTieuDe>();
            string sql_NoiDung = " SELECT * FROM QLTHONGBAO WHERE CONVERT(NVARCHAR(MAX), NoiDungTimKiem) <> '' and PHANMEM like N'Sys.GuiMailKinhDoanh'";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListTieuDe noiDung = new ListTieuDe();
                        noiDung.RowID = int.Parse(dt.Rows[i][0].ToString());
                        noiDung.NoiDungTimKiem = dt.Rows[i]["NoiDung"].ToString();
                        noiDung.TieuDe = dt.Rows[i]["TieuDe"].ToString();
                        result.Add(noiDung);
                    }

                }
            }

            return result;
        }
        public GuiMailDaiLyModel Guimaildaili()
        {

            List<GuiMailDaiLyKDModel> result = new List<GuiMailDaiLyKDModel>();
            string sql = "select * from GUIMAILDAILY_KD";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            guimailkinhdoanh.ListNoiDung = DSTieuDe();
            guimailkinhdoanh.Guimailkinhdoanh = result;
            return guimailkinhdoanh;
        }
        public GuiMailDaiLytModel MaiKinhDoanh(string idkhoachinh)
        {
            GuiMailDaiLytModel result = new GuiMailDaiLytModel();
            string sql = "select * from SendMailAgent_KD where ID = " + idkhoachinh;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];

            result.NOIDUNG = tb.Rows[0]["NOIDUNG"].ToString();
            return result;
        }
        public GuiMailHangModel MailHang(string idkhoachinh)
        {
            GuiMailHangModel result = new GuiMailHangModel();
            string sql = "select * from GuiMailSC where ID = " + idkhoachinh;
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            result.NoiDung = tb.Rows[0]["NoiDung"].ToString();
            return result;
        }
        public GuiMailHang Guimailhang()
        {

            List<GuiMailHangModel> result = new List<GuiMailHangModel>();
            string sql = "select * from GuiMailSC";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            guimailhang.ListTo = DSTo();
            guimailhang.ListEV = DSCC();
            guimailhang.ListTieuDe = DSTieude();
            guimailhang.Guimailhang = result;
            return guimailhang;
        }
        public List<ListTen> DSTen()
        {
            List<ListTen> result = new List<ListTen>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListTen ten = new ListTen();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        result.Add(ten);
                    }

                }
            }

            return result;
        }
        public List<DanhsachTO> DSTo()
        {
            List<DanhsachTO> result = new List<DanhsachTO>();
            string sql_NoiDung = "select * from ContactHang";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DanhsachTO noiDung = new DanhsachTO();
                        noiDung.RowID = int.Parse(dt.Rows[i][0].ToString());
                        noiDung.Ten = dt.Rows[i]["Ten"].ToString();
                        noiDung.Hang = dt.Rows[i]["To_Hang"].ToString();
                        result.Add(noiDung);
                    }

                }
            }

            return result;
        }
        public List<DanhsachEV> DSCC()
        {
            List<DanhsachEV> result = new List<DanhsachEV>();
            string sql_NoiDung = "select * from CC_EV";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DanhsachEV noiDung = new DanhsachEV();
                        noiDung.RowID = int.Parse(dt.Rows[i][0].ToString());
                        noiDung.Ten = dt.Rows[i]["Ten"].ToString();
                        noiDung.CCEV = dt.Rows[i]["CC_EV"].ToString();
                        result.Add(noiDung);
                    }

                }
            }

            return result;
        }
        public List<DanhsachTiêude> DSTieude()
        {
            List<DanhsachTiêude> result = new List<DanhsachTiêude>();
            string sql_NoiDung = "select * from ChuDeMail";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DanhsachTiêude noiDung = new DanhsachTiêude();
                        noiDung.RowID = int.Parse(dt.Rows[i][0].ToString());
                        noiDung.TieuDe = dt.Rows[i]["ChuDeMail"].ToString();
                        result.Add(noiDung);
                    }

                }
            }

            return result;
        }
        //public List<DanhsachTiêude> DSTieude()
        //{
        //    List<DanhsachTiêude> result = new List<DanhsachTiêude>();
        //    string sql_NoiDung = "select * from CC_EV";
        //    DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                DanhsachTiêude noiDung = new DanhsachTiêude();
        //                noiDung.RowID = int.Parse(dt.Rows[i][0].ToString());
        //                noiDung.TieuDe = dt.Rows[i]["[Ten]"].ToString();
        //                noiDung.NoiDungTimKiem = dt.Rows[i]["[CC_EV]"].ToString();
        //                result.Add(noiDung);
        //            }

        //        }
        //    }

        //    return result;
        //}
        public string GetTo(int id)
        {
            string result = "";
            string sql_NoiDung = " SELECT * FROM ContactHang WHERE ID = " + id;
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    result = dt.Rows[0]["To_Hang"].ToString();
                }
            }



            return result;
        }
        public string GetCC(int id)
        {
            string result = "";
            string sql_NoiDung = " SELECT * FROM CC_EV WHERE ID = " + id;
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    result = dt.Rows[0]["CC_EV"].ToString();
                }
            }



            return result;
        }
        public string GetTieuDe(int id)
        {
            string result = "";
            string sql_NoiDung = " SELECT * FROM ChuDeMail WHERE ID = " + id;
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    result = dt.Rows[0]["ChuDeMail"].ToString();
                }
            }



            return result;
        }
        public bool SendMailHang(string NGUOIGUI, string EMailNV, string DIENTHOAI_NV, string TENDANGNHAP, string mail, string CC, string CCkhac, string noidungtxt, string NoiDung, IFormFile[] files, string WebRootPath)
        {
            try
            {
                string sql_NV = @"select * from DM_NV where TenDangNhap =  '" + TENDANGNHAP + "'";
                string EMAIL = db.ExecuteDataSet(sql_NV, CommandType.Text, "server37", null).Tables[0].Rows[0]["Email"].ToString();
                string CCMAIL = EMAIL;
                if (CCkhac != "" && CCkhac != null)
                {
                    CCMAIL += "," + CCkhac;
                }
                string sql = "INSERT INTO [GuiMailSC] ([ToAddress],[CCAddress] ,[NoiDung],[FileDinhKem],[ChuDe],[NgayGui],[NhanVienGui],[OtherCCAddress],[TinhTrangGuiMail]) VALUES (@ToAddress,@CCAddress,@NoiDung,@FileDinhKem,@ChuDe,@NgayGui,@NhanVienGui,@OtherCCAddress,@TinhTrangGuiMail)";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@ToAddress", mail));
                Param.Add(new DBase.AddParameters("@CCAddress", CC));
                Param.Add(new DBase.AddParameters("@NoiDung", NoiDung));
                Param.Add(new DBase.AddParameters("@ChuDe", noidungtxt));
                Param.Add(new DBase.AddParameters("@NgayGui", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                Param.Add(new DBase.AddParameters("@NhanVienGui", NGUOIGUI));
                Param.Add(new DBase.AddParameters("@OtherCCAddress", CCkhac));
                Param.Add(new DBase.AddParameters("@TinhTrangGuiMail", "1"));
                string url = "";
                if (files.Length > 0)
                {
                    url = "http://daily.airline24h.com/upload/GuiSC/" + files[0].FileName;
                }

                Param.Add(new DBase.AddParameters("@FileDinhKem", url));

                int abc = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
                string result = "";

                if (result == "")
                {

                    string uploads = Path.Combine(WebRootPath, "UploadFile");
                    if (files.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, files[0].FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            files[0].CopyToAsync(fileStream);
                        }
                    }
                    bool resut_sendmail = SendMailHangEV(NGUOIGUI, EMailNV, DIENTHOAI_NV, TENDANGNHAP, mail, CC, CCkhac, noidungtxt, NoiDung, files);
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {
                throw ex;


            }

        }

        public bool SendMailHangEV(string NGUOIGUI, string EmailNV, string DIENTHOAI_NV, string TENDANGNHAP, string maill, string CC, string CCkhac, string noidungtxt, string NoiDung, IFormFile[] files)
        {

            MailMessage mail = new MailMessage("envietgroup@airline24h.vn", maill);
            mail.From = new MailAddress("envietgroup@airline24h.vn", "ENVIETGROUP");
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("envietgroup@airline24h.vn", "Enviet@7890");
            client.Host = "smtp.gmail.com";

            string subject = noidungtxt;
            try
            {
                string cctxt = CC;
                string mailCC = CCkhac;
                if (!string.IsNullOrEmpty(cctxt))
                {
                    mail.CC.Add(mailCC);
                }
                if (!string.IsNullOrEmpty(mailCC))
                {
                    mail.CC.Add(cctxt);
                }
            }
            catch { }
            try {/* mail.Bcc.Add(baoNS.BCC);*/ }
            catch { }

            if (files.Length > 0)
            {
                foreach (IFormFile file in files)
                {
                    string FileName = Path.GetFullPath(file.FileName);
                    mail.Attachments.Add(new Attachment(file.OpenReadStream(), FileName));
                }

            }

            mail.Subject = subject;
            ///-------- Start of mail body ------------
            string mailBody;
            var webRequest = WebRequest.Create("http://gateway.enviet-group.com/template/email_GuiMailSC.html");
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            { mailBody = reader.ReadToEnd(); }
            mailBody = mailBody.Replace("$_NGAYGUI", DateTime.Now.ToString("dd/MM/yyyy"));
            mailBody = mailBody.Replace("$_NoiDung", NoiDung);
            mailBody = mailBody.Replace("$_Ten", NGUOIGUI);
            mailBody = mailBody.Replace("$_Email", EmailNV);
            mailBody = mailBody.Replace("$_SoDienThoai", DIENTHOAI_NV);


            mail.Body = mailBody;
            mail.IsBodyHtml = true; // Format mail dạng HTML
            ///-------- End of mail body --------------
            client.Send(mail);
            return true;

        }



        public List<ListTen> DSZalo()
        {
            List<ListTen> result = new List<ListTen>();
            string sql_NoiDung = " SELECT TENDANGNHAP,Ten as TENNV FROM DM_NV WHERE MaPhongBan='KD' AND TINHTRANG=1 ";
            DataTable dt = db.ExecuteDataSet(sql_NoiDung, CommandType.Text, "server37", null).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListTen ten = new ListTen();
                        ten.Ten = dt.Rows[i]["TenNV"].ToString();
                        result.Add(ten);
                    }

                }
            }

            return result;
        }

    }
}
