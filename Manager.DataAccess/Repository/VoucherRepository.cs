using System.Data;
using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System;
//using System.Web.Mvc;
//using Antlr.Runtime.Misc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using Manager_Manager.Models.CarBooking;
using System.Transactions;
using System.Linq;
using EasyInvoice.Client.Interop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Manager_EV.Helpers;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using Manager.Model.Models.ViewModel.VoucherVM;
using Manager.Model.Models;

namespace Manager.DataAccess.Repository
{
    public class VoucherRepository
    {
        private readonly IConfiguration _configuration;
        private string SQL_VOUCHER;
        private string sqlGetAllOrderHeader = @"Select a.Id, a.OrderStatusId ,a.BookingCode, a.Total, a.Name, a.Phone, a.Email, a.CreateDate, a.DateCustomerUse, 
	                                               c.VoucherName,
	                                               e.Name as TourLocationName,
                                                   f.Status
                                            From OrderHeader a inner join OrderDetail b on a.Id = b.OrderHeaderId inner join
	                                             VOUCHER c on c.Id = b.VoucherId inner join
	                                             VoucherServiceType d on c.Id = d.VoucherId inner join 
	                                             TourLocation e on d.ServiceId = e.Id inner join
                                                 Status f on a.OrderStatusId = f.Id
                                            Where d.Type = 'TOURLOCATION' ";
        public VoucherRepository(IConfiguration configuration = null)
        {
            _configuration = configuration;
            SQL_VOUCHER = _configuration.GetConnectionString("SQL_VOUCHER");
        }
        #region CẤU HÌNH VOUCHER
        public List<VoucherModel> Voucher()
        {
            List<VoucherModel> result = new List<VoucherModel>();
            string sql = @" Select a.Id , a.VoucherCode, a.VoucherName, a.Price, a.DiscountPrice, a.IsActive
                            From Voucher a
                            Order by a.id desc";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                //result = (List<VoucherModel>)conn.Query<VoucherModel, VoucherType, VoucherModel>(sql, (voucher, voucherType) =>
                //{
                //    voucher.VoucherType = voucherType;
                //    voucher.VoucherTypeID = voucherType.Id;
                //    return voucher;
                //}, splitOn: "VoucherTypeID", commandTimeout: 30, commandType: System.Data.CommandType.Text);
                conn.Open();
                result = conn.Query<VoucherModel>(sql, commandTimeout: 30).ToList();
                foreach (var item in result)
                {
                    // Lấy Service Id 
                    int serviceId = GetVoucherServiceTypeById(item.Id).ServiceId;

                    string sqlGetNameService = @"Select b.Name
                                                From VoucherServiceType a inner join TourLocation b on a.ServiceId = b.id
                                                Where a.Type = @Type and a.voucherId = @voucherId and a.ServiceId = @ServiceId";
                    string name = conn.QueryFirst<string>(sqlGetNameService, new
                    {
                        Type = "TOURLOCATION",
                        voucherId = item.Id,
                        ServiceId = serviceId,
                    });

                    item.ServiceName = name;
                }
                conn.Close();
            }
            return result;
        }
        // Save Create Voucher
        public Task<string> SaveCreateVoucher(VoucherModel data, VoucherServiceType voucherServiceType, string HoTenNV)
        {
            int x = 0, y = 0, ID = 0;
            string sqlImg = "";
            string CodeRandom = GetCodeVoucher(); // Auto generate code
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO [Voucher] ([VoucherCode],[VoucherName],[ShortDescription],[Description],[Price], [DiscountPrice], [ExpiryDateFrom], [ExpiryDateTo], [CreateDate], [isActive], [CreateBy])
                                       VALUES ( @Code, @VoucherName, @ShortDescription, @Description, @Price, @DiscountPrice, @ExpiryDateFrom, @ExpiryDateTo, GETDATE(), 0, @HoTenNV) SELECT SCOPE_IDENTITY() AS ID";
                        string sqlInsertvoucherService = @"Insert into [VoucherServiceType] (Type, ServiceId, VoucherId)
                                                           Values (@Type, @ServiceId, @VoucherId)";

                        ID = conn.QueryFirst<int>(sql,
                                new
                                {
                                    Code = CodeRandom,
                                    data.VoucherName,
                                    ShortDescription = data.ShortDescription.Trim(),
                                    Description = data.Description.Trim(),
                                    data.Price,
                                    data.DiscountPrice,
                                    data.ExpiryDateFrom,
                                    data.ExpiryDateTo,
                                    HoTenNV,
                                    //VoucherTypeID = data.VoucherTypeID,
                                    //DiscountAmountWhenUse = data.DiscountAmountWhenUse,
                                    //PriceMinRequired = data.PriceMinRequired
                                }, transaction: transaction, commandType: CommandType.Text, commandTimeout: 30);
                        if (ID > 0)
                        {
                            for (int i = 0; i < data.listImage.Count; i++)
                            {
                                int isMain = 0;
                                if (data.listImage[i].isMainImage == true)
                                {
                                    isMain = 1;
                                }
                                sqlImg = @"INSERT INTO [ImageVoucher] ([voucherId],[imageUrl],[isMainImage]) 
                                            VALUES (@voucherId , @imageUrl, @isMainImage)";
                                x += conn.Execute(sqlImg, new
                                {
                                    voucherId = ID,
                                    data.listImage[i].imageUrl,
                                    isMainImage = isMain
                                }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                            }
                            // Thêm vào bảng Vourcher Service
                            voucherServiceType.VoucherId = ID;
                            y = conn.Execute(sqlInsertvoucherService, new
                            {
                                voucherServiceType.Type,
                                voucherServiceType.ServiceId,
                                voucherServiceType.VoucherId
                            }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                        }
                        if (x > 0 && ID > 0 && y > 0)
                        {
                            transaction.Commit();
                            conn.Close();
                            return Task.FromResult(StaticDetailVoucher.SUCCESS);
                        }
                        else
                        {
                            transaction.Rollback();
                            conn.Close();
                            return Task.FromResult("Lỗi truy xuất dữ liệu, vui lòng liên hệ IT Flight VN");
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        conn.Close();
                        return Task.FromResult("Lỗi truy xuất dữ liệu, vui lòng liên hệ IT Flight VN");
                    }
                }
            }

        }
        // Xóa Voucher
        public string DeleteVoucher(int voucherId)
        {
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int x = 0;
                        // Lấy danh sách URL hình
                        string sqlGetAllImageDelete = @"SELECT imageUrl FROM ImageVoucher WHERE voucherId = @voucherId";
                        List<string> listImageDelete = conn.Query<string>(sqlGetAllImageDelete,
                        new
                        {
                            voucherId
                        }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text).ToList();

                        // Delete Image of Voucher
                        string sqlDeleteImage = @"DELETE FROM [ImageVoucher] WHERE voucherId = @voucherId";
                        conn.Execute(sqlDeleteImage,
                        new
                        {
                            voucherId
                        }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                        // Delete VoucherServiceType
                        string sqlVourcherServiceType = @"DELETE FROM [VoucherServiceType] WHERE voucherId = @voucherId";
                        conn.Execute(sqlVourcherServiceType,
                        new
                        {
                            voucherId
                        }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                        // Delete Voucher
                        string sqlDeleteVoucher = @"DELETE FROM [Voucher] WHERE id = @voucherId";
                        x = conn.Execute(sqlDeleteVoucher,
                            new
                            {
                                voucherId
                            }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                        if (x > 0)
                        {
                            // Delete Successfully
                            transaction.Commit();
                            conn.Close();
                            // Xóa ảnh trên Server
                            foreach (string imageUrl in listImageDelete)
                            {
                                Manager.Common.Helpers.Common.DeleteImg(imageUrl);
                            }
                            return StaticDetailVoucher.SUCCESS;
                        }
                        else
                        {
                            transaction.Rollback();
                            conn.Close();
                            return StaticDetailVoucher.FAIL;
                        }
                    }
                    catch
                    {
                        // If run to this line -> fail
                        transaction.Rollback();
                        conn.Close();
                        return "Lỗi truy xuất dữ liệu, vui lòng liên hệ IT Flight VN";
                    }
                }
            }
        }
        // Chỉnh sửa VOUCHER
        public Task<string> SaveEditVoucher(VoucherModel data, VoucherServiceType voucherServiceType, List<int> CurrentImagesId = null, string mainImageName = null)
        {
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Cập nhật thông tin voucher
                        string sqlEdit = @"UPDATE [Voucher]
                                            SET [VoucherName] = @VoucherName, [ShortDescription] = @ShortDescription, [Description] = @Description, [Price] = @Price,
                                                [DiscountPrice] = @DiscountPrice, [ExpiryDateFrom] = @ExpiryDateFrom, [ExpiryDateTo] = @ExpiryDateTo,
                                                [CreateBy] = @CreateBy
                                            WHERE ID = @Id";

                        int rowsAffected = conn.Execute(sqlEdit, new
                        {
                            data.VoucherName,
                            data.ShortDescription,
                            data.Description,
                            data.Price,
                            data.DiscountPrice,
                            data.ExpiryDateFrom,
                            data.ExpiryDateTo,
                            data.CreateDate,
                            data.CreateBy,
                            data.Id,
                            //DiscountAmountWhenUse = data.DiscountAmountWhenUse,
                            //VoucherTypeID = data.VoucherTypeID,
                            //PriceMinRequired = data.PriceMinRequired,
                        }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);

                        if (rowsAffected <= 0) // Update không thành công
                        {
                            transaction.Rollback();
                            return Task.FromResult("Chỉnh sửa không thành công");
                        }

                        // Thêm hình ảnh và lấy ID vừa thêm
                        string sqlInsertImg = @"INSERT INTO [ImageVoucher] ([voucherId], [imageUrl], [isMainImage])
                                                OUTPUT INSERTED.ID
                                                VALUES (@voucherId, @imageUrl, @isMainImage)";

                        var insertImgIds = new List<int>();

                        foreach (var img in data.listImage)
                        {
                            int isMain = img.isMainImage ? 1 : 0;
                            var ids = conn.Query<int>(sqlInsertImg, new
                            {
                                voucherId = data.Id,
                                img.imageUrl,
                                isMainImage = isMain
                            }, transaction: transaction, commandTimeout: 30, commandType: CommandType.Text);
                            insertImgIds.AddRange(ids);
                        }

                        // Kiểm tra trong list thình mới thêm có main chưa
                        var isMainImageExists = data.listImage.Any(img => img.isMainImage);
                        // Cập nhật hình ảnh
                        if (isMainImageExists)
                        {
                            // Nếu hình ảnh mới thêm có main ->  Cập nhật tất cả hình ảnh cũ, bỏ qua các hình ảnh mới thêm
                            string sqlUpdateImg = @"UPDATE [ImageVoucher]
                                                    SET isMainImage = 0
                                                    WHERE Id NOT IN @IdsToSkip";

                            conn.Execute(sqlUpdateImg, new { IdsToSkip = insertImgIds }, transaction: transaction, commandTimeout: 30);
                        }
                        else if (CurrentImagesId != null)
                        {
                            // Cập nhật các hình ảnh hiện có để xác định hình ảnh chính
                            string sqlUpdateImg = @"UPDATE [ImageVoucher]
                                                    SET isMainImage = CASE WHEN Id = @MainImageId THEN 1 ELSE 0 END
                                                    WHERE Id IN @CurrentImagesId";

                            conn.Execute(sqlUpdateImg, new
                            {
                                MainImageId = mainImageName,
                                CurrentImagesId
                            }, transaction: transaction, commandTimeout: 30);
                        }

                        // Cập nhật bảng Voucher Service
                        string sqlUpdateVoucherService = @"UPDATE [VoucherServiceType]
                                                            SET Type = @Type, ServiceId = @ServiceId
                                                            WHERE VoucherId = @VoucherId";

                        conn.Execute(sqlUpdateVoucherService, new
                        {
                            voucherServiceType.Type,
                            voucherServiceType.ServiceId,
                            VoucherId = data.Id
                        }, transaction: transaction, commandTimeout: 30);

                        transaction.Commit();
                        return Task.FromResult(StaticDetailVoucher.SUCCESS);
                    }
                    catch
                    {
                        transaction.Rollback();
                        return Task.FromResult("Lỗi truy xuất dữ liệu, vui lòng liên hệ IT Flight VN");
                    }
                }
            }
        }
        // Ẩn hiện VOUCHER
        public bool ChangeActiveVoucher(int ID, int Active)
        {
            using (var con = new SqlConnection(SQL_VOUCHER))
            {
                try
                {
                    con.Open();
                    int x = 0;
                    string sql = $@"Update [Voucher] set [isActive] = @Active where id = @ID";
                    x = con.Execute(sql, new
                    {
                        Active,
                        ID
                    }, null, commandTimeout: 30, commandType: CommandType.Text);
                    con.Close();
                    if (x == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    con.Close();
                    return false;
                }
            };
        }
        // Tạo mã Code cho Voucher
        public string GetCodeVoucher()
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string prefix = "SPVC";
            string newCode = string.Empty;

            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                string sqlCheckVoucherCodeExist = @"SELECT TOP 1 * FROM [VoucherCodeHistory]";
                var voucherCodeHistory = conn?.QueryFirstOrDefault<VoucherCodeHistory>(sqlCheckVoucherCodeExist, null, null);
                if (voucherCodeHistory != null)
                {
                    int currentValue = voucherCodeHistory.CurrentValue + 1;
                    string sqlUpdateVoucherCode = @"UPDATE [VoucherCodeHistory] SET [CurrentValue] = @CurrentValue, FormatDate = @FormatDate, Prefix = @Prefix WHERE Id = @Id ";
                    conn.Execute(sqlUpdateVoucherCode, new
                    {
                        CurrentValue = currentValue,
                        FormatDate = date,
                        Prefix = prefix,
                        voucherCodeHistory.Id,
                    });
                    newCode = prefix + date + currentValue.ToString("D3");
                }
                else
                {
                    string insertSql = @"INSERT INTO [VoucherCodeHistory] ( [CurrentValue], [FormatDate], [Prefix]) VALUES (@CurrentValue, @FormatDate, @Prefix)";
                    conn.Execute(insertSql, new
                    {
                        CurrentValue = 1,
                        FormatDate = date,
                        Prefix = prefix,
                    });
                    newCode = prefix + date + 1.ToString("D3");
                }
            }
            return newCode;
        }

        //Xóa hình của Voucher
        public string DeleteImg(int id, int voucherId)
        {
            string sqlDeleteImage = @"DELETE FROM ImageVoucher WHERE ID = @Id";
            string sqlCountImages = @"SELECT COUNT(*) FROM ImageVoucher WHERE voucherId = @voucherId";
            string sqlUpdateMainImage = @"UPDATE ImageVoucher
                                       SET isMainImage = 1
                                       WHERE ID = ( SELECT TOP 1 ID
                                                    FROM ImageVoucher
                                                    WHERE voucherId = @voucherId
                                                    ORDER BY ID ASC)";
            string sqlGetAllImageUrl = @"SELECT imageUrl FROM ImageVoucher WHERE voucherId = @voucherId and id = @id";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                // Đếm số lượng hình ảnh hiện tại
                using (var transaction = conn.BeginTransaction())
                {
                    int imageCount = conn.ExecuteScalar<int>(sqlCountImages, new { Id = id, voucherId }, transaction: transaction);

                    // Kiểm tra nếu số lượng hình ảnh lớn hơn 1 thì mới xóa
                    if (imageCount > 1)
                    {
                        try
                        {
                            // Lấy ra danh sách URL sẽ xóa
                            List<string> listUrlImageDeleted = conn.Query<string>(sqlGetAllImageUrl, new
                            {
                                voucherId,
                                id
                            }, transaction: transaction, commandTimeout: 30).ToList();
                            // Xóa trong database
                            int rowsAffected = conn.Execute(sqlDeleteImage, new { Id = id }, transaction: transaction);
                            // Cập nhật hình gần nhất làm Main
                            int updateAffected = conn.Execute(sqlUpdateMainImage, new { voucherId }, transaction: transaction);
                            if (rowsAffected > 0 && updateAffected > 0)
                            {
                                transaction.Commit();
                                conn.Close();
                                foreach (string imageUrl in listUrlImageDeleted)
                                {
                                    // Xóa hình trên Server
                                    Manager.Common.Helpers.Common.DeleteImg(imageUrl);
                                }
                                return StaticDetailVoucher.SUCCESS;
                            }
                            else
                            {
                                transaction.Rollback();
                                conn.Close();

                                return StaticDetailVoucher.FAIL;
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            conn.Close();
                            return "Lỗi truy vấn database";
                        }

                    }
                    else
                    {
                        return "Danh sách hình không thể trống.";
                    }
                }
            }
        }
        #endregion

        #region BOOKING VOUCHER
        public (List<OrderHeaderVoucher>, int) GetAllOrderHeaderVoucher(int pageSize, int offset = 0, int statusId = 0, DateTime? dateForm = null, DateTime? dateTo = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Offset", offset); // Số hàng bỏ qua
            parameters.Add("PageSize", pageSize);
            string condition = string.Empty;
            string sqlCountTotalOrder = @"Select COUNT(*)
                                        From OrderHeader a inner join OrderDetail b on a.Id = b.OrderHeaderId inner join
	                                            VOUCHER c on c.Id = b.VoucherId inner join
	                                            VoucherServiceType d on c.Id = d.VoucherId inner join 
	                                            TourLocation e on d.ServiceId = e.Id inner join
                                                Status f on a.OrderStatusId = f.Id
                                        Where d.Type = 'TOURLOCATION' ";
            if (statusId != 0)
            {
                condition += " and a.OrderStatusId = @OrderStatusId ";
                parameters.Add("@OrderStatusId", statusId);
            }

            if (dateForm.HasValue && dateTo.HasValue)
            {
                condition += " AND a.CreateDate BETWEEN @DateFrom AND @DateTo ";
                parameters.Add("@DateFrom", dateForm.Value.ToString("yyyy-MM-dd 00:00"));
                parameters.Add("@DateTo", dateTo.Value.ToString("yyyy-MM-dd 23:59"));
            }
            sqlGetAllOrderHeader += condition;
            sqlGetAllOrderHeader += @" Order By a.id desc  OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
            sqlCountTotalOrder += condition; // Lấy câu sql lấy tổng số Order 

            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                List<OrderHeaderVoucher> result = conn.Query<OrderHeaderVoucher>(sqlGetAllOrderHeader, parameters, commandTimeout: 30).ToList();
                int totalRecord = conn.ExecuteScalar<int>(sqlCountTotalOrder, parameters, commandTimeout: 30);
                conn.Close();
                return (result, totalRecord);
            }
        }
        public OrderHeaderVoucher GetOrderHeaderVoucherById(int orderHeaderId, LocationRepository location_Rep = null)
        {
            string sqlSelectOrderHeader = @"Select a.Id ,a.OrderStatusId ,a.BookingCode, a.Total, a.Name, a.Phone, a.Email, a.Address, a.CompanyName, a.MaSoThue,
                                                   a.NumberAdult, a.NumberChild, a.NumberBaby, a.NoteStatus, a.Note, a.PriceVAT, a.CancelReason, a.CurrentPrice, a.Reciever,
	                                               c.VoucherName,
	                                               e.Name as TourLocationName, e.Phone as TourLocationPhone, e.Email as TourLocationEmail, 
                                                   e.Province as TourLocationProvince, e.District as TourLocationDistrict,
                                                   f.Status
                                            From OrderHeader a inner join OrderDetail b on a.Id = b.OrderHeaderId inner join
	                                             VOUCHER c on c.Id = b.VoucherId inner join
	                                             VoucherServiceType d on c.Id = d.VoucherId inner join 
	                                             TourLocation e on d.ServiceId = e.Id inner join
                                                 Status f on a.OrderStatusId = f.Id
                                            Where d.Type = 'TOURLOCATION' and a.Id = @OrderHeaderId";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                OrderHeaderVoucher result = new OrderHeaderVoucher();
                result = conn.Query<OrderHeaderVoucher>(sqlSelectOrderHeader, new
                {
                    OrderHeaderId = orderHeaderId
                }, commandTimeout: 30).FirstOrDefault();
                if (location_Rep != null)
                {
                    result.TourLocationProvince = location_Rep.GetProvinceByCode(result.TourLocationProvince);
                    result.TourLocationDistrict = location_Rep.GetDistrictByCode(result.TourLocationDistrict);
                }
                return result;
            };
        }

        public IEnumerable<VoucherModel> GetAllOrderVoucherById(int orderHeaderId)
        {
            string sqlSelectOrderDetail = @"Select b.VoucherName, b.VoucherCode, b.DiscountPrice 
                                            From OrderDetail a inner join Voucher b on a.VoucherId = b.id
                                            Where OrderHeaderId = @OrderHeaderId ";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                return conn.Query<VoucherModel>(sqlSelectOrderDetail, new
                {
                    OrderHeaderId = orderHeaderId
                }, commandTimeout: 30).ToList();
            }
        }
        // Lấy trạng thái của OrderHeader Theo Id
        public OrderHeaderVoucher GetStatusNoteAfterUpdate(int orderHeaderId)
        {
            string sqlGetUpdateStatusOfOrder = @" Select a.NoteStatus, a.CancelReason, a.OrderStatusId, a.Reciever, b.Status
                                                From OrderHeader a inner join Status b on a.OrderStatusId = b.id
                                                Where a.Id = @OrderHeaderId";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                return conn.Query<OrderHeaderVoucher>(sqlGetUpdateStatusOfOrder, new
                {
                    OrderHeaderId = orderHeaderId
                }).FirstOrDefault();
            }
        }

        // Lấy danh sách trạng thái
        public IEnumerable<Status> GetAllStatus()
        {
            string sqlGetAllStatus = @"Select Id, Status as StatusName
                                       From Status";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                return conn.Query<Status>(sqlGetAllStatus, commandTimeout: 30);
            }
        }
        // Cập nhật trạng thái Order 
        public string UpdateStatusBooking(OrderHeaderVoucher orderHeaderVoucher, AccountModel currentAccount, LocationRepository locationRepository = null)
        {
            if (currentAccount.MaPhongBan.Trim() == StaticDetailVoucher.MaPhongBan_DuLich || currentAccount.MaPhongBan.Trim() == StaticDetailVoucher.MaPhongBan_IT)
            {
                string sqlUpdateStatus = @" Update OrderHeader set OrderStatusId = @OrderStatusId, NoteStatus = @NewNoteStatus, Reciever = @Reciever,
                                                                   EditDate = @EditDate
                                            Where Id = @Id ";
                using (var conn = new SqlConnection(SQL_VOUCHER))
                {
                    try
                    {
                        int x = conn.Execute(sqlUpdateStatus, new
                        {
                            orderHeaderVoucher.OrderStatusId,
                            NewNoteStatus = orderHeaderVoucher.NoteStatus,
                            Reciever = currentAccount.HoTen,
                            orderHeaderVoucher.EditDate,
                            orderHeaderVoucher.Id
                        }, commandTimeout: 30);

                        if (x > 0)
                        {
                            OrderVM orderVM = new OrderVM()
                            {
                                OrderHeaderVoucher = GetOrderHeaderVoucherById(orderHeaderVoucher.Id, locationRepository),
                                OrderVouchers = GetAllOrderVoucherById(orderHeaderVoucher.Id),
                            };
                            // Send Mail
                            string subject = string.Empty;
                            string isSendMailSuccess = string.Empty;
                            if (orderHeaderVoucher.OrderStatusId == StaticDetailVoucher.OrderStatusId_COMPLETED)
                            {
                                subject = $"[VOUCHER] CẢM ƠN QUÝ KHÁCH";
                                isSendMailSuccess = SendMailUpdateStatus(orderVM, StaticDetailVoucher.Email_ProgramId_COMPLETED, subject);
                            }
                            else
                            {
                                subject = $"[VOUCHER] XÁC NHẬN {orderVM.OrderHeaderVoucher.Status.ToUpper()}";
                                isSendMailSuccess = SendMailUpdateStatus(orderVM, StaticDetailVoucher.Email_ProgramId_CHANGESTATUS, subject);
                            }
                            if (isSendMailSuccess == StaticDetailVoucher.SUCCESS)
                            {
                                conn.Close();
                                return StaticDetailVoucher.SUCCESS;
                            }
                        }
                        conn.Close();
                        return StaticDetailVoucher.FAIL;
                    }
                    catch
                    {
                        conn.Close();
                        return StaticDetailVoucher.FAIL;
                    }
                }
            }
            else
            {
                return "Bạn không thuộc phòng ban Du Lịch.";
            }
        }
        public string SendMailUpdateStatus(OrderVM orderVM, string programId, string subject)
        {
            bool isSuccess = false;
            EVMailRepository email_Rep = new EVMailRepository(_configuration);
            EVEmail ev_Email = new EVEmail();
            ev_Email = email_Rep.GetEVEMailContentByProgram(programId);
            string VATContent = templateVAT(orderVM.OrderHeaderVoucher);
            string postCondition = GetPostDescription().GetAwaiter().GetResult();
            try
            {
                if (postCondition == StaticDetailVoucher.FAIL)
                {
                    return StaticDetailVoucher.FAIL;
                }
                ///-------- Start of mail body ------------
                string mailBody;
                var webRequest = System.Net.WebRequest.Create(ev_Email.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new System.IO.StreamReader(content))
                { mailBody = reader.ReadToEnd(); }
                // Nếu trong trạng thái COMPLETED
                if (orderVM.OrderHeaderVoucher.OrderStatusId == StaticDetailVoucher.OrderStatusId_COMPLETED)
                {
                    mailBody = mailBody.Replace("$_evcode", orderVM.OrderHeaderVoucher.BookingCode);
                    mailBody = mailBody.Replace("$_Ngaygui", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                }
                else
                {
                    mailBody = mailBody.Replace("$_Date", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                    mailBody = mailBody.Replace("$_Fullname", $"{orderVM.OrderHeaderVoucher.Name.ToUpper()}");
                    mailBody = mailBody.Replace("$_evcode", orderVM.OrderHeaderVoucher.BookingCode);
                    mailBody = mailBody.Replace("$_ProductName", orderVM.OrderVouchers.FirstOrDefault().VoucherName);
                    mailBody = mailBody.Replace("$_VatPrice", orderVM.OrderHeaderVoucher.PriceVAT.ToString("#,0"));
                    mailBody = mailBody.Replace("$_Price", string.Format("{0:#,0}", orderVM.OrderHeaderVoucher.CurrentPrice));
                    mailBody = mailBody.Replace("$_Total", orderVM.OrderHeaderVoucher.Total.ToString("#,0"));
                    mailBody = mailBody.Replace("{{VATContent}}", VATContent);
                    mailBody = mailBody.Replace("$_Phone", orderVM.OrderHeaderVoucher.Phone);
                    mailBody = mailBody.Replace("$_Email", orderVM.OrderHeaderVoucher.Email);
                    mailBody = mailBody.Replace("$_BookingNotes", $"{orderVM.OrderHeaderVoucher.Note}");
                    mailBody = mailBody.Replace("$_Note", $"{orderVM.OrderHeaderVoucher.NoteStatus}");
                    mailBody = mailBody.Replace("$_Condition", postCondition);
                    mailBody = mailBody.Replace("$_ThisYear ", DateTime.Now.Year.ToString());
                    // Nếu trong trạng thái CANCELLED
                    if (orderVM.OrderHeaderVoucher.OrderStatusId == StaticDetailVoucher.OrderStatusId_CANCELLED)
                    {
                        mailBody = mailBody.Replace("$_CancellationReason", orderVM.OrderHeaderVoucher.CancelReason);
                    }
                    // Nếu khác trạng thái CANCELLED
                    if (orderVM.OrderHeaderVoucher.OrderStatusId != StaticDetailVoucher.OrderStatusId_CANCELLED)
                    {
                        mailBody = mailBody.Replace("$_StatusEnviet", orderVM.OrderHeaderVoucher.Status);
                    }
                }

                isSuccess = Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", subject, mailBody, orderVM.OrderHeaderVoucher.Email, ev_Email.username, ev_Email.password, ev_Email.hostName, ev_Email.port, ev_Email.useSSL, ev_Email.CC, ev_Email.BCC, isCC: false, isBCC: false);
                if (isSuccess == true)
                {
                    return StaticDetailVoucher.SUCCESS;
                }
                return StaticDetailVoucher.FAIL;
            }
            catch (Exception ex)
            {
                return StaticDetailVoucher.FAIL;
            }
        }
        // Cancell Booking
        public string CancelBooking(OrderHeaderVoucher orderHeaderVoucher, AccountModel currentAccount, LocationRepository locationRepository = null)
        {
            if (currentAccount.MaPhongBan.Trim() == StaticDetailVoucher.MaPhongBan_DuLich || currentAccount.MaPhongBan.Trim() == StaticDetailVoucher.MaPhongBan_IT)
            {
                string sqlUpdateOrderStatus = @"Update OrderHeader set OrderStatusId = 4, CancelReason = @CancelReason, EditDate = GETDATE(), NoteStatus = @NoteStatus,
                                                                   Reciever = @Reciever
                                            Where Id = @Id ";
                try
                {
                    using (var conn = new SqlConnection(SQL_VOUCHER))
                    {
                        conn.Open();
                        int x = conn.Execute(sqlUpdateOrderStatus, new
                        {
                            orderHeaderVoucher.Id,
                            orderHeaderVoucher.CancelReason,
                            Reciever = currentAccount.HoTen,
                            orderHeaderVoucher.NoteStatus,
                        }, commandTimeout: 30);
                        conn.Close();
                        if (x > 0)
                        {
                            OrderVM orderVM = new OrderVM()
                            {
                                OrderHeaderVoucher = GetOrderHeaderVoucherById(orderHeaderVoucher.Id, locationRepository),
                                OrderVouchers = GetAllOrderVoucherById(orderHeaderVoucher.Id),
                            };
                            // SendMail to Customer
                            string subject = $"[VOUCHER] XÁC NHẬN HỦY BOOKING";
                            string isSendMailSuccess = SendMailUpdateStatus(orderVM, StaticDetailVoucher.Email_ProgramId_CANCELVOUCHER, subject);
                            if (isSendMailSuccess == StaticDetailVoucher.SUCCESS)
                            {
                                conn.Close();
                                return StaticDetailVoucher.SUCCESS;
                            }
                            return StaticDetailVoucher.SUCCESS;
                        }
                        return StaticDetailVoucher.FAIL;
                    }
                }
                catch
                {
                    return StaticDetailVoucher.FAIL;
                }
            }
            else
            {
                return "Bạn không thuộc phòng ban Du Lịch.";
            }
        }
        public string templateVAT(OrderHeaderVoucher orderHeader)
        {
            string VATContent = string.Empty;
            if (!orderHeader.PriceVAT.Equals(0))
            {
                // Có xuất VAT 
                VATContent += $@"
                    <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                        <tr style='color:#fff; background-color: #006886; padding:5px 10px; font-size: 15px;'>
                            <td colspan='4' style='width:200px;'>Thông tin xuất hóa đơn</td>
                        </tr>
                    </table>
                    <div style='padding:5px 10px;color:#3f3d33;'>
                        <div class='row'>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Mã số thuế:</td>
                                        <td colspan='3'><strong>{orderHeader.MaSoThue}</strong></td>
                                    </tr>
                                </table>
                            </div>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Tên công ty:</td>
                                        <td colspan='3'><strong>{orderHeader.CompanyName}</strong></td>
                                    </tr>
                                </table>
                            </div>
                            <div class='col-sm-6'>
                                <table style='width: 100%;' cellspacing='0' cellpadding='7' border='0'>
                                    <tr>
                                        <td style='width: 110px;'>Địa chỉ:</td>
                                        <td colspan='3'><strong>{orderHeader.Address}</strong></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>";
                return VATContent;
            }
            return "";
        }
        public Task<string> GetPostDescription()
        {
            try
            {
                string SQL_POST = _configuration.GetConnectionString("SQL_POST");
                using (var con = new SqlConnection(SQL_POST))
                {
                    string query = @"SELECT Description FROM Posts WHERE ID = 9";
                    string description = con.Query<string>(query).FirstOrDefault();
                    return Task.FromResult(description);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(StaticDetailVoucher.FAIL, ex);
            }
        }
        #endregion
        public VoucherModel GetVoucherById(int id)
        {
            using (var con = new SqlConnection(SQL_VOUCHER))
            {
                con.Open();
                List<VoucherModel> result = new List<VoucherModel>();
                List<ImageVoucher> imageVouchers = new List<ImageVoucher>();
                string sql = @"select a.Id, a.CreateDate, a.CreateBy, a.VoucherName, a.ShortDescription, a.Description, a.Price, a.DiscountPrice, a.ExpiryDateFrom, a.ExpiryDateTo ,b.voucherId, b.* 
                               from Voucher a inner join ImageVoucher b on a.Id = b.voucherId 
                               where a.Id = @Id";

                result = (List<VoucherModel>)con.Query<VoucherModel, ImageVoucher, VoucherModel>(sql, (voucher, ImageVoucher) =>
                {
                    imageVouchers.Add(ImageVoucher);
                    return voucher;
                }, param: new { Id = id }, commandTimeout: 30, commandType: CommandType.Text);
                con.Close();

                if (imageVouchers.Count > 0)
                {
                    result.FirstOrDefault().listImage = imageVouchers;
                }
                return result.FirstOrDefault();

            }
        }

        public List<ServiceType> GetAllServiceType()
        {
            List<ServiceType> result = new List<ServiceType>();
            string sql = @" Select id, name, type
                            From ServiceType";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                result = (List<ServiceType>)conn.Query<ServiceType>(sql, commandTimeout: 30, commandType: CommandType.Text);
                conn.Close();
            }
            return result;
        }

        public VoucherServiceType GetVoucherServiceTypeById(int voucherId)
        {
            List<VoucherServiceType> result = new List<VoucherServiceType>();
            string sql = @" Select Type, ServiceId, VoucherId
                            From VoucherServiceType
                            Where VoucherId = @voucherId";
            using (var conn = new SqlConnection(SQL_VOUCHER))
            {
                conn.Open();
                result = (List<VoucherServiceType>)conn.Query<VoucherServiceType>(sql, new
                {
                    voucherId
                }, commandTimeout: 30, commandType: CommandType.Text);
                conn.Close();
            }
            return result.FirstOrDefault();
        }
    }
}
