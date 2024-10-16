using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System;
using Dapper;
using System.Linq;
using System.Data;
using System.Data.Common;
using Manager.Model.Models.PaymentGateway;

namespace Manager.DataAccess.Repository
{
    public class GatewayRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public GatewayRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("SQL_PAYMENT_GATEWAY");
        }

        public GatewayRepository()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
            connectionString = _configuration.GetConnectionString("SQL_PAYMENT_GATEWAY");
        }

        public async Task<string> GetLinkToPayment(string orderId)
        {
            string link = string.Empty;
            link = "https://gateway.envietgroup.com/Home/ChiTietDonHang?orderId=" + orderId;
            return link;

        }

        public List<Payment> GetListCongThanhToanDienTu()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = @"
                            SELECT p.*, pf.* 
                            FROM Payments p 
                            LEFT JOIN PaymentsFee pf ON p.Id = pf.PaymentId 
                            ORDER BY p.Id DESC ";
                var paymentDictionary = new Dictionary<int, Payment>();
                var payments = db.Query<Payment>(sql, null, null, true, null, CommandType.Text).ToList();
                return payments;
            }
        }

        public IEnumerable<Payment> GetPaymentsWithFees()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                SELECT 
                    p.*,
                    pf.* 
                FROM 
                    Payments p
                LEFT JOIN 
                    PaymentsFee pf ON p.Id = pf.PaymentId
                ORDER BY 
                    p.Id DESC";

                var paymentDictionary = new Dictionary<int, Payment>();

                var payments = db.Query<Payment, PaymentFee, Payment>(
                    sql,
                    (payment, paymentFee) =>
                    {
                        if (!paymentDictionary.TryGetValue(payment.Id, out var currentPayment))
                        {
                            currentPayment = payment;
                            currentPayment.PaymentFees = new List<PaymentFee>();
                            paymentDictionary.Add(currentPayment.Id, currentPayment);
                        }

                        if (paymentFee != null)
                        {
                            currentPayment.PaymentFees.Add(paymentFee);
                        }

                        return currentPayment;
                    },
                    splitOn: "Id"
                ).Distinct().ToList();
                db.Close();
                return payments;
            }
        }

        public List<PaymentMethod> GetListPaymentMethod()
        {
            var listPaymentMethod = new List<PaymentMethod>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                            SELECT * 
                            FROM PaymentMethods 
                            ";

                listPaymentMethod = db.Query<PaymentMethod>(sql, null).ToList();
                db.Close();
            }
            return listPaymentMethod;
        }

        public void AddPaymentMethod(PaymentMethod model)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = @"
                INSERT INTO PaymentMethods (Name, Alias, Image, [Percent], FixedCosts, Source, IsActived)
                VALUES (@Name, @Alias, @Image, @Percent, @FixedCosts, @Source, @IsActived);
                SELECT CAST(SCOPE_IDENTITY() as int);";

                // Execute the query and get the new Id of the inserted PaymentMethod
                var id = db.QuerySingle<int>(sql, new
                {
                    model.Name,
                    model.Alias,
                    model.Image,
                    model.Percent,
                    model.FixedCosts,
                    model.Source,
                    model.IsActived
                });

                model.Id = id;  // Set the Id of the model to the newly generated Id
            }
        }

        public int AddPayment(Payment model)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                        INSERT INTO Payments (Name, IsActived, Image)
                        VALUES (@Name, @IsActived, @Image);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

                return db.QuerySingle<int>(sql, new
                {
                    model.Name,
                    model.IsActived,
                    model.Image
                });
                db.Close();
            }
        }

        public void AddPaymentFee(PaymentFee fee)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                    INSERT INTO PaymentsFee (PaymentId, RequestType, Name, [Percent], IsActived, FixedCosts, PaymentName, Image)
                    VALUES (@PaymentId, @RequestType, @Name, @Percent, @IsActived, @FixedCosts, @PaymentName, @Image);";

                db.Execute(sql, fee);
                db.Close();
            }
        }

        public Payment GetPayment(int id)
        {
            var payment = new Payment();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                            SELECT * 
                            FROM Payments 
                            WHERE Id = @ID
                            ";

                payment = db.Query<Payment>(sql, new { ID = id }).FirstOrDefault();
                db.Close();
            }
            return payment;
        }

        public void SavePaymentImage(int id, string imageLink)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                var sql = @"
                            UPDATE Payments 
                            SET Image = @imageLink 
                            WHERE Id = @ID
                            ";
                db.Execute(sql, new { imageLink, ID = id });
                db.Close();
            }
        }


        public Payment GetPaymentByID(int Id)
        {
            var payment = new Payment();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                payment = dbConnection.QueryFirstOrDefault<Payment>("SELECT * FROM Payments WHERE Id = @Id", new { Id });
                dbConnection.Close();
            }
            return payment;
        }

        public void ChangeActivePayment(bool IsActive, int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE Payments SET IsActived = @IsActived WHERE Id = @Id", new { IsActived = IsActive, Id });
                dbConnection.Close();
            }
        }

        public PaymentMethod GetPaymentMethodByID(int Id)
        {
            var payment = new PaymentMethod();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                payment = dbConnection.QueryFirstOrDefault<PaymentMethod>("SELECT * FROM PaymentMethods WHERE Id = @Id", new { Id });
                dbConnection.Close();
            }
            return payment;
        }

        public void ChangeActivePaymentMethod(bool IsActive, int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE PaymentMethods SET IsActived = @IsActived WHERE Id = @Id", new { IsActived = IsActive, Id });
                dbConnection.Close();
            }
        }

        public void UpdatePaymentMethodImage(string linkImage, int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE PaymentMethods SET Image = @Image WHERE Id = @Id", new { Image = linkImage, Id });
                dbConnection.Close();
            }
        }

        public void UpdatePaymentFeeImage(string linkImage, int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE PaymentsFee SET Image = @Image WHERE Id = @Id", new { Image = linkImage, Id });
                dbConnection.Close();
            }
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE PaymentMethods SET FixedCosts = @FixedCosts, [Percent] = @Percent, Name = @Name WHERE Id = @Id",
                    new { paymentMethod.FixedCosts, paymentMethod.Percent, paymentMethod.Name, paymentMethod.Id });
                dbConnection.Close();
            }
        }

        public PaymentFee GetPaymentFeeByID(int Id)
        {
            var payment = new PaymentFee();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                payment = dbConnection.QueryFirstOrDefault<PaymentFee>("SELECT * FROM PaymentsFee WHERE Id = @Id", new { Id });
                dbConnection.Close();
            }
            return payment;
        }

        public void ChangeActivePaymentFee(bool IsActive, int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE PaymentsFee SET IsActived = @IsActived WHERE Id = @Id", new { IsActived = IsActive, Id });
                dbConnection.Close();
            }
        }

        public void DeletePaymentFee(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM PaymentsFee WHERE Id = @Id", new { Id = id });
                dbConnection.Close();
            }
        }

        public string GetRequestType(string paymentType, string requestType)
        {
            string requestTypeName = "";
            if (paymentType != null && requestType != null)
            {
                string paramsQuery = paymentType + "-" + requestType;
                string sql = $"SELECT NAME " +
                                $"FROM PaymentsFee " +
                                $"WHERE RequestType = @paramsQuery";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    requestTypeName = connection.Query<string>(sql.ToString(), new { paramsQuery }).FirstOrDefault();
                    connection.Close();
                }
            }
            return requestTypeName;

        }

        public string GetImagePaymentTypeUrl(string paymentType)
        {
            string imgUrl = "";
            if (paymentType != null)
            {
                paymentType = paymentType.ToLower();
                imgUrl = $"/images/PaymentGateway/PaymentType/{paymentType}-logo.png";
            }
            else
            {
                imgUrl = "/images/PaymentGateway/PaymentType/payment_processing-logo.png";
            }
            return imgUrl;
        }

        public List<UserPay> GetUsersPays(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType, int? resultCode, int pageNumber, int pageSize)
        {
            List<UserPay> usersPay = new List<UserPay>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) AS RowNum, * FROM UsersPays WHERE 1=1");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                if (resultCode != 999)
                {
                    query.Append(" AND ResultCode = @ResultCode");
                }

                query.Append(") AS UserPay WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");

                usersPay = connection.Query<UserPay>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                    ResultCode = resultCode,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }).ToList();
                connection.Close();
            }
            return usersPay;
        }

        public int GetTotalUsersPays(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType, int? resultCode)
        {
            int totalRecords = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT COUNT(*) FROM UsersPays WHERE 1=1");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                if (resultCode != 999)
                {
                    query.Append(" AND ResultCode = @ResultCode");
                }

                totalRecords = connection.QuerySingle<int>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                    ResultCode = resultCode
                });
                connection.Close();
            }
            return totalRecords;
        }

        public double GetTotalAmount(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType, int? resultCode)
        {
            double totalAmount = 0;
            string totalAmountString = "";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT SUM(Amount) FROM UsersPays WHERE 1=1");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                if (resultCode != 999)
                {
                    query.Append(" AND ResultCode = @ResultCode");
                }

                totalAmountString = connection.QuerySingle<string>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                    ResultCode = resultCode
                });

                if (string.IsNullOrEmpty(totalAmountString))
                {
                    totalAmount = 0;
                }
                else
                {
                    totalAmount = double.Parse(totalAmountString);
                }
                connection.Close();
            }
            return totalAmount;
        }

        public List<UserPay> GetUsersPays_KeToan(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType, int pageNumber, int pageSize)
        {
            List<UserPay> usersPay = new List<UserPay>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY CreatedDate DESC) AS RowNum, * FROM UsersPays WHERE 1=1 AND ResultCode = 0 ");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                query.Append(") AS UserPay WHERE RowNum BETWEEN ((@PageNumber-1)*@PageSize+1) AND (@PageNumber*@PageSize) ORDER BY CreatedDate DESC");

                usersPay = connection.Query<UserPay>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }).ToList();
                connection.Close();
            }
            return usersPay;
        }

        public int GetTotalUsersPays_KeToan(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType)
        {
            int totalRecords = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT COUNT(*) FROM UsersPays WHERE 1=1 AND ResultCode = 0 ");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                totalRecords = connection.QuerySingle<int>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                });
                connection.Close();
            }
            return totalRecords;
        }

        public double GetTotalAmount_KeToan(List<string> orderIds, DateTime? startDate, DateTime? endDate, string paymentType)
        {
            double totalAmount = 0;
            string totalAmountString = "";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("SELECT SUM(Amount) FROM UsersPays WHERE 1=1 AND ResultCode = 0 ");

                if (orderIds.Any())
                {
                    query.Append(" AND OrderId IN @OrderIds");
                }

                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate BETWEEN @StartDate AND @EndDate");
                }
                else if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate >= @StartDate");
                }
                else if (startDate == DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    query.Append(" AND CreatedDate <= @EndDate");
                }

                if (paymentType != "All")
                {
                    query.Append(" AND PaymentType LIKE @PaymentType");
                }

                totalAmountString = connection.QuerySingle<string>(query.ToString(), new
                {
                    OrderIds = orderIds,
                    StartDate = startDate,
                    EndDate = endDate,
                    PaymentType = $"%{paymentType}%",
                });

                if (string.IsNullOrEmpty(totalAmountString))
                {
                    totalAmount = 0;
                }
                else
                {
                    totalAmount = double.Parse(totalAmountString);
                }
                connection.Close();
            }
            return totalAmount;
        }


        public List<UserPay> GetUsersPays_KeToan_ChuyenKhoan()
        {
            List<UserPay> usersPay = new List<UserPay>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("select * from UsersPays WHERE PaymentType = 'chuyen-khoan' AND ResultCode = 2");
                usersPay = connection.Query<UserPay>(query.ToString()).ToList();
                connection.Close();
            }
            return usersPay;
        }

        public int GetTotalUsersPays_KeToan_ChuyenKhoan()
        {
            int totalRecords = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = new StringBuilder("select COUNT(*) from UsersPays WHERE PaymentType = 'chuyen-khoan' AND ResultCode = 2");
                totalRecords = connection.QuerySingle<int>(query.ToString());
                connection.Close();
            }
            return totalRecords;
        }

        public UserPay GetUsersPaysById(int Id)
        {
            var item = new UserPay();
            string sql = "SELECT * FROM UsersPays WHERE Id = @Id";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                item = connection.Query<UserPay>(sql, new { Id }).FirstOrDefault();
                connection.Close();
            }
            return item;
        }

        public bool UpdateUsersPaysChuyenKhoan(int Id, int resultCode, string paymentStatus)
        {
            bool isSuccess = false;
            bool isSuccess_SaleChannel = false;

            string sqlUpdateUsersPays = "UPDATE UsersPays " +
                                        "SET ResultCode = @resultCode, " +
                                        "    PaymentStatus = @paymentStatus " +
                                        "WHERE Id = @Id";

            string sqlSelectOrderId = "SELECT OrderId " +
                                      "FROM UsersPays " + // Corrected table name to UsersPays
                                      "WHERE Id = @Id";

            string orderID = "";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int rowsAffected = connection.Execute(sqlUpdateUsersPays, new { Id, resultCode, paymentStatus }, transaction);
                        orderID = connection.Query<string>(sqlSelectOrderId, new { Id }, transaction).FirstOrDefault();

                        int rowsAffected_CarBooking = 0;

                        if (orderID.StartsWith("EVSB"))
                        {
                            string paymentStatus_CarBooking = resultCode == 0 ? "Success" : "Processing";
                            string statusEnviet_CarBooking = resultCode == 0 ? "CONFIRM" : "PENDING";

                            string sqlUpdateCarBooking = "UPDATE Requests " +
                                                         "SET payment_status = @paymentStatus," +
                                                         "    status_enviet =  @statusEnviet " +
                                                         "WHERE evcode = @orderID";

                            string car_ConnectionString = _configuration.GetConnectionString("SQL_EV_CAR");

                            using (var carConnection = new SqlConnection(car_ConnectionString))
                            {
                                carConnection.Open();
                                using (var carTransaction = carConnection.BeginTransaction())
                                {
                                    try
                                    {
                                        rowsAffected_CarBooking = carConnection.Execute(sqlUpdateCarBooking,
                                            new { orderID, paymentStatus = paymentStatus_CarBooking, statusEnviet = statusEnviet_CarBooking }, carTransaction);
                                        if (rowsAffected_CarBooking > 0)
                                        {
                                            isSuccess_SaleChannel = true;
                                        }
                                        carTransaction.Commit();
                                    }
                                    catch (Exception)
                                    {
                                        carTransaction.Rollback();
                                        throw;
                                    }
                                }
                            }
                        }

                        if (rowsAffected > 0 && isSuccess_SaleChannel)
                        {
                            transaction.Commit();
                            isSuccess = true;
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return isSuccess;
        }


        public string GetOrderId(string orderId)
        {
            return orderId.Substring(0, 13);
        }
        public async Task<string> GetEnVietToken()
        {
            string apiUrl = _configuration["API_EV:EnVietAuth"];
            string userName = "enviet";
            string passWord = "EnViet@456";
            var token = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-master-key", "1370542bcb7c6b71e34e975d4697f89bab164a520934ff47a5aceb780fdc92e6");
                var content = new StringContent($"{{\"userName\":\"{userName}\",\"passWord\":\"{passWord}\"}}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject jmessage = JObject.Parse(responseContent);
                    token = jmessage.GetValue("result")["token"].ToString();
                }
            }
            return token;
        }

        public async Task<bool> UpdateUsersPaysChuyenKhoan(string orderId)
        {
            bool isSuccess = false;
            int rowUpdate = 0;
            string sql = "UPDATE UsersPays " +
                        "SET PaymentStatus = N'Thanh toán thành công'," +
                        "    ResultCode = 0 " +
                        "WHERE OrderId = @orderId AND PaymentType = 'chuyen-khoan' AND ResultCode = 2 ";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                rowUpdate = connection.Execute(sql, new { orderId });
                connection.Close();
            }

            if (rowUpdate > 0)
            {
                isSuccess = true;
            }

            return isSuccess;
        }

        /// <summary>
        /// <para> Anh em lưu ý tránh nhầm lẫn giữa discountValue và discountPrice. </para>
        /// <para> Khi truyền tham số, anh em truyền discountValue = giá bán - giá giảm giá (tuỳ kênh bán của anh em) </para>
        /// <para> Nếu sử dụng discountValue như CarBooking thì truyền thẳng tham số vào hàm </para>
        /// <para> Nếu sử dụng DiscountPrice như VISA thì phải tính discountValue nữa nha </para>
        /// </summary>
        public async Task<string> GetLinkToGateway(string orderId, string maKH, string customerName, string phoneNumber, string email, string address, object[] productInfo, double price, double? discountValue, double? vat, double? otherFee, double totalPrice, string note)
        {
            var bearToken = await GetEnVietToken();
            string apiUrl = _configuration["API_EV:EnVietGateway"];
            var url = "";
            var orderInfo = new
            {
                Name = customerName,
                AgentCode = maKH,
                Tel = phoneNumber,
                Email = email,
                Address = address
            };

            //var productInfo = new[]
            //{
            //     new { ProductName = "Vé máy bay", Quantity = 3, Price = 3000000 },
            //     new { ProductName = "Yến Sào", Quantity = 1, Price = 1000000 }
            //};

            var order = new
            {
                OrderID = orderId,
                Source = "ONL",
                RequestType = "",
                NotifyApi = "",
                ReturnUrl = "https://envietgroup.com/",
                Note = note,
                Info = orderInfo,
                Products = productInfo,
                Amount = price,
                OtherFees = otherFee,
                Discount = discountValue,
                VAT = vat,
                Total = totalPrice
            };

            string orderJson = JsonConvert.SerializeObject(order);

            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("x-master-key", "1370542bcb7c6b71e34e975d4697f89bab164a520934ff47a5aceb780fdc92e6");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearToken);
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject jmessage = JObject.Parse(responseContent);
                    url = jmessage.GetValue("responseBody")["uri"].ToString();
                }
            }
            return url;
        }

        public async Task<string> GetLinkToGateway_V2(string orderId, string maKH, string customerName, string phoneNumber, string email, string address, object[] productInfo, double price, double? discountValue, double? vat, double? otherFee, double totalPrice, string note, string returnUrl)
        {
            var bearToken = await GetEnVietToken();
            string apiUrl = _configuration["API_EV:EnVietGatewayV2"];
            var url = "";
            var PartnerInfo = new
            {
                UserName = "",
                PartnerCode = maKH,
                Name = customerName,
                Tel = phoneNumber,
                Email = email,
                Address = address
            };

            //var productInfo = new[]
            //{
            //     new { ProductName = "Vé máy bay", Quantity = 3, UnitPrice = 3000000 },
            //     new { ProductName = "Yến Sào", Quantity = 1, UnitPrice = 1000000 }
            //};

            var order = new
            {
                OrderID = orderId,
                Source = "ONL",
                RequestType = "",
                NotifyApi = "",
                ReturnUrl = returnUrl,
                Notes = note,
                PartnerInfo,
                Products = productInfo,
                GrossAmount = price,
                TotalFeeAmount = otherFee,
                DiscountAmount = discountValue,
                VAT_Amount = vat,
                TotalAmount = totalPrice,
                CurrencyCode = "VND"
            };

            string orderJson = JsonConvert.SerializeObject(order);

            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("x-master-key", "1370542bcb7c6b71e34e975d4697f89bab164a520934ff47a5aceb780fdc92e6");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearToken);
                var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    JObject jmessage = JObject.Parse(responseContent);
                    url = jmessage.GetValue("responseBody")["paymentLinkUrl"].ToString();
                }
            }
            return url;
        }

    }
}
