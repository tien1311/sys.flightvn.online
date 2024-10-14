using Manager.Model.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class DinhDanhBaoLuuRepository
    {
        DBase db = new DBase();
        public bool SaveCreateDDBL(DinhDanhBaoLuuModel DDBL, List<TenHanhKhach> ListTenHK, string MemberKH, string AgentName)
        {
            bool result = true;
            string MaDDBL = PhatSinhMaDDBL();
            if (DDBL.GhiChu == null)
            {
                DDBL.GhiChu = "";
            }
            if (DDBL.SoVe == null)
            {
                DDBL.SoVe = "";
            }
            if (DDBL.PNR == null)
            {
                DDBL.PNR = "";
            }
            List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
            Param.Add(new DBase.AddParameters("@Tel", DDBL.DienThoaiNB.Trim()));
            Param.Add(new DBase.AddParameters("@Note", DDBL.GhiChu.Trim()));
            Param.Add(new DBase.AddParameters("@DateEndSale", DDBL.HanDangBan.Trim()));
            Param.Add(new DBase.AddParameters("@Airline", DDBL.Hang.Trim()));
            Param.Add(new DBase.AddParameters("@Reserve", DDBL.Loai.Trim()));
            Param.Add(new DBase.AddParameters("@PNR", DDBL.PNR.Trim()));
            Param.Add(new DBase.AddParameters("@TicketNumber", DDBL.SoVe.Trim()));
            Param.Add(new DBase.AddParameters("@Name", DDBL.TenNB.Trim()));
            Param.Add(new DBase.AddParameters("@Status", DDBL.TrangThai.Trim()));
            Param.Add(new DBase.AddParameters("@Hidden", DDBL.Hidden.Trim()));
            Param.Add(new DBase.AddParameters("@MemberKH", MemberKH.Trim()));
            Param.Add(new DBase.AddParameters("@AgentName", AgentName.Trim()));
            Param.Add(new DBase.AddParameters("@ReservationCode", MaDDBL.Trim()));
            Param.Add(new DBase.AddParameters("@CreateDate", DateTime.Now.ToString("MM/dd/yyyy").Trim()));

            string ID = db.ExecuteDataSet("SP_INSERT_IDENTIFICATION", CommandType.StoredProcedure, "server18", Param).Tables[0].Rows[0][0].ToString();
            if (ID != "" && ID != null)
            {
                for (int i = 0; i < ListTenHK.Count; i++)
                {
                    string sql = "INSERT INTO [IdentificationDetail] ([Name] ,[IdentificationID],[PriceRoot],[Price]) VALUES ( @Name,@IdentificationID,@PriceRoot,@Price)";
                    List<DBase.AddParameters> Param_TenHK = new List<DBase.AddParameters>();
                    Param_TenHK.Add(new DBase.AddParameters("@Name", ListTenHK[i].TenHK));
                    Param_TenHK.Add(new DBase.AddParameters("@PriceRoot", ListTenHK[i].GiaBan));
                    Param_TenHK.Add(new DBase.AddParameters("@Price", ListTenHK[i].GiaGiam));
                    Param_TenHK.Add(new DBase.AddParameters("@IdentificationID", ID));
                    int y = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param_TenHK);
                    if (y <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool SaveEditDDBL(DinhDanhBaoLuuModel DDBL, List<TenHanhKhach> ListTenHK)
        {
            bool result = true;
            if (DDBL.GhiChu == null)
            {
                DDBL.GhiChu = "";
            }
            if (DDBL.SoVe == null)
            {
                DDBL.SoVe = "";
            }
            if (DDBL.PNR == null)
            {
                DDBL.PNR = "";
            }
            string sql_DDBL = @"UPDATE Identification SET Tel = '" + DDBL.DienThoaiNB.Trim() + "', Note = N'" + DDBL.GhiChu.Trim() + "',DateEndSale = '" + DDBL.HanDangBan.Trim() + "', Airline = N'" + DDBL.Hang.Trim() + "', Reserve = N'" + DDBL.Loai.Trim() + "',PNR = N'" + DDBL.PNR.Trim() + "',TicketNumber = '" + DDBL.SoVe.Trim() + "', Name = '" + DDBL.TenNB.Trim() + "',Status = '" + DDBL.TrangThai.Trim() + "',Hidden = '" + DDBL.Hidden.Trim() + "' where ID =" + DDBL.ID;
            int a = db.ExecuteNoneQuery(sql_DDBL, CommandType.Text, "server18", null);
            if (a > 0)
            {
                string delete = "Delete IdentificationDetail where IdentificationID = " + DDBL.ID;
                db.ExecuteNoneQuery(delete, CommandType.Text, "server18", null);
                for (int i = 0; i < ListTenHK.Count; i++)
                {
                    string sql = "INSERT INTO [IdentificationDetail] ([Name] ,[IdentificationID],[PriceRoot],[Price]) VALUES ( @Name,@IdentificationID,@PriceRoot,@Price)";
                    List<DBase.AddParameters> Param_TenHK = new List<DBase.AddParameters>();
                    Param_TenHK.Add(new DBase.AddParameters("@Name", ListTenHK[i].TenHK));
                    Param_TenHK.Add(new DBase.AddParameters("@PriceRoot", ListTenHK[i].GiaBan));
                    Param_TenHK.Add(new DBase.AddParameters("@Price", ListTenHK[i].GiaGiam));
                    Param_TenHK.Add(new DBase.AddParameters("@IdentificationID", DDBL.ID));
                    int y = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param_TenHK);
                    if (y <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public DDBL SearchDDBL(string from_date, string to_date, string Hang, string Loai, string tenHK, string MaKH)
        {

            DDBL item = new DDBL();
            string sqlDK = @"select top 1 * FROM BAIVIET where ROWID = 182";
            DataTable tbDK = db.ExecuteDataSet(sqlDK, CommandType.Text, "serverAirline24h", null).Tables[0];

            if (tbDK != null && tbDK.Rows.Count > 0)
            {
                item.DieuKien = tbDK.Rows[0]["MOTA"].ToString();
                item.NoiDungDK = tbDK.Rows[0]["NOIDUNG"].ToString();
            }
            string trangthai = "", loai = "", StrWheres = "";
            StrWheres += " AND Iden.CreateDate >= '" + from_date + "' AND Iden.CreateDate <='" + to_date + " 23:59:59'";

            if (Hang != "ALL")
            {
                StrWheres += " AND Iden.Airline = '" + Hang + "'";
            }
            if (Loai != "ALL")
            {
                StrWheres += " AND Iden.Reserve = '" + Loai + "'";
            }
            if (tenHK != null)
            {
                StrWheres += " AND IdenDetail.Name like N'%" + tenHK + "%'";
            }

            List<DinhDanhBaoLuuModel> ListDDBL = new List<DinhDanhBaoLuuModel>();
            string sql = @"select Iden.* from Identification Iden
                            left join IdentificationDetail IdenDetail on Iden.ID = IdenDetail.IdentificationID
                            where 1=1" + StrWheres + "Group by Iden.ID, DateEndSale, Reserve, AgentName, Airline,Tel,Iden.PriceRoot, Iden.Price, Note, ReservationCode, TicketNumber,PrivateNote, Iden.Name, MemberKH, Status, PNR, CreateDate,Hidden order by CreateDate desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        if (tb.Rows[i]["Hidden"].ToString() == "True")
                        {
                            trangthai = "Ẩn";
                        }
                        else
                        {
                            if (tb.Rows[i]["Status"].ToString() == "True")
                            {
                                trangthai = "Đang bán";
                            }
                            else
                            {
                                trangthai = "Ngưng bán";
                            }
                        }
                        if (tb.Rows[i]["Reserve"].ToString() == "DD")
                        {
                            loai = "Định Danh";
                        }
                        else
                        {
                            loai = "EMDS";
                        }
                        DinhDanhBaoLuuModel result = new DinhDanhBaoLuuModel();
                        result.ID = tb.Rows[i]["ID"].ToString();
                        result.DienThoaiNB = tb.Rows[i]["Tel"].ToString();
                        result.GhiChu = tb.Rows[i]["Note"].ToString();
                        result.HanDangBan = tb.Rows[i]["DateEndSale"].ToString();
                        result.Hang = tb.Rows[i]["Airline"].ToString();
                        result.Loai = loai;
                        result.PNR = tb.Rows[i]["PNR"].ToString();
                        result.SoVe = tb.Rows[i]["TicketNumber"].ToString();
                        result.TenNB = tb.Rows[i]["Name"].ToString();
                        result.TrangThai = trangthai;
                        result.NgayDang = DateTime.Parse(tb.Rows[i]["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                        ListDDBL.Add(result);
                    }
                    item.ListDDBL = ListDDBL;
                }
            }
            return item;
        }
        public DDBL QuanlyDDBL(string MaKH)
        {
            DDBL item = new DDBL();
            string trangthai = "", loai = "";
            string sqlDK = @"select top 1 * FROM BAIVIET where ROWID = 182";
            DataTable tbDK = db.ExecuteDataSet(sqlDK, CommandType.Text, "serverAirline24h", null).Tables[0];

            if (tbDK != null && tbDK.Rows.Count > 0)
            {
                item.DieuKien = tbDK.Rows[0]["MOTA"].ToString();
                item.NoiDungDK = tbDK.Rows[0]["NOIDUNG"].ToString();
            }
            List<DinhDanhBaoLuuModel> ListDDBL = new List<DinhDanhBaoLuuModel>();
            string sql = @"select * from Identification order by CreateDate desc";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        if (tb.Rows[i]["Hidden"].ToString() == "True")
                        {
                            trangthai = "Ẩn";
                        }
                        else
                        {
                            if (tb.Rows[i]["Status"].ToString() == "True")
                            {
                                trangthai = "Đang bán";
                            }
                            else
                            {
                                trangthai = "Ngưng bán";
                            }
                        }

                        if (tb.Rows[i]["Reserve"].ToString() == "DD")
                        {
                            loai = "Định Danh";
                        }
                        else
                        {
                            loai = "EMDS";
                        }
                        DinhDanhBaoLuuModel result = new DinhDanhBaoLuuModel();
                        result.ID = tb.Rows[i]["ID"].ToString();
                        result.DienThoaiNB = tb.Rows[i]["Tel"].ToString();
                        result.GhiChu = tb.Rows[i]["Note"].ToString();
                        result.HanDangBan = tb.Rows[i]["DateEndSale"].ToString();
                        result.Hang = tb.Rows[i]["Airline"].ToString();
                        result.Loai = loai;
                        result.PNR = tb.Rows[i]["PNR"].ToString();
                        result.SoVe = tb.Rows[i]["TicketNumber"].ToString();
                        result.TenNB = tb.Rows[i]["Name"].ToString();
                        result.MaKH = tb.Rows[i]["MemberKH"].ToString();
                        result.TrangThai = trangthai;
                        result.NgayDang = DateTime.Parse(tb.Rows[i]["CreateDate"].ToString()).ToString("dd/MM/yyyy");
                        ListDDBL.Add(result);
                    }
                    item.ListDDBL = ListDDBL;
                }
            }
            return item;
        }
        public DinhDanhBaoLuuModel EditDDBL(string ID)
        {
            DinhDanhBaoLuuModel result = new DinhDanhBaoLuuModel();
            List<TenHanhKhach> List = new List<TenHanhKhach>();
            string sql_DDBL = @"select top 1 * from Identification where ID = '" + ID + "'";
            DataTable tb_DDBL = db.ExecuteDataSet(sql_DDBL, CommandType.Text, "server18", null).Tables[0];
            if (tb_DDBL != null)
            {
                if (tb_DDBL.Rows.Count > 0)
                {
                    result.DienThoaiNB = tb_DDBL.Rows[0]["Name"].ToString();
                    result.ID = tb_DDBL.Rows[0]["ID"].ToString();
                    result.DienThoaiNB = tb_DDBL.Rows[0]["Tel"].ToString();
                    result.GhiChu = tb_DDBL.Rows[0]["Note"].ToString();
                    result.HanDangBan = DateTime.Parse(tb_DDBL.Rows[0]["DateEndSale"].ToString()).ToString("dd/MM/yyyy");
                    result.Hang = tb_DDBL.Rows[0]["Airline"].ToString();
                    result.Loai = tb_DDBL.Rows[0]["Reserve"].ToString();
                    result.PNR = tb_DDBL.Rows[0]["PNR"].ToString();
                    result.SoVe = tb_DDBL.Rows[0]["TicketNumber"].ToString();
                    result.TenNB = tb_DDBL.Rows[0]["Name"].ToString();
                    result.TrangThai = tb_DDBL.Rows[0]["Status"].ToString();
                    result.Hidden = tb_DDBL.Rows[0]["Hidden"].ToString();
                    result.NgayDang = DateTime.Parse(tb_DDBL.Rows[0]["CreateDate"].ToString()).ToString("dd/MM/yyyy");

                    string sql = @"select * from IdentificationDetail where IdentificationID = '" + tb_DDBL.Rows[0]["ID"].ToString() + "'";
                    DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                    if (tb != null)
                    {
                        if (tb.Rows.Count > 0)
                        {
                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                TenHanhKhach item = new TenHanhKhach();
                                item.TenHK = tb.Rows[i]["Name"].ToString();
                                item.GiaBan = double.Parse(tb.Rows[i]["PriceRoot"].ToString());
                                item.GiaGiam = double.Parse(tb.Rows[i]["Price"].ToString());
                                List.Add(item);
                            }
                        }
                    }
                    result.ListTenHK = List;
                }
            }
            return result;
        }
        public List<TenHanhKhach> ViewDetailDDBL(string ID)
        {
            List<TenHanhKhach> List = new List<TenHanhKhach>();
            string sql = @"select * from IdentificationDetail where IdentificationID = '" + ID + "'";
            DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
            if (tb != null)
            {
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        TenHanhKhach result = new TenHanhKhach();
                        result.TenHK = tb.Rows[i]["Name"].ToString();
                        result.GiaBan = double.Parse(tb.Rows[i]["PriceRoot"].ToString());
                        result.GiaGiam = double.Parse(tb.Rows[i]["Price"].ToString());
                        List.Add(result);
                    }
                }
            }
            return List;
        }
        public string PhatSinhMaDDBL()
        {
            try
            {
                string year = DateTime.Now.Year.ToString().Substring(2, 2);
                string Maphieu = "";
                string sql = @"select top 1 ReservationCode from Identification where ReservationCode like N'%DD" + DateTime.Now.ToString("MM") + "" + year + "%' order by CreateDate Desc ";
                DataTable tb = db.ExecuteDataSet(sql, CommandType.Text, "server18", null).Tables[0];
                if (tb != null)
                {
                    if (tb.Rows.Count > 0)
                    {
                        string ten = tb.Rows[0]["ReservationCode"].ToString();
                        if (ten != "")
                        {
                            string soThuTu = ten.Substring(6, 4);
                            int STT = int.Parse(soThuTu) + 1;
                            if (STT > 0 && STT < 10)
                            {
                                Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "000" + STT;
                            }
                            if (STT >= 10 && STT < 100)
                            {
                                Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "00" + STT;
                            }
                            if (STT >= 100 && STT < 1000)
                            {
                                Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "0" + STT;
                            }
                            if (STT >= 1000)
                            {
                                Maphieu = "DD" + DateTime.Now.ToString("MM") + year + STT;
                            }
                        }
                        else
                        {
                            Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "0001";
                        }
                    }
                    else
                    {
                        Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "0001";
                    }
                }
                else
                {
                    Maphieu = "DD" + DateTime.Now.ToString("MM") + year + "0001";
                }
                return Maphieu;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
