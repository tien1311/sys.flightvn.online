using Dapper;
using Manager.Model.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Manager.DataAccess.Repository
{
    public class EVMailRepository
    {
        public IConfiguration _configuration { get; set; }
        public string _connectionString { get; set; }
        public EVMailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SQL_EV_MAIN");
        }

        public EVMailRepository()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
            _connectionString = _configuration.GetConnectionString("SQL_EV_MAIN");
        }

        public EVEmail GetEVEMailContentByProgram(string program)
        {
            var ev_Email = new EVEmail();
            string sql_MailSetup = "select * from Mail_SETUP " +
                                "WHERE Program = @program  ";

            string sql_EMAIL_NOTIFI = "select * from EMAIL_NOTIFI " +
                                "WHERE ProgramID = @program  ";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var mailSetup = conn.QueryFirstOrDefault<EVEmail>(sql_MailSetup, new { program }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                var emailNotifi = conn.QueryFirstOrDefault<EVEmail>(sql_EMAIL_NOTIFI, new { program }, commandType: System.Data.CommandType.Text, commandTimeout: 30);
                conn.Close();

                if (mailSetup != null)
                {
                    ev_Email.Program = mailSetup.Program;
                    ev_Email.hostName = mailSetup.hostName;
                    ev_Email.port = mailSetup.port;
                    ev_Email.username = mailSetup.username;
                    ev_Email.password = mailSetup.password;
                    ev_Email.useSSL = mailSetup.useSSL;
                    ev_Email.templateUrl = mailSetup.templateUrl;
                }

                if (emailNotifi != null)
                {
                    ev_Email.MAIL = emailNotifi.MAIL;
                    ev_Email.CC = emailNotifi.CC;
                    ev_Email.BCC = emailNotifi.BCC;
                }
            }
            return ev_Email;
        }
    }
}
