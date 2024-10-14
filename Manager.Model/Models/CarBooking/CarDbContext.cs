using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Manager.Model.Models.CarBooking
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options) { }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
    }
}
