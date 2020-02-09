using System.Collections.Generic;
using System.Security.Claims;
using CoreTpl.Enums;
using CoreTpl.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{
	[AllowAnonymous]
	public class MngController : Controller
	{
		public IServiceContext Svc { private get; set; }



		public IActionResult Index()
		{
			return View();
		}




		public IActionResult AdminLogin()
		{
			List<Claim> claims = OrionUtils.EnumToDictionary<ACT>()
				.Keys.ToList(x => new Claim(ClaimTypes.Role, x));

			claims.Add(new Claim(ClaimTypes.Role, "DevelopAdmin"));

			claims.Add(new Claim(OrionUser.UserId, "11"));
			claims.Add(new Claim(OrionUser.UserName, "Admin"));
			claims.Add(new Claim(OrionUser.Account, "admin"));

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity)).Wait();

			return Content("");
		}



		public IActionResult TableSize()
		{
			//TODO TableSize
			return View();
		}




		public IActionResult Execute(string id = null)
		{
			switch (id)
			{
				case "CassetteRebuild": /* 重建 Cassette 綑綁 */
					//Bundles.RebuildCache();
					break;
				default:
					return Content("Action Not Found.");
			}

			return View("Rum Complete.");
		}





		public IActionResult Test()
		{
			//Svc.CtrlCommand.GetTodoList();

			return Content("End");
		}



	}
}
