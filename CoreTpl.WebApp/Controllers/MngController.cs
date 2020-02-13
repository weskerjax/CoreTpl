using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CoreTpl.Dao.Database;
using CoreTpl.Enums;
using CoreTpl.Service;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orion.API;
using Orion.API.Extensions;
using Orion.API.Models;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{
	[AllowAnonymous]
	public class MngController : Controller
	{
		public IServiceContext Svc { private get; set; }
		public TplDbContext Dc { private get; set; }
		


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



		public IActionResult TableSize(PageParams<TableInfo> pageParams)
		{
			List<TableInfo> tableInfoList = Dc.GetTableInfo();
			var query = tableInfoList.AsQueryable();

			try
			{ query = query.AdvancedOrderBy(pageParams.OrderField, pageParams.Descending); }
			catch
			{ query = query.OrderBy(x => x.Name); }

			ViewBag.TableInfoList = query.ToList();

			return View();
		}




		public IActionResult Execute(string id = null)
		{
			switch (id)
			{
				case "DbMigrate": /* 資料庫移轉 (PS: 會建立或更新 Table) */
					Dc.Database.Migrate();
					break;
				default:
					return Content("Action Not Found.");
			}

			return Content("Rum Complete.");
		}





		public IActionResult Test()
		{
			//Svc.CtrlCommand.GetTodoList();

			return Content("End");
		}



	}
}
