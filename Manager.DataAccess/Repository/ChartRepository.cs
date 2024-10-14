using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;
using System.Data;
using Manager.Model.Models.ViewModel;

namespace Manager.DataAccess.Repository
{
    public class ChartRepository
    {
        DBase db = new DBase();
        public List<ImportDoanhSoViewModel> GetDoanhSo(string MaKH, string Nam)
        {
            List<ImportDoanhSoViewModel> listDoanhSo = new List<ImportDoanhSoViewModel>();
            string sql = "select * from DoanhThuDaiLy where MaKH = '" + MaKH + "' and Nam = '" + Nam + "' order by Thang";
            DataTable dt = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (dt != null)
            {
                int x = 1;
                int z = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int y = x; y <= 12; y++)
                    {
                        ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();
                        if (int.Parse(dt.Rows[i]["Thang"].ToString()) == z)
                        {
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
                            break;
                        }
                        if (int.Parse(dt.Rows[i]["Thang"].ToString()) == y)
                        {
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

                            x = int.Parse(dt.Rows[i]["Thang"].ToString()) + 1;
                            z = int.Parse(dt.Rows[i]["Thang"].ToString());
                            break;
                        }
                        else
                        {
                            doanhso.MaKH = MaKH;
                            doanhso.Tong = 0;
                            doanhso.VN = 0;
                            doanhso.VJ = 0;
                            doanhso.IATA = 0;
                            doanhso.QH = 0;
                            doanhso.VU = 0;
                            doanhso.Khac = 0;
                            doanhso.Thang = y.ToString();
                            doanhso.Nam = Nam;
                            listDoanhSo.Add(doanhso);
                        }
                    }
                }
                int tongthang = listDoanhSo.Count;
                int dem = 1;
                for (int i = 1; i <= 12 - tongthang; i++)
                {
                    ImportDoanhSoViewModel doanhso = new ImportDoanhSoViewModel();

                    doanhso.MaKH = MaKH;
                    doanhso.Tong = 0;
                    doanhso.VN = 0;
                    doanhso.VJ = 0;
                    doanhso.IATA = 0;
                    doanhso.QH = 0;
                    doanhso.VU = 0;
                    doanhso.Khac = 0;
                    doanhso.Thang = (tongthang + dem).ToString();
                    doanhso.Nam = Nam;
                    listDoanhSo.Add(doanhso);
                    dem++;
                }
            }
            return listDoanhSo;
        }
    }
}
