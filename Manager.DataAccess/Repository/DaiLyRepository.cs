using EasyInvoice.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TangDuLieu;
using System.Data.SqlClient;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class DaiLyRepository
    {
        private readonly string SQL_EV_MAIN;
        private readonly string SQL_KH_KT;
        DBase db = new DBase();

        public DaiLyRepository(IConfiguration configuration)
        {
            SQL_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
            SQL_KH_KT = configuration.GetConnectionString("SQL_KH_KT");
        }

        public List<DaiLyEV> SearchThongTinHD(string DieuKien, string GiaTri, string tungay, string denngay)
        {
            string param = Param(DieuKien, GiaTri, tungay, denngay);
            List<DaiLyEV> daiLyModel = new List<DaiLyEV>();
            string sql = @" select STT=Row_Number() over (order by HD.NGAYLAP), KH.MAKETOAN,HD.IDHOPDONG,KH.TENCONGTY,NV.TEN,HD.NGAYLAP,HD.TRANGTHAIHUY,HD.TRANGTHAIHUYHOPDONG
                            from HOPDONG_KYQUY HD
                            left join KHACHHANG_HOPDONG KH on KH.ID = HD.ID
                            left join File_HopDOng f on HD.IDHOPDONG = f.MaHD
                            left join DM_NV NV on KH.MAKINHDOANH = NV.MANV
                            where" + param + "group by HD.RowID,KH.MAKETOAN,KH.TENCONGTY,HD.IDHOPDONG,HD.NGAYLAP,NV.TEN,HD.TRANGTHAIHUY,HD.TRANGTHAIHUYHOPDONG";
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
                        string sqlfile = @"select TenFile, NgayKy
                                    from FILE_HOPDONG f
                                    where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%' ";
                        DataTable tbfile = db.ExecuteDataSet(sqlfile, CommandType.Text, "server37", null).Tables[0];
                        string sqlSlFile = @"select Count(MaHD) as SOFILE
                                    from FILE_HOPDONG f
                                    where f.MaHD like '" + tb.Rows[i]["IDHOPDONG"].ToString() + "%'  group by MaHD";
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
                        if (tb.Rows[i]["TRANGTHAIHUY"].ToString() != "False")
                        {
                            daily.HuyKyLaiHD = "True";
                        }
                        else
                        {
                            daily.HuyKyLaiHD = "";
                        }
                        if (tb.Rows[i]["TRANGTHAIHUYHOPDONG"].ToString() != "False")
                        {
                            daily.HuyHD = "True";
                        }
                        else
                        {
                            daily.HuyHD = "";
                        }


                        daily.ngaylap = DateTime.Parse(tb.Rows[i]["NGAYLAP"].ToString()).ToString("dd/MM/yyyy");
                        daily.member_kh = tb.Rows[i]["MAKETOAN"].ToString();
                        daily.member_hd = tb.Rows[i]["IDHOPDONG"].ToString();
                        daily.NhanVienKD = tb.Rows[i]["TEN"].ToString();
                        List<string> abc = new List<string>();
                        int dem = 0;
                        for (int f = 0; f < tbfile.Rows.Count; f++)
                        {
                            string a = tbfile.Rows[f]["TenFile"].ToString();
                            abc.Add(a);
                            if (tbfile.Rows[f]["TenFile"].ToString() != null)
                            {
                                dem++;
                            }
                        }
                        if (dem > 0)
                        {
                            daily.ChuKy = "Đã ký";
                        }
                        else
                        {
                            daily.ChuKy = "Chưa ký";
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
                }
            }

            return daiLyModel;
        }
        public string Param(string DieuKien, string GiaTri, string tungay, string denngay)
        {
            string param = " 1=1 ";
            if (DieuKien == "1")
            {
                param += "AND KH.MAKETOAN like '" + GiaTri.Trim() + "%'";
            }
            else
            {
                param += "AND NV.Yahoo like '" + GiaTri.Trim() + "%'";
            }
            if (tungay != null && tungay != "")
            {
                param += "AND HD.NGAYLAP >= '" + tungay.Trim() + "'";
            }
            if (denngay != null && denngay != "")
            {
                param += "AND HD.NGAYLAP <= '" + denngay.Trim() + "'";
            }
            return param;
        }
        public DaiLyModel Search(string DieuKien, string GiaTri)
        {
            string sql = "";
            DaiLyModel daiLyModel = new DaiLyModel();
            DataTable tb = new DataTable();
            List<DaiLySearch> listDaiLy = new List<DaiLySearch>();
            string ma = GiaTri.Substring(0, 2);
            if (DieuKien == "0" && ma == "NV")
            {
                sql = string.Format("SELECT RowID,Yahoo,Ten FROM DM_NV WHERE Yahoo = '{0}'", GiaTri.ToUpper(), GiaTri);
                tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            DaiLySearch dailySearch = new DaiLySearch();
                            dailySearch.member_id = int.Parse(tb.Rows[i]["RowID"].ToString());
                            dailySearch.member_kh = tb.Rows[i]["Yahoo"].ToString();
                            dailySearch.member_company = tb.Rows[i]["Ten"].ToString();
                            dailySearch.checkMaKH = "1";
                            listDaiLy.Add(dailySearch);
                        }
                    }
                }
            }
            else
            {
                if (DieuKien == "1")
                {

                    sql = string.Format("  SELECT member_kh  FROM member left join yahoo_booker on member.member_id = yahoo_booker.member_id WHERE yahoo_nick LIKE N'%{0}%' group by member_kh  ", GiaTri.ToUpper(), GiaTri);
                    tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                }
                if (DieuKien == "0")
                {
                    sql = string.Format(" SELECT member_kh  FROM member WHERE member_kh = '{0}' group by member_kh", GiaTri.ToUpper(), GiaTri);
                    tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                }
                if (DieuKien == "2")
                {
                    sql = string.Format("  SELECT member_kh  FROM member left join phone on member.member_id = phone.AGENT_ID WHERE PHONE_NUMBER LIKE N'%{0}%' group by member_kh  ", GiaTri.ToUpper(), GiaTri);
                    tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                }
                if (DieuKien == "3")
                {
                    sql = string.Format("SELECT MAKETOAN as member_kh from KHACHHANG_HOPDONG WHERE TENCONGTY LIKE N'%{0}%' and MAKETOAN is not null and MAKETOAN <> '' group by MAKETOAN", GiaTri.ToUpper(), GiaTri);
                    tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                }
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            string sqlKH = @"select RowID, MAKETOAN, TENCONGTY, TENTINHTRANGAGENT from KHACHHANG_HOPDONG where MAKETOAN = '" + tb.Rows[i]["member_kh"].ToString() + "'";
                            DataTable tbKH = db.ExecuteDataSet(sqlKH, CommandType.Text, "server37", null).Tables[0];

                            if (tbKH != null)
                            {
                                if (tbKH.Rows.Count > 0)
                                {
                                    for (int y = 0; y < tbKH.Rows.Count; y++)
                                    {
                                        DaiLySearch dailySearch = new DaiLySearch();
                                        dailySearch.member_id = int.Parse(tbKH.Rows[y]["RowID"].ToString());
                                        dailySearch.member_kh = tbKH.Rows[y]["MAKETOAN"].ToString();
                                        dailySearch.member_company = tbKH.Rows[y]["TENCONGTY"].ToString();
                                        dailySearch.tinhtrang = tbKH.Rows[y]["TENTINHTRANGAGENT"].ToString();
                                        dailySearch.checkMaKH = "2";
                                        listDaiLy.Add(dailySearch);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            daiLyModel.DSDaiLy = listDaiLy;
            return daiLyModel;
        }


        public class Agent
        {
            public string AgentId { get; set; }

        }

        public DuNoDaiLy GetDataSoDu(string MaKH)
        {
            DuNoDaiLy result3 = new DuNoDaiLy();
            Agent agent = new Agent();
            try
            {

                string URL = "https://ev-agent.azurewebsites.net/api/debt/status";

                agent.AgentId = MaKH;


                string DATA = JsonConvert.SerializeObject(agent);


                string strUrl = string.Format(URL);
                WebRequest requestObjPost = WebRequest.Create(strUrl);
                requestObjPost.Method = "POST";
                requestObjPost.ContentType = "application/json";

                requestObjPost.Headers.Add("x-functions-key", "JGNXlDFecS7RNLXcUalLAbSdqa28qS8tSPQUb4RvnesqSMHY/gAjmA==");



                using (var streamWrite = new StreamWriter(requestObjPost.GetRequestStream()))
                {
                    streamWrite.Write(DATA);
                    streamWrite.Flush();
                    streamWrite.Close();


                    var httpResponse = (HttpWebResponse)requestObjPost.GetResponse();

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result2 = streamReader.ReadToEnd();
                            result3 = JsonConvert.DeserializeObject<DuNoDaiLy>(result2);

                        }
                    }


                }

                return result3;
            }
            catch (Exception ex)
            {
                return result3;
                throw;
            }

        }


        public DaiLyModel ChiTietDaiLy(string ID, string checkMaKH, string server_KH_KT)
        {
            try
            {
                DaiLyModel daiLyModel = new DaiLyModel();
                List<ThongTinLienHe> lstThongTinLienHe = new List<ThongTinLienHe>();
                List<PhoneModel> list_phone = new List<PhoneModel>();
                if (checkMaKH == "1")
                {
                    string sql = @"select * from DM_NV where RowID = '" + ID.Trim() + "'";
                    DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                    DaiLyEV daiLy = new DaiLyEV();
                    if (tb != null)
                    {
                        if (tb.Rows.Count > 0)
                        {
                            //daiLy.member_id = int.Parse(tb.Rows[0]["member_id"].ToString());
                            daiLy.member_kh = tb.Rows[0]["Yahoo"].ToString();
                            daiLy.member_company = tb.Rows[0]["Ten"].ToString();
                            daiLy.member_email = tb.Rows[0]["Email"].ToString();
                            daiLy.member_tel = tb.Rows[0]["DienThoai"].ToString();
                            if (tb.Rows[0]["TinhTrang"].ToString() == "True")
                            {
                                if (tb.Rows[0]["ThuViec"].ToString() == "True")
                                {
                                    daiLy.tinhtrangdaily = "Thử việc";
                                }
                                if (tb.Rows[0]["ThucTap"].ToString() == "True")
                                {
                                    daiLy.tinhtrangdaily = "Thực tập";
                                }
                                if (tb.Rows[0]["BienChe"].ToString() == "True")
                                {
                                    daiLy.tinhtrangdaily = "Đang làm việc";
                                }
                            }
                            else
                            {
                                daiLy.tinhtrangdaily = "Nghỉ việc";
                            }

                            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                            DuNoDaiLy result = GetDataSoDu(tb.Rows[0]["Yahoo"].ToString());
                            if (result != null)
                            {
                                if (result.Balance != null)
                                {
                                    double sodu = double.Parse(result.Balance);
                                    if (sodu > 0)
                                    {
                                        daiLy.SoDu = "Dương quỹ " + "+" + sodu.ToString("#,###", cul.NumberFormat).Replace(".", ",") + " VNĐ";
                                    }
                                    if (sodu < 0)
                                    {
                                        daiLy.SoDu = "Âm quỹ " + sodu.ToString("#,###", cul.NumberFormat).Replace(".", ",") + " VNĐ";
                                    }
                                    if (sodu == 0)
                                    {
                                        daiLy.SoDu = "0 VNĐ";
                                    }
                                    if (result.Status.Trim() == "K")
                                    {
                                        daiLy.Hang = "K (Khóa)";
                                    }
                                    if (result.Status.Trim() == "C")
                                    {
                                        daiLy.Hang = "C (Xuất vé theo số dư)";
                                    }
                                    if (result.Status.Trim() == "B")
                                    {
                                        daiLy.Hang = "B (Xuất vé theo số dư  và không quá âm quỹ cho phép)";
                                    }
                                    if (result.Status.Trim() == "A")
                                    {
                                        daiLy.Hang = "A (Xuất vé không cần quan tâm số dư)";
                                    }
                                    if (result.AllowedCredit.Substring(0, 3) == "0.0")
                                    {
                                        daiLy.AmQuyChoPhep = "0 VNĐ";
                                    }
                                    else
                                    {
                                        daiLy.AmQuyChoPhep = double.Parse(result.AllowedCredit).ToString("#,###", cul.NumberFormat).Replace(".", ",");

                                    }

                                    if (result.Allowance == "0.0")
                                    {
                                        daiLy.TienBaoLanh = "0 VNĐ";
                                    }
                                    else
                                    {
                                        daiLy.TienBaoLanh = double.Parse(result.Allowance).ToString("#,###", cul.NumberFormat).Replace(".", ",");

                                    }
                                }
                            }
                        }

                        daiLyModel.ThongTinDaiLy = daiLy;
                        daiLyModel.DSLienHe = lstThongTinLienHe;
                        daiLyModel.ListPhone = list_phone;
                    }
                }
                else
                {
                    string sql = @"select top 1 KH.*,HD.MucKyQuy, HD.IDHOPDONG as MaHopDong, Convert(nvarchar(10),HD.NGAYLAP,103) as NgayLapHopDong, HD.GhiChuHD  from KHACHHANG_HOPDONG KH 
                                    left join HOPDONG_KYQUY HD on KH.ID = HD.ID 
                                    left join DM_HOPDONG DM on HD.MAKYQUY = DM.ID
                                     where KH.RowID = '" + ID.Trim() + "'";
                    DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                    DaiLyEV daiLy = new DaiLyEV();
                    if (tb != null)
                    {
                        //if (tb.Rows.Count > 1)
                        //{
                        //    daiLyModel.Message = "Mã KH này bị DUPE thông tin đại lý (trùng mã KH nên không hiển thị được, báo ngay Mr Thiết Trưởng P.Kinh doanh (ĐT : 0969270270 hoặc Chat) để xử lý kịp thời.";
                        //    return daiLyModel;
                        //}
                        if (tb.Rows.Count > 0)
                        {
                            daiLy.GhiChuHopDong = tb.Rows[0]["GhiChuHD"].ToString();
                            daiLy.MaHopDong = tb.Rows[0]["MaHopDong"].ToString();
                            daiLy.NgayLapHopDong = tb.Rows[0]["NgayLapHopDong"].ToString();
                            daiLy.LoaiHopDong = tb.Rows[0]["MucKyQuy"].ToString();
                            daiLy.member_kh = tb.Rows[0]["MAKETOAN"].ToString();
                            daiLy.member_company = tb.Rows[0]["TENCONGTY"].ToString();
                            daiLy.member_address = tb.Rows[0]["DIACHI"].ToString();
                            daiLy.member_email = tb.Rows[0]["EMAIL"].ToString();
                            daiLy.member_tel = tb.Rows[0]["DIENTHOAI"].ToString();
                            if (tb.Rows[0]["TENTINHTRANGAGENT"].ToString() == "2")
                            {
                                daiLy.tinhtrangdaily = "Không hoạt động";
                            }
                            else
                            {
                                daiLy.tinhtrangdaily = "Đang hoạt động";
                            }
                            string sqlNhomVIP = @"SELECT * FROM KHACHHANG_VIP KH left join NHOMKH NHOM on KH.NHOMKH = NHOM.ID where MAKH = '" + tb.Rows[0]["MAKETOAN"].ToString().Trim() + "'";
                            DataTable tbNhomVIP = db.ExecuteDataSet(sqlNhomVIP, CommandType.Text, "server37", null).Tables[0];
                            if (tbNhomVIP != null)
                            {
                                if (tbNhomVIP.Rows.Count > 0)
                                {
                                    daiLy.NhomVIP = tbNhomVIP.Rows[0]["TENNHOM"].ToString();
                                    daiLy.NoiDungVIP = tbNhomVIP.Rows[0]["NOIDUNG"].ToString();
                                    daiLy.GhiChuVIP = tbNhomVIP.Rows[0]["GHICHU"].ToString();
                                }
                            }

                            string sqlTinhTrang = @"select member_isactive, lockReason, member_id from member where member_kh = '" + tb.Rows[0]["MAKETOAN"].ToString().Trim() + "'";
                            DataTable tbTinhTrang = db.ExecuteDataSet(sqlTinhTrang, CommandType.Text, "server18", null).Tables[0];
                            if (tbTinhTrang != null)
                            {
                                if (tbTinhTrang.Rows.Count > 0)
                                {
                                    if (tbTinhTrang.Rows[0]["member_isactive"].ToString() == "0")
                                    {
                                        daiLy.tinhtrang = "Không hoạt động";
                                    }
                                    else { daiLy.tinhtrang = "Đang hoạt động"; }

                                    daiLy.lydo = tbTinhTrang.Rows[0]["lockReason"].ToString();
                                    string sql_lienhe = "SELECT * FROM yahoo_booker WHERE member_id='" + tbTinhTrang.Rows[0]["member_id"].ToString().Trim() + "' order by yahoo_isshow DESC";
                                    DataTable tb_lienhe = db.ExecuteDataSet(sql_lienhe, CommandType.Text, "server18", null).Tables[0];
                                    if (tb_lienhe != null)
                                    {
                                        if (tb_lienhe.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < tb_lienhe.Rows.Count; i++)
                                            {
                                                ThongTinLienHe thongTinLienHe = new ThongTinLienHe();
                                                thongTinLienHe.Nick = tb_lienhe.Rows[i]["yahoo_nick"].ToString();
                                                thongTinLienHe.HoTen = tb_lienhe.Rows[i]["yahoo_fullname"].ToString();
                                                thongTinLienHe.NgayCapNhat = DateTime.Parse(tb_lienhe.Rows[i]["yahoo_date"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                                if (tb_lienhe.Rows[i]["yahoo_isshow"].ToString() == "1")
                                                {
                                                    thongTinLienHe.TinhTrang = "Hoạt động";
                                                }
                                                else
                                                {
                                                    thongTinLienHe.TinhTrang = "Không hoạt động";
                                                }
                                                lstThongTinLienHe.Add(thongTinLienHe);
                                            }
                                        }
                                    }

                                    string sqlPhone = "SELECT * FROM phone WHERE AGENT_ID ='" + tbTinhTrang.Rows[0]["member_id"].ToString().Trim() + "' order by TINHTRANG desc";
                                    DataTable tbl_phone = db.ExecuteDataSet(sqlPhone, CommandType.Text, "server18", null).Tables[0];
                                    if (tbl_phone.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < tbl_phone.Rows.Count; i++)
                                        {
                                            PhoneModel phone = new PhoneModel();
                                            phone.ROWID = int.Parse(tbl_phone.Rows[i]["RowID"].ToString());
                                            phone.AGENT_ID = int.Parse(tbl_phone.Rows[i]["AGENT_ID"].ToString());
                                            phone.AGENT_KH = tbl_phone.Rows[i]["AGENT_KH"].ToString();
                                            phone.FULLNAME = tbl_phone.Rows[i]["FULLNAME"].ToString();
                                            phone.DATE = DateTime.Parse(tbl_phone.Rows[i]["DATE_CREATED"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                                            phone.OFFICE = tbl_phone.Rows[i]["OFFICE"].ToString();
                                            phone.PHONE = tbl_phone.Rows[i]["PHONE_NUMBER"].ToString();
                                            if (tbl_phone.Rows[i]["TINHTRANG"].ToString() == "1")
                                            {
                                                phone.TINHTRANG = "Hoạt động";
                                            }
                                            else
                                            {
                                                phone.TINHTRANG = "Không hoạt động";
                                            }
                                            list_phone.Add(phone);
                                        }
                                    }
                                }
                                else
                                {
                                    daiLy.tinhtrang = "Không có";
                                    daiLy.lydo = "Chưa tạo tài khoản";
                                }
                            }

                            string sql1 = @"select Ten, DienThoai from DM_NV where MaNV = '" + tb.Rows[0]["MAKINHDOANH"].ToString().Trim() + "'";
                            DataTable tb1 = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
                            if (tb1 != null)
                            {
                                if (tb1.Rows.Count > 0)
                                {
                                    daiLy.NhanVienKD = tb1.Rows[0]["Ten"].ToString();
                                    daiLy.SDTKinhDoanh = tb1.Rows[0]["DienThoai"].ToString();
                                }
                                else
                                {
                                    daiLy.NhanVienKD = "Chưa cập nhật";
                                    daiLy.SDTKinhDoanh = "";
                                }
                            }
                            string sql2 = @"select Ten, DienThoai from DM_NV where MaNV = '" + tb.Rows[0]["MANVKETOAN"].ToString().Trim() + "'";
                            DataTable tb2 = db.ExecuteDataSet(sql2, CommandType.Text, "server37", null).Tables[0];
                            if (tb2 != null)
                            {
                                if (tb2.Rows.Count > 0)
                                {
                                    daiLy.KeToanEV = tb2.Rows[0]["Ten"].ToString();
                                    daiLy.SDTKeToan = tb2.Rows[0]["DienThoai"].ToString();
                                }
                                else
                                {
                                    daiLy.KeToanEV = "Chưa cập nhật";
                                    daiLy.SDTKeToan = "";
                                }
                            }

                            string sql4 = @"select note from _DUCUOI_NEW where ID_KhachHang like '%" + tb.Rows[0]["MAKETOAN"].ToString().Trim() + "%'";
                            //DataTable tb4 = db.ExecuteDataSet(sql4, CommandType.Text, "serverKT", null).Tables[0];
                            //if (tb4.Rows.Count > 0 && tb4 != null)
                            //{
                            //    daiLy.NoteKeToan = tb4.Rows[0]["note"].ToString();
                            //}

                            try
                            {
                                using (var conn = new SqlConnection(server_KH_KT))
                                {
                                    daiLy.NoteKeToan = conn.QueryFirst<string>(sql4, null, null, commandType: CommandType.Text, commandTimeout: 30);
                                    conn.Dispose();
                                }
                            }
                            catch (Exception)
                            {
                                daiLy.NoteKeToan = "";

                            }

                            string sql5 = @"select top 1 SoPhut,NgayLap from PHIEUBAOLANH where ID_KhachHang like '%" + tb.Rows[0]["MAKETOAN"].ToString().Trim() + "%' order by NgayLap desc";
                            DataTable tb5 = db.ExecuteDataSet(sql5, CommandType.Text, "server37", null).Tables[0];
                            if (tb5.Rows.Count > 0 && tb5 != null)
                            {
                                daiLy.SoPhut = tb5.Rows[0]["SoPhut"].ToString();
                                daiLy.NgayLapPBL = tb5.Rows[0]["NgayLap"].ToString();
                            }

                            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                            DuNoDaiLy result = GetDataSoDu(tb.Rows[0]["MAKETOAN"].ToString());
                            if (result != null)
                            {
                                if (result.Balance != null)
                                {
                                    double sodu = double.Parse(result.Balance);
                                    if (sodu > 0)
                                    {
                                        daiLy.SoDu = "Dương quỹ " + "+" + sodu.ToString("#,###", cul.NumberFormat).Replace(".", ",") + " VNĐ";
                                    }
                                    if (sodu < 0)
                                    {
                                        daiLy.SoDu = "Âm quỹ " + sodu.ToString("#,###", cul.NumberFormat).Replace(".", ",") + " VNĐ";
                                    }
                                    if (sodu == 0)
                                    {
                                        daiLy.SoDu = "0 VNĐ";
                                    }
                                    if (result.Status.Trim() == "K")
                                    {
                                        daiLy.Hang = "K (Khóa)";
                                    }
                                    if (result.Status.Trim() == "C")
                                    {
                                        daiLy.Hang = "C (Xuất vé theo số dư)";
                                    }
                                    if (result.Status.Trim() == "B")
                                    {
                                        daiLy.Hang = "B (Xuất vé theo số dư  và không quá âm quỹ cho phép)";
                                    }
                                    if (result.Status.Trim() == "A")
                                    {
                                        daiLy.Hang = "A (Xuất vé không cần quan tâm số dư)";
                                    }
                                    if (result.AllowedCredit.Substring(0, 3) == "0.0")
                                    {
                                        daiLy.AmQuyChoPhep = "0 VNĐ";
                                    }
                                    else
                                    {
                                        daiLy.AmQuyChoPhep = double.Parse(result.AllowedCredit).ToString("#,###", cul.NumberFormat).Replace(".", ",");

                                    }

                                    if (result.Allowance == "0.0")
                                    {
                                        daiLy.TienBaoLanh = "0 VNĐ";
                                    }
                                    else
                                    {
                                        daiLy.TienBaoLanh = double.Parse(result.Allowance).ToString("#,###", cul.NumberFormat).Replace(".", ",");

                                    }
                                }
                            }

                            string sql6 = @"select top 1 * from HopDong_KyQuy HD left join KH";


                        }

                        daiLyModel.ThongTinDaiLy = daiLy;
                        daiLyModel.DSLienHe = lstThongTinLienHe;
                        daiLyModel.ListPhone = list_phone;
                    }
                }

                return daiLyModel;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public List<SoVeModel> SearchSoVe(string sove)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            List<SoVeModel> listSubject = new List<SoVeModel>();
            string sql = @"select ID_HHK, AIRCODEDTC, Code_Sign, PNR, TKNockt, Ngay, ID_HanhT, DonGiaMua, LePhiMua, Phidhv, TienThue, PhiDichVM, GiaMua, ChietKhau1, ChietKhau2, GiaBan, PhiDichVB1,PhiDichVB2, KH.Expr1 as DoiTuongNo,bk.Expr1 as Booker
                            from VIEWVEALL 
                            left join VIEW_khachhang KH on KH.f_identity = VIEWVEALL.iden3025
                            left join VIEW_khachhang BK on BK.f_identity = VIEWVEALL.iden3372
                            where TKNockt = '" + sove.Trim() + "'";
            using (var conn = new SqlConnection(SQL_KH_KT))
            {
                listSubject = (List<SoVeModel>)conn.Query<SoVeModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }

            return listSubject;
        }
        public List<VeHoanModel> SearchVeHoan(string sove)
        {
            List<VeHoanModel> listSubject = new List<VeHoanModel>();
            string sql = @"SELECT  stt=Row_Number() over (order by subject_id DESC),subject_ishot,subject_isnew,member.member_kh, subject_date, subject_id,subject_number,subject_content,(convert(nvarchar(10),subject_date,103) + ' ' + convert(nvarchar(10),subject_date,108)) as sub_date,subject_isshow,(SELECT section_name FROM subject_section WHERE subject_section.Section_id=subject.section_id) as SecName,subject_isnew,subject_status_daily,subject_updateby,subject_picnote,convert(nvarchar(10),NgayHoan,103)  as NgayHoanVe   
                            FROM subject 
                            left join member on member.member_id = subject.subject_author
                            WHERE subject_number = '" + sove + "' ORDER BY subject_id DESC";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (dt.Rows.Count > 0 && dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VeHoanModel subject = new VeHoanModel();
                    subject.STT = int.Parse(dt.Rows[i]["stt"].ToString());
                    subject.SoVe = dt.Rows[i]["subject_number"].ToString();
                    subject.NgayGui = dt.Rows[i]["sub_date"].ToString();
                    subject.TinhTrang = dt.Rows[i]["subject_isshow"].ToString();
                    subject.DanhMuc = dt.Rows[i]["SecName"].ToString();
                    subject.ThongTin = dt.Rows[i]["subject_content"].ToString();
                    subject.subject_id = dt.Rows[i]["subject_id"].ToString();
                    subject.subject_ishot = dt.Rows[i]["subject_ishot"].ToString();
                    subject.member_kh = dt.Rows[i]["member_kh"].ToString();
                    if (dt.Rows[i]["NgayHoanVe"].ToString() != "01/01/1900")
                    {
                        subject.NgayHoan = dt.Rows[i]["NgayHoanVe"].ToString();
                    }
                    else
                    {
                        subject.NgayHoan = "";
                    }
                    if (dt.Rows[i]["subject_isnew"].ToString() == "1")
                    {
                        subject.YeuCau = "Delay";
                    }
                    else
                    {
                        if (dt.Rows[i]["subject_isnew"].ToString() == "3")
                        {
                            subject.YeuCau = "EMD";
                        }
                        else
                        {
                            if (dt.Rows[i]["subject_isnew"].ToString() == "2")
                            {
                                subject.YeuCau = "Khẩn";
                            }
                            else
                            {
                                subject.YeuCau = "Bình thường";
                            }
                        }
                    }

                    listSubject.Add(subject);
                }
            }
            return listSubject;
        }
        //Thông tin vé
        public SubjectModel ThongTinVe(int id)
        {
            string Sql = " SELECT *,convert(varchar(10),subject_update,103)+' '+convert(varchar(10),subject_update,108) as ngayxuly,convert(varchar(10),NgayHoan,103) as NgayHoanVe FROM subject WHERE subject_id='" + id + "' ";
            DataTable dt = db.ExecuteDataSet(Sql, CommandType.Text, "server18", null).Tables[0];
            SubjectModel subject = new SubjectModel();
            if (dt.Rows.Count > 0)
            {
                string bookerName = dt.Rows[0]["subject_updateby"].ToString();
                string dienthoaiNV = "";
                string skypeNV = "";
                if (!string.IsNullOrEmpty(bookerName))
                {
                    string sqlBooker = @"SELECT Ten, DienThoai, Skyper From DM_NV where TenDangNhap='" + bookerName + "'";
                    DataTable tb = db.ExecuteDataSet(sqlBooker, CommandType.Text, "server37", null).Tables[0];
                    bookerName = tb.Rows[0][0].ToString();
                    dienthoaiNV = tb.Rows[0][1].ToString();
                    skypeNV = tb.Rows[0][2].ToString();
                }

                string[] subject_name_en = dt.Rows[0]["subject_name_en"].ToString().Split("-");
                subject.NguoiGui = subject_name_en[0].ToString();
                subject.SoDienThoai = subject_name_en[1].ToString();

                subject.subject_code = dt.Rows[0]["subject_code"].ToString();
                subject.subject_name = dt.Rows[0]["subject_name"].ToString();
                subject.subject_id = int.Parse(dt.Rows[0]["subject_id"].ToString());
                subject.subject_isshow = int.Parse(dt.Rows[0]["subject_isshow"].ToString());
                subject.subject_ishot = int.Parse(dt.Rows[0]["subject_ishot"].ToString());
                subject.subject_isnew = int.Parse(dt.Rows[0]["subject_isnew"].ToString());
                subject.subject_number = dt.Rows[0]["subject_number"].ToString();
                subject.subject_status_daily = dt.Rows[0]["subject_status_daily"].ToString();

                subject.subject_content = dt.Rows[0]["subject_content"].ToString();
                subject.SoVeEMD = dt.Rows[0]["SoVeEMD"].ToString();
                subject.TenKhachEMD = dt.Rows[0]["TenKhachHang"].ToString();
                subject.subject_updateby = bookerName;
                subject.subject_picnote = dt.Rows[0]["subject_picnote"].ToString();
                subject.subject_number = dt.Rows[0]["subject_number"].ToString();
                subject.subject_date = dt.Rows[0]["ngayxuly"].ToString();
                subject.code_ticket = dt.Rows[0]["code_ticket"].ToString();
                subject.SoDienThoai = dienthoaiNV;
                subject.Skype = skypeNV;

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
                            subject.YeuCau = "Bình thường";
                        }
                    }
                }

                //Lấy nội dung delay money
                string sql_delaymoney = @"select top 1 * from RefundMoneyAirline";
                string noidung = db.ExecuteDataSet(sql_delaymoney, CommandType.Text, "server18", null).Tables[0].Rows[0]["NoiDung"].ToString();
                subject.DelayMoney = noidung;

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
                subject.ListNhatKy = ListNhatKy;


            }
            return subject;
        }
        public DanhSachDaiLy ListKinhDoanh()
        {
            DanhSachDaiLy TrangDaiLy = new DanhSachDaiLy();
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
                    TrangDaiLy.ListKinhDoanh = ListKD;
                }
            }
            return TrangDaiLy;
        }
        public DanhSachDaiLy ListDaiLy(string MaNV, string DieuKien, string GiaTri, string search, string search_ALL)
        {

            DanhSachDaiLy TrangDaiLy = new DanhSachDaiLy();

            List<KinhDoanh> ListKD = new List<KinhDoanh>();
            List<DaiLyEV> daiLyModel = new List<DaiLyEV>();
            List<CodeSignIn> ListCodeSignIn = new List<CodeSignIn>();
            CodeSignIn CodeSignIn = new CodeSignIn();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string sql = "";

            try
            {
                string sqlTenKD = @"select Ten as TenNV,Yahoo as MaNV from DM_NV where NVKD = '1' and TinhTrang = '1'";
                using (var conn = new SqlConnection(SQL_EV_MAIN))
                {
                    ListKD = (List<KinhDoanh>)conn.Query<KinhDoanh>(sqlTenKD, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                TrangDaiLy.ListKinhDoanh = ListKD;
                if (search_ALL == "ALL")
                {
                    sql = @"select  NVKT.Ten as KeToanEV,KH.TENCONGTY as member_company, KH.MAKETOAN as member_kh,SODU.Status as Hang, SODU.ChoXuat as AmQuyChoPhep, SODU.Sodu as SoDu, SODU.note as NoteKeToan,isnull(MEM.member_isactive,0) as tinhtrang
                            from DM_NV NV  
                            left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MaNV 
                            left join DM_NV NVKT on KH.MANVKETOAN = NVKT.MaNV
                            left join DATATEMP_VE.DATATEMP_VE.dbo._DUCUOI_NEW SODU on SODU.ID_KhachHang COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
							left join SERVER18.Agent.dbo.member MEM on MEM.member_kh COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            where NV.Yahoo = '" + MaNV + "' and TENTINHTRANGAGENT <> 2 and KH.MAKETOAN <> '' group by  NVKT.Ten, KH.MAKETOAN,KH.TENCONGTY,SODU.Status, SODU.ChoXuat, SODU.Sodu, SODU.note,MEM.member_isactive order by SODU.Sodu desc";
                }
                if (search == "search" && DieuKien == "0")
                {
                    sql = @"select  NVKT.Ten as KeToanEV,KH.TENCONGTY as member_company, KH.MAKETOAN as member_kh,SODU.Status as Hang, SODU.ChoXuat as AmQuyChoPhep, SODU.Sodu as SoDu, SODU.note as NoteKeToan,isnull(MEM.member_isactive,0) as tinhtrang
                            from DM_NV NV  
                            left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MaNV 
                            left join DM_NV NVKT on KH.MANVKETOAN = NVKT.MaNV
                            left join DATATEMP_VE.DATATEMP_VE.dbo._DUCUOI_NEW SODU on SODU.ID_KhachHang COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
							left join SERVER18.Agent.dbo.member MEM on MEM.member_kh COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            where NV.Yahoo = '" + MaNV + "' and KH.MAKETOAN like '%" + GiaTri + "%' and TENTINHTRANGAGENT <> 2 and KH.MAKETOAN <> '' group by  NVKT.Ten, KH.MAKETOAN,KH.TENCONGTY,SODU.Status, SODU.ChoXuat, SODU.Sodu, SODU.note,MEM.member_isactive order by SODU.Sodu desc ";
                }
                if (search == "search" && DieuKien == "1")
                {
                    sql = @"select  NVKT.Ten as KeToanEV,KH.TENCONGTY as member_company, KH.MAKETOAN as member_kh,SODU.Status as Hang, SODU.ChoXuat as AmQuyChoPhep, SODU.Sodu as SoDu, SODU.note as NoteKeToan,MEM.isnull(MEM.member_isactive,0) as tinhtrang
                            from DM_NV NV  
                            left join KHACHHANG_HOPDONG KH on KH.MAKINHDOANH = NV.MaNV 
                            left join DM_NV NVKT on KH.MANVKETOAN = NVKT.MaNV
                            left join DATATEMP_VE.DATATEMP_VE.dbo._DUCUOI_NEW SODU on SODU.ID_KhachHang COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            left join DATATEMP_VE.DATATEMP_VE.dbo.VIEW_CodeSignIn as CODE on CODE.ma_3025 COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            left join SERVER18.Agent.dbo.member MEM on MEM.member_kh COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            where NV.Yahoo = '" + MaNV + "' and CODE.f1_3373 like '%" + GiaTri + "%' and TENTINHTRANGAGENT <> 2 and KH.MAKETOAN <> '' group by   NVKT.Ten,KH.MAKETOAN,KH.TENCONGTY,SODU.Status, SODU.ChoXuat, SODU.Sodu, SODU.note,MEM.member_isactive order by SODU.Sodu desc";
                }
                using (var conn = new SqlConnection(SQL_EV_MAIN))
                {
                    daiLyModel = (List<DaiLyEV>)conn.Query<DaiLyEV>(sql, null, commandType: CommandType.Text, commandTimeout: 300);
                    conn.Dispose();
                }
                if (daiLyModel.Count > 0)
                {
                    for (int i = 0; i < daiLyModel.Count; i++)
                    {
                        double soDu = 0;
                        if (daiLyModel[i].SoDu != "" && daiLyModel[i].SoDu != null)
                        {
                            soDu = double.Parse(daiLyModel[i].SoDu.ToString());
                            soDu = soDu * -1;
                            daiLyModel[i].SoDu = soDu.ToString("#,###", cul.NumberFormat).Replace(".", ",");
                        }
                        else { daiLyModel[i].SoDu = "0"; }
                        if (daiLyModel[i].AmQuyChoPhep != "" && daiLyModel[i].AmQuyChoPhep != null)
                        {
                            daiLyModel[i].AmQuyChoPhep = double.Parse(daiLyModel[i].AmQuyChoPhep.ToString()).ToString("#,###", cul.NumberFormat).Replace(".", ",");
                        }
                        else { daiLyModel[i].AmQuyChoPhep = "0"; }
                        if (daiLyModel[i].tinhtrang.ToString() == "0" || daiLyModel[i].tinhtrang == null)
                        {
                            daiLyModel[i].tinhtrang = "Không hoạt động";
                        }
                        else { daiLyModel[i].tinhtrang = "Đang hoạt động"; }

                        string sqlCode = @"select f2_3367 as Hang, f1_3373 as Signin,ma_3025 as MaKH from VIEW_CodeSignIn where ma_3025 = '" + daiLyModel[i].member_kh.ToString() + "' and f2_3367 <> 'BL' order by f2_3367";
                        using (var conn = new SqlConnection(SQL_KH_KT))
                        {
                            ListCodeSignIn = (List<CodeSignIn>)conn.Query<CodeSignIn>(sqlCode, null, commandType: CommandType.Text, commandTimeout: 30);
                            conn.Dispose();
                        }
                        if (ListCodeSignIn.Count > 0)
                        {
                            int demVN = 0, demVJ = 0, demQH = 0, demVU = 0, demBSP = 0;
                            string chuoi = "";
                            for (int y = 0; y < ListCodeSignIn.Count; y++)
                            {
                                string sqlSI = @"select top 1 ma_3025 as MaKH from VIEW_CodeSignIn where f1_3373 = '" + ListCodeSignIn[y].Signin.ToString() + "' and f2_3367 <> 'BL' order by ngay_thc desc";
                                using (var conn = new SqlConnection(SQL_KH_KT))
                                {
                                    CodeSignIn = conn.QueryFirst<CodeSignIn>(sqlSI, null, commandType: CommandType.Text, commandTimeout: 30);
                                    conn.Dispose();
                                }

                                if (CodeSignIn != null)
                                {
                                    if (ListCodeSignIn[y].MaKH.ToString() == CodeSignIn.MaKH.ToString())
                                    {
                                        if (ListCodeSignIn[y].Hang.ToString().Trim() == "VN")
                                        {
                                            demVN++;
                                        }
                                        else
                                        {
                                            if (ListCodeSignIn[y].Hang.ToString().Trim() == "VJ")
                                            {
                                                demVJ++;
                                            }
                                            else
                                            {
                                                if (ListCodeSignIn[y].Hang.ToString().Trim() == "QH")
                                                {
                                                    demQH++;
                                                }
                                                else
                                                {
                                                    if (ListCodeSignIn[y].Hang.ToString().Trim() == "VU")
                                                    {
                                                        demVU++;
                                                    }
                                                    else
                                                    {
                                                        demBSP++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (demVN != 0)
                            {
                                chuoi += demVN + "VN, ";
                            }
                            if (demVJ != 0)
                            {
                                chuoi += demVJ + "VJ, ";
                            }
                            if (demVU != 0)
                            {
                                chuoi += demVU + "VU, ";
                            }
                            if (demQH != 0)
                            {
                                chuoi += demQH + "QH, ";
                            }
                            if (demBSP != 0)
                            {
                                chuoi += demBSP + "BSP";
                            }

                            daiLyModel[i].CodeSignIn = chuoi;
                        }
                        else { daiLyModel[i].CodeSignIn = "Không có Sign In"; }
                    }
                    TrangDaiLy.ListDaiLy = daiLyModel;
                }
                else
                {
                    TrangDaiLy.ThongBao = "Không tìm thấy dữ liệu hoặc giá trị bạn tìm không thuộc quản lý của bạn";
                }
            }
            catch (Exception)
            {
                return TrangDaiLy;
            }

            return TrangDaiLy;
        }
        public List<CodeSignIn> ListCodeSignin(string MaKH)
        {
            List<CodeSignIn> daiLyModel = new List<CodeSignIn>();
            string sql = @"select f2_3367, f5_3373,f1_3373,f6_3373,ma_3025 from VIEW_CodeSignIn where ma_3025 = '" + MaKH + "' and f2_3367 <> 'BL' order by f2_3367";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "serverKT", null).Tables[0];
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    string sqlSI = @"select top 1 f2_3367, f5_3373,f1_3373,f6_3373,ma_3025 from VIEW_CodeSignIn where f1_3373 = '" + tb.Rows[i]["f1_3373"].ToString() + "' and f2_3367 <> 'BL' order by ngay_thc desc";
                    DataTable tbSI = db.ExecuteDataSet(sqlSI, CommandType.Text, "serverKT", null).Tables[0];

                    if (tbSI != null)
                    {
                        if (tbSI.Rows.Count > 0)
                        {
                            if (tb.Rows[i]["ma_3025"].ToString() == tbSI.Rows[0]["ma_3025"].ToString())
                            {
                                CodeSignIn code = new CodeSignIn();
                                code.Hang = tb.Rows[i]["f2_3367"].ToString();
                                code.Mien = tb.Rows[i]["f5_3373"].ToString();
                                code.Code = tb.Rows[i]["f6_3373"].ToString();
                                code.Signin = tb.Rows[i]["f1_3373"].ToString();

                                daiLyModel.Add(code);
                            }
                        }
                    }
                }
            }
            return daiLyModel;
        }
        public List<DaiLyEV> TraCuuSignIn(string giatri, string dieukien)
        {
            List<DaiLyEV> listDaiLy = new List<DaiLyEV>();
            try
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                string sql = "", sqlCode = "", maKT = "", Ma = "";
                if (dieukien == "0")
                {
                    sqlCode = @"select member_kh = ma_3025,HangBay = f2_3367,Mien = f5_3373,Signin = f1_3373,Code = f6_3373 from VIEW_CodeSignIn where f1_3373 = '" + giatri + "' order by ngay_thc desc";
                }
                if (dieukien == "1")
                {
                    sqlCode = @"select member_kh = ma_3025,HangBay = f2_3367,Mien = f5_3373,Signin = f1_3373,Code = f6_3373 from VIEW_CodeSignIn where f6_3373 = '" + giatri + "' order by ngay_thc desc";
                }
                using (var conn = new SqlConnection(SQL_KH_KT))
                {
                    listDaiLy = (List<DaiLyEV>)conn.Query<DaiLyEV>(sqlCode, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                if (listDaiLy.Count > 0)
                {
                    for (int i = 0; i < listDaiLy.Count; i++)
                    {
                        DaiLyEV daiLy = new DaiLyEV();
                        maKT = listDaiLy[i].member_kh.ToString().Trim();
                        Ma = maKT.Substring(0, 2);

                        if (Ma == "NV")
                        {
                            sql = @"select top 1 Yahoo, Ten from DM_NV where Yahoo = '" + maKT + "'";
                            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                            if (tb != null)
                            {
                                if (tb.Rows.Count > 0)
                                {
                                    listDaiLy[i].member_company = tb.Rows[0]["Ten"].ToString();
                                    listDaiLy[i].member_kh = tb.Rows[0]["Yahoo"].ToString();
                                }
                            }
                            ThongTinDaiLyRepository thongTinDaiLy_Res = new ThongTinDaiLyRepository(null);
                            Task<DuNoDaiLy> duNoDaiLy = thongTinDaiLy_Res.LayCongNoNhanVien(maKT);
                            var tongNo = string.Format("{0:0,0}", double.Parse(duNoDaiLy.Result.Balance));
                            listDaiLy[i].SoDu = tongNo;
                        }
                        if (Ma == "KH")
                        {
                            sql = @"select top 1 KeToanEV = NVKT.Ten, NhanVienKD = NVKD.Ten, member_company = KH.TENCONGTY,member_kh = KH.MAKETOAN,SoDu = SODU.Sodu
                            from KHACHHANG_HOPDONG KH
                            left join DM_NV NVKT on KH.MANVKETOAN = NVKT.MaNV
							left join DM_NV NVKD on KH.MAKINHDOANH = NVKD.MaNV
                            left join DATATEMP_VE.DATATEMP_VE.dbo._DUCUOI_NEW SODU on SODU.ID_KhachHang COLLATE DATABASE_DEFAULT = KH.MAKETOAN COLLATE DATABASE_DEFAULT
                            where KH.MAKETOAN like '%" + maKT + "%' and KH.MAKETOAN <> '' ";
                            using (var conn = new SqlConnection(SQL_EV_MAIN))
                            {
                                daiLy = conn.QueryFirst<DaiLyEV>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                                conn.Dispose();
                            }
                            if (daiLy != null)
                            {
                                listDaiLy[i].member_company = daiLy.member_company.ToString();
                                listDaiLy[i].member_kh = daiLy.member_kh.ToString();
                                double soDu = 0;
                                if (daiLy.SoDu.ToString() != "")
                                {
                                    soDu = double.Parse(daiLy.SoDu.ToString());
                                    soDu = soDu * -1;
                                    listDaiLy[i].SoDu = soDu.ToString("#,###", cul.NumberFormat).Replace(".", ",");
                                }
                                else { listDaiLy[i].SoDu = "0"; }
                                listDaiLy[i].KeToanEV = daiLy.KeToanEV.ToString();
                                listDaiLy[i].NhanVienKD = daiLy.NhanVienKD.ToString();
                            }
                            else
                            {
                                sql = @"select top 1 member_kh = ma, member_company = Expr1, KeToanEV = tenkt from VIEW_khachhang where ma like '%" + maKT + "%' ";
                                using (var conn = new SqlConnection(SQL_KH_KT))
                                {
                                    daiLy = conn.QueryFirst<DaiLyEV>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                                    conn.Dispose();
                                }
                                if (daiLy != null)
                                {
                                    listDaiLy[i].member_company = daiLy.member_company.ToString();
                                    listDaiLy[i].member_kh = daiLy.member_kh.ToString();
                                    listDaiLy[i].KeToanEV = daiLy.KeToanEV.ToString();
                                }
                            }
                        }
                    }
                }
                return listDaiLy;
            }
            catch (Exception)
            {
                return listDaiLy;
            }

        }
    }
}
