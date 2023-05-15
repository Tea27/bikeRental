using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace bikeRental.Frontend.Filters
{
    public class CustomAuthorize : TypeFilterAttribute
    {
        public CustomAuthorize() : base(typeof(CustomAuthorizeFilter))
        {
        }

        private class CustomAuthorizeFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
        }

    }
}
