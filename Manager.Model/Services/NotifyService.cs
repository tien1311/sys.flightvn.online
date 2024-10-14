using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Manager.Model.Services.Notification.Request;
using Manager.Model.Services.Model.Request;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services.Model.Response;
using Newtonsoft.Json;

namespace Manager.Model.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticateService _authenticateService;

        //NotifyLisaRequest request = new NotifyLisaRequest("TOPUP", "100,000", "NOTIFICATION", "NV00006", "", "");
        //await _notifyService.SendNotify(request);

        public NotifyService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IAuthenticateService authenticateService)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
        }

        public async Task<bool> SendNotify<T>(T request) where T : NotifyLisaRequest
        {
            var httpClient = _httpClientFactory.CreateClient("enviet-service");
            //Chứng thực
            AuthenticateRequest authenticate = new AuthenticateRequest();
            AuthenticateResponse authenticate_response = await _authenticateService.Authenticate(authenticate);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticate_response.Result.Token);
            //Notify
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/v1/Lisa/SendNotify", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            JObject result = JObject.Parse(responseContent);
            var message = result.GetValue("message").ToString();
            var statusCode = result.GetValue("status").ToString();
            if (result != null)
            {
                if (statusCode == "200" && message == "Success")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
