using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lxcn_movie_web_app.Data
{
    /// <summary>
    /// Main database context that handles Identity authentication and will include movie data
    /// Inherits from IdentityDbContext to provide user authentication functionality
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configure database model relationships and constraints
        /// </summary>
        /// <param name="builder">Model builder for configuring entities</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional model configurations will be added here
        }
    }
}