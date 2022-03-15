using Microsoft.EntityFrameworkCore;

namespace Passman.Data
{
    public class PassManDbContext : DbContext
    {
        public DbSet<Password> Passwords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=passman.db");
        }
    }
}