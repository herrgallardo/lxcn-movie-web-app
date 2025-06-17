namespace lxcn_movie_web_app.Data
{
    /// <summary>
    /// Constants for user roles in the application
    /// Defines the three-tier authorization system: Admin, User, Non-user
    /// </summary>
    public static class RoleConstants
    {
        /// <summary>
        /// Admin role - Full CRUD permissions (Create, Read, Update, Delete)
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// User role - Limited permissions (Read, Update only)
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// All authenticated roles (Admin + User)
        /// </summary>
        public const string AdminAndUser = "Admin,User";
    }
}