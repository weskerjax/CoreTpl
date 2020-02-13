using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Mvc.Attributes;

namespace CoreTpl.WebApp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TplSearchRememberAttribute : SearchRememberAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Ignore = Ignore.Concat(new[] { "export" }).ToArray();
            base.OnActionExecuting(filterContext);
        }
    }

}
