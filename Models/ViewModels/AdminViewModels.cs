using Microsoft.AspNetCore.Identity;
using lxcn_movie_web_app.Models;

namespace lxcn_movie_web_app.Models.ViewModels
{
    /// <summary>
    /// View model for admin dashboard showing system overview
    /// </summary>
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalMovies { get; set; }
        public IList<IdentityUser> AdminUsers { get; set; } = new List<IdentityUser>();
        public IList<IdentityUser> RegularUsers { get; set; } = new List<IdentityUser>();
        public List<Movie> RecentMovies { get; set; } = new List<Movie>();
        public List<IdentityUser> RecentUsers { get; set; } = new List<IdentityUser>();
    }

    /// <summary>
    /// View model for displaying user information with roles
    /// </summary>
    public class UserViewModel
    {
        public IdentityUser User { get; set; } = new IdentityUser();
        public List<string> Roles { get; set; } = new List<string>();
    }

    /// <summary>
    /// View model for editing user roles
    /// </summary>
    public class EditUserViewModel
    {
        public IdentityUser User { get; set; } = new IdentityUser();
        public List<string> UserRoles { get; set; } = new List<string>();
        public List<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();
        public List<IdentityRole> AvailableRoles { get; set; } = new List<IdentityRole>();
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }

    /// <summary>
    /// View model for movie statistics and management
    /// </summary>
    public class MovieStatsViewModel
    {
        public int TotalMovies { get; set; }
        public Dictionary<string, int> MoviesByGenre { get; set; } = new Dictionary<string, int>();
        public decimal AveragePrice { get; set; }
        public Movie? MostExpensiveMovie { get; set; }
        public Movie? MostRecentMovie { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}