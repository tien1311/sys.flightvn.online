using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model.Services.Notification.Response
{
    public class NotifyLisaResponse
    {
        public bool Result { get; set; }
        public string[] Messages { get; set; }
    }
}
