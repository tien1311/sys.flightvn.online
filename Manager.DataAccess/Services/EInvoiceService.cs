using EasyInvoice.Client;
using EasyInvoice.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TangDuLieu;
using System.Globalization;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services.Model.Request;
using Manager.Model.Models.HoaDonModels.HDDT;
using Manager.Model.Models.ViewModel;
using Manager.DataAccess.Helpers;

namespace Manager.DataAccess.Services
{
    public class EInvoiceService : IEInvoiceService
    {
        public IHttpClientFactory _httpClientFactory;
        public EInvoiceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        DBase db = new DBase();
        public async Task<DanhSachHDDTResponse> DanhSachHDDT(DanhSachHDDTRequest request)
        {

            var httpClient = _httpClientFactory.CreateClient("invoice-service");
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("eInvoice/search", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DanhSachHDDTResponse>(responseContent);
        }

        public async Task<List<EInvoice_CT>> DanhSachVeHDDT(DanhSachHDDTResponse response, string ikey, string pattern, string serial)
        {
            List<EInvoice_CT> ChiTietHDDT = new List<EInvoice_CT>();
            for (int i = 0; i < response.Result.Count; i++)
            {
                if (response.Result[i].tongQuat.iKey == ikey && response.Result[i].tongQuat.MAUSO == pattern)
                {
                    ChiTietHDDT = response.Result[i].ChiTietHDDT;
                }

            }
            return ChiTietHDDT;
        }
        public async Task<string> GetMaYeuCau(string ikey)
        {
            string result = "";
            string sql = @"select MaYeuCau from YEUCAU_HOADON_VAT where NgayLap >= '2021-11-01' and ikey = '" + ikey + "'";
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
        public async Task<ReturnObject> HDDTIn(InHDDTRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("invoice-service");
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("eInvoice/pdf", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReturnObject>(responseContent);


        }
        public async Task<BaseResponse> HDDTCancel(DeleteCancelHDDTRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("invoice-service");
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("eInvoice/cancel", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BaseResponse>(responseContent);


        }


        public async Task<bool> UpdateNgayChungTu(string Ngay)
        {
            try
            {

                DateTime date = DateTime.ParseExact(Ngay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string sql = "select  * from HDDT_TONGQUAT where DaXoa = 0 and MONTH(NgayLap) = " + date.Month + " and YEAR(NGAYLAP) = " + date.Year + " and NgayChungTu is  null";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];

                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string[] ikeys = new string[tb.Rows.Count];
                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            ikeys[i] = tb.Rows[i]["IKey"].ToString();
                        }
                        //Lấy No bên EasyInvoice
                        Response res = HoaDonHelper.GetInvoicesKey(ikeys);
                        for (int y = 0; y < tb.Rows.Count; y++)
                        {
                            for (int z = 0; z < res.Data.Invoices.Count; z++)
                            {
                                if (tb.Rows[y]["Ikey"].ToString().Trim() == res.Data.Invoices[z].Ikey.Trim())
                                {
                                    string update = @"Update HDDT_TONGQUAT set NgayChungTu = '" + DateTime.ParseExact(res.Data.Invoices[z].ArisingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                                .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' where ikey = '" + tb.Rows[y]["Ikey"].ToString().Trim() + "'";
                                    db.ExecuteNoneQuery(update, CommandType.Text, "server37", null);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
    }
}
