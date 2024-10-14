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
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using Dapper;
using Manager.Model.Services.Notification.Request;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class HangRepository
    {
        private readonly INotifyService _notifyService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticateService _authenticateService;
        public string grant_type_tr = "password";
        public string username_tr = "nv0001602";
        public string password_tr = "Enviet@123";
        public string client_secret_tr = "VsDk65AUskJgxlsKZQFl2uq57p7I0y2o";
        public string client_id_tr = "enviet";
        private readonly string SERVER_EV_V2 = "";

        public HangRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAuthenticateService authenticateService, INotifyService notifyService)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
            _notifyService = notifyService;
            SERVER_EV_V2 = _configuration.GetConnectionString("SQL_EV_MAIN_V2");
        }

        DBase db = new DBase();
        public List<SoDuHangViewModel> ListSoDuHang(int status)
        {
            List<SoDuHangViewModel> ListSoDuHangView = new List<SoDuHangViewModel>();
            string where = "";
            if (status != 0)
            {
                where = " and status = " + status;
            }
            string sql = "select top 20 * from SoDuHang where 1=1 and Status = 1 order by ID Desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            string sql2 = "select top 20 * from SoDuHang where 1=1 and Status in (2,3)  order by ID Desc";
            DataTable tb2 = db.ExecuteDataSet(sql2, CommandType.Text, "server37", null).Tables[0];

            if (tb2 != null)
            {
                if (tb2.Rows.Count > 0)
                {
                    for (int i = 0; i < tb2.Rows.Count; i++)
                    {
                        DataRow dr = tb.NewRow();
                        dr[0] = tb2.Rows[i][0].ToString();
                        dr[1] = tb2.Rows[i][1].ToString();
                        dr[2] = tb2.Rows[i][2].ToString();
                        dr[3] = tb2.Rows[i][3].ToString();
                        tb.Rows.Add(dr);
                    }
                }
            }
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        List<ChiTietSoDuHangModel> ListChiTiet = new List<ChiTietSoDuHangModel>();
                        SoDuHangViewModel soDuHangView = new SoDuHangViewModel();
                        SoDuHangModel soDuHang = new SoDuHangModel();
                        soDuHang.ID = int.Parse(tb.Rows[i][0].ToString());
                        soDuHang.NgayLap = DateTime.Parse(tb.Rows[i][1].ToString());
                        soDuHang.NguoiLap = tb.Rows[i][2].ToString();
                        soDuHang.Status = int.Parse(tb.Rows[i][3].ToString());
                        soDuHangView.SoDuHangModel = soDuHang;

                        string sql1 = "select * from SoDuHang_Detail where IDSODUHANG=" + tb.Rows[i]["ID"].ToString();
                        DataTable tb1 = db.ExecuteDataSet(sql1, CommandType.Text, "server37", null).Tables[0];
                        for (int y = 0; y < tb1.Rows.Count; y++)
                        {
                            ChiTietSoDuHangModel chiTiet = new ChiTietSoDuHangModel();
                            chiTiet.ID = int.Parse(tb1.Rows[y][0].ToString());
                            chiTiet.IDSoDuHang = int.Parse(tb1.Rows[y][1].ToString());
                            chiTiet.Hang = tb1.Rows[y][2].ToString();
                            chiTiet.SoTien = tb1.Rows[y][3].ToString();
                            ListChiTiet.Add(chiTiet);
                        }
                        soDuHangView.chiTietHangModel = ListChiTiet;
                        ListSoDuHangView.Add(soDuHangView);
                    }
                }
            }
            return ListSoDuHangView;
        }

        //Get token bsp api
        public async Task<string> AuthenticateTR()
        {
            string access_token = "";
            try
            {
                string endPoint = "https://secure.onlineairticket.vn/realms/jhipster/protocol/openid-connect/token";
                var client = new HttpClient();
                var data = new[]
                {
                    new KeyValuePair<string, string>("username", username_tr),
                    new KeyValuePair<string, string>("password", password_tr),
                    new KeyValuePair<string, string>("grant_type", grant_type_tr),
                    new KeyValuePair<string, string>("client_id", client_id_tr),
                    new KeyValuePair<string, string>("client_secret", client_secret_tr)
                 };
                var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateModel>(responseContent);
                return result.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> Authenticate()
        {
            try
            {
                string endPoint = "http://api.bsp.onlineairticket.vn/token";
                var client = new HttpClient();

                var data = new[]
                {
                    new KeyValuePair<string, string>("username", "2110212"),
                    new KeyValuePair<string, string>("password", "Enviet@2345@"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "api"),
                 };
                var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
                var responseContent = await response.Content.ReadAsStringAsync();

                AuthenticateModel result = JsonConvert.DeserializeObject<AuthenticateModel>(responseContent);
                return result.access_token;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //Get token bsp api
        public async Task<string> Authenticate_MT()
        {
            try
            {
                string endPoint = "http://api2.bsp.onlineairticket.vn/token";
                var client = new HttpClient();

                var data = new[]
                {
                    new KeyValuePair<string, string>("username", "2110212"),
                    new KeyValuePair<string, string>("password", "Enviet@2345@"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", "api"),
                 };
                var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
                var responseContent = await response.Content.ReadAsStringAsync();

                AuthenticateModel result = JsonConvert.DeserializeObject<AuthenticateModel>(responseContent);
                return result.access_token;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetTokenVNA(int id)
        {
            try
            {
                string sql = "select top 1 * from Token_VNA where id = " + id;
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
                string result = tb.Rows[0][1].ToString();
                string[] split = result.Split(" ");
                result = split[1].Trim().ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<double> GetBalance_TR()
        {
            double result = 0;
            try
            {

                string accessToken = await AuthenticateTR();
                BalanceResponse result_data = new BalanceResponse();
                try
                {
                    string endPoint = "https://bsp.onlineairticket.vn/services/proxy/api/flights/balance-tay-ho";
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await client.GetAsync(endPoint);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    result_data = Newtonsoft.Json.JsonConvert.DeserializeObject<BalanceResponse>(responseContent);
                    result = result_data.balance;
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
            catch (Exception)
            {
                return result;
            }
        }
        public async Task<ResponseRetrieveAgencyCredit> RetrieveAgencyCredit(string airlineSystem, string Airline, string Region)
        {
            RequestAirlineSystem request = new RequestAirlineSystem();
            RequestAirlineSystem_MT request_MT = new RequestAirlineSystem_MT();
            string accessToken = "";
            string strUrl = "";
            string jsonContent = "";
            if (Region != "")
            {
                strUrl = string.Format("http://api2.bsp.onlineairticket.vn/api/Category/RetrieveAgencyCredit");
                request_MT.AirlineSystem = airlineSystem;
                request_MT.BookingRegion = Region;
                jsonContent = JsonConvert.SerializeObject(request_MT);
                accessToken = Authenticate_MT().Result;
            }
            else
            {
                strUrl = string.Format("http://api.bsp.onlineairticket.vn/api/Category/RetrieveAgencyCredit");
                request.AirlineSystem = airlineSystem;
                jsonContent = JsonConvert.SerializeObject(request);
                accessToken = Authenticate().Result;
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(strUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            ResponseRetrieveAgencyCredit result = JsonConvert.DeserializeObject<ResponseRetrieveAgencyCredit>(responseContent);
            return result;
        }
        //Lấy số dư VNA
        public async Task<ResponseRetrieveAgencyCreditVNA> RetrieveAgencyCreditVNA(string airlineSystem, int id)
        {
            try
            {
                RequestVNA request = new RequestVNA();
                string strUrl = string.Format("https://selfservice.vietnamairlines.com/api/getBalance");
                request.iata = airlineSystem;
                string jsonContent = JsonConvert.SerializeObject(request);
                string accessToken = GetTokenVNA(id);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                client.Timeout = TimeSpan.FromMinutes(10);
                var response = await client.PostAsync(strUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                ResponseRetrieveAgencyCreditVNA result = JsonConvert.DeserializeObject<ResponseRetrieveAgencyCreditVNA>(responseContent);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<bool> AutoCheckSoDuHang(string NguoiLap)
        {
            DBase db = new DBase();
            string sql = "select top 1 DATEDIFF(MINUTE, NgayLap, GetDate()) from SODUHANG order by ID DESC ";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server37", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    if (int.Parse(tb.Rows[0][0].ToString()) >= 5)
                    {
                        LuuSoDuHang(NguoiLap);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        //Other airline
        public async Task<bool> LuuSoDuHang(string NguoiLap)
        {
            bool result = true;
            string sql_soduhang = "";

            Task<ResponseRetrieveAgencyCredit> VietTravel = RetrieveAgencyCredit("VIETRAVEL", "VIETRAVEL", "");
            Task<ResponseRetrieveAgencyCredit> BamBoo = RetrieveAgencyCredit("BAMBOO", "BAMBOO", "");
            Task<ResponseRetrieveAgencyCredit> Vietjet = RetrieveAgencyCredit("VIETJET", "VIETJET", "");
            Task<ResponseRetrieveAgencyCredit> BamBoo_MT = RetrieveAgencyCredit("BAMBOO", "BAMBOO_MT", "CENTRAL");
            //VN
            Task<ResponseRetrieveAgencyCredit> FHQ = RetrieveAgencyCredit("VNA", "FHQ", "DEFAULT");
            Task<ResponseRetrieveAgencyCredit> UJU = RetrieveAgencyCredit("VNA", "UJU", "CENTRAL");
            Task<ResponseRetrieveAgencyCredit> JPQ = RetrieveAgencyCredit("VNA", "JPQ", "NORTHERN");
            Task<ResponseRetrieveAgencyCredit> LXQ = RetrieveAgencyCredit("VNA", "LXQ", "");

            double TR_TH = await GetBalance_TR();

            //Task<ResponseRetrieveAgencyCredit> VietTravel = RetrieveAgencyCredit("VIETRAVEL", "VIETRAVEL");
            //Task<ResponseRetrieveAgencyCredit> BamBoo = RetrieveAgencyCredit("BAMBOO", "BAMBOO");
            //Task<ResponseRetrieveAgencyCredit> Vietjet = RetrieveAgencyCredit("VIETJET", "VIETJET");
            //Task<ResponseRetrieveAgencyCredit> BamBoo_MT = RetrieveAgencyCredit("BAMBOO", "BAMBOO_MT");
            string FHQ_SoDu = "0";
            string JPQ_SoDu = "0";
            string LXQ_SoDu = "0";
            string UJU_SoDu = "0";
            if (FHQ.Result.Result == "true")
            {
                FHQ_SoDu = FHQ.Result.AgencyCreditAmount.ToString();
            }
            //if (UJU.Result.Result == "true")
            //{
            //    UJU_SoDu = UJU.Result.AgencyCreditAmount;
            //}
            if (JPQ.Result.Result == "true")
            {
                JPQ_SoDu = JPQ.Result.AgencyCreditAmount.ToString();
            }
            if (LXQ.Result.Result == "true")
            {
                LXQ_SoDu = LXQ.Result.AgencyCreditAmount.ToString();
            }

            string VietTravel_SoDu = "";
            if (VietTravel.Result.Result != "true")
            {
                VietTravel_SoDu = "0";
            }
            else
            {
                VietTravel_SoDu = VietTravel.Result.AgencyCreditAmount.ToString();
            }
            string BamBoo_SoDu = "";
            if (BamBoo.Result.Result != "true")
            {
                BamBoo_SoDu = "0";
            }
            else
            {
                BamBoo_SoDu = BamBoo.Result.AgencyCreditAmount.ToString();
            }
            string Vietjet_SoDu = "";
            if (Vietjet.Result.Result != "true")
            {
                Vietjet_SoDu = "0";
            }
            else
            {
                Vietjet_SoDu = Vietjet.Result.AgencyCreditAmount.ToString();
            }
            string result_BamBooMT = "";
            if (BamBoo_MT.Result.Result != "true")
            {
                result_BamBooMT = "0";
            }
            else
            {
                result_BamBooMT = BamBoo_MT.Result.AgencyCreditAmount.ToString();
            }

            sql_soduhang = "SP_INSERT_SODUHANG";
            List<DBase.AddParameters> Para = new List<DBase.AddParameters>();
            Para.Add(new DBase.AddParameters("@NguoiLap", NguoiLap));
            Para.Add(new DBase.AddParameters("@Status", 1));
            string ID = db.ExecuteDataSet(sql_soduhang, CommandType.StoredProcedure, "server37", Para).Tables[0].Rows[0][0].ToString();
            string sql_chitiet = "";
            string VIETTRAVEL = "VU";
            string BAMBOO_MN = "QH MN";
            string BAMBOO_MT = "QH MT";
            string VIETJET = "VJ";
            sql_chitiet = "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'FHQ'," + FHQ_SoDu + ")";
            await SendAirlineNotify("FHQ", decimal.Parse(FHQ_SoDu));
            //sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + BAMBOO_MT + "'," + BamBoo_MT.Result.AgencyCreditAmount + ")";
            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'UJU'," + UJU_SoDu + ")";
            await SendAirlineNotify("UJU", decimal.Parse(UJU_SoDu));

            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'JPQ'," + JPQ_SoDu + ")";
            await SendAirlineNotify("JPQ", decimal.Parse(JPQ_SoDu));

            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'LXQ'," + LXQ_SoDu + ")";
            await SendAirlineNotify("LXQ", decimal.Parse(LXQ_SoDu));

            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + BAMBOO_MN + "'," + BamBoo_SoDu + ")";
            await SendAirlineNotify("QH MN", decimal.Parse(BamBoo_SoDu));

            //sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + BAMBOO_MT + "'," + BamBoo_MT.Result.AgencyCreditAmount + ")";
            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + VIETJET + "'," + Vietjet_SoDu + ")";
            await SendAirlineNotify("VJ", decimal.Parse(Vietjet_SoDu));

            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + VIETTRAVEL + "'," + VietTravel_SoDu + ")";
            await SendAirlineNotify("VU", decimal.Parse(VietTravel_SoDu));

            sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",N'" + "TÂY HỒ" + "'," + TR_TH + ")";
            await SendAirlineNotify("TÂY HỒ", decimal.Parse(TR_TH.ToString()));

            db.ExecuteNoneQuery(sql_chitiet, CommandType.Text, "server37", null);

            return true;

        }
        //public static async Task<decimal> RetrieveAgencyCreditAirasia(string userName, string password)
        //{
        //    decimal result = 0;
        //    try
        //    {
        //        var pw = await Playwright.CreateAsync();
        //        var browser = await pw.Chromium.LaunchAsync();
        //        var page = await browser.NewPageAsync();
        //        await page.GotoAsync("https://www.airasia.com/agent/login/vi/vn");
        //        var title = await page.TitleAsync();
        //        if (title.Contains("Login"))
        //        {
        //            await page.Locator("input[name='username']").FillAsync(userName);
        //            await page.Locator("input[name='password']").FillAsync(password);
        //            await page.Locator("button.LoginLayout__LoginButton-kxowu0-3").ClickAsync();
        //            // Wait for navigation to the post-login page
        //            await page.WaitForNavigationAsync();
        //            await page.WaitForTimeoutAsync(10000);
        //            // Evaluate JavaScript code to retrieve data from sessionStorage
        //            var sessionValue = await page.EvaluateAsync<string>(@"() => {
        //                return sessionStorage.getItem('agentCompanyInfo');
        //            }");
        //            var jsonObject = System.Text.Json.JsonSerializer.Deserialize<AirlineAirasia>(sessionValue);
        //            result = jsonObject.AvailableCredits;
        //            title = await page.TitleAsync();
        //            await browser.CloseAsync();
        //        }
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return result;
        //    }
        //}



        //Lưu số dư hãng VNA
        public bool LuuSoDuHangVNA(string NguoiLap)
        {

            string UserName_Airasia = "EVAKVND0114";
            string Password_Airasia = "Enviet4444@4444";
            bool result = true;
            string sql_soduhang = "";
            Task<ResponseRetrieveAgencyCreditVNA> UJU = RetrieveAgencyCreditVNA("37971102", 1);
            Task<ResponseRetrieveAgencyCreditVNA> JPQ = RetrieveAgencyCreditVNA("37960705", 2);
            Task<ResponseRetrieveAgencyCreditVNA> FHQ = RetrieveAgencyCreditVNA("37981414", 3);
            Task<ResponseRetrieveAgencyCreditVNA> LXQ = RetrieveAgencyCreditVNA("37983094", 4);
            //Task<decimal> Airasia = RetrieveAgencyCreditAirasia(UserName_Airasia, Password_Airasia);
            if (UJU.Result.balance == 0)
            {
                result = false;
                return result;
            }
            if (JPQ.Result.balance == 0)
            {
                result = false;
                return result;
            }
            if (FHQ.Result.balance == 0)
            {
                result = false;
                return result;
            }
            if (LXQ.Result.balance == 0)
            {
                result = false;
                return result;
            }
            sql_soduhang = "SP_INSERT_SODUHANG";
            List<DBase.AddParameters> Para = new List<DBase.AddParameters>();
            Para.Add(new DBase.AddParameters("@NguoiLap", NguoiLap));
            Para.Add(new DBase.AddParameters("@Status", 2));
            string ID = db.ExecuteDataSet(sql_soduhang, CommandType.StoredProcedure, "server37", Para).Tables[0].Rows[0][0].ToString();
            string sql_chitiet = "";
            string FHQ_text = "FHQ (37981414)";
            string UJU_text = "UJU (37971102)";
            string JPQ_text = "JPQ  (37960705)";
            string LXQ_text = "LXQ  (37983094)";

            //sql_chitiet = "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + FHQ_text + "'," + FHQ.Result.balance + ")";
            sql_chitiet = "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + UJU_text + "'," + UJU.Result.balance + ")";
            //sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + JPQ_text + "'," + JPQ.Result.balance + ")";
            //sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'" + LXQ_text + "'," + LXQ.Result.balance + ")";
            //sql_chitiet += "Insert into SODUHANG_DETAIL(IDSODUHANG, HANG, SOTIEN) VALUES(" + ID + ",'AIRASIA'," + Airasia.Result + ")";
            db.ExecuteNoneQuery(sql_chitiet, CommandType.Text, "server37", null);

            return true;



        }


        #region api notify
        public async Task<bool> SendAirlineNotify(string Airline, decimal Amount)
        {
            bool result = false;
            List<Config_Balance> Configs = new List<Config_Balance>();
            string sql_config = "select * from CONFIG_BALANCE_AIRLINE";
            using (var conn = new SqlConnection(SERVER_EV_V2))
            {
                Configs = (List<Config_Balance>)await conn.QueryAsync<Config_Balance>(sql_config, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Close();
            }

            foreach (var item in Configs)
            {
                if (item.Airline == Airline)
                {
                    if (Amount > 0 && Amount < item.Amount)
                    {
                        //string[] UserNames = { "NV0001601" };
                        string[] UserNames = new string[] { "NV0001601", "NV00001", "NV00035" };
                        for (int i = 0; i < UserNames.Length; i++)
                        {
                            NotifyService _notifyService = new NotifyService(_configuration, _httpClientFactory, _authenticateService);
                            NotifyLisaUserNameRequest request = new NotifyLisaUserNameRequest("CREDIT LIMIT", Airline, Amount, "NOTIFICATION", UserNames[i], "");
                            result = await _notifyService.SendNotify(request);
                        }
                    }
                    break;
                }
            }
            return result;
        }
        #endregion


    }
}
