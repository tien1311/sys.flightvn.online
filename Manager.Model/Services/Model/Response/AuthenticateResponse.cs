using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Manager.Model.Services.Model.Response
{
    public class AuthenticateResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = "Success";
        public Authenticate Result { get; set; } = null;
    }
    public class Authenticate
    {
        public string Message { get; set; }
        public string CompanyName { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }

}
