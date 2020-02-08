using System.Web;
using Microsoft.AspNetCore.Html;

namespace CoreTpl.WebApp
{
	public static class Fmt
	{

		public static IHtmlContent Decimal(decimal? value)
		{
			if (!value.HasValue) { return HtmlString.Empty; }
			return new HtmlString($"{value:G29}");
		}


		public static IHtmlContent Ellipsis(object value)
		{
			if (value == null) { return HtmlString.Empty; }

			value = HttpUtility.HtmlEncode(value);
			return new HtmlString($"<div class=\"ellipsis\" title=\"{value}\">{value}</div>");
		}

		 
	}
}