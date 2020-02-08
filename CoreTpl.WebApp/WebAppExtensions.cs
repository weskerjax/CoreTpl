using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Orion.API.Extensions;
using Orion.API.Models;
using Orion.Mvc.Html;
using Orion.Mvc.Extensions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using System.Security.Claims;
using Orion.API;

namespace CoreTpl.WebApp
{
	/// <summary></summary>
	public static class WebAppExtensions
	{

		public static int UserId(this Controller controller)
		{
			return controller.User.GetUserId();
		}



		/// <summary></summary>
		public static ViewResult ExcelView(this Controller controller, string downloadName, object model = null)
		{
			return ExcelView(controller, downloadName, null, model);
		}

		/// <summary></summary>
		public static ViewResult ExcelView(this Controller controller, string downloadName, string viewName, object model = null)
		{
			if (model != null) { controller.ViewData.Model = model; }

			string disposition = "attachment; filename*=UTF-8''" + HttpUtility.UrlEncode(downloadName);
			controller.Response.ContentType = "application/octet-stream";
			controller.Response.Headers["Content-Disposition"] = disposition;
			controller.ViewData["IsExport"] = true;
			controller.ViewData["Layout"] = "_ExcelLayout";

			return new ViewResult
			{
				ViewName = viewName,
				ViewData = controller.ViewData,
				TempData = controller.TempData,
				ContentType = "application/octet-stream",
			};
		}




		/*=========================================*/


		public static bool IsRouteAction(this RazorPage page, string actionName)
		{
			var routeAction = page.ViewContext.RouteData.Values["Action"] as string;
			return actionName.EqualsIgnoreCase(routeAction);
		}



		public static Dictionary<string, bool> ColumnStatus(this RazorPage page, string name)
		{
			var provider = page.Context.RequestServices.GetService<IOptionItemsProvider>();

			int userId = page.User.GetUserId();
			Dictionary<string, bool> columnStatus = provider.GetColumnStatus(userId, name);
			if (columnStatus != null) { return columnStatus; }

			return null;
		}


		public static List<string> ColumnOrder(this RazorPage page, string name)
		{
			Dictionary<string, bool> columnStatus = ColumnStatus(page, name);
			if (columnStatus == null) { return new List<string>(); }

			List<string> columns = columnStatus.Where(y => y.Value).ToList(y => y.Key);
			return columns;
		}


		public static List<string> ColumnOrder<T>(this RazorPage page, string name)
		{
			List<string> columns = ColumnOrder(page, name);

			if (columns == null || columns.Count == 0)
			{ columns = typeof(T).GetProperties().ToList(prop => prop.Name); }

			return columns;
		}




		/// <summary>顯示日期時間</summary>
		public static IHtmlContent DateTimeLabel(this IHtmlHelper helper, DateTime? datatime, object htmlAttributes = null)
		{
			if (datatime == null) { return HtmlString.Empty; }

			IDictionary<string, object> attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

			var span = new TagBuilder("span");
			span.Attributes["title"] = helper.ShowLiveTime(datatime);
			span.MergeAttributes(attrs);
			span.InnerHtml.Append(datatime.ToString());

			return span;
		}




        /// <summary></summary>
        public static Task<IHtmlContent> OrderableBtn(this IHtmlHelper helper, string targetId, bool isEllipsis = false)
        {
            return helper.PartialAsync("_OrderableBtn", new OrderableBtnModel { TargetId = targetId, IsEllipsis = isEllipsis });
        }

        /// <summary></summary>
        public static Task<IHtmlContent> SortBuilderBtn(this IHtmlHelper helper, string targetId)
        {
            return helper.PartialAsync("_SortBuilderBtn", new SortBuilderBtnModel { TargetId = targetId });
        }


        /// <summary></summary>
        public static Task<IHtmlContent> PageSizeChange(this IHtmlHelper helper, IPagination pagination, int[] pageSizeItems = null)
		{
			var viewData = new ViewDataDictionary(helper.ViewData) { { "PageSizeItems", pageSizeItems } };
			return helper.PartialAsync("_PageSizeChange", pagination, viewData);
		}

        /// <summary></summary>
        public static Task<IHtmlContent> PageEmptyAlert(this IHtmlHelper helper, IPagination pagination)
        {
            return helper.PartialAsync("_PageEmptyAlert", pagination?.TotalItems ?? 0);
        }

        /// <summary></summary>
        public static Task<IHtmlContent> PageEmptyAlert(this IHtmlHelper helper, ICollection list)
        {
            return helper.PartialAsync("_PageEmptyAlert", list?.Count ?? 0);
        }

    }
}
