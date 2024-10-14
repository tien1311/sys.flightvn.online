using Manager.Model.Services.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manager.Model.Services.Model.Response
{
    public class AirportResponse
    {
        public class AirportCodeResponse_Search
        {
            public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
            public string Message { get; set; } = "Success";
            public List<AirportCodeRequest> Result { get; set; } = null;
        }
        public class AirportCodeResponse_Insert
        {
            public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
            public string Message { get; set; } = "Success";
            public string Result { get; set; } = null;
        }
        public class AirportCodeResponse_Update
        {
            public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
            public string Message { get; set; } = "Success";
            public string Result { get; set; } = null;
        }
        public class AirportCodeResponse_Delete
        {
            public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
            public string Message { get; set; } = "Success";
            public string Result { get; set; } = null;
        }
    }
}
