namespace ModelFilter.Api.Utils.Authorize
{
    public static class CustomAuthorize
    {
        public static bool ValidateClaim(HttpContext context, string listClaims)
        {

            var claimsInHeader = listClaims.Trim().Split(',');

            if (!context.User.Identity.IsAuthenticated)
                return false;

            var listRoles = context.User.Claims.Where(x => 
                                                  string.Equals(x.Type, "rolesToUser", StringComparison.OrdinalIgnoreCase))
                                               .Select(x => x.Value)
                                               .FirstOrDefault("")
                                               .Split(',');

            if (listRoles.Where(x => claimsInHeader.Any(c => c == x)).Any())
                return true;

            return false;
        }
    }
}
