using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Headers;
using static Manager.Model.Services.Model.Response.AirportResponse;
using System.Data.SqlClient;
using Dapper;
using Manager.Model.Services.Model.Request;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services.Model.Response;
using Newtonsoft.Json;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
//using System.Web.WebPages;

namespace Manager.Model.Services
{
    public class AirportService : IAirportService
    {
        public IHttpClientFactory _httpClientFactory;
        public IAuthenticateService _authenticateService;
        public AirportService(IHttpClientFactory httpClientFactory, IAuthenticateService authenticateService)
        {
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
        }

        public async Task<AirportCodeResponse_Insert> Insert(AirportCodeRequest request)
        {
            AuthenticateRequest authenticate = new AuthenticateRequest();
            AuthenticateResponse authenticate_response = await _authenticateService.Authenticate(authenticate);
            var httpClient = _httpClientFactory.CreateClient("enviet-service");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticate_response.Result.Token);
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Airport/Insert", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AirportCodeResponse_Insert>(responseContent);
        }
        public async Task<AirportCodeResponse_Update> Update(AirportCodeRequest request)
        {
            AuthenticateRequest authenticate = new AuthenticateRequest();
            AuthenticateResponse authenticate_response = await _authenticateService.Authenticate(authenticate);
            var httpClient = _httpClientFactory.CreateClient("enviet-service");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticate_response.Result.Token);
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Airport/Update", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AirportCodeResponse_Update>(responseContent);
        }
        public async Task<AirportCodeResponse_Delete> Delete(string request)
        {
            AuthenticateRequest authenticate = new AuthenticateRequest();
            AuthenticateResponse authenticate_response = await _authenticateService.Authenticate(authenticate);

            var httpClient = _httpClientFactory.CreateClient("enviet-service");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticate_response.Result.Token);
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.DeleteAsync("Airport/Delete/" + request);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AirportCodeResponse_Delete>(responseContent);
        }
        public async Task<List<AirportCodeRequest>> Search(string AirportCode, string Server)
        {
            List<AirportCodeRequest> result = new List<AirportCodeRequest>();
            try
            {
                string where = "";
                if (AirportCode != "")
                {
                    where = " and AirportCode like N'" + AirportCode + "'";
                }
                string sql = "select * from AIRPORT where 1=1" + where;
                using (var conn = new SqlConnection(Server))
                {
                    result = (List<AirportCodeRequest>)await conn.QueryAsync<AirportCodeRequest>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
        public async Task<List<Profile>> SearchID(string ID, string Server)
        {
            List<Profile> result = new List<Profile>();
            try
            {
                string where = " and AirportID = " + ID;
                string sql = "select * from AIRPORT_PROFILE where 1=1" + where;
                using (var conn = new SqlConnection(Server))
                {
                    result = (List<Profile>)await conn.QueryAsync<Profile>(sql, null, commandType: CommandType.Text, commandTimeout: 30);
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
