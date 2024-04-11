using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DFW_CW_40452913.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Petition> Petitions { get; set; }
        public string UserName { get; internal set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Petition>().ToTable("Petition");

            // Define the relationship between Petitions and Comments
            builder.Entity<Petition>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Petition)
                .HasForeignKey(c => c.PetitionId);

        }
    }
}
