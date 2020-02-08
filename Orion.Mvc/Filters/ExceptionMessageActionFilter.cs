using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.API;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Filters
{
	/// <summary>例外訊息過濾器</summary>
	public class ExceptionMessageActionFilter : ActionFilterAttribute
	{

		private Type[] _exceptionTypes;

		/// <summary></summary>
		public ExceptionMessageActionFilter(params Type[] exceptionTypes)
		{
			_exceptionTypes = exceptionTypes;
		}


		/// <summary></summary>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var parameters = filterContext.ActionArguments.Values.Where(x => x != null);

			object model = parameters
				.OrderBy(x => Regex.IsMatch(x.GetType().Name, @"(ViewModel|Domain)\b") ? 0 : 1)
				.FirstOrDefault();

			var controller = filterContext.Controller as Controller;
			controller.ViewData.Model = model;
		}


		/// <summary></summary>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			if (filterContext.ExceptionHandled) { return; }

			var ex = filterContext.Exception;
			if (!_exceptionTypes.Any(x => x.IsInstanceOfType(ex))) { return; }


			filterContext.ExceptionHandled = true;
			var controller = filterContext.Controller as Controller;

			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				//TODO
				//filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
				//filterContext.HttpContext.Response.StatusCode = 400;
				filterContext.Result = new ContentResult { StatusCode = 400, Content = ex.Message };
			}
			else if (ex is OrionNoDataException) 
			{
				controller.TempData["StatusError"] = ex.Message;
				filterContext.Result = new ContentResult { StatusCode = 404, Content = "[" + ex.Message + "]" };
			}
			else
			{
				controller.TempData["StatusError"] = ex.Message;
				filterContext.Result = new ViewResult
				{
					ViewData = controller.ViewData,
					TempData = controller.TempData
				};
			}
		}

	}
}