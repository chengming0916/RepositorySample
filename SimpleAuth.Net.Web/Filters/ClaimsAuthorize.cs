using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SimpleAuth.Net.Web.Filters
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        public string Issuer { get; set; }

        public string ClaimType { get; set; }

        public string Value { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IPrincipal principal = httpContext.User;

            if (!principal.Identity.IsAuthenticated) return false;

            if (!(httpContext.User.Identity is ClaimsIdentity claimsIdentity)) return false;

            return claimsIdentity.HasClaim(x => x.Issuer == Issuer && x.Type == ClaimType && x.Value == Value);

        }
    }
}