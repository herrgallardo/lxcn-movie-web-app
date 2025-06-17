using Microsoft.AspNetCore.Identity;

namespace lxcn_movie_web_app.Data
{
    /// <summary>
    /// Seeds the database with default roles and admin user
    /// Creates the initial role structure and default administrator account
    /// </summary>
    public static class RoleSeeder
    {
        /// <summary>
        /// Initialize roles and create default admin user
        /// </summary>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Create roles if they don't exist
            await CreateRoleIfNotExists(roleManager, RoleConstants.Admin);
            await CreateRoleIfNotExists(roleManager, RoleConstants.User);

            // Create default admin user
            await CreateDefaultAdmin(userManager);
        }

        /// <summary>
        /// Create a role if it doesn't already exist
        /// </summary>
        /// <param name="roleManager">Role manager instance</param>
        /// <param name="roleName">Name of the role to create</param>
        private static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        /// <summary>
        /// Create default admin user if no admin exists
        /// </summary>
        /// <param name="userManager">User manager instance</param>
        private static async Task CreateDefaultAdmin(UserManager<IdentityUser> userManager)
        {
            // Check if any admin users exist
            var adminUsers = await userManager.GetUsersInRoleAsync(RoleConstants.Admin);
            if (adminUsers.Any())
            {
                return; // Admin already exists
            }

            // Create default admin user
            var adminUser = new IdentityUser
            {
                UserName = "admin@hollyview.com",
                Email = "admin@hollyview.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                // Assign admin role
                await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }
    }
}