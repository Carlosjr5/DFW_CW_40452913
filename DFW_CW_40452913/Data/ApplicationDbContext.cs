using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DFW_CW_40452913.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Petition> Petitions { get; set; }
        public string UserName { get; internal set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Petition>().ToTable("Petition");

        }
    }
}
