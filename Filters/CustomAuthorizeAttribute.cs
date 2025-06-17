using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace lxcn_movie_web_app.Filters
{
    /// <summary>
    /// Custom authorization attribute that provides better error handling
    /// for unauthorized access attempts with user-friendly messages
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _customMessage;

        public CustomAuthorizeAttribute(string customMessage = "")
        {
            _customMessage = customMessage;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated == true)
            {
                // User is not authenticated, redirect to login
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Account/Login", new { area = "Identity", returnUrl });
                return;
            }

            // User is authenticated but check for role authorization
            if (!string.IsNullOrEmpty(Roles))
            {
                var requiredRoles = Roles.Split(',').Select(r => r.Trim()).ToArray();
                var hasRequiredRole = requiredRoles.Any(role => user.IsInRole(role));

                if (!hasRequiredRole)
                {
                    // User doesn't have required role, show access denied
                    var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                    context.Result = new RedirectToActionResult("AccessDenied", "Error", new { returnUrl });

                    // Set custom message if provided
                    if (!string.IsNullOrEmpty(_customMessage))
                    {
                        context.HttpContext.Items["AccessDeniedMessage"] = _customMessage;
                    }
                }
            }
        }
    }
}