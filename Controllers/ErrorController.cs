using Microsoft.AspNetCore.Mvc;

namespace lxcn_movie_web_app.Controllers
{
    /// <summary>
    /// Controller for handling application errors and access denied scenarios
    /// Provides user-friendly error pages and access denied information
    /// </summary>
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Display access denied page when users try to access restricted resources
        /// GET: Error/AccessDenied
        /// </summary>
        /// <param name="returnUrl">URL user was trying to access</param>
        /// <returns>Access denied view with helpful information</returns>
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            // Determine what the user was trying to access for better messaging
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.Contains("/Movies/Create"))
                    ViewBag.AttemptedAction = "create a new movie";
                else if (returnUrl.Contains("/Movies/Delete"))
                    ViewBag.AttemptedAction = "delete a movie";
                else if (returnUrl.Contains("/Movies/Edit"))
                    ViewBag.AttemptedAction = "edit a movie";
                else if (returnUrl.Contains("/Admin"))
                    ViewBag.AttemptedAction = "access the admin panel";
                else
                    ViewBag.AttemptedAction = "access this resource";
            }
            else
            {
                ViewBag.AttemptedAction = "access this resource";
            }

            // Log the access denied attempt
            _logger.LogWarning("Access denied for user {User} attempting to access {Url}",
                User.Identity?.Name ?? "Anonymous", returnUrl ?? "Unknown");

            return View();
        }

        /// <summary>
        /// Display general error page for unhandled exceptions
        /// GET: Error
        /// </summary>
        /// <returns>Error view</returns>
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View("Error");
        }

        /// <summary>
        /// Display 404 Not Found page
        /// GET: Error/PageNotFound
        /// </summary>
        /// <returns>Not found view</returns>
        [Route("Error/PageNotFound")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}