using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Web;
//using System.Web.Mvc;
//using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;
//using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
//using Manager_Manager.Models.Repository;
using System.Security.Principal;
using Manager.Model.Models;


namespace Manager.Model.Models.Other
{
    public static class AccountManager
    {
        //Thấy thông tin account đang đăng nhập
        public static AccountModel GetAccountCurrent(HttpContext context)
        {

            AccountModel acc = new AccountModel();

            if (context.User.Claims.Count() > 0)
            {
                acc.RowID = int.Parse(context.User.Claims.First(claim => claim.Type == "RowID").Value);
                acc.MaNV = context.User.Claims.First(claim => claim.Type == "MaNV").Value;
                acc.Ten = context.User.Claims.First(claim => claim.Type == "Ten").Value;
                acc.HoTen = context.User.Claims.First(claim => claim.Type == "HoTen").Value;
                acc.ChiNhanh = context.User.Claims.First(claim => claim.Type == "ChiNhanh").Value;
                acc.DiaChiThuongTru = context.User.Claims.First(claim => claim.Type == "DiaChi").Value;
                acc.ChiNhanh = context.User.Claims.First(claim => claim.Type == "ChiNhanh").Value;
                acc.PhongBan = context.User.Claims.First(claim => claim.Type == "PhongBan").Value;
                acc.MaPhongBan = context.User.Claims.First(claim => claim.Type == "MaPhongBan").Value;
                acc.DienThoai = context.User.Claims.First(claim => claim.Type == "DienThoai").Value;
                acc.Email = context.User.Claims.First(claim => claim.Type == "Email").Value;
                acc.Per_Group = context.User.Claims.First(claim => claim.Type == "Per_Group").Value;
                acc.TenDangNhap = context.User.Claims.First(claim => claim.Type == "TenDangNhap").Value;
                acc.TenHinh = context.User.Claims.First(claim => claim.Type == "TenHinh").Value;
                acc.Active = context.User.Claims.First(claim => claim.Type == "Active").Value;
                //phan quyen phong ban
                acc.TNMoi = context.User.Claims.First(claim => claim.Type == "TNMoi").Value;
                acc.TBao = context.User.Claims.First(claim => claim.Type == "TBao").Value;
                acc.BCVe = context.User.Claims.First(claim => claim.Type == "BCVe").Value;
                acc.NBo = context.User.Claims.First(claim => claim.Type == "NBo").Value;
                acc.DLi = context.User.Claims.First(claim => claim.Type == "DLi").Value;
                acc.KToan = context.User.Claims.First(claim => claim.Type == "KToan").Value;
                acc.KDoanh = context.User.Claims.First(claim => claim.Type == "KDoanh").Value;
                acc.PVe = context.User.Claims.First(claim => claim.Type == "PVe").Value;
                acc.BPDoan = context.User.Claims.First(claim => claim.Type == "BPDoan").Value;
                acc.HDon = context.User.Claims.First(claim => claim.Type == "HDon").Value;
                acc.CA = context.User.Claims.First(claim => claim.Type == "CA").Value;
                acc.YSao = context.User.Claims.First(claim => claim.Type == "YSao").Value;
                acc.CS = context.User.Claims.First(claim => claim.Type == "CS").Value;
                acc.DTa = context.User.Claims.First(claim => claim.Type == "DTa").Value;
                acc.STing = context.User.Claims.First(claim => claim.Type == "STing").Value;
                acc.KThuat = context.User.Claims.First(claim => claim.Type == "KThuat").Value;
                acc.Dulich = context.User.Claims.First(claim => claim.Type == "Dulich").Value;
                //phan quyen member
                acc.TNMoiTV = context.User.Claims.First(claim => claim.Type == "TNMoiTV").Value;
                acc.TBaoTV = context.User.Claims.First(claim => claim.Type == "TBaoTV").Value;
                acc.BCVeTV = context.User.Claims.First(claim => claim.Type == "BCVeTV").Value;
                acc.NBoTV = context.User.Claims.First(claim => claim.Type == "NBoTV").Value;
                acc.DLiTV = context.User.Claims.First(claim => claim.Type == "DLiTV").Value;
                acc.KToanTV = context.User.Claims.First(claim => claim.Type == "KToanTV").Value;
                acc.KDoanhTV = context.User.Claims.First(claim => claim.Type == "KDoanhTV").Value;
                acc.PVeTV = context.User.Claims.First(claim => claim.Type == "PVeTV").Value;
                acc.BPDoanTV = context.User.Claims.First(claim => claim.Type == "BPDoanTV").Value;
                acc.HDonTV = context.User.Claims.First(claim => claim.Type == "HDonTV").Value;
                acc.CATV = context.User.Claims.First(claim => claim.Type == "CATV").Value;
                acc.YSaoTV = context.User.Claims.First(claim => claim.Type == "YSaoTV").Value;
                acc.CSTV = context.User.Claims.First(claim => claim.Type == "CSTV").Value;
                acc.DTaTV = context.User.Claims.First(claim => claim.Type == "DTaTV").Value;
                acc.STingTV = context.User.Claims.First(claim => claim.Type == "STingTV").Value;
                acc.KThuatTV = context.User.Claims.First(claim => claim.Type == "KThuatTV").Value;
                acc.DulichTV = context.User.Claims.First(claim => claim.Type == "DulichTV").Value;
            }

            return acc;
        }

    }


}






