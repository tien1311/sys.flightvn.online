using Manager.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class ThongTinHDRepository
    {
        DBase db = new DBase();
        public DSHopDongDaiLy SearchThongTinHD(string MaNV, string MaKH, string tungay, string denngay)
        {
            DSHopDongDaiLy DS_HopDongDaiLy = new DSHopDongDaiLy();
            if (MaKH != "" && MaKH != null)
            {
                List<DaiLyEV> daiLyModel = new List<DaiLyEV>();
                string sql = @" select STT=Row_Number() over (order by HD.NGAYLAP), KH.MAKETOAN,HD.IDHOPDONG,KH.TENCONGTY, KH.MAKINHDOANH
                                from HOPDONG_KYQUY HD
                                left join KHACHHANG_HOPDONG KH on KH.ID = HD.ID
                                left join File_HopDOng f on HD.IDHOPDONG = f.MaHD
                                left join DM_NV NV on KH.MAKINHDOANH = NV.MANV
                                where KH.MAKETOAN like '" + MaKH + "%' and NV.YAHOO = '" + MaNV + "' group by HD.RowID,KH.MAKETOAN,KH.TENCONGTY,HD.IDHOPDONG,HD.NGAYLAP,KH.MAKINHDOANH";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            string sqltinhtrangCD = @"select LYDO
                                        from THANHLY_CHAMDUT CD
                                        where CD.IDHOPDONG like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbtinhtrangCD = db.ExecuteDataSet(sqltinhtrangCD, CommandType.Text, "server37", null).Tables[0];

                            string sqltinhtrangLV = @"select LYDO
                                        from THANHLY_CONLAMVIEC CD
                                        where CD.IDHOPDONG like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbtinhtrangLV = db.ExecuteDataSet(sqltinhtrangLV, CommandType.Text, "server37", null).Tables[0];
                            string sqlfile = @"select TenFile
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbfile = db.ExecuteDataSet(sqlfile, CommandType.Text, "server37", null).Tables[0];
                            string sqlSlFile = @"select Count(MaHD) as SOFILE
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%' group by MaHD";
                            DataTable tbslfile = db.ExecuteDataSet(sqlSlFile, CommandType.Text, "server37", null).Tables[0];


                            DaiLyEV daily = new DaiLyEV();
                            daily.member_company = tb.Rows[i]["TENCONGTY"].ToString();
                            if (tbslfile.Rows.Count > 0)
                            {
                                daily.member_file = tbslfile.Rows[0]["SOFILE"].ToString();
                            }
                            else { daily.member_file = "0"; }

                            string sqlHD = @"select MaHD, TENFILE, NGAYKY, ISHOPDONG
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbHD = db.ExecuteDataSet(sqlHD, CommandType.Text, "server37", null).Tables[0];
                            for (int y = 0; y < tbHD.Rows.Count; y++)
                            {
                                if (tbHD.Rows[y]["NGAYKY"].ToString() != null && tbHD.Rows[y]["NGAYKY"].ToString() != "")
                                {
                                    TimeSpan ngay = DateTime.Now - DateTime.Parse(tbHD.Rows[y]["NGAYKY"].ToString());
                                    if (tbHD.Rows[y]["ISHOPDONG"].ToString() != null && int.Parse(tbHD.Rows[y]["ISHOPDONG"].ToString()) > 0 && ngay.Days < 30)
                                    {
                                        daily.HD = "OK";
                                    }
                                    else { daily.HD = "False"; }
                                }
                            }

                            daily.member_kh = tb.Rows[i]["MAKETOAN"].ToString();
                            daily.member_hd = tb.Rows[i]["IDHOPDONG"].ToString();
                            List<string> abc = new List<string>();
                            for (int f = 0; f < tbfile.Rows.Count; f++)
                            {
                                string a = tbfile.Rows[f]["TenFile"].ToString();
                                abc.Add(a);
                            }
                            daily.file = abc;

                            if (tbtinhtrangCD.Rows.Count > 0)
                            {
                                daily.trangthai = "Không hoạt động";
                                daily.tinhtrang = "Thanh lý chấm dứt";
                                daily.lydo = tbtinhtrangCD.Rows[0]["LYDO"].ToString();
                            }
                            else
                            {
                                if (tbtinhtrangLV.Rows.Count > 0)
                                {
                                    daily.trangthai = "Đang hoạt động";
                                    daily.tinhtrang = "Thanh lý còn làm việc";
                                    daily.lydo = tbtinhtrangLV.Rows[0]["LYDO"].ToString();
                                }
                                if (tbtinhtrangCD.Rows.Count == 0 && tbtinhtrangLV.Rows.Count == 0)
                                {
                                    daily.trangthai = "Đang hoạt động";
                                    daily.tinhtrang = "";
                                    daily.lydo = "";
                                }
                            }

                            daiLyModel.Add(daily);
                        }
                        DS_HopDongDaiLy.DSHopDong = daiLyModel;
                    }
                    else
                    {
                        try
                        {
                            string sqlNV = @" select Ten
                                from DM_NV NV
                                left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MANV
                                where KH.MAKETOAN = '" + MaKH + "' ";
                            DataTable tbNV = db.ExecuteDataSet(sqlNV, CommandType.Text, "server37", null).Tables[0];
                            DS_HopDongDaiLy.ThongBao = "Mã KH này thuộc quyền quản lý của nhân viên " + tbNV.Rows[0][0].ToString();
                        }
                        catch (Exception ex)
                        {
                            DS_HopDongDaiLy.ThongBao = "Mã KH này thuộc quyền quản lý của nhân viên";
                        }
                    }
                }
                return DS_HopDongDaiLy;
            }
            else
            {
                List<DaiLyEV> daiLyModel = new List<DaiLyEV>();
                string sql = @" select STT=Row_Number() over (order by HD.NGAYLAP), KH.MAKETOAN,HD.IDHOPDONG,KH.TENCONGTY
                                from HOPDONG_KYQUY HD
                                left join KHACHHANG_HOPDONG KH on KH.ID = HD.ID
                                left join File_HopDOng f on HD.IDHOPDONG = f.MaHD
                                left join DM_NV NV on KH.MAKINHDOANH = NV.MANV
                                where HD.NGAYLAP >= '" + tungay + "' and HD.NGAYLAP <= '" + denngay + "' and NV.YAHOO = '" + MaNV + "' group by HD.RowID,KH.MAKETOAN,KH.TENCONGTY,HD.IDHOPDONG,HD.NGAYLAP";

                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            string sqltinhtrangCD = @"select LYDO
                                        from THANHLY_CHAMDUT CD
                                        where CD.IDHOPDONG like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbtinhtrangCD = db.ExecuteDataSet(sqltinhtrangCD, CommandType.Text, "server37", null).Tables[0];

                            string sqltinhtrangLV = @"select LYDO
                                        from THANHLY_CONLAMVIEC CD
                                        where CD.IDHOPDONG like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbtinhtrangLV = db.ExecuteDataSet(sqltinhtrangLV, CommandType.Text, "server37", null).Tables[0];

                            string sqlfile = @"select TenFile
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbfile = db.ExecuteDataSet(sqlfile, CommandType.Text, "server37", null).Tables[0];
                            string sqlSlFile = @"select Count(MaHD) as SOFILE
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%' group by MaHD";
                            DataTable tbslfile = db.ExecuteDataSet(sqlSlFile, CommandType.Text, "server37", null).Tables[0];


                            DaiLyEV daily = new DaiLyEV();
                            daily.member_company = tb.Rows[i]["TENCONGTY"].ToString(); ;
                            if (tbslfile.Rows.Count > 0)
                            {
                                daily.member_file = tbslfile.Rows[0]["SOFILE"].ToString();
                            }
                            else { daily.member_file = "0"; }
                            string sqlHD = @"select MaHD, TENFILE, NGAYKY, ISHOPDONG
                                        from FILE_HOPDONG f
                                        where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'";
                            DataTable tbHD = db.ExecuteDataSet(sqlHD, CommandType.Text, "server37", null).Tables[0];
                            for (int y = 0; y < tbHD.Rows.Count; y++)
                            {
                                if (tbHD.Rows[y]["NGAYKY"].ToString() != null && tbHD.Rows[y]["NGAYKY"].ToString() != "")
                                {
                                    TimeSpan ngay = DateTime.Now - DateTime.Parse(tbHD.Rows[y]["NGAYKY"].ToString());
                                    if (tbHD.Rows[y]["ISHOPDONG"].ToString() != null && int.Parse(tbHD.Rows[y]["ISHOPDONG"].ToString()) > 0 && ngay.Days < 30)
                                    {
                                        daily.HD = "OK";
                                    }
                                    else { daily.HD = "False"; }
                                }
                            }
                            daily.member_kh = tb.Rows[i]["MAKETOAN"].ToString();
                            daily.member_hd = tb.Rows[i]["IDHOPDONG"].ToString();


                            for (int f = 0; f < tbfile.Rows.Count; f++)
                            {
                                string a = tbfile.Rows[f]["TenFile"].ToString();
                                daily.file.Add(a);
                            }
                            if (tbtinhtrangCD.Rows.Count > 0)
                            {
                                daily.trangthai = "Không hoạt động";
                                daily.tinhtrang = "Thanh lý chấm dứt";
                                daily.lydo = tbtinhtrangCD.Rows[0]["LYDO"].ToString();
                            }
                            else
                            {
                                if (tbtinhtrangLV.Rows.Count > 0)
                                {
                                    daily.trangthai = "Đang hoạt động";
                                    daily.tinhtrang = "Thanh lý còn làm việc";
                                    daily.lydo = tbtinhtrangLV.Rows[0]["LYDO"].ToString();
                                }
                                if (tbtinhtrangCD.Rows.Count == 0 && tbtinhtrangLV.Rows.Count == 0)
                                {
                                    daily.trangthai = "Đang hoạt động";
                                    daily.tinhtrang = "";
                                    daily.lydo = "";
                                }
                            }
                            daiLyModel.Add(daily);
                        }
                        DS_HopDongDaiLy.DSHopDong = daiLyModel;
                    }
                }
                return DS_HopDongDaiLy;
            }

        }
    }
}
