using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Filters
{
    public class DevelopAuthorizationFilter<TActEnum> : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated) { return; }

            List<Claim> claims = OrionUtils.EnumToDictionary<TActEnum>()
                .Keys.ToList(x => new Claim(ClaimTypes.Role, x));

            claims.Add(new Claim(OrionUser.UserId, "1"));
            claims.Add(new Claim(OrionUser.UserName, "Admin"));
            claims.Add(new Claim(OrionUser.Account, "admin"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
            context.HttpContext.SignInAsync(context.HttpContext.User).Wait();
        }
    }
}