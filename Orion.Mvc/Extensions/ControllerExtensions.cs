using System;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Orion.API.Extensions;
using Orion.API.Models;

namespace Orion.Mvc.Extensions
{
	/// <summary></summary>
	public static class ControllerExtensions
	{
		/// <summary></summary>
		public static ContentResult Error(this Controller controller, int httpCode, string message) 
		{
			//TODO 
			//controller.Response.StatusCode = httpCode;

			return new ContentResult
			{
				StatusCode = httpCode,
				Content = message,
				ContentType = null,
				//ContentType = "text/plain charset=utf-8",
			};
		}


		/// <summary></summary>
		public static void SetStatusSuccess(this Controller controller, string message)
		{
			controller.TempData["StatusSuccess"] = message;
		}


		/// <summary></summary>
		public static void SetStatusError(this Controller controller, string message)
		{
			controller.TempData["StatusError"] = message;
		}



		//TODO 

		///// <summary></summary>
		//public static void SetStatusError(this Controller controller, ModelStateDictionary modelState)
		//{
		//	string message = modelState.Values
		//		.SelectMany(x => x.Errors)
		//		.Select(x => x.Exception?.Message ?? x.ErrorMessage)
		//		.Where(x => x.HasText())
		//		.JoinBy("\n");

		//	if (message.NoText()) { return; }

		//	controller.TempData["StatusError"] = message;
		//}


		///// <summary></summary>
		//public static void SetWhereParamsError(this Controller controller, ModelStateDictionary modelState)
		//{
		//	string message = modelState
		//		.Where(pair => pair.Key.StartsWith(nameof(WhereParams)))
		//		.SelectMany(pair => pair.Value.Errors)
		//		.Select(x => x.Exception?.Message ?? x.ErrorMessage)
		//		.Where(x => x.HasText())
		//		.JoinBy("\n");

		//	if (message.NoText()) { return; }

		//	controller.TempData["StatusError"] = message;
		//}


	}
}