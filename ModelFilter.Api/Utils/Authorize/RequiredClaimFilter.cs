using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelFilter.Domain.Models;
using System.Security.Claims;

namespace ModelFilter.Api.Utils.Authorize
{
    public class RequiredClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequiredClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomAuthorize.ValidateClaim(context.HttpContext, _claim.Value))
            {
                context.Result = new JsonResult(new ReturnDefault<object>
                {
                    Sucess = false,
                    Errors = new List<string>
                    {
                        "Forbidden"
                    }
                })
                { StatusCode = 403 };
            }
        }
    }
}
