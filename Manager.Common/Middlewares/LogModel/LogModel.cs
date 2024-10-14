using Manager.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Common.Middlewares.LogModel
{
    public class LogModel
    {
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// Thời gian khi gửi Request đến server dd/MM/yyyy HH:mm:ss.ttt
        /// </summary>
        string RequestTime { get; set; }
        /// <summary>
        /// Request Header dạng json
        /// </summary>
        string RequestHeader { get; set; }
        /// <summary>
        /// Param query của request
        /// </summary>
        string RequestParams { get; set; }
        /// <summary>
        /// Path request
        /// </summary>
        string RequestPath { get; set; }
        /// <summary>
        /// IP của client khi gọi request
        /// </summary>
        string RequestIP { get; set; }
        /// <summary>
        /// Method: Get/post/put...
        /// </summary>
        string Method { get; set; }
        /// <summary>
        /// Body khi post Request
        /// </summary>
        string RequestBody { get; set; }
        /// <summary>
        /// Log session dùng để authenticate
        /// </summary>
        string SessionLog { get; set; }
        /// <summary>
        /// Custom log, log ghi chú thêm khi gọi API
        /// </summary>
        string CustomLog { get; set; }
        /// <summary>
        /// Thời gian khi server trả về response
        /// </summary>
        string ResponseTime { get; set; }
        /// <summary>
        /// Khoản thời gian kể từ khi request cho đến lúc nhận response
        /// </summary>
        string ExecutionTime { get; set; }
        /// <summary>
        /// Response Header dạng json
        /// </summary>
        string ResponseHeader { get; set; }
        /// <summary>
        /// Body khi nhận về
        /// </summary>
        string ResponseBody { get; set; }

        /// <summary>
        /// Gửi log đến server dạng async
        /// </summary>
        /// <returns></returns>
        public static async Task LogUser(object log)
        {
            await Task.Run(() =>
            {
                string jsonString = JsonConvert.SerializeObject(log);
                var content = new StringContent(jsonString);
                string checkSum = Helper.GenerateCheckSum(jsonString);
                client.DefaultRequestHeaders.Add("CheckSum", checkSum);
                client.PostAsync("http://log.enviet-group.com/v1/LogUser", content);

            });
        }

        public static async Task LogServer(object log)
        {
            await Task.Run(() =>
            {
                string jsonString = JsonConvert.SerializeObject(log);
                var content = new StringContent(jsonString);
                string checkSum = Helper.GenerateCheckSum(jsonString);
                client.DefaultRequestHeaders.Add("CheckSum", checkSum);
                client.PostAsync("http://log.enviet-group.com/v1/LogServer", content);

            });
        }

        public LogModel(string reqTime, string reqBody, string resBody, string resTime, string excTime, HttpContext context)
        {
            RequestTime = reqTime;
            RequestBody = reqBody;
            ResponseBody = resBody;
            ResponseTime = resTime;
            ExecutionTime = excTime;
            //SessionLog = Helper.GetSessionLog();
            //CustomLog = Helper.GetCustomLog();
            //Method = Helper.GetMethod();
            //RequestParams = Helper.GetParams();
            //RequestIP = Helper.GetIP();
            //RequestPath = Helper.GetPath();
            //RequestHeader = Helper.GetRequestHeaders();
            //ResponseHeader = Helper.GetResponseHeaders();



            Method = context.Request.Method;
            RequestParams = context.Request.QueryString.ToString();
            RequestIP = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            RequestPath = context.Request.Path;
            RequestHeader = context.Request.QueryString.ToString();
            ResponseHeader = JsonConvert.SerializeObject(context.Response.Headers);
            //SessionLog = context.Items["SessionLog"].ToString();
            //CustomLog = context.Items["CustomLog"].ToString();
        }
    }
}
