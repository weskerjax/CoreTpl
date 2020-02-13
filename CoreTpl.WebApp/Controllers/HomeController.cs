using System.Diagnostics;
using System.Reflection;
using CoreTpl.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Models;

namespace CoreTpl.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IServiceContext Svc { private get; set; }



        //[AllowAnonymous]
        public IActionResult Index()
        {
            //throw new Exception("錯誤測試");
            //throw new OrionException("錯誤測試");
            //throw new OrionNoDataException("沒有資料測試");

            //return RedirectToAction("Login", "Account");
            //return RedirectToAction("Create", "Role");
            return RedirectToAction("List", "Role");
            //return RedirectToAction("TableSize", "Mng");

            

            return View();
        }



        /*#################################################################*/

        [HttpPost]
        public IActionResult SavePreference(string name, string value)
        {
            Svc.User.SavePreference(this.UserId(), name, value);
            return Content("OK");
        }



        public IActionResult AboutMe()
        {
            AssemblyMeta asmMeta = AssemblyUtils.GetMeta(Assembly.GetExecutingAssembly());
            ViewBag.AsmMeta = asmMeta;

            return View();
        }






        /*=================================================================*/

        [AllowAnonymous]
        public IActionResult Error()
        {
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }



    }
}
