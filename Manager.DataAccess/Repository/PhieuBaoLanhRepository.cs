using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using Microsoft.AspNetCore.Mvc;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class PhieuBaoLanhRepository
    {
        DBase db = new DBase();
        PhieuBaoLanhModel PhieuBaoLanh = new PhieuBaoLanhModel();
        public PhieuBaoLanhModel DSPhieuBaoLanh()
        {

            List<DSPhieuBaoLanhModel> listDSPhieuBaoLanh = new List<DSPhieuBaoLanhModel>();
            string sql = @"select * from PHIEUBAOLANH where TinhTrang = 1  order by NgayLap Desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DSPhieuBaoLanhModel phieuBaoLanh = new DSPhieuBaoLanhModel();

                    phieuBaoLanh.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                    phieuBaoLanh.ID_KhachHang = tb.Rows[i]["ID_KhachHang"].ToString();
                    phieuBaoLanh.TenDaiLy = tb.Rows[i]["TenDaiLy"].ToString();
                    phieuBaoLanh.GhiChu = tb.Rows[i]["GhiChu"].ToString();
                    phieuBaoLanh.BaoLanh = double.Parse(tb.Rows[i]["BaoLanh"].ToString());
                    phieuBaoLanh.MaPhieu = tb.Rows[i]["MaPhieu"].ToString();
                    phieuBaoLanh.NgayLap = DateTime.Parse(tb.Rows[i]["NgayLap"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    phieuBaoLanh.NhanVienLap = tb.Rows[i]["NhanVienLap"].ToString();
                    phieuBaoLanh.SoPhut = tb.Rows[i]["SoPhut"].ToString();
                    if (tb.Rows[i]["NgaySua"].ToString() != "")
                    {
                        phieuBaoLanh.NgaySua = DateTime.Parse(tb.Rows[i]["NgaySua"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        phieuBaoLanh.NgaySua = "";
                    }
                    phieuBaoLanh.NhanVienSua = tb.Rows[i]["NhanVienSua"].ToString();
                    if (tb.Rows[i]["NgayXoa"].ToString() != "")
                    {
                        phieuBaoLanh.NgayXoa = DateTime.Parse(tb.Rows[i]["NgayXoa"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        phieuBaoLanh.NgayXoa = "";
                    }

                    phieuBaoLanh.NhanVienXoa = tb.Rows[i]["NhanVienXoa"].ToString();
                    phieuBaoLanh.TinhTrang = bool.Parse(tb.Rows[i]["TinhTrang"].ToString());

                    listDSPhieuBaoLanh.Add(phieuBaoLanh);
                }

                PhieuBaoLanh.DSPhieuBaoLanh = listDSPhieuBaoLanh;
            }
            return PhieuBaoLanh;
        }
        public PhieuBaoLanhModel DSDaiLy(string MaKH)
        {
            List<DSDaiLyModel> listDSDaiLy = new List<DSDaiLyModel>();
            string SqlDaiLy = " SELECT *,convert(nvarchar(10),member_date,103) as member_dates FROM member WHERE member_kh='" + MaKH + "'";
            DataTable dl = db.ExecuteDataSet(SqlDaiLy, CommandType.Text, "server18", null).Tables[0];
            if (dl.Rows.Count > 0 && dl != null)
            {
                DSDaiLyModel thongtin_dl = new DSDaiLyModel();
                thongtin_dl.member_kh = dl.Rows[0]["member_kh"].ToString();
                thongtin_dl.member_company = dl.Rows[0]["member_company"].ToString();
                listDSDaiLy.Add(thongtin_dl);
            }
            PhieuBaoLanh.DSDaiLy = listDSDaiLy;
            PhieuBaoLanh.DSPhieuBaoLanh = DSPhieuBaoLanh().DSPhieuBaoLanh;
            return PhieuBaoLanh;
        }
        public PhieuBaoLanhModel SearchPBL(string MaKH)
        {

            List<DSPhieuBaoLanhModel> listDSPhieuBaoLanh = new List<DSPhieuBaoLanhModel>();
            string sql = @"select * from PHIEUBAOLANH where TinhTrang = 1 and ID_KhachHang='" + MaKH + "' order by NgayLap Desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DSPhieuBaoLanhModel phieuBaoLanh = new DSPhieuBaoLanhModel();

                    phieuBaoLanh.ID = int.Parse(tb.Rows[i]["ID"].ToString());
                    phieuBaoLanh.ID_KhachHang = tb.Rows[i]["ID_KhachHang"].ToString();
                    phieuBaoLanh.TenDaiLy = tb.Rows[i]["TenDaiLy"].ToString();
                    phieuBaoLanh.GhiChu = tb.Rows[i]["GhiChu"].ToString();
                    phieuBaoLanh.BaoLanh = double.Parse(tb.Rows[i]["BaoLanh"].ToString());
                    phieuBaoLanh.SoPhut = tb.Rows[i]["SoPhut"].ToString();
                    phieuBaoLanh.NgayLap = DateTime.Parse(tb.Rows[i]["NgayLap"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    phieuBaoLanh.NhanVienLap = tb.Rows[i]["NhanVienLap"].ToString();
                    if (tb.Rows[i]["NgaySua"].ToString() != "")
                    {
                        phieuBaoLanh.NgaySua = DateTime.Parse(tb.Rows[i]["NgaySua"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        phieuBaoLanh.NgaySua = "";
                    }
                    phieuBaoLanh.NhanVienSua = tb.Rows[i]["NhanVienSua"].ToString();
                    if (tb.Rows[i]["NgayXoa"].ToString() != "")
                    {
                        phieuBaoLanh.NgayXoa = DateTime.Parse(tb.Rows[i]["NgayXoa"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        phieuBaoLanh.NgayXoa = "";
                    }

                    phieuBaoLanh.NhanVienXoa = tb.Rows[i]["NhanVienXoa"].ToString();
                    phieuBaoLanh.TinhTrang = bool.Parse(tb.Rows[i]["TinhTrang"].ToString());

                    listDSPhieuBaoLanh.Add(phieuBaoLanh);
                }

                PhieuBaoLanh.DSPhieuBaoLanh = listDSPhieuBaoLanh;
            }
            return PhieuBaoLanh;
        }
        public bool SavePBL(string MaKH, string tenDL, string ghichu, string tien, string tenNV, string thoigian)
        {
            try
            {
                string MaPBL = PhatSinhMaPhieu();
                double baolanh = Convert.ToDouble(tien);
                if (ghichu == null)
                {
                    ghichu = "";
                }
                string sql = "INSERT INTO [PHIEUBAOLANH] ([ID_KhachHang] ,[BaoLanh],[GhiChu] ,[NgayLap] ,[NhanVienLap] ,[NgaySua] ,[NhanVienSua] ,[NgayXoa],[NhanVienXoa],[TinhTrang],[TenDaiLy],[MaPhieu],[SoPhut]) VALUES ( @MaKH,@baolanh,@ghichu,GETDATE(),@tenNV,null,null,null,null,@tinhtrang,@tenDL,@MaPBL,@thoigian)";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@MaKH", MaKH));
                Param.Add(new DBase.AddParameters("@tenDL", tenDL));
                Param.Add(new DBase.AddParameters("@ghichu", ghichu));
                Param.Add(new DBase.AddParameters("@baolanh", baolanh));
                Param.Add(new DBase.AddParameters("@tenNV", tenNV));
                Param.Add(new DBase.AddParameters("@MaPBL", MaPBL));
                Param.Add(new DBase.AddParameters("@tinhtrang", "1"));
                Param.Add(new DBase.AddParameters("@ngaysua", ""));
                Param.Add(new DBase.AddParameters("@nhanviensua", ""));
                Param.Add(new DBase.AddParameters("@ngayxoa", ""));
                Param.Add(new DBase.AddParameters("@nhanvienxoa", ""));
                Param.Add(new DBase.AddParameters("@thoigian", thoigian));

                int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);

                if (i > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool EditPBL(string MaKH, string tenDL, string ghichu, string tien, string tenNV, string ID, string thoigian)
        {

            if (ID == null)
            {
                try
                {
                    string MaPBL = PhatSinhMaPhieu();
                    double baolanh = Convert.ToDouble(tien);
                    if (ghichu == null)
                    {
                        ghichu = "";
                    }
                    string sql = "INSERT INTO [PHIEUBAOLANH] ([ID_KhachHang] ,[BaoLanh],[GhiChu] ,[NgayLap] ,[NhanVienLap] ,[NgaySua] ,[NhanVienSua] ,[NgayXoa],[NhanVienXoa],[TinhTrang],[TenDaiLy],[SoPhut],[MaPhieu]) VALUES ( @MaKH,@baolanh,@ghichu,GETDATE(),@tenNV,null,null,null,null,@tinhtrang,@tenDL,@thoigian,@MaPBL)";
                    List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                    Param.Add(new DBase.AddParameters("@MaKH", MaKH));
                    Param.Add(new DBase.AddParameters("@tenDL", tenDL));
                    Param.Add(new DBase.AddParameters("@ghichu", ghichu));
                    Param.Add(new DBase.AddParameters("@baolanh", baolanh));
                    Param.Add(new DBase.AddParameters("@tenNV", tenNV));
                    Param.Add(new DBase.AddParameters("@tinhtrang", "1"));
                    Param.Add(new DBase.AddParameters("@ngaysua", ""));
                    Param.Add(new DBase.AddParameters("@nhanviensua", ""));
                    Param.Add(new DBase.AddParameters("@ngayxoa", ""));
                    Param.Add(new DBase.AddParameters("@nhanvienxoa", ""));
                    Param.Add(new DBase.AddParameters("@thoigian", thoigian));
                    Param.Add(new DBase.AddParameters("@MaPBL", MaPBL));

                    int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);

                    if (i > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                try
                {
                    double baolanh = Convert.ToDouble(tien);
                    string sql = @"UPDATE PHIEUBAOLANH SET SoPhut = '" + thoigian + "', ID_KhachHang = '" + MaKH + "', BaoLanh = '" + baolanh + "',GhiChu = N'" + ghichu + "', NgaySua = GETDATE(),NhanVienSua = N'" + tenNV + "',TinhTrang = 1,TenDaiLy = N'" + tenDL + "' where ID = " + ID;
                    int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                    if (i > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
        public bool DelPBL(string ID, string tenNV)
        {
            try
            {
                string sql = @"UPDATE PHIEUBAOLANH SET NgayXoa = GETDATE(),NhanVienXoa = N'" + tenNV + "', TinhTrang = 0 where ID = " + ID;
                int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", null);
                if (i > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string PhatSinhMaPhieu()
        {
            try
            {
                string Maphieu = "";
                string sql = @"select top 1 MaPhieu from PhieuBaoLanh where MaPhieu like N'%-" + DateTime.Now.Year + "%' order by NgayLap Desc ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string ten = tb.Rows[0]["MaPhieu"].ToString();
                        if (ten != "")
                        {
                            string soThuTu = ten.Substring(3, 4);
                            int STT = int.Parse(soThuTu) + 1;
                            if (STT > 0 && STT < 10)
                            {
                                Maphieu = "PBL000" + STT + "-" + DateTime.Now.Year;
                            }
                            if (STT >= 10 && STT < 100)
                            {
                                Maphieu = "PBL00" + STT + "-" + DateTime.Now.Year;
                            }
                            if (STT >= 100 && STT < 1000)
                            {
                                Maphieu = "PBL0" + STT + "-" + DateTime.Now.Year;
                            }
                            if (STT >= 1000)
                            {
                                Maphieu = "PBL" + STT.ToString() + "-" + DateTime.Now.Year;
                            }


                        }
                        else
                        {
                            Maphieu = "PBL0001-" + DateTime.Now.Year;
                        }

                    }
                }
                else
                {
                    Maphieu = "PBL0001-" + DateTime.Now.Year;
                }
                return Maphieu;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
