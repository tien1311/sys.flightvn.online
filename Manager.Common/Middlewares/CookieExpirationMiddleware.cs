using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Manager.Common.Middlewares
{
    public class CookieExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private int _NumberCount = 0;

        public CookieExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //if (!authResult.Succeeded || authResult?.Principal == null)
            //{
            //    await _next(context);
            //    return;
            //}

            // Kiểm tra tính hợp lệ của cookie
            string s = context.Request.Path.ToString();
            var expirationDate = authResult.Properties?.ExpiresUtc;
          
            if (!context.Request.Path.ToString().ToUpper().Contains("LOGIN") && !context.Request.Path.ToString().ToUpper().Contains("UPLOADIMG") && !context.Request.Path.ToString().ToUpper().Contains("IMAGES") && context.Request.Path.ToString() != "/")
            {
                if (expirationDate != null)
                {
                    _NumberCount = 1;
                    if (expirationDate.HasValue && expirationDate.Value < DateTime.UtcNow)
                    {
                        _NumberCount = 0;
                        // Cookie hết hạn, chuyển hướng người dùng về trang đăng nhập
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        context.Response.Redirect("/LoginArea/Login/Index");
                        return;
                    }
                }
                else
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/LoginArea/Login/Index");
                    return;
                }    
            }
            await _next(context);
        }
    }
}
