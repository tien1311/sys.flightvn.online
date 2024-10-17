using Dapper;
using EasyInvoice.Client;
using EasyInvoice.Json.Linq;
using Manager.DataAccess.Repository;
using Manager.Model.Models;
using Manager.Model.Models.CarBooking;
using Manager.Model.Models.CarBooking.Result;









//using Manager_EV.Helpers;

//using Manager_Manager.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TangDuLieu;
//using static Manager_EV.HoaDonHelper;

namespace Manager.DataAccess.Services.CarBooking
{
    public class TaxiServices
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly CarDbContext _context;
        private string token;
        public TaxiServices(IHttpClientFactory httpClientFactory, IConfiguration configuration, CarDbContext context)
        {
            _client = httpClientFactory.CreateClient("carbooking");
            _configuration = configuration;
            _context = context;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("SQL_POST");
        }

        public string CarConnectionString()
        {
            return _configuration.GetConnectionString("SQL_EV_CAR");
        }

        public PostsAdsModel PostsNote()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(GetConnectionString()))
                {
                    string query = @"SELECT top 1 * FROM Posts WHERE CategoryID = 7";
                    PostsAdsModel postsAdsModel = db.QueryFirstOrDefault<PostsAdsModel>(query);
                    return postsAdsModel;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi trong quá trình truy vấn dữ liệu", ex);
            }
        }

        public async Task<string> GetLocationValue(List<string> list)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetToken();
                }
                string apiUrl = $"api/v1/Taxi/GetAddressStart?keyword={list[1]}";
                // Thêm Bearer Token vào request header
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(apiUrl).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //Cách 1:
                    dynamic obj = JsonConvert.DeserializeObject(responseData);
                    string id = null;
                    foreach (var item in obj.data)
                    {
                        if (((string)item.street).Contains(list[0]))
                        {
                            id = item.id;
                            break;
                        }
                    }
                    return id;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }


        public async Task<string> GetDescription(Manager.Model.Models.CarBooking.Request okok)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetToken();
                }
                Dictionary<string, string> vehicleSeats = new Dictionary<string, string>
                {
                    { "Xe 4 chỗ", "1" },
                    { "Xe 7 chỗ", "4" },
                    { "Xe 16 chỗ", "5" },
                    { "Xe 29 chỗ", "6" }
                };

                string type_car = string.Empty;
                vehicleSeats.TryGetValue(okok.type_car, out type_car);

                List<string> resultList = new List<string>();
                string[] splitStrings = okok.location_from.Split('-');
                foreach (var str in splitStrings)
                {
                    resultList.Add(str.Trim());
                }
                string address_start = await GetLocationValue(resultList);
                string apiUrl = $"api/v1/Taxi/GetPrices?address_start={address_start}&address_end=10&schedule=[0]&type_of_car={type_car}&html=0";
                // Thêm Bearer Token vào request header
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _client.GetAsync(apiUrl).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //Cách 1:
                    dynamic obj = JsonConvert.DeserializeObject(responseData);
                    // Get giá trị đầu tiên từ obj.data
                    var data = obj.data[vehicleSeats[okok.type_car]].data[0];
                    // Chuyển đổi dữ liệu cụ thể thành chuỗi JSON
                    string json = JsonConvert.SerializeObject(data);
                    // Chuyển đổi dữ liệu cụ thể thành đối tượng CarData
                    CarData result = JsonConvert.DeserializeObject<CarData>(json);
                    var description = $"Đối với xe đi khứ hồi thì xe chỉ chờ trong 1 giờ sau thời gian này sẽ tính phí như sau:<br>- <strong style='color:red'>{result.description}</strong><br>";
                    return description;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }


        public string GetVatPercent()
        {
            DCHH record = null;
            string conn = _configuration.GetConnectionString("SQL_EV_TOUR");
            using (IDbConnection dbConnection = new SqlConnection(conn))
            {
                record = dbConnection.QuerySingleOrDefault<DCHH>(
                     "SELECT ID, TiGia FROM CommissionRates WHERE ID = @Id", new { Id = 1004 }
                );
            }
            return record.TiGia;
        }


        //public async Task<string> GetToken()
        //{
        //    // Địa chỉ API endpoint
        //    string apiUrl = "auth/login";

        //    // Dữ liệu cần gửi đi
        //    var data = new Dictionary<string, string>
        //    {
        //        { "username", "enviet" },
        //        { "password", "EnViet@156#@125" }
        //    };
        //    // Tạo nội dung x-www-form-urlencoded
        //    var content = new FormUrlEncodedContent(data);
        //    try
        //    {
        //        // Gửi request POST
        //        HttpResponseMessage response = await _client.PostAsync(apiUrl, content).ConfigureAwait(false);
        //        // Kiểm tra xem response có thành công không
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Đọc và hiển thị nội dung của response
        //            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //            dynamic obj = JsonConvert.DeserializeObject(responseContent);
        //            string token = obj.data["token"];
        //            return token;
        //        }
        //        else
        //        {
        //            return "Error: " + response.StatusCode;
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return "Exception: " + ex.Message;
        //    }
        //}

        public async Task<string> GetToken()
        {
            var data = new
            {
                userName = "enviet",
                passWord = "EnViet@456"
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_configuration["API_EV:EnVietAuth"]);
                request.Content = content;
                var response = await _client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(result);
                if (json != null)
                {
                    return json["result"]["token"];
                }
                else
                {
                    return "error token";
                }
            }
        }


        public async Task<List<Record>> GetDataRecordAPI(string jsonString)
        {
            if (string.IsNullOrEmpty(token))
            {
                token = await GetToken();
            }
            // Địa chỉ API endpoint
            string apiUrl = "booking/list";

            // Thêm Bearer Token vào request header
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var uri_builder = new UriBuilder(apiUrl);
            if (jsonString != null && jsonString != "null")
            {
                //dynamic obj = JsonConvert.DeserializeObject(jsonString);
                var search = JsonConvert.DeserializeObject<Search>(jsonString);

                // Tạo dữ liệu query parameters
                var data = new Dictionary<string, string>();

                // Kiểm tra và thêm tham số truy vấn nếu giá trị tồn tại
                if (!string.IsNullOrEmpty(search.time))
                    data.Add("time", Uri.EscapeDataString(search.time));

                if (!string.IsNullOrEmpty(search.keyword))
                    data.Add("keyword", Uri.EscapeDataString(search.keyword));

                if (!string.IsNullOrEmpty(search.vat))
                    data.Add("is_have_bill", Uri.EscapeDataString(search.vat));

                if (!string.IsNullOrEmpty(search.status))
                    data.Add("status", Uri.EscapeDataString(search.status));

                // Thêm query parameters vào URL
                uri_builder.Query = string.Join("&", data.Select(x => $"{x.Key}={x.Value}"));
            }
            try
            {
                // Send request GET
                HttpResponseMessage response = await _client.GetAsync(uri_builder.Uri).ConfigureAwait(false);
                List<string> idList = await GetListIdBooking().ConfigureAwait(false);
                // Kiểm tra xem response có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc và hiển thị nội dung của response
                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var root = JsonConvert.DeserializeObject<Manager.Model.Models.CarBooking.Result.Root>(responseContent);
                    var data = root.data.records
                        .Where(record => idList.Contains(record.id.ToString()))
                        .OrderByDescending(record => record.id)
                        .Take(200)
                        .ToList();
                    // Get qua từng record và lấy mã evcode set  
                    foreach (var record in data)
                    {
                        record.evcode = await Getevcode(record.id.ToString());
                    }
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public Task<Manager.Model.Models.CarBooking.Request> GetBookingByevcode(string evcode)
        {
            var item = new Manager.Model.Models.CarBooking.Request();
            string sqlQuery = "SELECT * FROM Requests WHERE evcode = @evcode";
            using (IDbConnection conn = new SqlConnection(CarConnectionString()))
            {
                item = conn.Query<Manager.Model.Models.CarBooking.Request>(sqlQuery, new { evcode }).FirstOrDefault();
            }
            return Task.FromResult(item);
        }

        public Task<bool> UpdateOtherFee(string evcode, double otherFee, string reason)
        {
            var CAR_ConnectionString = _configuration.GetConnectionString("SQL_EV_CAR");
            bool isSuccess = false;
            string sql = "UPDATE Requests " +
                        "SET other_fee = @otherFee, " +
                            "total = price - discount + commission + @otherFee + vat_price, " +
                            "other_fee_reason = @reason " +
                        "WHERE evcode = @evcode";
            using (var conn = new SqlConnection(CAR_ConnectionString))
            {
                conn.Open();
                var rowsAffected = conn.Execute(sql, new { evcode, otherFee, reason });
                isSuccess = rowsAffected > 0;
                conn.Close();
            }
            return Task.FromResult(isSuccess);
        }

        public async Task<bool> UpdateThanhToanChuyenKhoan(string evcode)
        {
            bool isSuccess = false;
            string sql = "UPDATE Requests " +
                        "SET payment_status = 'Success', " +
                            "status_enviet = 'CONFIRM' " +
                        "WHERE evcode = @evcode";
            using (var conn = new SqlConnection(CarConnectionString()))
            {
                await conn.OpenAsync();
                var rowsAffected = await conn.ExecuteAsync(sql, new { evcode });
                isSuccess = rowsAffected > 0;
                conn.Close();
            }
            return isSuccess;
        }

        public async Task<string> Getevcode(string idbooking)
        {
            var evcode = await _context.Requests
                .Where(record => record.id_booking == idbooking)
                .Select(record => record.evcode)
                .FirstOrDefaultAsync();

            return evcode;
        }

        public List<Coupon> GetCoupon()
        {
            var record = _context.Coupons.ToList();
            return record;
        }

        public async Task<List<Manager.Model.Models.CarBooking.Request>> GetDataRecordSQL(string jsonString)
        {
            IQueryable<Manager.Model.Models.CarBooking.Request> data = _context.Requests;

            if (!string.IsNullOrEmpty(jsonString) && jsonString != "null")
            {
                var search = JsonConvert.DeserializeObject<Search>(jsonString);

                if (!string.IsNullOrEmpty(search.keyword))
                {
                    data = data.Where(x => x.location_from == search.keyword);
                }
                if (!string.IsNullOrEmpty(search.time))
                {
                    int time = ExtensionHelper.get_month(search.time);
                    data = data.Where(x => x.departure.Month == time);
                }
                if (!string.IsNullOrEmpty(search.vat))
                {
                    data = data.Where(x => x.vat == search.vat);
                }
                if (!string.IsNullOrEmpty(search.status))
                {
                    data = data.Where(x => x.status_enviet == search.status);
                }
            }
            return await data.OrderByDescending(x => x.id).ToListAsync();
        }

        public async Task<string> BookingTypeReject(string type_reject)
        {
            string apiUrl = "search/type-reject";
            // Gọi API bằng phương thức GET
            HttpResponseMessage response = await _client.GetAsync(apiUrl).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                TypeReject apiData = JsonConvert.DeserializeObject<TypeReject>(responseContent);
                if (apiData != null && apiData.status == 200)
                {
                    return apiData.data[type_reject];
                }
            }
            return null;
        }


        public async Task<string> SyncStatusBooking(string id)
        {
            if (string.IsNullOrEmpty(token))
            {
                token = await GetToken();
            }
            // Địa chỉ API endpoint
            string apiUrl = "api/v1/Taxi/GetBookings";
            // Thêm Bearer Token vào request header
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Send request GET
            HttpResponseMessage response = await _client.GetAsync(apiUrl).ConfigureAwait(false);
            List<string> idList = await GetListIdBooking();
            // Kiểm tra xem response có thành công không
            if (response.IsSuccessStatusCode)
            {
                // Đọc và hiển thị nội dung của response
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var root = JsonConvert.DeserializeObject<Manager.Model.Models.CarBooking.Result.Root>(responseContent);
                var data = root.data.records;
                if (idList.Count > 0)
                {
                    if (id == "all")
                    {
                        data = data.Where(record => idList.Contains(record.id.ToString())).ToList();
                    }
                    else
                    {
                        data = data.Where(record => record.id == int.Parse(id.ToString())).ToList();
                    }
                    // Get qua từng record và set 
                    foreach (var record in data)
                    {
                        var booking = await GetBooking(record.id.ToString());
                        booking.status_booking = record.status;
                        // Chỉ set giá trị khi record.price_customer != null
                        if (!string.IsNullOrEmpty(record.price_customer))
                        {
                            booking.price_customer = double.Parse(record.price_customer);
                        }
                        if (record.status == "CANCEL")
                        {
                            booking.dt_cancellation_reason = await BookingTypeReject(record.type_reject);
                        }
                        else if (record.status == "CONFIRM")
                        {
                            if (record.trip_status == "CANCEL" || record.trip_status == "COMPLETE")
                            {
                                booking.status_booking = record.trip_status;
                                if (record.trip_status == "CANCEL")
                                {
                                    booking.dt_cancellation_reason = await BookingTypeReject(record.type_reject);
                                }
                            }
                        }
                        _context.Requests.Update(booking);
                        await _context.SaveChangesAsync();
                    }
                }
                return "Cập nhật trạng thái thành công!";
            }
            else
            {
                return "Cập nhật trạng thái thất bại!";
            }
        }


        //public async Task<string> SendBooking(Manager.Model.Models.CarBooking.Request obj)
        //{
        //    try
        //    {
        //        string apiUrl = "client/catch";

        //        var data = new Dictionary<string, string>
        //        {
        //            { "start-point", obj.location_from },
        //            { "end-point", obj.location_to },
        //            { "roundtrip", obj.type },
        //            { "is_have_bill", obj.vat },
        //            { "your-name", obj.fullname },
        //            { "your-phone", obj.phone },
        //            { "ngaydatxe", obj.departure.ToString("yyyy-MM-dd") },
        //            { "loai_xe", obj.type_car },
        //            { "note", obj.booking_notes },
        //            { "price", obj.price.ToString() },
        //            { "token", "-fXENCKulUwRCf2GDfswob2CA7ttdqYnnvknPme2XxI2lZdZqsNnv2H3wZg4CL7W" },
        //        };

        //        // Create form content
        //        var content = new FormUrlEncodedContent(data);
        //        // Send POST request
        //        HttpResponseMessage response = await _client.PostAsync(apiUrl, content).ConfigureAwait(false);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //            dynamic responseData = JsonConvert.DeserializeObject(responseContent);
        //            var request = await GetRequests(obj.id);
        //            if (request != null)
        //            {
        //                request.email_send = true;
        //                request.status_booking = responseData.data.status;
        //                request.id_booking = responseData.data.id;
        //                _context.Requests.Update(request);
        //                await _context.SaveChangesAsync();
        //            }
        //            return "Đặt xe thành công!";
        //        }
        //        else
        //        {
        //            return "Error: " + response.StatusCode;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Exception: " + ex.Message + "\n" + ex.InnerException?.Message;
        //    }
        //}

        public async Task<string> SendBooking(Manager.Model.Models.CarBooking.Request obj)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetToken();
                }
                return "Đặt xe thành công!";
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message + "\n" + ex.InnerException?.Message;
            }
        }


        public async Task<Manager.Model.Models.CarBooking.Request> GetRequests(int id)
        {
            IQueryable<Manager.Model.Models.CarBooking.Request> data = _context.Requests;
            var request = await data.FirstOrDefaultAsync(x => x.id == id);
            return request;
        }

        public async Task<Manager.Model.Models.CarBooking.Request> GetRequests(string evcode)
        {
            IQueryable<Manager.Model.Models.CarBooking.Request> data = _context.Requests;
            var request = await data.FirstOrDefaultAsync(x => x.evcode == evcode);
            return request;
        }

        public async Task<Manager.Model.Models.CarBooking.Request> GetBooking(string idbooking)
        {
            IQueryable<Manager.Model.Models.CarBooking.Request> data = _context.Requests;
            var request = await data.FirstOrDefaultAsync(x => x.id_booking == idbooking);
            return request;
        }

        public async Task<Manager.Model.Models.CarBooking.Request> GetBookingFromevcode(string evcode)
        {
            IQueryable<Manager.Model.Models.CarBooking.Request> data = _context.Requests;
            var request = await data.FirstOrDefaultAsync(x => x.evcode == evcode);
            return request;
        }

        public async Task<bool> UpdatePayment([FromBody] JObject data)
        {
            int idbooking = int.Parse(data["idbooking"].ToString());
            string payment_type = data["payment_type"].ToString();
            var booking = await GetRequests(idbooking);
            if (booking != null)
            {
                booking.payment_type = payment_type;
                _context.Requests.Update(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> GetLinkPayment([FromBody] JObject data)
        {
            int id = int.Parse(data["id"].ToString());
            var request = await GetRequests(id);
            GatewayRepository gateway = new GatewayRepository(_configuration);
            var productInfo = new[]
            {
                new { ProductName = "XE ĐƯA ĐÓN", Quantity = 1, UnitPrice = request.price }
            };
            //string gateway_url = await gateway.GetLinkToGateway(request.evcode, request.agent_code, request.fullname, request.phone, request.email, request.vat_address, productInfo, request.price, request.discount, request.vat_price, request.other_fee, request.total, request.booking_notes);
            string gateway_url = await gateway.GetLinkToGateway_V2(request.evcode, request.agent_code, request.fullname, request.phone, request.email, request.vat_address, productInfo, request.price, request.discount, request.vat_price, request.other_fee, request.total, request.booking_notes, "https://gateway.envietgroup.com/Home/ChiTietDonHang?orderId=" + request.evcode);

            return gateway_url;
        }

        public async Task<string> GetLinkPayment_Ver2([FromBody] JObject data)
        {
            int id = int.Parse(data["id"].ToString());
            var request = await GetRequests(id);
            string gateway_url = "https://www.google.com/";
            return gateway_url;
        }
        public async Task UpdateStatusBooking(string idbooking, string status)
        {
            var booking = await GetBooking(idbooking);
            if (booking != null)
            {
                booking.status_booking = status;
                _context.Requests.Update(booking);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateStatusEnviet(string evcode, string status, string user)
        {
            var booking = await GetBookingFromevcode(evcode);
            if (booking != null)
            {
                if (booking.user_booking == null)
                {
                    booking.user_booking = user;
                }
                booking.status_enviet = status;
                _context.Requests.Update(booking);
                //if (booking.status_enviet == "WAITING")
                //{
                //    Task.Run(() => SendEmail(booking));
                //}
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<int> CancelBooking(dynamic data)
        {
            string evcode = data["evcode"].ToString();
            var booking = await GetBookingFromevcode(evcode);
            if (booking != null)
            {
                booking.ev_cancellation_reason = data["reason"].ToString();
                booking.status_enviet = "CANCEL";
                booking.email_cancel = true;
                _context.Requests.Update(booking);
                await _context.SaveChangesAsync();
                //Task.Run(() => SendEmail(booking));
            }
            return 1;
        }

        public async Task<string> AddCoupon([FromBody] JObject data)
        {
            var coupon = new Coupon();
            coupon.name = data["code"].ToString();
            coupon.discount = int.Parse(data["price"].ToString());
            coupon.type = data["type"].ToString();
            await _context.Coupons.AddAsync(coupon);
            int result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Thêm mã giảm giá thành công!";
            }
            else
            {
                return "Thêm mã giảm giá thất bại!";
            }
        }

        public async Task<string> Get_Status_Payment(Manager.Model.Models.CarBooking.Request request)
        {
            var record = await _context.Requests.FirstOrDefaultAsync(x => x.evcode == request.evcode);
            return record.payment_status;
        }

        public async Task<string> ChangeStatus([FromBody] JObject data)
        {
            int id = int.Parse(data["id"].ToString());
            string status = data["status"].ToString();
            var request = await _context.Requests.FindAsync(id);
            if (request.payment_type == "Debt" && status == "CONFIRM")
            {
                request.payment_status = "Success";
            }
            request.status_enviet = status;
            //if (status != "ACCEPT")
            //{
            //    if (!(request.payment_type == "Online" && request.payment_status == "Success"))
            //    {
            //        Task.Run(() => SendEmail(request));
            //    }
            //}
            if (status == "WAITING")
            {
                request.date_xacnhan = DateTime.Now;
            }
            _context.Requests.Update(request);
            int result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Đổi trạng thái thành công!";
            }
            else
            {
                return "Đổi trạng thái thất bại!";
            }
        }

        public async Task<string> EditCoupon([FromBody] JObject data)
        {
            int id = int.Parse(data["id"].ToString());
            var coupon = await _context.Coupons.FindAsync(id);
            coupon.name = data["code"].ToString(); ;
            coupon.discount = int.Parse(data["price"].ToString());
            coupon.type = data["type"].ToString();
            _context.Coupons.Update(coupon);
            int result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Cập nhật mã giảm giá thành công!";
            }
            else
            {
                return "Cập nhật mã giảm giá thất bại!";
            }
        }

        public async Task<string> ApplyCoupon([FromBody] JObject data)
        {
            try
            {
                int id = int.Parse(data["id"].ToString());
                var deactive = await _context.Coupons.FirstOrDefaultAsync(x => x.active == true);
                if (deactive != null)
                {
                    deactive.active = false;
                    _context.Coupons.Update(deactive);
                }
                var active = await _context.Coupons.FindAsync(id);
                if (active != null)
                {
                    active.active = true;
                    _context.Coupons.Update(active);
                }
                int result = await _context.SaveChangesAsync();
                if (result == 2)
                {
                    return "Áp dụng mã giảm giá thành công!";
                }
                else
                {
                    return "Áp dụng mã giảm giá thất bại!";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi khi áp dụng mã giảm giá: " + ex.Message;
            }
        }

        public async Task<string> DeleteCoupon([FromBody] JObject data)
        {
            int id = int.Parse(data["id"].ToString());
            var coupon = await _context.Coupons.FindAsync(id);
            _context.Coupons.Remove(coupon);
            int result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Xóa mã giảm giá thành công!";
            }
            else
            {
                return "Xóa mã giảm giá thất bại!";
            }
        }

        public async Task<List<Address>> GetLocation(string keyword)
        {
            string apiUrl = "api/v1/Taxi/GetAddressStart?keyword=" + keyword;

            // Gọi API bằng phương thức GET
            HttpResponseMessage response = await _client.GetAsync(apiUrl).ConfigureAwait(false);
            // Kiểm tra xem request có thành công hay không
            if (response.IsSuccessStatusCode)
            {
                // Đọc dữ liệu từ response
                string response_data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var root = JsonConvert.DeserializeObject<Manager.Model.Models.CarBooking.Location>(response_data);
                return root.data;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<string>> GetLocationData()
        {
            var locations = await _context.Requests
                .Select(record => record.location_from)
                .Distinct()
                .ToListAsync();

            return locations;
        }


        public async Task<List<string>> GetListIdBooking()
        {
            var list_id_booking = await _context.Requests
                .Where(record => record.id_booking != null)
                .Select(record => record.id_booking)
                .ToListAsync();
            return list_id_booking;
        }

        //public async Task<string> SendEmail(Manager.Model.Models.CarBooking.Request result)
        //{
        //    Dictionary<string, List<string>> arr = new Dictionary<string, List<string>>();
        //    arr["WAITING"] = new List<string>() { "Đã xác nhận", "[XE ĐƯA ĐÓN] XÁC NHẬN ĐƠN HÀNG", "EVM_XACNHANDONHANG_CAR" };
        //    arr["CONFIRM"] = new List<string>() { "Đã thanh toán", "[XE ĐƯA ĐÓN] XÁC NHẬN THANH TOÁN", "EVM_CHANGESTATUSCAR" };
        //    arr["COMPLETE"] = new List<string>() { "Đã hoàn thành", "[XE ĐƯA ĐÓN] CẢM ƠN QUÝ KHÁCH HÀNG", "EVM_SUCCESSBOOKING" };
        //    arr["CANCEL"] = new List<string>() { "Đã hủy", "[XE ĐƯA ĐÓN] XÁC NHẬN ĐÃ HỦY BOOKING", "EVM_CANCELCAR" };
        //    var note = GetDescription(result).Result + PostsNote().Description;

        //    Mail mailDb = new Mail(arr[result.status_enviet][2]);
        //    MailMessage message = new MailMessage(mailDb.username, result.email);
        //    message.Subject = arr[result.status_enviet][1];
        //    SmtpClient smtpclient = new SmtpClient(mailDb.host, mailDb.port);
        //    string linkGateway = "";

        //    if (result.other_fee == null)
        //    {
        //        result.other_fee = 0;
        //    }
        //    if (result.status_enviet == "WAITING")
        //    {
        //        GatewayRepository gateway = new GatewayRepository(_configuration);
        //        var productInfo = new[]
        //        {
        //        new { ProductName = "XE ĐƯA ĐÓN", Quantity = 1, UnitPrice = result.price }
        //        };
        //        linkGateway = await gateway.GetLinkToGateway(result.evcode, result.agent_code, result.fullname, result.phone, result.email, result.vat_address, productInfo, result.price, result.discount, result.vat_price, result.other_fee, result.total, result.booking_notes);
        //        linkGateway = await gateway.GetLinkToGateway_V2(result.evcode, result.agent_code, result.fullname, result.phone, result.email, result.vat_address, productInfo, result.price, result.discount, result.vat_price, result.other_fee, result.total, result.booking_notes, "https://gateway.envietgroup.com/Home/ChiTietDonHang?orderId=" + result.evcode);
        //    }

        //    var strSanPham = "";

        //    strSanPham += "<tr>";
        //    strSanPham += "<td style='text-align: center;'>" + result.type_car + "</td>";
        //    strSanPham += "<td style='text-align: center;'>" + result.location_from + "</td>";
        //    strSanPham += "<td style='text-align: center;'>" + result.location_to + "</td>";
        //    strSanPham += "<td style='text-align: center;'>" + result.departure + "</td>";
        //    string hanhTrinh_Car = "";
        //    if (result.type == "0")
        //    {
        //        hanhTrinh_Car = "Một chiều";
        //    }
        //    else
        //    {
        //        hanhTrinh_Car = "Hai chiều";
        //    }
        //    strSanPham += "<td style='text-align: center;'>" + hanhTrinh_Car + "</td>";
        //    strSanPham += "<td style='text-align: center;'>" + result.booking_notes + "</td>";
        //    strSanPham += "</tr>";



        //    string discountRow = result.discount > 0
        //   ? $"<tr id='discount-row'><td style='width: 110px;'> <span class='payment-status_title'>Giảm giá: </span> </td><td class='table-price-value text-success' style='color: #14A44D;'><span id='discount-value'>- {Manager.Common.Helpers.Common.FormatNumber(result.discount, 0)} VNĐ</span></td></tr>"
        //        : string.Empty;

        //    string otherFeeRow = result.other_fee > 0
        //    ? $"<tr id='other-fee-row'><td style='width: 110px;'> <span class='payment-status_title'>Phí khác: </span> </td><td class='table-price-value text-dark' style='color:#332D2D;'> <span id='other-fee-value'>+ {Manager.Common.Helpers.Common.FormatNumber(result.other_fee, 0)} VNĐ ({result.other_fee_reason})</span></td></tr>"
        //    : string.Empty;
        //    string vatRow = result.vat_price > 0
        //    ? $"<tr id='vat-price-row'><td style='width: 110px;'> <span class='payment-status_title'>VAT ({result.vat_percent}%): </span> </td><td class='table-price-value text-dark' style='color:#332D2D;'><span id='vat-price-value'>+ {Manager.Common.Helpers.Common.FormatNumber(result.vat_price, 0)} VNĐ </span></td></tr>"
        //    : string.Empty;

        //    try
        //    {
        //        MailAddress fromAddress = new MailAddress(mailDb.username, "ENVIET GROUP");
        //        message.From = fromAddress;
        //        if (result.booking_notes.Trim() != "ENVIETTESTING")
        //        {
        //            message.CC.Add("services@enviet-group.com");
        //        }
        //        StringBuilder mailBody = new StringBuilder();
        //        using (var webResponse = System.Net.WebRequest.Create(mailDb.templateUrl).GetResponse())
        //        using (var content = webResponse.GetResponseStream())
        //        using (var reader = new System.IO.StreamReader(content))
        //        {
        //            mailBody.Append(reader.ReadToEnd());
        //        }
        //        mailBody = mailBody.Replace(result.status_enviet == "COMPLETE" ? "$_Code" : "$_evcode", result.evcode)
        //        .Replace("$_evcode", result.evcode)
        //        .Replace("$_LocationFrom", result.location_from)
        //        .Replace("$_LocationTo", result.location_to)
        //        .Replace("$_Type", result.type == "0" ? "Một chiều" : "Hai chiều")
        //        .Replace("$_Car", result.type_car)
        //        .Replace("$_Departure", result.departure.ToString("dd/MM/yyyy HH:mm tt"))
        //        .Replace("$_Price", result.price.ToString("#,0") + " VNĐ")
        //        .Replace("$_Discount", result.discount.ToString("#,0") + " VNĐ")
        //        .Replace("$_OtherFee", result.other_fee.Value.ToString("#,0") + " VNĐ")
        //        .Replace("$_VatPrice", result.vat_price.ToString("#,0") + " VNĐ")
        //        .Replace("$_Total", result.total.ToString("#,0") + " VNĐ")
        //        .Replace("$_AgentCode", result.agent_code)
        //        .Replace("$_Fullname", result.fullname)
        //        .Replace("$_Email", result.email)
        //        .Replace("$_Phone", result.phone)
        //        .Replace("$_StatusEnviet", arr[result.status_enviet][0])
        //        .Replace("$_CancellationReason", result.ev_cancellation_reason)
        //        .Replace("$_BookingNotes", result.booking_notes)
        //        .Replace("$_VatNotes", result.vat_notes)
        //        .Replace("$_VatMst", result.vat_mst)
        //        .Replace("$_VatAddress", result.vat_address)
        //        .Replace("$_VATPercent", result.vat_percent.ToString())
        //        .Replace("$_Condition", note)
        //        .Replace("$_LinkGateway", linkGateway)
        //        .Replace("{{DISCOUNT_ROW}}", discountRow)
        //        .Replace("{{OTHER_FEE_ROW}}", otherFeeRow)
        //        .Replace("{{VAT_ROW}}", vatRow)
        //        .Replace("$_SanPham", strSanPham)
        //        .Replace(result.status_enviet == "COMPLETE" ? "$_Ngaygui" : "$_Date", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

        //        if (string.IsNullOrEmpty(result.other_fee_reason))
        //        {
        //            mailBody = mailBody.Replace("$_Reason_OtherFee", "Không có");
        //        }
        //        else
        //        {
        //            mailBody = mailBody.Replace("$_Reason_OtherFee", result.other_fee_reason);
        //        }

        //        message.Body = mailBody.ToString();
        //        message.IsBodyHtml = true;
        //        smtpclient.Host = mailDb.host;  // We use gmail as our smtp client
        //        smtpclient.Port = mailDb.port;
        //        smtpclient.EnableSsl = Convert.ToBoolean(mailDb.useSSL);
        //        smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpclient.UseDefaultCredentials = false;
        //        smtpclient.Credentials = new System.Net.NetworkCredential(mailDb.username, new DBase().Decrypt(mailDb.password, "vodacthe", true));
        //        smtpclient.Send(message);
        //        return "Successful";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    finally
        //    {
        //        message.Dispose();
        //        smtpclient.Dispose();
        //    }
        //}

    }
}
