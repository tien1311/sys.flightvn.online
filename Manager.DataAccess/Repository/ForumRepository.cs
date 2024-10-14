using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;

namespace Manager.DataAccess.Repository
{
    public class ForumRepository
    {
        private string SQL_FORUM; /*"Data Source=.;Initial Catalog=FORUM;User ID=sa;Password=EnViet@123;";*/
        public ForumRepository(IConfiguration configuration)
        {
            SQL_FORUM = configuration.GetConnectionString("SQL_FORUM");
        }
        public List<PostForumModel> DuyetBaiViet()
        {
            List<PostForumModel> result = new List<PostForumModel>();
            string sql = @"select Posts.Id,Title, Posts.CreatedOn,Users.UserName as UserCreated,Posts.IsDeleted,IsActive from Posts 
                            left join AspNetUsers Users on Users.Id = Posts.AuthorId order by CreatedOn desc";
            using (var conn = new SqlConnection(SQL_FORUM))
            {
                result = (List<PostForumModel>)conn.Query<PostForumModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return result;
        }
        public PostForumModel ViewPosts(int ID)
        {
            PostForumModel result = new PostForumModel();
            List<TagModel> ListTag = new List<TagModel>();
            string sql = @"select Posts.Id,Title,Description,Cate.Name as Category, IsActive, Posts.IsDeleted, Note from Posts left join Categories Cate on Cate.Id = Posts.CategoryId where Posts.Id = '" + ID + "' ";
            using (var conn = new SqlConnection(SQL_FORUM))
            {
                result = conn.QueryFirst<PostForumModel>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (result.IsDeleted == true)
            {
                result.Status = "0";
            }
            if (result.IsDeleted == false && result.IsActive == "1")
            {
                result.Status = "1";
            }
            if (result.IsDeleted == false && result.IsActive == "0")
            {
                result.Status = "2";
            }
            if (result.IsDeleted == false && result.IsActive == null)
            {
                result.Status = "3";
            }
            string sqlTag = @"select Name from PostsTags
                                left join Tags on Tags.Id = PostsTags.TagId
                                where PostsTags.PostId = '" + ID + "' ";
            using (var conn = new SqlConnection(SQL_FORUM))
            {
                ListTag = (List<TagModel>)conn.Query<TagModel>(sqlTag, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            result.ListTags = ListTag;
            return result;
        }
        public bool ConfirmPost(int ID, string Status, string Note)
        {
            string sql = "";
            int i = 0;
            if (Status == "0")
            {
                sql = "UPDATE Posts SET IsDeleted = 1, Note = N'" + Note + "' WHERE ID = '" + ID + "' ";
            }
            if (Status == "1")
            {
                sql = "UPDATE Posts SET IsActive = 1, IsDeleted = 0, Note = N'" + Note + "' WHERE ID = '" + ID + "' ";
            }
            if (Status == "2")
            {
                sql = "UPDATE Posts SET IsActive = 0, IsDeleted = 0, Note = N'" + Note + "' WHERE ID = '" + ID + "' ";
            }
            if (Status == "3")
            {
                sql = "UPDATE Posts SET IsActive = NULL, IsDeleted = 0, Note = N'" + Note + "' WHERE ID = '" + ID + "' ";
            }
            using (var conn = new SqlConnection(SQL_FORUM))
            {
                i = conn.Execute(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        public Task<int> SoLuongBaiVietAsync()
        {
            int i = 0;
            string sql = @"select count (id) from Posts where IsDeleted = 0 and IsActive is null ";
            using (var conn = new SqlConnection(SQL_FORUM))
            {
                i = conn.QueryFirst<int>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                conn.Dispose();
            }
            return Task.FromResult(i);
        }
    }
}
