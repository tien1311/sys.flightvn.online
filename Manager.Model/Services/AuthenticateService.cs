using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Manager.Model.Services.Model.Request;
using Manager.Model.Services.Abstraction;
using Manager.Model.Services.Model.Response;
using Newtonsoft.Json;

namespace Manager.Model.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        public IHttpClientFactory _httpClientFactory;
        public AuthenticateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("enviet-service");
            string jsonContent = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("daily/Account/Authenticate", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
        }
    }
}
