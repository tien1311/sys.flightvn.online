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
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class LichLamViecRepository
    {
        DBase db = new DBase();
        public List<LichLamViecModel> LichLamViec()
        {
            List<LichLamViecModel> list = new List<LichLamViecModel>();
            try
            {
                //Lấy 10 ngày tiếp theo
                DateTime ngay_1 = DateTime.Now;
                DateTime ngay_hwa = ngay_1.AddDays(-1);
                DateTime ngay_2 = ngay_1.AddDays(1);
                DateTime ngay_3 = ngay_1.AddDays(2);
                DateTime ngay_4 = ngay_1.AddDays(3);
                DateTime ngay_5 = ngay_1.AddDays(4);

                //ngày
                string ngayhwa = "NGAY" + ngay_hwa.Day;
                string ngay1 = "NGAY" + ngay_1.Day;
                string ngay2 = "NGAY" + ngay_2.Day;
                string ngay3 = "NGAY" + ngay_3.Day;
                string ngay4 = "NGAY" + ngay_4.Day;
                string ngay5 = "NGAY" + ngay_5.Day;

                //tháng năm
                string thanghwa = ngay_hwa.Month + "/" + ngay_hwa.Year;
                string thang1 = ngay_1.Month + "/" + ngay_1.Year;
                string thang2 = ngay_2.Month + "/" + ngay_2.Year;
                string thang3 = ngay_3.Month + "/" + ngay_3.Year;
                string thang4 = ngay_4.Month + "/" + ngay_4.Year;
                string thang5 = ngay_5.Month + "/" + ngay_5.Year;

                //ngày tháng năm
                string datehwa = ngay_hwa.Day + "/" + ngay_hwa.Month + "/" + ngay_hwa.Year;
                string date1 = ngay_1.Day + "/" + ngay_1.Month + "/" + ngay_1.Year;
                string date2 = ngay_2.Day + "/" + ngay_2.Month + "/" + ngay_2.Year;
                string date3 = ngay_3.Day + "/" + ngay_3.Month + "/" + ngay_3.Year;
                string date4 = ngay_4.Day + "/" + ngay_4.Month + "/" + ngay_4.Year;
                string date5 = ngay_5.Day + "/" + ngay_5.Month + "/" + ngay_5.Year;

                string demhwa = "Dem" + ngay_hwa.Day + "/" + ngay_hwa.Month + "/" + ngay_hwa.Year;
                string demNgay1 = "Dem" + ngay_1.Day + "/" + ngay_1.Month + "/" + ngay_1.Year;
                string demNgay2 = "Dem" + ngay_2.Day + "/" + ngay_2.Month + "/" + ngay_2.Year;
                string demNgay3 = "Dem" + ngay_3.Day + "/" + ngay_3.Month + "/" + ngay_3.Year;
                string demNgay4 = "Dem" + ngay_4.Day + "/" + ngay_4.Month + "/" + ngay_4.Year;
                string demNgay5 = "Dem" + ngay_5.Day + "/" + ngay_5.Month + "/" + ngay_5.Year;
                string FileHwa = "FileHwa", FileNgay1 = "FileNgay1", FileNgay2 = "FileNgay2", FileNgay3 = "FileNgay3", FileNgay4 = "FileNgay4", FileNgay5 = "FileNgay5";
                string SqlView = @"select DM.Ten, DM.DienThoai, DM.Line, GLV.GioLamViec,DM.MaNV,DM.PHONGBAN,DM.THEOCA,
                        (select " + ngayhwa + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thanghwa + @"') as '" + datehwa + @"',
			            (select " + ngay1 + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thang1 + @"') as '" + date1 + @"',
			            (select " + ngay2 + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thang2 + @"') as '" + date2 + @"',
			            (select " + ngay3 + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thang3 + @"') as '" + date3 + @"',
                        (select " + ngay4 + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thang4 + @"') as '" + date4 + @"',
                        (select " + ngay5 + @" from chamcong where CHAMCONG.MANV = DM.MaNV and THANGNAM='" + thang5 + @"') as '" + date5 + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngayhwa + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thanghwa + @"') as '" + demhwa + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngay1 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang1 + @"') as '" + demNgay1 + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngay2 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang2 + @"') as '" + demNgay2 + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngay3 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang3 + @"') as '" + demNgay3 + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngay4 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang4 + @"') as '" + demNgay4 + @"',
                        (select COUNT(TENFILE) from FileChamCong where NGAY='" + ngay5 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang5 + @"') as '" + demNgay5 + @"',
                         (select TENFILE from FileChamCong where NGAY='" + ngayhwa + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thanghwa + @"') as '" + FileHwa + @"',
                                    (select TENFILE from FileChamCong where NGAY='" + ngay1 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang1 + @"') as '" + FileNgay1 + @"',
                                    (select TENFILE from FileChamCong where NGAY='" + ngay2 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang2 + @"') as '" + FileNgay2 + @"',
                                    (select TENFILE from FileChamCong where NGAY='" + ngay3 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang3 + @"') as '" + FileNgay3 + @"',
                                    (select TENFILE from FileChamCong where NGAY='" + ngay4 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang4 + @"') as '" + FileNgay4 + @"',
                                    (select TENFILE from FileChamCong where NGAY='" + ngay5 + "' and FileChamCong.MANV = DM.MaNV and THANGNAM='" + thang5 + @"') as '" + FileNgay5 + @"'
			            from DM_NV DM 		
					    left join GIOLAMVIEC GLV on DM.GioLamViec = GLV.ID
					    WHERE DM.TinhTrang = 1 group by DM.Ten, DM.DienThoai, DM.Line, GLV.GioLamViec,DM.TENNV,DM.MaNV,DM.PHONGBAN,DM.THEOCA order by DM.TENNV";
                DataTable dt = db.ExecuteDataSet(SqlView, CommandType.Text, "server37", null).Tables[0];


                string ThuHwa = ngay_hwa.DayOfWeek.ToString();
                string ThuNgay1 = ngay_1.DayOfWeek.ToString();
                string ThuNgay2 = ngay_2.DayOfWeek.ToString();
                string ThuNgay3 = ngay_3.DayOfWeek.ToString();
                string ThuNgay4 = ngay_4.DayOfWeek.ToString();
                string ThuNgay5 = ngay_5.DayOfWeek.ToString();
                switch (ThuHwa)
                {
                    case "Monday":
                        ThuHwa = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuHwa = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuHwa = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuHwa = "Thứ 5";
                        break;
                    case "Friday":
                        ThuHwa = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuHwa = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuHwa = "Chủ Nhật";
                        break;

                }
                switch (ThuNgay1)
                {
                    case "Monday":
                        ThuNgay1 = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuNgay1 = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuNgay1 = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuNgay1 = "Thứ 5";
                        break;
                    case "Friday":
                        ThuNgay1 = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuNgay1 = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuNgay1 = "Chủ Nhật";
                        break;

                }
                switch (ThuNgay2)
                {
                    case "Monday":
                        ThuNgay2 = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuNgay2 = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuNgay2 = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuNgay2 = "Thứ 5";
                        break;
                    case "Friday":
                        ThuNgay2 = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuNgay2 = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuNgay2 = "Chủ Nhật";
                        break;

                }
                switch (ThuNgay3)
                {
                    case "Monday":
                        ThuNgay3 = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuNgay3 = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuNgay3 = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuHwa = "Thứ 5";
                        break;
                    case "Friday":
                        ThuNgay3 = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuNgay3 = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuNgay3 = "Chủ Nhật";
                        break;

                }
                switch (ThuNgay4)
                {
                    case "Monday":
                        ThuNgay4 = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuNgay4 = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuNgay4 = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuNgay4 = "Thứ 5";
                        break;
                    case "Friday":
                        ThuNgay4 = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuNgay4 = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuNgay4 = "Chủ Nhật";
                        break;

                }
                switch (ThuNgay5)
                {
                    case "Monday":
                        ThuNgay5 = "Thứ 2";
                        break;
                    case "Tuesday":
                        ThuNgay5 = "Thứ 3";
                        break;
                    case "Wednesday":
                        ThuNgay5 = "Thứ 4";
                        break;
                    case "Thursday":
                        ThuNgay5 = "Thứ 5";
                        break;
                    case "Friday":
                        ThuNgay5 = "Thứ 6";
                        break;
                    case "Saturday":
                        ThuNgay5 = "Thứ 7";
                        break;
                    case "Sunday":
                        ThuNgay5 = "Chủ Nhật";
                        break;

                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LichLamViecModel NV = new LichLamViecModel();
                    NV.PhongBan = dt.Rows[i]["PhongBan"].ToString();
                    NV.Ten = dt.Rows[i]["Ten"].ToString();
                    NV.DT = dt.Rows[i]["DienThoai"].ToString();
                    NV.Line = dt.Rows[i]["Line"].ToString();
                    NV.Gio = dt.Rows[i]["GioLamViec"].ToString();
                    NV.FileHwa = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileHwa"].ToString();
                    NV.FileNgay1 = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileNgay1"].ToString();
                    NV.FileNgay2 = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileNgay2"].ToString();
                    NV.FileNgay3 = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileNgay3"].ToString();
                    NV.FileNgay4 = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileNgay4"].ToString();
                    NV.FileNgay5 = "https://hr.tripdata.vn/upload/ChamCong/" + dt.Rows[i]["FileNgay5"].ToString();
                    NV.ThuHwa = ThuHwa;
                    NV.ThuNgay1 = ThuNgay1;
                    NV.ThuNgay2 = ThuNgay2;
                    NV.ThuNgay3 = ThuNgay3;
                    NV.ThuNgay4 = ThuNgay4;
                    NV.ThuNgay5 = ThuNgay5;
                    NV.HWA = dt.Rows[i][datehwa].ToString();
                    NV.Ngay1 = dt.Rows[i][date1].ToString();
                    NV.Ngay2 = dt.Rows[i][date2].ToString();
                    NV.Ngay3 = dt.Rows[i][date3].ToString();
                    NV.Ngay4 = dt.Rows[i][date4].ToString();
                    NV.Ngay5 = dt.Rows[i][date5].ToString();
                    NV.DemFileHW = dt.Rows[i][demhwa].ToString();
                    NV.DemFileNgay1 = dt.Rows[i][demNgay1].ToString();
                    NV.DemFileNgay2 = dt.Rows[i][demNgay2].ToString();
                    NV.DemFileNgay3 = dt.Rows[i][demNgay3].ToString();
                    NV.DemFileNgay4 = dt.Rows[i][demNgay4].ToString();
                    NV.DemFileNgay5 = dt.Rows[i][demNgay5].ToString();


                    if (dt.Rows[i]["PHONGBAN"].ToString().ToUpper() == "PHÒNG VÉ")
                    {
                        if (dt.Rows[i]["THEOCA"].ToString() == "True")
                        {
                            string sqlGioHwa = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_hwa.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_hwa.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGIOHWA = db.ExecuteDataSet(sqlGioHwa, CommandType.Text, "server37", null).Tables[0];
                            string sqlGioNgay1 = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_1.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_1.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGioNgay1 = db.ExecuteDataSet(sqlGioNgay1, CommandType.Text, "server37", null).Tables[0];
                            string sqlGioNgay2 = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_2.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_2.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGioNgay2 = db.ExecuteDataSet(sqlGioNgay2, CommandType.Text, "server37", null).Tables[0];
                            string sqlGioNgay3 = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_3.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_3.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGioNgay3 = db.ExecuteDataSet(sqlGioNgay3, CommandType.Text, "server37", null).Tables[0];
                            string sqlGioNgay4 = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_4.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_4.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGioNgay4 = db.ExecuteDataSet(sqlGioNgay4, CommandType.Text, "server37", null).Tables[0];
                            string sqlGioNgay5 = @"select LT.GIOBD, LT.PHUTBD, LT.GIOKT, LT.PHUTKT
                                        from LICHTRUC LT
                                        left join DM_NV DM on LT.MANV = DM.MANV
                                        where '" + dt.Rows[i]["MANV"].ToString() + "' = DM.MANV and LT.NGAYLAM >= '" + ngay_5.ToString("MM/dd/yyyy") + "' and LT.NGAYLAM <= '" + ngay_5.ToString("MM/dd/yyyy") + "' order by GIOBD desc";
                            DataTable dtGioNgay5 = db.ExecuteDataSet(sqlGioNgay5, CommandType.Text, "server37", null).Tables[0];
                            if (dtGIOHWA != null)
                            {
                                if (dtGIOHWA.Rows.Count > 0)
                                {
                                    NV.GioHwa_2 = dtGIOHWA.Rows[0]["GIOBD"].ToString() + ":" + dtGIOHWA.Rows[0]["PHUTBD"].ToString() + " - " + dtGIOHWA.Rows[0]["GIOKT"].ToString() + ":" + dtGIOHWA.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGIOHWA.Rows.Count > 1)
                                {
                                    NV.GioHwa_1 = dtGIOHWA.Rows[1]["GIOBD"].ToString() + ":" + dtGIOHWA.Rows[1]["PHUTBD"].ToString() + " - " + dtGIOHWA.Rows[1]["GIOKT"].ToString() + ":" + dtGIOHWA.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            if (dtGioNgay1 != null)
                            {
                                if (dtGioNgay1.Rows.Count > 0)
                                {
                                    NV.GioNgay1_2 = dtGioNgay1.Rows[0]["GIOBD"].ToString() + ":" + dtGioNgay1.Rows[0]["PHUTBD"].ToString() + " - " + dtGioNgay1.Rows[0]["GIOKT"].ToString() + ":" + dtGioNgay1.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGioNgay1.Rows.Count > 1)
                                {
                                    NV.GioNgay1_1 = dtGioNgay1.Rows[1]["GIOBD"].ToString() + ":" + dtGioNgay1.Rows[1]["PHUTBD"].ToString() + " - " + dtGioNgay1.Rows[1]["GIOKT"].ToString() + ":" + dtGioNgay1.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            if (dtGioNgay2 != null)
                            {
                                if (dtGioNgay2.Rows.Count > 0)
                                {
                                    NV.GioNgay2_2 = dtGioNgay2.Rows[0]["GIOBD"].ToString() + ":" + dtGioNgay2.Rows[0]["PHUTBD"].ToString() + " - " + dtGioNgay2.Rows[0]["GIOKT"].ToString() + ":" + dtGioNgay2.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGioNgay2.Rows.Count > 1)
                                {
                                    NV.GioNgay2_1 = dtGioNgay2.Rows[1]["GIOBD"].ToString() + ":" + dtGioNgay2.Rows[1]["PHUTBD"].ToString() + " - " + dtGioNgay2.Rows[1]["GIOKT"].ToString() + ":" + dtGioNgay2.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            if (dtGioNgay3 != null)
                            {
                                if (dtGioNgay3.Rows.Count > 0)
                                {
                                    NV.GioNgay3_2 = dtGioNgay3.Rows[0]["GIOBD"].ToString() + ":" + dtGioNgay3.Rows[0]["PHUTBD"].ToString() + " - " + dtGioNgay3.Rows[0]["GIOKT"].ToString() + ":" + dtGioNgay3.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGioNgay3.Rows.Count > 1)
                                {
                                    NV.GioNgay3_1 = dtGioNgay3.Rows[1]["GIOBD"].ToString() + ":" + dtGioNgay3.Rows[1]["PHUTBD"].ToString() + " - " + dtGioNgay3.Rows[1]["GIOKT"].ToString() + ":" + dtGioNgay3.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            if (dtGioNgay4 != null)
                            {
                                if (dtGioNgay4.Rows.Count > 0)
                                {
                                    NV.GioNgay4_2 = dtGioNgay4.Rows[0]["GIOBD"].ToString() + ":" + dtGioNgay4.Rows[0]["PHUTBD"].ToString() + " - " + dtGioNgay4.Rows[0]["GIOKT"].ToString() + ":" + dtGioNgay4.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGioNgay4.Rows.Count > 1)
                                {
                                    NV.GioNgay4_1 = dtGioNgay4.Rows[1]["GIOBD"].ToString() + ":" + dtGioNgay4.Rows[1]["PHUTBD"].ToString() + " - " + dtGioNgay4.Rows[1]["GIOKT"].ToString() + ":" + dtGioNgay4.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            if (dtGioNgay5 != null)
                            {
                                if (dtGioNgay5.Rows.Count > 0)
                                {
                                    NV.GioNgay5_2 = dtGioNgay5.Rows[0]["GIOBD"].ToString() + ":" + dtGioNgay5.Rows[0]["PHUTBD"].ToString() + " - " + dtGioNgay5.Rows[0]["GIOKT"].ToString() + ":" + dtGioNgay5.Rows[0]["PHUTKT"].ToString();
                                }
                                if (dtGioNgay5.Rows.Count > 1)
                                {
                                    NV.GioNgay5_1 = dtGioNgay5.Rows[1]["GIOBD"].ToString() + ":" + dtGioNgay5.Rows[1]["PHUTBD"].ToString() + " - " + dtGioNgay5.Rows[1]["GIOKT"].ToString() + ":" + dtGioNgay5.Rows[1]["PHUTKT"].ToString();
                                }
                            }
                            NV.Gio = "False";
                        }
                    }
                    list.Add(NV);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
