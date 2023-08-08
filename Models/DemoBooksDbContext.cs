using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoLibrary.Models
{
    public class DemoBooksDbContext : IdentityDbContext<User>
    {
        public DemoBooksDbContext(DbContextOptions<DemoBooksDbContext> contextOptions) : base(contextOptions)
        {


        }


        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<User> users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .HasMaxLength(255); // Set the maximum length for the FirstName property if desired

                entity.Property(u => u.LastName)
                    .HasMaxLength(255); // Set the maximum length for the LastName property if desired
            });
        }

    }
}
