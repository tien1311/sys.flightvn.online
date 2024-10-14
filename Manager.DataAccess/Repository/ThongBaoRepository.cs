using EasyInvoice.Json;
using Manager.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class ThongBaoRepository
    {
        DBase db = new DBase();
        public ThongBaoModel ThongBao(string maKH)
        {
            ThongBaoModel ThongBao = new ThongBaoModel();
            try
            {
                List<ThongBao_ALL> List_TBALL = new List<ThongBao_ALL>();
                List<ThongBao_KT> List_TBKT = new List<ThongBao_KT>();
                List<ThongBao_PV> List_TBPV = new List<ThongBao_PV>();
                List<ThongBao_DL> List_TBDL = new List<ThongBao_DL>();

                string SqlDL = @"select KHOA.*,NV.Ten,NV.Yahoo,QLTB.TieuDe,TT.Name from KhoaCodeDaiLy KHOA 
	                                left join KHACHHANG_HOPDONG KH on KH.MAKETOAN = KHOA.MaKH
                                    left join DM_NV NV on KH.MAKINHDOANH = NV.MaNV 
                                    left join QLTHONGBAO QLTB on QLTB.ROWID = KHOA.IDNoiDungKhoa
									Left join [SERVER37].[Manager_V2].[dbo].[AGENT_NOTIFICATION_STATUS] TT on TT.ID =  KHOA.TinhTrangKhoa
                                    where  KHOA.MaKH <> '' order by KHOA.NgayLap Desc ";

                DataTable tb_DL = db.ExecuteDataSet(SqlDL, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < tb_DL.Rows.Count; i++)
                {
                    ThongBao_DL TBDL = new ThongBao_DL();
                    TBDL.MaKH = tb_DL.Rows[i]["MaKH"].ToString();
                    TBDL.TenDaiLy = tb_DL.Rows[i]["TenDaiLy"].ToString();
                    TBDL.NguoiKhoa = tb_DL.Rows[i]["NguoiLap"].ToString();
                    TBDL.NgayKhoa = tb_DL.Rows[i]["NgayLap"].ToString();
                    TBDL.TinhTrang = tb_DL.Rows[i]["Name"].ToString();
                    TBDL.ID = int.Parse(tb_DL.Rows[i]["ID"].ToString());
                    TBDL.TieuDe = tb_DL.Rows[i]["TieuDe"].ToString();
                    TBDL.NVKD = tb_DL.Rows[i]["Ten"].ToString();
                    TBDL.MaKD = tb_DL.Rows[i]["Yahoo"].ToString();
                    TBDL.MaNVLap = tb_DL.Rows[i]["MaNVLap"].ToString();
                    TBDL.MaNV = maKH;
                    if (tb_DL.Rows[i]["NVDaXem"].ToString() != "")
                    {
                        TBDL.DaXem = bool.Parse(tb_DL.Rows[i]["NVDaXem"].ToString());
                    }
                    List_TBDL.Add(TBDL);
                }

                string SqlALL = @"select * from THONGBAO_NOIBO order by NgayLap";
                DataTable tb_ALL = db.ExecuteDataSet(SqlALL, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < tb_ALL.Rows.Count; i++)
                {
                    ThongBao_ALL TBALL = new ThongBao_ALL();

                    TBALL.NguoiLap = tb_ALL.Rows[i]["NguoiLap"].ToString();
                    TBALL.NgayLap = tb_ALL.Rows[i]["NgayLap"].ToString();
                    TBALL.ID = int.Parse(tb_ALL.Rows[i]["ID"].ToString());
                    TBALL.TieuDe = tb_ALL.Rows[i]["TieuDe"].ToString();
                    TBALL.NoiDung = tb_ALL.Rows[i]["NoiDung"].ToString();
                    List_TBALL.Add(TBALL);
                }

                string SqlKT = @"select * from THONGBAO_NOIBO where PhongBan = 'KT' order by NgayLap";
                DataTable tb_KT = db.ExecuteDataSet(SqlKT, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < tb_KT.Rows.Count; i++)
                {
                    ThongBao_KT TBKT = new ThongBao_KT();

                    TBKT.NguoiLap = tb_KT.Rows[i]["NguoiLap"].ToString();
                    TBKT.NgayLap = tb_KT.Rows[i]["NgayLap"].ToString();
                    TBKT.ID = int.Parse(tb_KT.Rows[i]["ID"].ToString());
                    TBKT.TieuDe = tb_KT.Rows[i]["TieuDe"].ToString();
                    TBKT.NoiDung = tb_KT.Rows[i]["NoiDung"].ToString();
                    List_TBKT.Add(TBKT);
                }
                string SqlPV = @"select * from THONGBAO_NOIBO  where PhongBan = 'PV' order by NgayLap";
                DataTable tb_PV = db.ExecuteDataSet(SqlPV, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < tb_PV.Rows.Count; i++)
                {
                    ThongBao_PV TBPV = new ThongBao_PV();

                    TBPV.NguoiLap = tb_PV.Rows[i]["NguoiLap"].ToString();
                    TBPV.NgayLap = tb_PV.Rows[i]["NgayLap"].ToString();
                    TBPV.ID = int.Parse(tb_PV.Rows[i]["ID"].ToString());
                    TBPV.TieuDe = tb_PV.Rows[i]["TieuDe"].ToString();
                    TBPV.NoiDung = tb_PV.Rows[i]["NoiDung"].ToString();
                    List_TBPV.Add(TBPV);
                }

                ThongBao.list_TBALL = List_TBALL;
                ThongBao.list_TBKT = List_TBKT;
                ThongBao.list_TBPV = List_TBPV;
                ThongBao.list_TBDL = List_TBDL;
                return ThongBao;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ThongBaoSinhNhatDaiLi> GetThongBaoSinhNhatDaiLi(string MaNV)
        {
            List<ThongBaoSinhNhatDaiLi> danhSachThongBaoSinhNhat = new List<ThongBaoSinhNhatDaiLi>();
            string sqlQuery = @"select ID,NguoiNhan, NgaySinh, NguoiTiepQuan, NgayTao,ChucVu,Hang,Mien,DaXem from ThongBaoSinhNhatDaiLy where NguoiTiepQuan='"+MaNV+"'  order by NgayTao";
            DataTable tbThongBaoSinhNhat = db.ExecuteDataSet(sqlQuery, CommandType.Text, "server37", null).Tables[0];

            if (tbThongBaoSinhNhat != null && tbThongBaoSinhNhat.Rows.Count > 0)
            {
                for (int i = 0; i < tbThongBaoSinhNhat.Rows.Count; i++)
                {
                    ThongBaoSinhNhatDaiLi thongBao = new ThongBaoSinhNhatDaiLi
                    {
                        ID = (int)tbThongBaoSinhNhat.Rows[i]["ID"],
                        KhachHang = tbThongBaoSinhNhat.Rows[i]["NguoiNhan"].ToString(),
                        NgaySinh = DateTime.Parse(tbThongBaoSinhNhat.Rows[i]["NgaySinh"].ToString()),
                        NguoiTiepQuan = tbThongBaoSinhNhat.Rows[i]["NguoiTiepQuan"].ToString(),
                        NgayTao = DateTime.Parse(tbThongBaoSinhNhat.Rows[i]["NgayTao"].ToString()),
                        ChucVu = tbThongBaoSinhNhat.Rows[i]["ChucVu"].ToString(),
                        Hang = tbThongBaoSinhNhat.Rows[i]["Hang"].ToString(),
                        Mien = tbThongBaoSinhNhat.Rows[i]["Mien"].ToString(),
                        DaXem = tbThongBaoSinhNhat.Rows[i]["DaXem"] == DBNull.Value
                    ? (bool?)null
                    : (bool)tbThongBaoSinhNhat.Rows[i]["DaXem"] // Sử dụng điều kiện để chuyển đổi thành bool?
                    };

                    danhSachThongBaoSinhNhat.Add(thongBao);
                }
            }

            return danhSachThongBaoSinhNhat;
        }


        public ChiTietTB ChiTietThongBao(int ID)
        {
            try
            {
                ChiTietTB TB = new ChiTietTB();
                string SqlView = @"select KHOA.NOIDUNG from KhoaCodeDaiLy KHOA left join DM_NV NV on NV.Ten = KHOA.NguoiLap where KHOA.ID = '" + ID + "'";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TB.NoiDung = dt.Rows[i]["NOIDUNG"].ToString();
                }
                return TB;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public bool DanhDauDaXem(int ID)
        {
            string sql = "update KhoaCodeDaiLy set NVDaXem = 1 where ID = '" + ID + "'";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ActiveKHV(int ID)
        {
            string sql = "update ThongBaoSinhNhatDaiLi set DaXem = 1 where ID = '" + ID + "'";
            List<TangDuLieu.DBase.AddParameters> Param = new List<TangDuLieu.DBase.AddParameters>();
            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public Task<int> DemTBChuaXem(string MaKH)
        {
            int dem =0 , demNB = 0, demDL = 0 ,demKHV=0;
            string SqlDL = @"select KHOA.*,QLTB.TieuDe from KhoaCodeDaiLy KHOA 
	                                left join KHACHHANG_HOPDONG KH on KH.MAKETOAN = KHOA.MaKH
                                    left join DM_NV NV on KH.MAKINHDOANH = NV.MaNV
                                    left join QLTHONGBAO QLTB on QLTB.ROWID = KHOA.IDNoiDungKhoa
                                    where NV.Yahoo = '" + MaKH + "' and KHOA.MaKH <> '' and isnull(KHOA.NVDaXem,0) <> 1 order by KHOA.NgayLap Desc ";
            DataTable tb_DL = db.ExecuteDataSet(SqlDL, CommandType.Text, "server37", null).Tables[0];

            string SqlNB = @"select KHOA.*,QLTB.TieuDe from KhoaCodeDaiLy KHOA 
                                    left join QLTHONGBAO QLTB on QLTB.ROWID = KHOA.IDNoiDungKhoa
                                    where KHOA.MaNV = '" + MaKH + "' and isnull(KHOA.NVDaXem,0) <> 1 order by KHOA.NgayLap Desc";
            DataTable tb_NB = db.ExecuteDataSet(SqlNB, CommandType.Text, "server37", null).Tables[0];
            string SqlKHV = @"select * from ThongBaoSinhNhatDaiLy where NguoiTiepQuan='"+MaKH+ "' and DaXem =0 order by NgayTao Desc";
            DataTable tb_KHV = db.ExecuteDataSet(SqlKHV, CommandType.Text, "server37", null).Tables[0];
            
            demNB = tb_NB.Rows.Count;
            demDL = tb_DL.Rows.Count;
            demKHV = tb_KHV.Rows.Count;
            dem = demDL + demNB+demKHV;
            return Task.FromResult(dem);
        }
        public List<ThongBao_ALL> ThongBaoALL()
        {
            List<ThongBao_ALL> List_TBALL = new List<ThongBao_ALL>();
            string SqlALL = @"select * from THONGBAO_NOIBO order by NgayLap";
            DataTable tb_ALL = db.ExecuteDataSet(SqlALL, CommandType.Text, "server37", null).Tables[0];
            if (tb_ALL != null && tb_ALL.Rows.Count > 0)
            {
                for (int i = 0; i < tb_ALL.Rows.Count; i++)
                {
                    ThongBao_ALL TBALL = new ThongBao_ALL();

                    TBALL.NguoiLap = tb_ALL.Rows[i]["NguoiLap"].ToString();
                    TBALL.NgayLap = tb_ALL.Rows[i]["NgayLap"].ToString();
                    TBALL.ID = int.Parse(tb_ALL.Rows[i]["ID"].ToString());
                    TBALL.TieuDe = tb_ALL.Rows[i]["TieuDe"].ToString();
                    TBALL.NoiDung = tb_ALL.Rows[i]["NoiDung"].ToString();
                    TBALL.PhongBan = tb_ALL.Rows[i]["PhongBan"].ToString();
                    List_TBALL.Add(TBALL);
                }
            }
            return List_TBALL;
        }
        public bool SaveCreateThongBao(string Danhmuc, string Title, string Content, string NguoiLap)
        {
            string sql = "INSERT INTO [THONGBAO_NOIBO] ([TieuDe] ,[NoiDung],[NgayLap],[NguoiLap],[PhongBan]) VALUES ( @Title,@Content,GETDATE(),@CreateEmployee,@DanhMuc)";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@Title", Title));
            Param.Add(new DBase.AddParameters("@Content", Content));
            Param.Add(new DBase.AddParameters("@CreateEmployee", NguoiLap));
            Param.Add(new DBase.AddParameters("@DanhMuc", Danhmuc));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public ChiTietTB EditThongBao(int ID)
        {
            ChiTietTB TBALL = new ChiTietTB();
            string SqlALL = @"select top 1 * from THONGBAO_NOIBO where ID = '" + ID + "'";
            DataTable tb_ALL = db.ExecuteDataSet(SqlALL, CommandType.Text, "server37", null).Tables[0];
            if (tb_ALL != null && tb_ALL.Rows.Count > 0)
            {
                TBALL.ID = tb_ALL.Rows[0]["ID"].ToString();
                TBALL.TieuDe = tb_ALL.Rows[0]["TieuDe"].ToString();
                TBALL.NoiDung = tb_ALL.Rows[0]["NoiDung"].ToString();
            }
            return TBALL;
        }
        public bool SaveEditThongBao(string Danhmuc, string Title, string Content, string NguoiSua, string ID)
        {
            string sql = "UPDATE THONGBAO_NOIBO SET TieuDe = @Title, NoiDung = @Content,  NgaySua = GETDATE(), NguoiSua = @NguoiSua, PhongBan = @DanhMuc WHERE ID = @ID ";
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@Title", Title));
            Param.Add(new DBase.AddParameters("@Content", Content));
            Param.Add(new DBase.AddParameters("@NguoiSua", NguoiSua));
            Param.Add(new DBase.AddParameters("@DanhMuc", Danhmuc));
            Param.Add(new DBase.AddParameters("@ID", ID));

            int i = db.ExecuteNoneQuery(sql, CommandType.Text, "server37", Param);
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
