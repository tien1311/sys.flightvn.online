using Dapper;
using Manager.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

//using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TangDuLieu;

namespace Manager.DataAccess.Repository
{
    public class PostsAdsRepository
    {
        string SQL_POSTS; /*= "Data Source=27.71.232.40,1453;Initial Catalog=POSTS;User ID=sa;Password=EnViet@123;";*/
        string SQL_Agent_MAIN; /*= "Data Source=27.71.232.40,1453;Initial Catalog=Agent;User ID=sa;Password=Dv2UAxZz8LKNgqSX5r4TBm;";*/

        public PostsAdsRepository(IConfiguration configuration)
        {
            SQL_POSTS = configuration.GetConnectionString("SQL_POST");
            SQL_Agent_MAIN = configuration.GetConnectionString("SQL_Agent_MAIN");

        }
        public List<PostsAdsModel> PostsAds()
        {
            List<PostsAdsModel> result = new List<PostsAdsModel>();
            string sql = @"select Posts.*, Categories.CategoryName from Posts left join Categories on Categories.ID = Posts.CategoryID ";
            using (var conn = new SqlConnection(SQL_POSTS))
            {
                result = (List<PostsAdsModel>)conn.Query<PostsAdsModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public PostsAdsModel CreatePostsAds()
        {
            PostsAdsModel result = new PostsAdsModel();
            result.List_CategoriesPostsAds = ListCategories();
            return result;
        }
        public PostsAdsModel EditPostsAds(int ID)
        {
            PostsAdsModel result = new PostsAdsModel();
            string sql = "select * from Posts where ID = '" + ID + "'";
            using (var conn = new SqlConnection(SQL_POSTS))
            {
                result = conn.QueryFirst<PostsAdsModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result != null)
            {
                result.List_CategoriesPostsAds = ListCategories();
            }
            return result;
        }
        public bool SaveCreatePostsAds(int Category, string Title, string CreateContent, string MaNV)
        {
            int i = 0;
            string sql = "INSERT INTO [Posts] ([Title] ,[Description],[CreatedDate],[CreatedBy],[CategoryID]) VALUES ( N'" + Title + "',N'" + CreateContent + "',GETDATE(),N'" + MaNV + "'," + Category + ")";
            using (var conn = new SqlConnection(SQL_POSTS))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool SaveEditPostsAds(string ID, string Title, string CreateContent, int Category)
        {
            int i = 0;
            string sql = "UPDATE Posts SET Title = N'" + Title + "', Description = N'" + CreateContent + "', CategoryID= " + Category + " WHERE ID = " + ID;
            using (var conn = new SqlConnection(SQL_POSTS))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public List<CategoriesPostsAds> ListCategories()
        {
            List<CategoriesPostsAds> result = new List<CategoriesPostsAds>();
            try
            {
                string sql = "select * from Categories";
                using (var conn = new SqlConnection(SQL_POSTS))
                {
                    result = (List<CategoriesPostsAds>)conn.Query<CategoriesPostsAds>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Dispose();
                }
                return result;
            }
            catch (System.Exception)
            {
                return result;
            }

        }
        public Upload_ImgModel BannerAdsDuLich()
        {
            Upload_ImgModel result = new Upload_ImgModel();
            Detail_Img img = new Detail_Img();
            string sql = @"SELECT top 1 * FROM [Adv] WHERE adv_id = 273";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                img = conn.QueryFirst<Detail_Img>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.bannerADSDuLich = img;
            return result;
        }
        public bool SaveImgBannerAdsDuLich(string url, string status, string link)
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
            string sql = "UPDATE Adv SET adv_picture = '" + url + "', adv_date = GETDATE(),adv_linkto = '" + link + "',adv_isshow = '" + isshow + "' WHERE adv_id = 273";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                i = conn.Execute(sql, null, null, commandTimeout: 30, commandType: CommandType.Text);
                conn.Dispose();
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
