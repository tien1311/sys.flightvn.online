using System.Threading.Tasks;
using Manager.Model.Services.Notification.Request;

namespace Manager.Model.Services.Abstraction
{
    public interface INotifyService
    {
        Task<bool> SendNotify<T>(T request) where T : NotifyLisaRequest;
    }
}
