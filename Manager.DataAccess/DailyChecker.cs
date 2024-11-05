using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Manager.Model.Models.CarBooking;
using Manager.DataAccess.Services.CarBooking;
using Manager.DataAccess.IRepository.IUnitOfWork_Repository;
using System.Data.SqlClient;
using Dapper;
using Manager.Model.Services.Abstraction;

namespace Manager.DataAccess.Services
{
    public class DailyChecker : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public DailyChecker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Đặt lịch để kiểm tra trạng thái công việc mỗi ngày vào lúc 00:00
            var now = DateTime.Now;
            var newDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);
            var newDayMorning = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0).AddDays(1);
            var timeToNextRunJobActive = newDay - now;
            var timeToNextRunBirthday = newDayMorning - now;
            //_timer = new Timer(async _ => await CheckJobActived(), null, timeToNextRunJobActive, TimeSpan.FromDays(1));
            //_timer = new Timer(async _ => await CheckBirthday(), null, timeToNextRunBirthday, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async Task CheckBookingStatus_CAR()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<CarDbContext>();
                var taxiServices = scope.ServiceProvider.GetRequiredService<TaxiServices>();
                var bookings = await _context.Requests
                                             .Where(r => r.status_enviet == "WAITING" && r.date_xacnhan.HasValue && (r.payment_status == "Failure" || r.payment_status == "Processing"))
                                             .ToListAsync();

                foreach (var booking in bookings)
                {
                    if (booking.date_xacnhan.Value.AddHours(24) <= DateTime.Now)
                    {
                        var data = new JObject();
                        data["evcode"] = booking.evcode;
                        data["reason"] = "Quá hạn thanh toán";

                        await taxiServices.CancelBooking(data);
                    }
                }
            }
        }

        private async Task CheckJobActived()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork_Repository>();
                await unitOfWork.LandingPage_Rep.ChangeActiveJob();
            }
        }

        private async Task CheckBirthday()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var sinhnhatdailyService = scope.ServiceProvider.GetRequiredService<ISinhnhatdailyService>();
                await sinhnhatdailyService.CheckBirthdayAsync();
            }
        }

        // CheckBirthday test
        //sự kiện kiểm tra thời gian thực hiện sau mỗi 10 giây thay vì đợi đến 7h sáng
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {

        //        var delay = TimeSpan.FromSeconds(20);

        //        await Task.Delay(delay, stoppingToken);
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var sinhnhatdailyService = scope.ServiceProvider.GetRequiredService<ISinhnhatdailyService>();
        //            await sinhnhatdailyService.CheckBirthdayAsync();
        //        }
        //        Console.WriteLine("BirthdayCheckerService chạy lúc: " + DateTime.UtcNow);
        //    }
        //}


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
