
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Manager.Model.Services.Model.Response.AirportResponse;
using Manager.Model.Services.Model.Request;

namespace Manager.Model.Services.Abstraction
{
    public interface IAirportService
    {
        Task<List<Profile>> SearchID(string ID, string Server);
        Task<List<AirportCodeRequest>> Search(string AirportCode, string Server);
        Task<AirportCodeResponse_Insert> Insert(AirportCodeRequest request);
        Task<AirportCodeResponse_Update> Update(AirportCodeRequest request);
        Task<AirportCodeResponse_Delete> Delete(string request);

    }
}
