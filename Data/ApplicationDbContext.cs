using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using lxcn_movie_web_app.Models;

namespace lxcn_movie_web_app.Data
{
    /// <summary>
    /// Main database context that handles Identity authentication and movie data
    /// Inherits from IdentityDbContext to provide user authentication functionality
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for Movie entities - represents the Movies table in the database
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// Configure database model relationships and constraints
        /// </summary>
        /// <param name="builder">Model builder for configuring entities</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Movie entity
            builder.Entity<Movie>(entity =>
            {
                // Set table name explicitly
                entity.ToTable("Movies");

                // Configure Title as required with index for search performance
                entity.HasIndex(e => e.Title)
                      .HasDatabaseName("IX_Movies_Title");

                // Configure Genre with index for filtering
                entity.HasIndex(e => e.Genre)
                      .HasDatabaseName("IX_Movies_Genre");

                // Configure ReleaseDate with index for sorting
                entity.HasIndex(e => e.ReleaseDate)
                      .HasDatabaseName("IX_Movies_ReleaseDate");

                // Set default value for DateCreated
                entity.Property(e => e.DateCreated)
                      .HasDefaultValueSql("GETDATE()");

                // Configure decimal precision for Price
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}