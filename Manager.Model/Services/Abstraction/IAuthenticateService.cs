using System.Threading.Tasks;
using Manager.Model.Services.Model.Request;
using Manager.Model.Services.Model.Response;

namespace Manager.Model.Services.Abstraction
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}
