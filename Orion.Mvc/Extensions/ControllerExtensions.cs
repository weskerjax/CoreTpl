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
		public static void SetStatusSuccess(this Controller controller, string message)
		{
			controller.TempData["StatusSuccess"] = message;
		}


		/// <summary></summary>
		public static void SetStatusError(this Controller controller, string message)
		{
			controller.TempData["StatusError"] = message;
		}


	}
}