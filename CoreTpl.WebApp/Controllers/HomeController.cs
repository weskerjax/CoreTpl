using System.Diagnostics;
using System.Reflection;
using CoreTpl.Service;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Models;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IServiceContext Svc { private get; set; }



        //[AllowAnonymous]
        public IActionResult Index()
        {
            //return RedirectToAction("Login", "Account");
            //return RedirectToAction("Create", "Role");
            
            return View();
        }



        /*#################################################################*/

        [HttpPost]
        public IActionResult SavePreference(string name, string value)
        {
            Svc.User.SavePreference(this.UserId(), name, value);
            return Content("OK");
        }



        //[AllowAnonymous]
        public IActionResult AboutMe()
        {
            AssemblyMeta asmMeta = AssemblyUtils.GetMeta(Assembly.GetExecutingAssembly());
            ViewBag.AsmMeta = asmMeta;

            return View();
        }






        /*=================================================================*/


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var vm = new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            };

            return View(vm);
        }


    }
}
