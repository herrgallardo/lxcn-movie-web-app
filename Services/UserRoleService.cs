using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace lxcn_movie_web_app.Services
{
    /// <summary>
    /// Service for checking user roles and permissions in views and controllers
    /// Provides centralized role checking functionality
    /// </summary>
    public interface IUserRoleService
    {
        Task<bool> IsUserInRoleAsync(ClaimsPrincipal user, string role);
        Task<string> GetUserRoleAsync(ClaimsPrincipal user);
        Task<bool> IsAdminAsync(ClaimsPrincipal user);
        Task<bool> IsUserAsync(ClaimsPrincipal user);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Check if user is in a specific role
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(ClaimsPrincipal user, string role)
        {
            if (!user.Identity?.IsAuthenticated == true)
                return false;

            var identityUser = await _userManager.GetUserAsync(user);
            if (identityUser == null)
                return false;

            return await _userManager.IsInRoleAsync(identityUser, role);
        }

        /// <summary>
        /// Get the primary role of the user
        /// </summary>
        public async Task<string> GetUserRoleAsync(ClaimsPrincipal user)
        {
            if (!user.Identity?.IsAuthenticated == true)
                return "Non-user";

            var identityUser = await _userManager.GetUserAsync(user);
            if (identityUser == null)
                return "Non-user";

            var roles = await _userManager.GetRolesAsync(identityUser);

            // Return highest privilege role
            if (roles.Contains("Admin"))
                return "Admin";
            if (roles.Contains("User"))
                return "User";

            return "Non-user";
        }

        /// <summary>
        /// Check if user is an admin
        /// </summary>
        public async Task<bool> IsAdminAsync(ClaimsPrincipal user)
        {
            return await IsUserInRoleAsync(user, "Admin");
        }

        /// <summary>
        /// Check if user is a regular user (not admin)
        /// </summary>
        public async Task<bool> IsUserAsync(ClaimsPrincipal user)
        {
            return await IsUserInRoleAsync(user, "User");
        }
    }
}