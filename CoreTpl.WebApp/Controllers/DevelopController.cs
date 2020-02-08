using System.Collections.Generic;
using CoreTpl.Domain;
using CoreTpl.Service;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Extensions;


namespace CoreTpl.WebApp.Controllers
{
    public class DevelopController : Controller
    {

        //[Authorize(Roles = "DevelopAdmin")]
        public IActionResult Index() { return RedirectToAction(nameof(BootstrapOverview)); }
        public IActionResult BootstrapOverview() { return View(); }
        public IActionResult Dialog() { return View(); }
        public IActionResult DialogShow() { return View(); }
        public IActionResult Icons() { return View(); }
        public IActionResult IconsExamples() { return View(); }
        public IActionResult SampleForm() { return View(); }
        public IActionResult SampleList1() { return View(); }
        public IActionResult SampleList2() { return View(); }
        public IActionResult SampleList3() { return View(); }
        public IActionResult SampleList4() { return View(); }
        public IActionResult WhereBuilder() { return View(); }




        //public IActionResult SampleExcel(string output)
        //{
        //    string filename = String.Format("SampleExcel-{0:yyyyMMddHHmmss}", DateTime.Now);

        //    if (output == "pdf")
        //    {
        //        return new PdfResult() { FileDownloadName = filename + ".pdf" };
        //    }
        //    else
        //    {
        //        filename = HttpUtility.UrlEncode(filename + ".xls");

        //        Response.ContentType = "application/octet-stream";
        //        Response.AddHeader("Content-Disposition", "attachment; filename*=UTF-8''" + filename);
        //        return View(null, "~/Views/shared/_ExcelLayout.cshtml");
        //    }
        //}



    }
}