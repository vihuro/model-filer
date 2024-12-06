using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ModelFilter.Api.Utils.Authorize
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claim) : base(typeof(RequiredClaimFilter))
        {
            Arguments = new object[]
            {
                new Claim("",claim)
            };
        }
    }
}
