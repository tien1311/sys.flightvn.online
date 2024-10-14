using Manager.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Manager.DataAccess.Services.Sinhnhatkhachvip
{
    public class SinhnhatDbContext : DbContext
    {
        public SinhnhatDbContext(DbContextOptions<SinhnhatDbContext> options) : base(options) { }

        public DbSet<KHACHHANGVIP> KHACHHANGVIP { get; set; }


        public DbSet<ThongBaoSinhNhatDaiLi> ThongBaoSinhNhatDaiLi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
