using Manager.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class PopupRepository
    {
        DBase db = new DBase();
        public Task<string> LoadPopup()
        {
            string popup = "";
            try
            {
                string Sql = " SELECT top 1 * FROM [Adv] WHERE category_id = 9 and adv_isshow = 1";
                DataTable dt = db.ExecuteDataSet(Sql, CommandType.Text, "server18", null).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["adv_linkto"].ToString() == "")
                    {
                        popup = "<a href='#'><img class='ResizeImage' align=center src='" + dt.Rows[0]["adv_picture"].ToString() + "' border='0' /> </a>";
                    }
                    else
                    {
                        popup = "<a href='" + dt.Rows[0]["adv_linkto"].ToString() + "' target='_blank'><img class='ResizeImage' align=center src='" + dt.Rows[0]["adv_picture"].ToString() + "' border='0' /> </a>";
                    }

                }
                return Task.FromResult(popup);
            }
            catch (Exception)
            {
                return Task.FromResult(popup);
            }
        }
        public Upload_ImgModel UploadPopup()
        {
            Upload_ImgModel result = new Upload_ImgModel();
            Detail_Img Pop_Sys = new Detail_Img();
            Detail_Img Banner_Air = new Detail_Img();
            Detail_Img banner_LGEvbay = new Detail_Img();
            Detail_Img banner_ADVEvbay = new Detail_Img();
            Detail_Img banner_ADVVeDoanEvbay = new Detail_Img();
            Detail_Img banner_ADSEnvietGroup = new Detail_Img();
            try
            {
                string SqlPop_Sys = "SELECT top 1 * FROM [Adv] WHERE category_id = 9";
                DataTable dtPop_Sys = db.ExecuteDataSet(SqlPop_Sys, CommandType.Text, "server18", null).Tables[0];
                if (dtPop_Sys.Rows.Count > 0)
                {
                    Pop_Sys.adv_isshow = dtPop_Sys.Rows[0]["adv_isshow"].ToString();
                    Pop_Sys.adv_linkto = dtPop_Sys.Rows[0]["adv_linkto"].ToString();
                    Pop_Sys.adv_picture = dtPop_Sys.Rows[0]["adv_picture"].ToString();
                }

                string SqlBanner_Air = "SELECT top 1 * FROM [Adv] WHERE category_id = 10";
                DataTable dtBanner_Air = db.ExecuteDataSet(SqlBanner_Air, CommandType.Text, "server18", null).Tables[0];
                if (dtBanner_Air.Rows.Count > 0)
                {
                    Banner_Air.adv_isshow = dtBanner_Air.Rows[0]["adv_isshow"].ToString();
                    Banner_Air.adv_linkto = dtBanner_Air.Rows[0]["adv_linkto"].ToString();
                    Banner_Air.adv_picture = dtBanner_Air.Rows[0]["adv_picture"].ToString();
                }
                string SqlBanner_LGEvbay = "SELECT top 1 * FROM [Adv] WHERE category_id = 6";
                DataTable dtBanner_LGEvbay = db.ExecuteDataSet(SqlBanner_LGEvbay, CommandType.Text, "server18", null).Tables[0];
                if (dtBanner_LGEvbay.Rows.Count > 0)
                {
                    banner_LGEvbay.adv_isshow = dtBanner_LGEvbay.Rows[0]["adv_isshow"].ToString();
                    banner_LGEvbay.adv_linkto = dtBanner_LGEvbay.Rows[0]["adv_linkto"].ToString();
                    banner_LGEvbay.adv_picture = dtBanner_LGEvbay.Rows[0]["adv_picture"].ToString();
                }
                string Sqlbanner_ADVEvbay = "SELECT top 1 * FROM [Adv] WHERE category_id = 11";
                DataTable dtbanner_ADVEvbay = db.ExecuteDataSet(Sqlbanner_ADVEvbay, CommandType.Text, "server18", null).Tables[0];
                if (dtBanner_LGEvbay.Rows.Count > 0)
                {
                    banner_ADVEvbay.adv_isshow = dtbanner_ADVEvbay.Rows[0]["adv_isshow"].ToString();
                    banner_ADVEvbay.adv_linkto = dtbanner_ADVEvbay.Rows[0]["adv_linkto"].ToString();
                    banner_ADVEvbay.adv_picture = dtbanner_ADVEvbay.Rows[0]["adv_picture"].ToString();
                }
                string Sqlbanner_ADVVeDoanEvbay = "SELECT top 1 * FROM [Adv] WHERE category_id = 12";
                DataTable dtbanner_ADVVeDoanEvbay = db.ExecuteDataSet(Sqlbanner_ADVVeDoanEvbay, CommandType.Text, "server18", null).Tables[0];
                if (dtbanner_ADVVeDoanEvbay.Rows.Count > 0)
                {
                    banner_ADVVeDoanEvbay.adv_isshow = dtbanner_ADVVeDoanEvbay.Rows[0]["adv_isshow"].ToString();
                    banner_ADVVeDoanEvbay.adv_linkto = dtbanner_ADVVeDoanEvbay.Rows[0]["adv_linkto"].ToString();
                    banner_ADVVeDoanEvbay.adv_picture = dtbanner_ADVVeDoanEvbay.Rows[0]["adv_picture"].ToString();
                }
                string Sqlbanner_ADSBannerEnvietGroup = "SELECT top 1 * FROM [Adv] WHERE category_id = 13";
                DataTable dtbanner_ADSBannerEnvietGroup = db.ExecuteDataSet(Sqlbanner_ADSBannerEnvietGroup, CommandType.Text, "server18", null).Tables[0];
                if (dtbanner_ADVVeDoanEvbay.Rows.Count > 0)
                {
                    banner_ADSEnvietGroup.adv_isshow = dtbanner_ADSBannerEnvietGroup.Rows[0]["adv_isshow"].ToString();
                    banner_ADSEnvietGroup.adv_linkto = dtbanner_ADSBannerEnvietGroup.Rows[0]["adv_linkto"].ToString();
                    banner_ADSEnvietGroup.adv_picture = dtbanner_ADSBannerEnvietGroup.Rows[0]["adv_picture"].ToString();
                }
                result.bannerADSEnvietGroup = banner_ADSEnvietGroup;
                result.bannerADVVeDoanEvbay = banner_ADVVeDoanEvbay;
                result.bannerADVEvbay = banner_ADVEvbay;
                result.bannerloginEvbay = banner_LGEvbay;
                result.bannerAirline_Img = Banner_Air;
                result.popupSys_Img = Pop_Sys;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveImgPopup(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {
                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 9";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            //string sql = "INSERT INTO Adv (adv_picture,adv_date,adv_linkto, category_id) VALUES(@adv_picture, GETDATE(), @adv_linkto, 9)";

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveImgBannerAirline(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {
                //string sql = "INSERT INTO Adv (adv_picture,adv_date,adv_linkto, category_id) VALUES(@adv_picture, GETDATE(), @adv_linkto, 10)";
                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 10";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveImgBannerEvbay(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {

                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 6";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveImgBannerADVEvbay(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {

                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 11";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveImgBannerADVVeDoanEvbay(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {

                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 12";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveImgBannerADSEnvietGroup(string url, string Link, string status)
        {
            string isshow = "";
            int i = 0;
            if (status == null)
            {
                isshow = "1";
            }
            else
            {
                isshow = "0";
            }
            if (url != null)
            {

                string sql = "UPDATE Adv SET adv_picture = @adv_picture, adv_date = GETDATE(),adv_linkto = @adv_linkto,adv_isshow = @adv_isshow WHERE category_id = 13";
                List<DBase.AddParameters> Param = new List<DBase.AddParameters>();
                Param.Add(new DBase.AddParameters("@adv_picture", url));
                Param.Add(new DBase.AddParameters("@adv_linkto", Link));
                Param.Add(new DBase.AddParameters("@adv_isshow", isshow));
                i = db.ExecuteNoneQuery(sql, CommandType.Text, "server18", Param);
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
