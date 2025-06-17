using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lxcn_movie_web_app.Data;
using lxcn_movie_web_app.Models.ViewModels;

namespace lxcn_movie_web_app.Controllers
{
    /// <summary>
    /// Admin controller for managing users, roles, and system administration
    /// Only accessible by users with Admin role
    /// </summary>
    [Authorize(Roles = RoleConstants.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Admin dashboard with system overview and statistics
        /// GET: Admin
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalMovies = await _context.Movies.CountAsync(),
                AdminUsers = await _userManager.GetUsersInRoleAsync(RoleConstants.Admin),
                RegularUsers = await _userManager.GetUsersInRoleAsync(RoleConstants.User),
                RecentMovies = await _context.Movies
                    .OrderByDescending(m => m.DateCreated)
                    .Take(5)
                    .ToListAsync(),
                RecentUsers = await _userManager.Users
                    .OrderByDescending(u => u.Id)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        /// <summary>
        /// Display all users with their roles
        /// GET: Admin/Users
        /// </summary>
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        /// <summary>
        /// Display form to edit user roles
        /// GET: Admin/EditUser/id
        /// </summary>
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new EditUserViewModel
            {
                User = user,
                UserRoles = userRoles.ToList(),
                AllRoles = allRoles,
                AvailableRoles = allRoles.Where(r => !string.IsNullOrEmpty(r.Name) && !userRoles.Contains(r.Name)).ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Process user role changes
        /// POST: Admin/EditUser
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (model.User?.Id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(model.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent admin from removing their own admin role
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Unable to identify current user.";
                return RedirectToAction(nameof(Users));
            }

            if (user.Id == currentUser.Id && !model.SelectedRoles.Contains(RoleConstants.Admin))
            {
                TempData["ErrorMessage"] = "You cannot remove your own Admin role!";
                return RedirectToAction(nameof(EditUser), new { id = user.Id });
            }

            // Remove all current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add selected roles
            if (model.SelectedRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            }
            else
            {
                // If no roles selected, assign default User role
                await _userManager.AddToRoleAsync(user, RoleConstants.User);
            }

            TempData["SuccessMessage"] = $"User roles updated successfully for {user.Email}!";
            return RedirectToAction(nameof(Users));
        }

        /// <summary>
        /// Delete a user account
        /// POST: Admin/DeleteUser
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent admin from deleting their own account
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "Unable to identify current user.";
                return RedirectToAction(nameof(Users));
            }

            if (user.Id == currentUser.Id)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account!";
                return RedirectToAction(nameof(Users));
            }

            // Check if this is the last admin
            var adminUsers = await _userManager.GetUsersInRoleAsync(RoleConstants.Admin);
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.Admin);

            if (isAdmin && adminUsers.Count <= 1)
            {
                TempData["ErrorMessage"] = "Cannot delete the last admin user!";
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User {user.Email} has been deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user. Please try again.";
            }

            return RedirectToAction(nameof(Users));
        }

        /// <summary>
        /// Display system statistics and movie management
        /// GET: Admin/Movies
        /// </summary>
        public async Task<IActionResult> Movies()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.DateCreated)
                .ToListAsync();

            var movieStats = new MovieStatsViewModel
            {
                TotalMovies = movies.Count,
                MoviesByGenre = movies.GroupBy(m => m.Genre)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AveragePrice = movies.Any() ? movies.Average(m => m.Price) : 0,
                MostExpensiveMovie = movies.OrderByDescending(m => m.Price).FirstOrDefault(),
                MostRecentMovie = movies.OrderByDescending(m => m.DateCreated).FirstOrDefault(),
                Movies = movies
            };

            return View(movieStats);
        }

        /// <summary>
        /// Bulk delete selected movies
        /// POST: Admin/BulkDeleteMovies
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDeleteMovies(int[] selectedMovies)
        {
            if (selectedMovies == null || selectedMovies.Length == 0)
            {
                TempData["ErrorMessage"] = "No movies selected for deletion.";
                return RedirectToAction(nameof(Movies));
            }

            var moviesToDelete = await _context.Movies
                .Where(m => selectedMovies.Contains(m.Id))
                .ToListAsync();

            _context.Movies.RemoveRange(moviesToDelete);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully deleted {moviesToDelete.Count} movie(s).";
            return RedirectToAction(nameof(Movies));
        }
    }
}