using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace prjOniqueWebsite.Models.Infra
{
    public class MemberVerify : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            if (!httpContext.Session.Keys.Contains("Login"))
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }
}
