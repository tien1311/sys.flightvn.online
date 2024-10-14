using Dapper;
using Manager.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class VeSotRepository
    {
        string server_EV_MAIN; /*= "Data Source=.;Initial Catalog=Manager;User ID=sa;Password=EnViet@123;";*/
        DBase db = new DBase();

        public VeSotRepository(IConfiguration configuration)
        {
            server_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        public ListVeSotModel DSVeSot(string server_EV_MAIN)
        {
            ListVeSotModel DSVeSot = new ListVeSotModel();
            try
            {
                List<ListTenNVModel> ListTenNV = new List<ListTenNVModel>();
                List<VeSotModel> ListVeSot = new List<VeSotModel>();
                string sqlSMS = @"select top 1 NGUOIGUI as NGUOIGUISMS, NGAYGUI as NGAYGUISMS from LOG_BAOCAOVE order by NGAYGUI desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    DSVeSot = conn.QueryFirst<ListVeSotModel>(sqlSMS, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }

                string sql = @"select * from DANHSACHBAOCAOVE_TEMP";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    ListVeSot = (List<VeSotModel>)conn.Query<VeSotModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }

                string sqlNV = @"select Expr1, MA from DANHSACHBAOCAOVE_TEMP group by Expr1, MA order by Expr1";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    ListTenNV = (List<ListTenNVModel>)conn.Query<ListTenNVModel>(sqlNV, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                    conn.Close();
                }
                if (ListVeSot != null)
                {
                    DSVeSot.VeSot = ListVeSot;
                }
                if (ListTenNV != null)
                {
                    DSVeSot.ListTen = ListTenNV;
                }
                return DSVeSot;
            }
            catch (Exception)
            {
                DSVeSot.ThongBao = "Lấy dữ liệu bị lỗi, Liên hệ IT";
                return DSVeSot;
                throw;
            }

        }
        public ListVeSotModel VeSot(string server_KH_KT, string Ten)
        {
            ListVeSotModel TrangVeSot = new ListVeSotModel();
            try
            {
                List<ListTenNVModel> ListTenNV = new List<ListTenNVModel>();
                List<VeSotModel> ListVeSot = new List<VeSotModel>();
                string sqlSMS = @"select top 1 NGUOIGUI as NGUOIGUISMS, NGAYGUI as NGAYGUISMS from LOG_BAOCAOVE order by NGAYGUI desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    TrangVeSot = conn.QueryFirst<ListVeSotModel>(sqlSMS, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                DateTime ngayDieuKien = DateTime.Parse("11/01/2022");
                string sql = "";
                if (DateTime.Now < ngayDieuKien)
                {
                    sql = @"select VEALL.PNR, VEALL.TKNockt, VEALL.Nghiepvu, VEALL.ID_HanhT, VEALL.NgayMua, KH.ma, KH.Expr1,HHK.mahk
                            from VIEWVEALL VEALL WITH (NOLOCK)
                            left join VIEW_khachhang KH on KH.f_identity = VEALL.iden3372
                            left join VIEW_HHKHONG HHK on HHK.f_identity = VEALL.iden3367
                            where iden3025='1' and KH.ma like N'NV%' and [NgayMua] >= '2022/10/01' and ((VEALL.Nghiepvu = N'Void' and VEALL.AIRCODECU in ('1G','1A','1B')) or (VEALL.Nghiepvu = 'EMDS' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = '' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS in ('1G','1A','1B')) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS not in ('1G','1A','1B') and HHK.mahk = 'VN' and HHK.mahk <> 'QH' and HHK.mahk <> 'VJ') or (VEALL.TKNockt like N'73815%' and VEALL.GiaMua > 0 ) ) order by VEALL.NgayMua,KH.Expr1 ";
                }
                else
                {
                    sql = @"select VEALL.PNR, VEALL.TKNockt, VEALL.Nghiepvu, VEALL.ID_HanhT, VEALL.NgayMua, KH.ma, KH.Expr1,HHK.mahk
                            from VIEWVEALL VEALL WITH (NOLOCK)
                            left join VIEW_khachhang KH on KH.f_identity = VEALL.iden3372
                            left join VIEW_HHKHONG HHK on HHK.f_identity = VEALL.iden3367
                            where iden3025='1'  and [NgayMua] >= DATEADD(month,-1,GETDATE()) and KH.ma like N'NV%' and DATEDIFF(day,VEALL.NGAYMUA,GETDATE()) < 30 and ((VEALL.Nghiepvu = N'Void' and VEALL.AIRCODECU in ('1G','1A','1B')) or (VEALL.Nghiepvu = 'EMDS' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = '' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS in ('1G','1A','1B')) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS not in ('1G','1A','1B') and HHK.mahk = 'VN' and HHK.mahk <> 'QH' and HHK.mahk <> 'VJ') or (VEALL.TKNockt like N'73815%' and VEALL.GiaMua > 0 ) ) order by VEALL.NgayMua,KH.Expr1 ";
                }
                using (var conn = new SqlConnection(server_KH_KT))
                {
                    ListVeSot = (List<VeSotModel>)conn.Query<VeSotModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                string sqlNV = "";
                if (DateTime.Now < ngayDieuKien)
                {
                    sqlNV = @"select KH.Expr1, KH.ma
                            from VIEWVEALL VEALL WITH (NOLOCK) 
                            left join VIEW_khachhang KH on KH.f_identity = VEALL.iden3372
                            left join VIEW_HHKHONG HHK on HHK.f_identity = VEALL.iden3367
                            where iden3025='1' and KH.ma like N'NV%' and [NgayMua] >= '2022/10/01' and ((VEALL.Nghiepvu = N'Void' and VEALL.AIRCODECU in ('1G','1A','1B')) or (VEALL.Nghiepvu = 'EMDS' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = '' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS in ('1G','1A','1B')) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS not in ('1G','1A','1B') and HHK.mahk = 'VN' and HHK.mahk <> 'QH' and HHK.mahk <> 'VJ') or (VEALL.TKNockt like N'73815%' and VEALL.GiaMua > 0 ) ) group by KH.Expr1, KH.ma order by KH.Expr1";
                }
                else
                {
                    sqlNV = @"select KH.Expr1, KH.ma
                            from VIEWVEALL VEALL WITH (NOLOCK) 
                            left join VIEW_khachhang KH on KH.f_identity = VEALL.iden3372
                            left join VIEW_HHKHONG HHK on HHK.f_identity = VEALL.iden3367
                            where iden3025='1'  and [NgayMua] >= DATEADD(month,-1,GETDATE()) and KH.ma like N'NV%' and DATEDIFF(day,VEALL.NGAYMUA,GETDATE()) < 30 and ((VEALL.Nghiepvu = N'Void' and VEALL.AIRCODECU in ('1G','1A','1B')) or (VEALL.Nghiepvu = 'EMDS' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = '' and VEALL.GiaMua > 0 ) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS in ('1G','1A','1B')) or (VEALL.Nghiepvu = N'đổi' and  VEALL.GDS not in ('1G','1A','1B') and HHK.mahk = 'VN' and HHK.mahk <> 'QH' and HHK.mahk <> 'VJ') or (VEALL.TKNockt like N'73815%' and VEALL.GiaMua > 0 ) ) group by KH.Expr1, KH.ma order by KH.Expr1";
                }
                using (var conn = new SqlConnection(server_KH_KT))
                {
                    ListTenNV = (List<ListTenNVModel>)conn.Query<ListTenNVModel>(sqlNV, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }

                if (ListVeSot != null)
                {
                    for (int i = 0; i < ListVeSot.Count; i++)
                    {
                        string ma;
                        if (ListVeSot[i].Ma != null && ListVeSot[i].Ma != "")
                        {
                            ma = ListVeSot[i].Ma.Substring(0, 2);
                            if (ma == "NV")
                            {
                                string sqlVeSot = "";
                                if (ListVeSot[i].MaHK.Trim() == "QH")
                                {
                                    sqlVeSot = @"select top 1 * from BAOCAOVESOT where PNR = '" + ListVeSot[i].PNR + "' and SOVE = '" + ListVeSot[i].PNR + "' order by NGAYSUA desc";
                                }
                                else
                                {
                                    sqlVeSot = @"select top 1 * from BAOCAOVESOT where PNR = '" + ListVeSot[i].PNR + "' and SOVE = '" + ListVeSot[i].TKNockt + "' order by NGAYSUA desc";
                                }
                                DataTable tbVeSot = db.ExecuteDataSet(sqlVeSot, CommandType.Text, "server37", null).Tables[0];
                                if (tbVeSot != null)
                                {
                                    if (tbVeSot.Rows.Count > 0)
                                    {
                                        ListVeSot[i].TinhTrang = "Đã báo cáo, chờ KT cập nhật";
                                        ListVeSot[i].NgayUp = DateTime.Parse(tbVeSot.Rows[0]["NGAYSUA"].ToString()).ToString("dd/MM/yyyy HH:mm:ss ");
                                    }
                                    else
                                    {
                                        ListVeSot[i].TinhTrang = "Chưa báo cáo";
                                    }
                                }
                                ListVeSot[i].NgayMua = DateTime.Parse(ListVeSot[i].NgayMua).ToString("dd/MM/yyyy");
                                ListVeSot[i].NgayCapNhat = DateTime.Now.ToString();
                                ListVeSot[i].NhanVienTao = Ten;
                            }
                        }
                    }
                    Del_BCVS_TEMP();
                    Ins_BCVS_TEMP(ListVeSot, Ten);
                    TrangVeSot.ListTen = ListTenNV;
                    TrangVeSot.VeSot = ListVeSot;
                };
                return TrangVeSot;
            }
            catch (Exception)
            {
                TrangVeSot.ThongBao = "Lấy dữ liệu bị lỗi, Liên hệ IT";
                return TrangVeSot;
                throw;
            }
        }
        public bool Ins_BCVS_TEMP(List<VeSotModel> ListVeSot, string Ten)
        {
            int y = 0;
            for (int i = 0; i < ListVeSot.Count; i++)
            {
                string sql = @"INSERT INTO [DANHSACHBAOCAOVE_TEMP] ([MaHK],[Expr1],[PNR],[TKNockt],[NgayMua],[Ma],[ID_HanhT],[TINHTRANG],[NgayUp],[NGAYCAPNHAT],[NHANVIENTAO],[NghiepVu]) 
                                                            VALUES ( @MaHK, @Expr1, @PNR, @TKNockt, @NgayMua, @Ma, @ID_HanhT, @TINHTRANG, @NgayUp, GETDATE(), @NHANVIENTAO,@NghiepVu)";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@MaHK", ListVeSot[i].MaHK));
                Param.Add(new DBase.AddParameters("@Expr1", ListVeSot[i].Expr1));
                Param.Add(new DBase.AddParameters("@PNR", ListVeSot[i].PNR));
                Param.Add(new DBase.AddParameters("@TKNockt", ListVeSot[i].TKNockt));
                Param.Add(new DBase.AddParameters("@NgayMua", ListVeSot[i].NgayMua));
                Param.Add(new DBase.AddParameters("@Ma", ListVeSot[i].Ma));
                Param.Add(new DBase.AddParameters("@ID_HanhT", ListVeSot[i].ID_HanhT));
                Param.Add(new DBase.AddParameters("@TINHTRANG", ListVeSot[i].TinhTrang));
                Param.Add(new DBase.AddParameters("@NgayUp", ListVeSot[i].NgayUp));
                Param.Add(new DBase.AddParameters("@NghiepVu", ListVeSot[i].Nghiepvu));
                Param.Add(new DBase.AddParameters("@NHANVIENTAO", Ten));

                y = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            }

            if (y > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool Del_BCVS_TEMP()
        {
            string sql = "DELETE DANHSACHBAOCAOVE_TEMP";
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
            return true;

        }
        //Update báo cáo vé sót
        public int UpdateBaoCaoVeSot(string server_KT, string server_KH_KT, string PNR, string Hang, string SoVe, string MaKH, string GiaMua = "0", string PhiMua = "0", string PhiBan = "0", string PhiHoan = "0", string ChietKhau = "0", string NguoiUp = "", string MaGioiThieu = "", string NguoiGioiThieu = "", int ID = 0, string LoaiPhi = "", string PhiXuatVe = "0", string SoLuong = "0")
        {
            int x = 0;
            try
            {
                string PNR_old = "";
                string SoVe_old = "";
                string MAKH_old = "";
                string val = "";
                string sqlCheck = @"select * from BAOCAOVESOT where ID = " + ID;
                DataTable tbCheck = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server37", null).Tables[0];
                if (tbCheck != null)
                {
                    PNR_old = tbCheck.Rows[0]["PNR"].ToString();
                    SoVe_old = tbCheck.Rows[0]["SOVE"].ToString();
                    MAKH_old = tbCheck.Rows[0]["MAKH"].ToString();
                }
                string sqlKH = @"select top 1 Expr1 from VIEW_khachhang WITH (NOLOCK)  where ma = '" + MaKH.Trim() + "'";
                using (var conn = new SqlConnection(server_KH_KT))
                {
                    val = conn.QueryFirst<string>(sqlKH, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (val == null || val == "")
                {
                    x = -2;
                    return x;
                }
                if (SoLuong == null)
                {
                    SoLuong = "1";
                }
                if (GiaMua == null)
                {
                    GiaMua = "0";
                }
                if (PhiBan == null)
                {
                    PhiBan = "0";
                }
                if (PhiMua == null)
                {
                    PhiMua = "0";
                }
                if (LoaiPhi == null)
                {
                    LoaiPhi = "";
                }
                if (PhiHoan == null)
                {
                    PhiHoan = "0";
                }
                if (ChietKhau == null)
                {
                    ChietKhau = "0";
                }
                if (PNR == null)
                {
                    PNR = "";
                }
                if (MaGioiThieu == null)
                {
                    MaGioiThieu = "";
                }
                if (NguoiGioiThieu == null)
                {
                    NguoiGioiThieu = "";
                }
                int result1 = 0;
                string SqlUpdateKT = @"Update Life_obj3535 set field13 = '" + MaKH.Trim() + "'," +
                    " field14 = '" + Hang.Trim() + "'," +
                    " field1='" + SoVe.Trim() + "'," +
                    " field6=" + GiaMua.Replace(",", "").Replace(".", "").Trim() + "," +
                    " field8=" + PhiMua.Replace(",", "").Replace(".", "").Trim() + "," +
                    " field17=" + PhiXuatVe.Replace(",", "").Replace(".", "").Trim() + "," +
                    " field11=" + PhiHoan.Replace(",", "").Replace(".", "").Trim() + "," +
                    " field5=" + ChietKhau.Replace(",", "").Replace(".", "").Trim() + "," +
                    " field9='" + PNR.Trim() + "'," +
                    " field15='" + MaGioiThieu.Trim() + "'," +
                    " field16=N'" + NguoiGioiThieu.Trim() + "'," +
                    " dtime=GETDATE(), " +
                    " field4=" + PhiBan.Replace(",", "").Replace(".", "").Trim() + "" +
                    " where field1 = '" + SoVe_old + "' and field9 = '" + PNR_old + "' and field13 = '" + MAKH_old.Trim() + "' ";
                //string SqlInsertKT = @"Insert into Life_obj3535(ngay_thc,field12,field13,field14,field1,field6,field8,field4,field11,field5,field9, field15, field16) 
                //               Values( GETDATE() ,'" + NguoiUp.Trim() + "','" + MaKH.Trim() + "','" + Hang.Trim() + "','" + SoVe.Trim() + "'," + GiaMua.Replace(",", "").Replace(".", "").Trim() + "," + PhiMua.Replace(",", "").Replace(".", "").Trim() + "," + PhiBan.Replace(",", "").Replace(".", "").Trim() + "," + PhiHoan.Replace(",", "").Replace(".", "").Trim() + "," + ChietKhau.Replace(",", "").Replace(".", "").Trim() + ",'" + PNR.Trim() + "',N'" + MaGioiThieu.Trim() + "',N'" + NguoiGioiThieu.Trim() + "')";
                using (var conn = new SqlConnection(server_KT))
                {
                    result1 = conn.Execute(SqlUpdateKT, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result1 > 0)
                {
                    string SqlUpdate = @"Update BAOCAOVESOT set PNR = '" + PNR.Trim() + "'," +
                                       " SOVE = '" + SoVe.Trim() + "'," +
                                       " MAKH='" + MaKH.Trim() + "'," +
                                       " CHIETKHAU=" + ChietKhau.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " GIAMUA=" + GiaMua.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " PHIDVMUA=" + PhiMua.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " LOAIPHI='" + LoaiPhi.Trim() + "'," +
                                       " PHIXUATVE=" + PhiXuatVe.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " PHIDVBAN=" + PhiBan.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " PHIHOAN=" + PhiHoan.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " HANG='" + Hang.Trim() + "'," +
                                       " MAGIOITHIEU='" + MaGioiThieu.Trim() + "'," +
                                       " NGUOIGIOITHIEU=N'" + NguoiGioiThieu.Trim() + "'," +
                                       " NGAYSUA_NEW=GETDATE()," +
                                       " SOLUONG=" + SoLuong.Replace(",", "").Replace(".", "").Trim() + "," +
                                       " SOLANSUA=isnull(SOLANSUA,0) + 1 " +
                                       " where ID = " + ID;

                    int result = db.ExecuteNoneQuery(SqlUpdate, CommandType.Text, "server37", null);
                    if (result > 0)
                    {
                        string sql_update = "updatethongtinbookernhap";
                        using (var conn = new SqlConnection(server_KT))
                        {
                            conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                            conn.Dispose();
                        }
                        x++;
                    }
                }
                else
                {
                    return x;
                }

                if (x > 0)
                {
                    return x;
                }
                return x;
            }
            catch (Exception ex)
            {
                return x;
                throw;
            }

        }
        public bool CheckDataMainEFF(string SoVe, string PNR, string Server)
        {
            string result_sql = "";
            bool result = true;
            try
            {
                string sql = "select top 1 iden3025 from VIEWVEALL  where TKNockt = '" + SoVe + "' and PNR = '" + PNR + "'";
                using (var conn = new SqlConnection(Server))
                {
                    result_sql = conn.QueryFirst<string>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                    conn.Close();
                }
                if (result_sql == "1")
                {
                    result = false;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }

        }
        //Update báo cáo vé sót
        public string InsertBaoCaoVeSotWebVoQuy(string server_EV, string server_KT, string server_KH_KT, string PNR, string Hang, string SoVe, string MaKH, string GiaMua = "", string PhiMua = "", string PhiBan = "", string PhiHoan = "", string ChietKhau = "", string NguoiUp = "", string MaGioiThieu = "", string NguoiGioiThieu = "", int ID = 0, string LoaiPhi = "", string PhiXuatVe = "")
        {
            string result = "";
            try
            {
                if (PhiXuatVe == null)
                {
                    PhiXuatVe = "0";
                }
                if (GiaMua == null)
                {
                    GiaMua = "0";
                }
                if (PhiBan == null)
                {
                    PhiBan = "0";
                }
                if (PhiMua == null)
                {
                    PhiMua = "0";
                }
                if (PhiHoan == null)
                {
                    PhiHoan = "0";
                }
                if (ChietKhau == null)
                {
                    ChietKhau = "0";
                }
                if (PNR == null)
                {
                    PNR = "";
                }
                if (MaGioiThieu == null)
                {
                    MaGioiThieu = "";
                }
                if (NguoiGioiThieu == null)
                {
                    NguoiGioiThieu = "";
                }
                if (LoaiPhi == null)
                {
                    LoaiPhi = "";
                }

                GuiMailDaiLyRepository guimail_Rep = new GuiMailDaiLyRepository(null);
                List<ChiTietVe> chiTietVes = new List<ChiTietVe>();
                ChiTietVe chiTietVe = new ChiTietVe();
                chiTietVe.PNR = PNR;
                chiTietVe.MAHHK = Hang;
                chiTietVe.SoVe = SoVe;
                chiTietVe.MAKH = MaKH;
                chiTietVe.LoaiPhi = LoaiPhi;
                chiTietVe.GiaMua = GiaMua.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.PhiDVMua = PhiMua.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.PhiXuatVe = PhiXuatVe.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.PhiDVBan = PhiBan.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.PhiHoan = PhiHoan.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.ChietKhau = ChietKhau.Replace(",", "").Replace(".", "").Trim();
                chiTietVe.MAGIOITHIEU = MaGioiThieu;
                chiTietVe.NGUOIGIOITHIEU = NguoiGioiThieu;
                chiTietVes.Add(chiTietVe);
                result = guimail_Rep.LuuDetailTicketVoQuy(NguoiUp, chiTietVes, server_KT, server_KH_KT, server_EV);
                return result;

            }
            catch (Exception)
            {
                return result;
            }


        }
        public int SaveBaoCaoVeSot(string server_KT, string server_KH_KT, string PNR, string Hang, string SoVe, string MaKH, string GiaMua, string PhiMua, string PhiBan, string PhiHoan, string ChietKhau, string NguoiUp, string MaGioiThieu = "", string NguoiGioiThieu = "", string LoaiPhi = "", string PhiXuatVe = "0", string SoLuong = "0")
        {
            int x = 0;
            try
            {
                string val = "";
                string sqlCheck = @"select * from BAOCAOVESOT where PNR = '" + PNR + "' and SOVE = '" + SoVe + "'";
                DataTable tbCheck = db.ExecuteDataSet(sqlCheck, CommandType.Text, "server37", null).Tables[0];
                if (tbCheck != null)
                {
                    if (tbCheck.Rows.Count > 0)
                    {
                        x = -1;
                        return x;
                    }
                }
                string sqlKH = @"select top 1 Expr1 from VIEW_khachhang WITH (NOLOCK)  where ma = '" + MaKH.Trim() + "'";
                using (var conn = new SqlConnection(server_KH_KT))
                {
                    val = conn.QueryFirst<string>(sqlKH, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (val == null || val == "")
                {
                    x = -2;
                    return x;
                }
                if (SoLuong == null)
                {
                    SoLuong = "0";
                }
                if (PhiXuatVe == null)
                {
                    PhiXuatVe = "0";
                }
                if (GiaMua == null)
                {
                    GiaMua = "0";
                }
                if (PhiBan == null)
                {
                    PhiBan = "0";
                }
                if (PhiMua == null)
                {
                    PhiMua = "0";
                }
                if (PhiHoan == null)
                {
                    PhiHoan = "0";
                }
                if (ChietKhau == null)
                {
                    ChietKhau = "0";
                }
                if (PNR == null)
                {
                    PNR = "";
                }
                if (MaGioiThieu == null)
                {
                    MaGioiThieu = "";
                }
                if (NguoiGioiThieu == null)
                {
                    NguoiGioiThieu = "";
                }
                if (LoaiPhi == null)
                {
                    LoaiPhi = "";
                }
                int result1 = 0;
                string SqlInsertKT = @"Insert into Life_obj3535(ngay_thc,field12,field13,field14,field1,field6,field8,field17,field11,field5,field9, field15, field16, field4) 
                               Values( GETDATE() ,'" + NguoiUp.Trim() + "','" + MaKH.Trim() + "','" + Hang.Trim() + "','" + SoVe.Trim() + "'," + GiaMua.Replace(",", "").Replace(".", "").Trim() + "," + PhiMua.Replace(",", "").Replace(".", "").Trim() + "," + PhiXuatVe.Replace(",", "").Replace(".", "").Trim() + "," + PhiHoan.Replace(",", "").Replace(".", "").Trim() + "," + ChietKhau.Replace(",", "").Replace(".", "").Trim() + ",'" + PNR.Trim() + "',N'" + MaGioiThieu.Trim() + "',N'" + NguoiGioiThieu.Trim() + "'," + PhiBan.Replace(",", "").Replace(".", "").Trim() + ")";
                using (var conn = new SqlConnection(server_KT))
                {
                    result1 = conn.Execute(SqlInsertKT, null, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (result1 > 0)
                {
                    string SqlInsert = @"INSERT INTO BAOCAOVESOT (PNR, SOVE, NGUOISUA, NGAYSUA, MAKH, CHIETKHAU, GIAMUA, PHIDVMUA, PHIDVBAN, PHIHOAN, HANG, THUOCTINH, MAGIOITHIEU, NGUOIGIOITHIEU, LOAIPHI, PHIXUATVE, SOLUONG) VALUES('" + PNR.Trim() + "', '" + SoVe.Trim() + "', '" + NguoiUp.Trim() + "', GETDATE(), '" + MaKH.Trim() + "', '" + ChietKhau.Replace(",", "").Replace(".", "").Trim() + "', '" + GiaMua.Replace(",", "").Replace(".", "").Trim() + "', '" + PhiMua.Replace(",", "").Replace(".", "").Trim() + "', '" + PhiBan.Replace(",", "").Replace(".", "").Trim() + "', '" + PhiHoan.Replace(",", "").Replace(".", "").Trim() + "', '" + Hang.Trim() + "',0, '" + MaGioiThieu.Trim() + "', N'" + NguoiGioiThieu.Trim() + "', N'" + LoaiPhi.Trim() + "', " + PhiXuatVe.Replace(",", "").Replace(".", "").Trim() + ", " + SoLuong.Replace(",", "").Replace(".", "").Trim() + " )";
                    int result = db.ExecuteNoneQuery(SqlInsert, CommandType.Text, "server37", null);
                    if (result > 0)
                    {
                        string sqlUpdateTemp = @"UPDATE DANHSACHBAOCAOVE_TEMP SET TINHTRANG = N'Đã báo cáo, chờ KT cập nhật' WHERE PNR = '" + PNR.Trim() + "' and TKNockt = '" + SoVe.Trim() + "' and MaHK = '" + Hang.Trim() + "'";
                        int y = db.ExecuteNoneQuery(sqlUpdateTemp, CommandType.Text, "server37", null);

                        string sql_update = "updatethongtinbookernhap";
                        using (var conn = new SqlConnection(server_KT))
                        {
                            conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                            conn.Dispose();
                        }
                        x++;
                    }
                    else
                    {
                        string sqldel = @"delete Life_obj3535 where field1 = '" + SoVe.Trim() + "' and field9 = '" + PNR.Trim() + "'";
                        using (var conn = new SqlConnection(server_KT))
                        {
                            conn.Execute(sqldel, null, null, commandType: CommandType.Text, commandTimeout: 30);
                            conn.Dispose();
                        }
                        return x;
                    }

                }
                else
                {
                    return x;
                }

                if (x > 0)
                {
                    return x;
                }
                return x;
            }
            catch (Exception)
            {
                return x;
                throw;
            }

        }
        public string SearchMaKHBaoCaoVeSot(string MaKH, string server_KH_KT)
        {
            string result = "";
            try
            {
                string sql = @"select top 1 Expr1 from VIEW_khachhang WITH (NOLOCK) where ma = '" + MaKH.Trim() + "'";
                using (var conn = new SqlConnection(server_KH_KT))
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
        public ListVeSotModel SearchVeSot(string maNV, string server_EV_MAIN)
        {
            ListVeSotModel DSVeSot = new ListVeSotModel();
            try
            {
                List<VeSotModel> ListVeSot = new List<VeSotModel>();
                List<ListTenNVModel> ListTenNV = new List<ListTenNVModel>();
                string sqlSMS = @"select top 1 NGUOIGUI as NGUOIGUISMS, NGAYGUI as NGAYGUISMS from LOG_BAOCAOVE order by NGAYGUI desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    DSVeSot = conn.QueryFirst<ListVeSotModel>(sqlSMS, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                string sql = @"select * from DANHSACHBAOCAOVE_TEMP where Ma = '" + maNV + "'";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    ListVeSot = (List<VeSotModel>)conn.Query<VeSotModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                string sqlNV = @"select Expr1, MA from DANHSACHBAOCAOVE_TEMP group by Expr1, MA order by Expr1";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    ListTenNV = (List<ListTenNVModel>)conn.Query<ListTenNVModel>(sqlNV, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }

                if (ListVeSot != null)
                {
                    DSVeSot.VeSot = ListVeSot;
                }
                if (ListTenNV != null)
                {
                    DSVeSot.ListTen = ListTenNV;
                }
                return DSVeSot;
            }
            catch (Exception)
            {
                DSVeSot.ThongBao = "Lấy dữ liệu bị lỗi, Liên hệ IT";
                return DSVeSot;
                throw;
            }
        }
        public Task<string> SoLuongVeSotAsync(string maNV)
        {
            VeSotModel veSot = new VeSotModel();
            string result = "";
            try
            {
                string sql = @"select COUNT(Ma) as SoLuongVeSot from DANHSACHBAOCAOVE_TEMP where Ma = '" + maNV + "' and TINHTRANG = N'Chưa báo cáo'";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    veSot = conn.QueryFirst<VeSotModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                result = veSot.SoLuongVeSot;
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }

        }


        public bool SendSMS_VeSot(string NguoiGui)
        {
            bool result = true;
            List<VeSotModel> ListVeSot = new List<VeSotModel>();
            List<VeSotModel> ListThongTinSMS = new List<VeSotModel>();
            VeSotModel SDT = new VeSotModel();
            VeSotModel SoLuongVeSot = new VeSotModel();
            string sql = @"select Ma from DANHSACHBAOCAOVE_TEMP where TINHTRANG = N'Chưa báo cáo' and NgayMua <> CONVERT(varchar, GETDATE(), 103) group by Ma";
            using (var conn = new SqlConnection(server_EV_MAIN))
            {
                ListVeSot = (List<VeSotModel>)conn.Query<VeSotModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            for (int i = 0; i < ListVeSot.Count; i++)
            {
                VeSotModel ThongTinSMS = new VeSotModel();
                string sqlSdt = @"select DienThoaiSMS as DienThoai, MaNV as Ma from DanhSachLienHe where MaNV = '" + ListVeSot[i].Ma.Trim() + "'";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    SDT = conn.QueryFirst<VeSotModel>(sqlSdt, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                    ThongTinSMS.DienThoai = SDT.DienThoai;
                    ThongTinSMS.Ma = SDT.Ma;
                }

                string sqlSoLuong = @"select COUNT(Ma) as SoLuongVeSot from DANHSACHBAOCAOVE_TEMP where Ma = '" + ListVeSot[i].Ma + "' and TINHTRANG = N'Chưa báo cáo'";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    SoLuongVeSot = conn.QueryFirst<VeSotModel>(sqlSoLuong, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                    ThongTinSMS.SoLuongVeSot = SoLuongVeSot.SoLuongVeSot;
                }
                ListThongTinSMS.Add(ThongTinSMS);
            }
            //SMSService.ServiceSoapClient sms = new SMSService.ServiceSoapClient(SMSService.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
            //string error = sms.SendSMS_VeSot("a1di6Ex+ASHUSbw7Od2bkA==", "8", "84949670670", "NV00293", NguoiGui);
            for (int i = 0; i < ListThongTinSMS.Count; i++)
            {
                ListThongTinSMS[i].DienThoai = "84" + ListThongTinSMS[i].DienThoai.Substring(1, ListThongTinSMS[i].DienThoai.Length - 1);
                SMSService.ServiceSoapClient sms = new SMSService.ServiceSoapClient(SMSService.ServiceSoapClient.EndpointConfiguration.ServiceSoap);
                string error = sms.SendSMS_VeSot("a1di6Ex+ASHUSbw7Od2bkA==", ListThongTinSMS[i].SoLuongVeSot, ListThongTinSMS[i].DienThoai, ListThongTinSMS[i].Ma, NguoiGui);
            }
            return result;
        }
        public bool UpdateStatusEFF(string Active, int RowID)
        {
            bool result = true;
            int result_sql = 0;
            int bool_active = 1;
            if (Active == "false")
            {
                bool_active = 0;
            }
            string sql_update = "update BAOCAOVESOT set TINHTRANGEFF = " + bool_active + " where ID = " + RowID;
            using (var conn = new SqlConnection(server_EV_MAIN))
            {
                result_sql = conn.Execute(sql_update, null, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result_sql > 0)
            {
                return result;
            }
            return false;

        }
    }
}
