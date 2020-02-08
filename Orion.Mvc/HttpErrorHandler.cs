using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Orion.Mvc
{

    /// <summary>
    /// Enables support for CustomErrors ResponseRewrite mode in MVC.
    /// </summary>
    public class HttpErrorHandler : IHttpModule
    {
        private Dictionary<int, string> _errorPaths;
        private bool _isRewrite = false;


		/// <summary></summary>
		public void Init(HttpApplication application)
        {
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var customErrors = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");

            _isRewrite = customErrors.RedirectMode == CustomErrorsRedirectMode.ResponseRewrite;

            _errorPaths = new Dictionary<int, string>();
            foreach (CustomError error in customErrors.Errors)
            {
                _errorPaths.Add(error.StatusCode, error.Redirect);
            }

            application.EndRequest += Application_EndRequest;
        }



		/// <summary></summary>
		protected void Application_EndRequest(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            if (!_isRewrite || !httpContext.IsCustomErrorEnabled) { return; }


            int status = httpContext.Response.StatusCode;
            string message = "" + httpContext.Response.StatusDescription;

			var httpException = httpContext.Error as HttpException;
            if (httpException != null)
            {
                status = httpException.GetHttpCode();
            }
            else if (httpContext.Error != null)
            {
                status = (int)HttpStatusCode.InternalServerError;
            }

            
            if (HttpStatusCode.OK.Equals(status)) { return; }
            if (!_errorPaths.ContainsKey(status)) { return; }
        
            string url = _errorPaths[status];

            /* avoid circular redirects */
            if (httpContext.Request.Url.AbsolutePath.Equals(VirtualPathUtility.ToAbsolute(url))) { return; }

			if (message.StartsWith("[") && message.EndsWith("]"))
			{
				var queryString = HttpUtility.ParseQueryString("");
				queryString["message"] = message.Trim('[', ']');
				url = url + "?" + queryString;
			}

			httpContext.Response.Clear();
            httpContext.Server.ClearError();
            httpContext.Response.TrySkipIisCustomErrors = true;


            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(url, true);
            }
            else
            {
                httpContext.RewritePath(url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }

            httpContext.Response.StatusCode = status;
        }



		/// <summary></summary>
		public void Dispose() { }


    }

}