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
using EasyInvoice.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Dapper;
using Manager.Model.Models.ViewModel;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class DoanhSoHangRepository
    {
        private string server_EV_MAIN; /* "Data Source = .; Initial Catalog = Manager; User ID = sa; Password=EnViet@123;";*/

        public DoanhSoHangRepository(IConfiguration configuration)
        {
            server_EV_MAIN = configuration.GetConnectionString("SQL_EV_MAIN");
        }
        DBase db = new DBase();
        public List<DoanhSoHangViewModel> ListDoanhSoHang(string TuNgay, string DenNgay, string server_KH_KT)
        {
            List<DoanhSoHangViewModel> ListDoanhSoHang = new List<DoanhSoHangViewModel>();
            try
            {
                string sql = @"select convert(nvarchar(10),B.[NgayMua],103) as Ngay, 
                  VN = (select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' AND B.NGAYMUA = A.NGAYMUA),
                  QH = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where ID_HHK='QH' and A.AIRCODEDTC not in ('1A','1B','1G')  AND B.NGAYMUA = A.NGAYMUA),0),
			      VJ = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where A.ID_HHK = 'VJ' and A.AIRCODEDTC not in ('1A','1B','1G') AND B.NGAYMUA = A.NGAYMUA),0),
			      VU = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where A.ID_HHK = 'VU' and A.AIRCODEDTC not in ('1A','1B','1G') AND B.NGAYMUA = A.NGAYMUA),0),
                  IATA = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where A.AIRCODEDTC  in ('1A','1B','1G') and left(A.TKNockt,3) <> '738'   AND B.NGAYMUA = A.NGAYMUA),0),
			      KHAC = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where A.ID_HHK != 'VN' and A.ID_HHK != 'VJ' and A.ID_HHK != 'BL' and A.ID_HHK != 'QH' and A.ID_HHK != 'VU' and A.ID_HanhT != 'BAOHIEM' and A.AIRCODEDTC not in ('1A','1B','1G')   AND B.NGAYMUA = A.NGAYMUA),0)
                  FROM dbo.VIEWVEALL B WITH(NOLOCK)  where B.NgayMua >= '" + TuNgay + "' and B.NgayMua <= '" + DenNgay + @"'
                  group by B.[NgayMua]  order by [NgayMua] ";

                using (var conn = new SqlConnection(server_KH_KT))
                {
                    ListDoanhSoHang = (List<DoanhSoHangViewModel>)conn.Query<DoanhSoHangViewModel>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                    conn.Dispose();
                }
                if (ListDoanhSoHang != null)
                {
                    if (ListDoanhSoHang.Count > 0)
                    {
                        for (int i = 0; i < ListDoanhSoHang.Count; i++)
                        {
                            ListDoanhSoHang[i].TONG = ListDoanhSoHang[i].VN + ListDoanhSoHang[i].QH + ListDoanhSoHang[i].VJ + ListDoanhSoHang[i].VU + ListDoanhSoHang[i].IATA + ListDoanhSoHang[i].KHAC;
                        }
                    }
                }

                return ListDoanhSoHang;
            }
            catch (Exception)
            {
                return ListDoanhSoHang;
            }
        }
        public ChiTietDoanhSoHang ChiTietDoanhSoHang(DateTime Ngay, string Hang, string NgayHienThi, string server_KH_KT)
        {
            ChiTietDoanhSoHang chiTiet = new ChiTietDoanhSoHang();
            List<DoanhSoHang_VN> ListDoanhSoVN = new List<DoanhSoHang_VN>();
            List<DoanhSoHang_IATA> ListDoanhSoIATA = new List<DoanhSoHang_IATA>();
            List<DoanhSoHang_BAMBOO> ListDoanhSoBamBoo = new List<DoanhSoHang_BAMBOO>();
            string sql = "";
            try
            {
                if (Hang == "VN")
                {
                    sql = @"select convert(nvarchar(10),B.[NgayMua],103) as Ngay, 
                      DS_1A = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('1A')    AND B.NGAYMUA = A.NGAYMUA),0),
                      DS_1B = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('1B')    AND B.NGAYMUA = A.NGAYMUA),0),
				      DS_1G = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('1G')    AND B.NGAYMUA = A.NGAYMUA),0),
                      FHQ = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('FHQ')    AND B.NGAYMUA = A.NGAYMUA),0),
                      UJU = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('UJU')    AND B.NGAYMUA = A.NGAYMUA),0),
				      JPQ = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('JPQ')    AND B.NGAYMUA = A.NGAYMUA),0),
				      LXQ = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)= '738' and AIRCODEDTC  in ('LXQ')    AND B.NGAYMUA = A.NGAYMUA),0)
                       FROM dbo.VIEWVEALL B WITH(NOLOCK)  where B.NgayMua >= '" + Ngay + "' and B.NgayMua <= '" + Ngay + @"'
                      group by B.[NgayMua] ";
                    using (var conn = new SqlConnection(server_KH_KT))
                    {
                        ListDoanhSoVN = (List<DoanhSoHang_VN>)conn.Query<DoanhSoHang_VN>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                        conn.Dispose();
                    }
                    chiTiet.ChiTietDoanhSoHang_VN = ListDoanhSoVN;
                }
                else
                {
                    if (Hang == "BAMBOO")
                    {
                        sql = @"select convert(nvarchar(10),B.[NgayMua],103) as Ngay, 
                              DS_1A = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)<> '738' and  AIRCODEDTC  in ('1A')    AND B.NGAYMUA = A.NGAYMUA),0),
                              DS_1B = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where  left(A.TKNockt,3)<> '738' and AIRCODEDTC  in ('1B')    AND B.NGAYMUA = A.NGAYMUA),0),
				              DS_1G = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)<> '738' and  AIRCODEDTC  in ('1G')    AND B.NGAYMUA = A.NGAYMUA),0)
                                FROM dbo.VIEWVEALL B WITH(NOLOCK) where B.NgayMua >= '" + Ngay + "' and B.NgayMua <= '" + Ngay + @"'
                              group by B.[NgayMua] ";

                        using (var conn = new SqlConnection(server_KH_KT))
                        {
                            ListDoanhSoIATA = (List<DoanhSoHang_IATA>)conn.Query<DoanhSoHang_IATA>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                            conn.Dispose();
                        }
                        chiTiet.ChiTietDoanhSoHang_IATA = ListDoanhSoIATA;
                    }
                    else
                    {
                        sql = @"select convert(nvarchar(10),B.[NgayMua],103) as Ngay, 
                              DS_1A = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)<> '738' and  AIRCODEDTC  in ('1A')    AND B.NGAYMUA = A.NGAYMUA),0),
                              DS_1B = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where  left(A.TKNockt,3)<> '738' and AIRCODEDTC  in ('1B')    AND B.NGAYMUA = A.NGAYMUA),0),
				              DS_1G = isnull((select Sum(ABS(ISNULL(A.[GiaMua], 0))) FROM dbo.VIEWVEALL A WITH(NOLOCK)  where left(A.TKNockt,3)<> '738' and  AIRCODEDTC  in ('1G')    AND B.NGAYMUA = A.NGAYMUA),0)
                                FROM dbo.VIEWVEALL B WITH(NOLOCK) where B.NgayMua >= '" + Ngay + "' and B.NgayMua <= '" + Ngay + @"'
                              group by B.[NgayMua] ";
                        using (var conn = new SqlConnection(server_KH_KT))
                        {
                            ListDoanhSoIATA = (List<DoanhSoHang_IATA>)conn.Query<DoanhSoHang_IATA>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                            conn.Dispose();
                        }
                        chiTiet.ChiTietDoanhSoHang_IATA = ListDoanhSoIATA;
                    }
                }
                return chiTiet;
            }
            catch (Exception)
            {
                return chiTiet;
                throw;
            }
        }
        public Task<List<ChiTietSoDuHangModel>> SD_HangIndexAsync()
        {
            List<ChiTietSoDuHangModel> result = new List<ChiTietSoDuHangModel>();
            try
            {
                string sql = "select top 8 *, WarningAmount=isnull(B.Amount,0) from SODUHANG_DETAIL Detail left join SODUHANG on Detail.IDSODUHANG = SODUHANG.ID left join [SERVER37].[Manager_V2].[dbo].[CONFIG_BALANCE_AIRLINE] B on B.Airline = Detail.HANG where STATUS = 1 order by Detail.IDSODUHANG desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    result = (List<ChiTietSoDuHangModel>)conn.Query<ChiTietSoDuHangModel>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                    conn.Dispose();
                }
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }
        }
        public Task<List<ChiTietSoDuHangModel>> SD_HangVNIndexAsync()
        {
            List<ChiTietSoDuHangModel> result = new List<ChiTietSoDuHangModel>();
            try
            {
                string sql = "select top 1 *, WarningAmount=isnull(B.Amount,0) from SODUHANG_DETAIL Detail left join SODUHANG on Detail.IDSODUHANG = SODUHANG.ID  left join [SERVER37].[Manager_V2].[dbo].[CONFIG_BALANCE_AIRLINE] B on B.Airline = Detail.HANG where STATUS = 2 order by Detail.IDSODUHANG desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    result = (List<ChiTietSoDuHangModel>)conn.Query<ChiTietSoDuHangModel>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                    conn.Dispose();
                }
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }
        }
        public Task<List<ChiTietSoDuHangModel>> SD_HangAirInternationIndexAsync()
        {
            List<ChiTietSoDuHangModel> result = new List<ChiTietSoDuHangModel>();
            try
            {
                string sql = "select top 1 * from SODUHANG_DETAIL Detail left join SODUHANG on Detail.IDSODUHANG = SODUHANG.ID where STATUS = 3 order by Detail.IDSODUHANG desc";
                using (var conn = new SqlConnection(server_EV_MAIN))
                {
                    result = (List<ChiTietSoDuHangModel>)conn.Query<ChiTietSoDuHangModel>(sql, null, commandType: CommandType.Text, commandTimeout: 90);
                    conn.Dispose();
                }
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                return Task.FromResult(result);
            }

        }
    }
}
