using Dapper;
using Manager.Model.Models;
using Manager.Model.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RtfPipe.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
namespace Manager.DataAccess.Repository
{
    public class PostRepository
    {
        private readonly IConfiguration _configuration;
        private string SQL_Agent_MAIN;
        private string sqlGetAllPost = @"WITH NumberedSubjects AS (
                                            SELECT 
                                                ROW_NUMBER() OVER (ORDER BY subject.subject_id DESC, subject.subject_isnew DESC) AS RowNum,
                                                subject.subject_id,
                                                subject.subject_name,
                                                subject.subject_author,
                                                subject.subject_header, 
                                                subject.isMail, 
                                                subject.subject_isshow, 
                                                subject.subject_isnew, 
                                                subject.subject_ishot, 
                                                subject.subject_com, 
                                                subject.section_id, 
                                                subject.subject_seq, 
                                                CONVERT(VARCHAR(10), subject.subject_date, 103) + ' ' + CONVERT(VARCHAR(8), subject.subject_date, 108) AS subject_date,
                                                subject_section.section_name,
                                                (CASE WHEN ISNULL(subject.isMail, 0) = 0 THEN 'm1' ELSE 'm2' END) AS MailPNG, 
                                                (CASE WHEN ISNULL(subject.subject_ishot, 0) = 0 THEN '' ELSE 'hot-icon' END) AS hoticon
                                            FROM 
                                                subject
                                                INNER JOIN subject_section ON subject.section_id = subject_section.section_id
                                            WHERE 
                                                subject.section_id IN (
                                                    SELECT section_id 
                                                    FROM subject_section 
                                                    WHERE section_id <> '76' AND parent_id <> '76'

                                         ";
        public PostRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SQL_Agent_MAIN = _configuration.GetConnectionString("SQL_Agent_MAIN");
        }

        public (List<PostModel>, int) GetAllPost(int categoryId, int page, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("page", page); // Số hàng bỏ qua
            parameters.Add("pageSize", pageSize);
            string selectCondition = string.Empty;
            string countCondition = string.Empty;
            string sqlCountTotalRecord = @"SELECT COUNT(*) AS TotalRows
                                        FROM 
                                            subject
                                            INNER JOIN subject_section ON subject.section_id = subject_section.section_id
                                        WHERE 
                                            subject.section_id IN (
                                                SELECT section_id 
                                                FROM subject_section 
                                                WHERE section_id <> '76' AND parent_id <> '76'
                                            
                                         ";

            if (categoryId > 0)
            {
                selectCondition += " AND subject.section_id = @categoryId ";
                countCondition += " AND subject.section_id = @categoryId";
                parameters.Add("categoryId", categoryId);
            }

            sqlGetAllPost += selectCondition;
            sqlCountTotalRecord += countCondition; // Lấy câu sql lấy tổng số Order 
            sqlGetAllPost += @" )
                                )SELECT * 
                                FROM NumberedSubjects";
            sqlGetAllPost += @" WHERE RowNum BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize ";
            sqlCountTotalRecord += ")";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                List<PostModel> result = conn.Query<PostModel>(sqlGetAllPost, parameters, commandTimeout: 30).ToList();
                int totalRecord = conn.ExecuteScalar<int>(sqlCountTotalRecord, parameters, commandTimeout: 30);
                conn.Close();
                return (result, totalRecord);
            }
        }

        // Lấy danh sách trạng thái
        public IEnumerable<Section> GetAllCategory()
        {
            string sqlGetAllSection = @"SELECT 
                                        section_id,section_name,CASE WHEN parent_id=0 then null else parent_id end as parent_id 
                                    FROM subject_section 
                                    WHERE  section_isshow=1 AND position_id=4  ORDER BY parent_id,section_id asc";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                return conn.Query<Section>(sqlGetAllSection, commandTimeout: 30);
            }
        }

        public Section GetCategoryByID(int sectionId)
        {
            Section section = null;
            string sqlGetSection = @"SELECT 
                                        section_id,section_name,CASE WHEN parent_id=0 then null else parent_id end as parent_id 
                                    FROM subject_section 
                                    WHERE  section_isshow=1 AND position_id=4 AND section_id = @sectionId
                                    ORDER BY parent_id,section_id asc";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                section = conn.Query<Section>(sqlGetSection,new {sectionId = sectionId}, commandTimeout: 30).FirstOrDefault();
                conn.Close();
            }
            return section;
        }

        public bool DeletePost(int subjectId)
        {
            bool isSuccess = false;
            string sql = @"DELETE FROM [subject] where subject_id = @subjectId";
            int result = 0;
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                result = conn.Execute(sql, new { subjectId = subjectId }, commandTimeout: 30);
                conn.Close();
                if (result > 0) { 
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public bool SendMail(PostModel request)
        {
            bool isSuccess = false;
            string programId = "EVM_CONGVAN";
            EVMailRepository evMailRepository = new EVMailRepository();
            EVEmail evEmail = evMailRepository.GetEVEMailContentByProgram(programId);
            if (evEmail != null)
            {
                string contentEmail = "";
                var webRequest = WebRequest.Create(evEmail.templateUrl);
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                { contentEmail = reader.ReadToEnd(); }
                contentEmail = contentEmail.Replace("$_NoiDung", request.subject_content);

                bool isCC = true;
                bool isBCC = true;
                if (!string.IsNullOrEmpty(request.subject_author) && request.subject_author.Trim() == "nguyenanhthanhhcm")
                {
                    isCC = false;
                    isBCC = false;
                }
                isSuccess = Manager.Common.Helpers.Common.SendMail("ENVIET GROUP", request.subject_name, contentEmail, evEmail.MAIL, evEmail.username, evEmail.password, evEmail.hostName, evEmail.port, evEmail.useSSL, evEmail.CC, evEmail.BCC, isCC, isBCC);
            }
            return isSuccess;
        }

        public bool UpdateSendMail(int subjectId)
        {
            bool isSuccess = false;
            int result = 0;
            string sql = @"UPDATE [subject]
                            SET IsMail = 1
                            WHERE subject_id = @subjectId
                            ";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                result = conn.Execute(sql, new { subjectId = subjectId}, commandTimeout: 30);
                if (result > 0)
                {
                    isSuccess = true;
                }
                conn.Close();
            }

            return isSuccess;
        }


        public PostModel GetPost(int subjectId)
        {
            PostModel postModel = null;
            string sql = @"SELECT * FROM subject 
                            WHERE subject_id = @subjectId ";

            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                postModel = conn.Query<PostModel>(sql, new { subjectId = subjectId }, commandTimeout: 30).FirstOrDefault();
                conn.Close();
            }
            return postModel;
        }

        public bool ToggleShowItem(int subjectId, int isShowValue)
        {
            bool isSuccess = false;
            int result = 0;
            string sql = @"UPDATE [subject]
                            SET subject_isshow = @isShowValue
                            WHERE subject_id = @subjectId
                            ";
            using (var conn = new SqlConnection(SQL_Agent_MAIN))
            {
                conn.Open();
                result = conn.Execute(sql, new { subjectId = subjectId, isShowValue = isShowValue }, commandTimeout: 30);
                if(result > 0)
                {
                    isSuccess = true;
                }
                conn.Close();
            }

            return isSuccess;
        }
    }
}
