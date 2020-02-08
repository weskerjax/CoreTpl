using Microsoft.AspNetCore.Mvc;

namespace CoreTpl.WebApp.Controllers
{
	//[AllowAnonymous]
	public class ErrorController : Controller
	{

		//protected override void OnActionExecuted(ActionExecutedContext filterContext)
		//{
		//	Response.TrySkipIisCustomErrors = true;
		//}


		public IActionResult Forbidden(string message = null)
		{
			Response.StatusCode = 403;
			ViewBag.Title = "權限不足無法執行！";
			ViewBag.Message = message ?? "抱歉！您的帳號權限無法觀看此頁面，請回上一頁，或前往其他頁面。";
			ViewBag.Color = "#f4b04f";
			ViewBag.OOPS = "OOPS!";

			return View("Error");
		}

		public IActionResult NotFound(string message = null)
		{
			Response.StatusCode = 404;
			ViewBag.Title = "網址頁面並不存在！";
			ViewBag.Message = message ?? "抱歉！您所指定網址的資料或頁面並不存在，請回上一頁，或前往其他頁面。";
			ViewBag.Color = "#f4b04f";
			ViewBag.OOPS = "OOPS!";

			return View("Error");
		}


		public IActionResult ServerError(string message = null)
		{
			Response.StatusCode = 500;
			ViewBag.Title = "伺服器發生錯誤！";
			ViewBag.Message = message ?? "抱歉！伺服器發生錯誤，請回上一頁，或前往其他頁面。";
			ViewBag.Color = "#e66454";
			ViewBag.OOPS = "OUCH!";

			return View("Error");
		}



	}
}
