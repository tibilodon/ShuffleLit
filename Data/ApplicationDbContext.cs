using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShuffleLit.Models;

namespace ShuffleLit.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Literature> Literatures { get; set; }
        public DbSet<LiteratureCollection> LiteratureCollections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            modelBuilder.Entity<LiteratureCollection>().HasKey(lc => new { lc.AppUserId, lc.LiteratureId });
            modelBuilder.Entity<LiteratureCollection>().HasOne(lc => lc.Literature).WithMany(lit => lit.LiteratureCollections).HasForeignKey(l => l.LiteratureId);
            modelBuilder.Entity<LiteratureCollection>().HasOne(au => au.AppUser).WithMany(lit => lit.LiteratureCollections).HasForeignKey(u => u.AppUserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
