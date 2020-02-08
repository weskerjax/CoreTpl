using System.Web.Mvc;

namespace Orion.Mvc.Filters
{
	/// <summary></summary>
	public class JsonNetActionFilter : ActionFilterAttribute
    {
		/// <summary></summary>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as JsonResult;
            if (result != null) { filterContext.Result = new JsonNetResult(result); }
        }
    }
}