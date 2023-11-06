using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerOnly.Model;

namespace ServerOnly.DB
{
    public class DataBaseContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Audio> audios { get; set; } = null!;

        public DataBaseContext(
            DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
