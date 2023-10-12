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
        public DbSet<Comments> comments { get; set; }

        public DbSet<favoritBooks> favoritBooks { get; set; }
        
        public DbSet<RatingUserBook> ratingUserBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //join book and comments
            builder.Entity<Comments>()
         .HasOne(c => c.Book)
         .WithMany(b => b.Comments)
         .HasForeignKey(c => c.BookId);

            builder.Entity<Comments>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);
            //join favorit book user
            builder.Entity<favoritBooks>()
        .HasKey(ub => new { ub.UserId, ub.BookId });

            builder.Entity<favoritBooks>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserFavoritBooks)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<favoritBooks>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.UserBooks)
                .HasForeignKey(ub => ub.BookId);
            //join download user book
            builder.Entity<DownloadBook>()
     .HasKey(ub => new { ub.UserId, ub.BookId });

            builder.Entity<DownloadBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserDowloadBooks)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<DownloadBook>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.DownBooks)
                .HasForeignKey(ub => ub.BookId);
            //join Rating book user
            builder.Entity<RatingUserBook>()
   .HasKey(ub => new { ub.UserId, ub.BookId });

            builder.Entity<RatingUserBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserRatingBooks)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<RatingUserBook>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.RatingBooks)
                .HasForeignKey(ub => ub.BookId);
            //Add customs columns to user
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
