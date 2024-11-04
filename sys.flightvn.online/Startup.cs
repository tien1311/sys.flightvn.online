using Manager.Common.Extensions;
using Manager.Common.Middlewares;
using Manager.Common.Types;
using Manager.DataAccess;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using Manager.DataAccess.Repository;
using Manager.DataAccess.Services;
using Manager.DataAccess.Services.CarBooking;
using Manager.DataAccess.Services.Sinhnhatkhachvip;
using Manager.Model.Models.CarBooking;
using Manager.Model.Models.FTP;
using Manager.Model.Services;
using Manager.Model.Services.Abstraction;
//using JwtAuthenticationHelper.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
//using RedisConfig;
using System;


namespace sys.flightvn.online
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký HttpClient cho carbooking
            services.AddHttpClient("carbooking", (service, client) =>
            {
                client.BaseAddress = new System.Uri(Configuration["API_CarBooking:URL"]);
                client.DefaultRequestHeaders.Add("x-master-key", Configuration["EnVietService:ApiKey"]);
            });

            services.AddHostedService<DailyChecker>();

            // Đăng ký các service cần thiết cho việc xử lý sinh nhật
            services.AddScoped<ISinhnhatdailyService, SinhnhatdailyService>();
            services.AddScoped<SinhnhatdailyRepository>();

            // Đăng ký DbContext cho SinhnhatDbContext
            services.AddDbContext<SinhnhatDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SQL_EV_MAIN")));

            // Các service và repository khác
            services.AddScoped<TaxiServices>();
            services.AddScoped<GatewayRepository>();
            services.AddScoped<HotelRepository>();
            services.AddScoped<PermissionRepository>();

           

            services.AddScoped<IUnitOfWork_Repository, UnitOfWork_Repository>();
            //services.AddScoped<FeedbackRepository>();

            // Access HttpContext

            // Token Authentication
            var tokenOptions = new TokenOptions(
                Configuration["Token:Audience"],
                Configuration["Token:Issuer"],
                Configuration["Token:SigningKey"]);

            services.AddJwtAuthenticationWithProtectedCookie(tokenOptions);

            // Authorization Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiresAdmin", policy => policy.RequireClaim("HasAdminRights"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpContextAccessor();
            services.AddSession();

           
            services.AddScoped<IAirportService, AirportService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<INotifyService, NotifyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next.Invoke();

                //After going down the pipeline check if we 404'd. 
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await context.Response.WriteAsync("Woops! We 404'd");
                }
            });

            //Add our new middleware to the pipeline
            app.UseMiddleware<CookieExpirationMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{area=MenuArea}/{controller=Menu}/{action=Index}/{id?}"

                    );
            });
        }
    }
}
