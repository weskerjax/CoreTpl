using System.Collections.Generic;
using CoreTpl.Domain;
using CoreTpl.Enums;
using CoreTpl.Service;
using Microsoft.AspNetCore.Mvc;
using Orion.API.Models;
using Orion.Mvc.Attributes;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{

    [ActAuthorize(ACT.STK_RoleSetting)]
    public class RoleController : Controller
    {
        public IServiceContext Svc { private get; set; }



 
        //protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    if (User.Identity.IsAuthenticated) { return; }

        //    var result = filterContext.Result as HttpUnauthorizedResult;
        //    if (result?.StatusCode == 401) { filterContext.Result = Redirect("~/"); }
        //}




        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }



        public IActionResult List(PageParams<RoleDomain> pageParams, string keyword = null)
        {
            //Pagination<RoleDomain> domainPage = Svc.Role.GetPagination(keyword, pageParams);
            //ViewBag.Pagination = domainPage;

            ViewBag.Pagination = Pagination<RoleDomain>.Empty();

            return View();
        }



        [HttpGet]
        [UseViewPage("Form")]
        public IActionResult Create()
        {
            var domain = new RoleDomain
            {
                UserIds = new List<int>(),
            };
            return View(domain);
        }



        [HttpPost]
        [UseViewPage("Form")]
        public IActionResult Create(RoleDomain domain)
        {
            //TODO
            return View(domain); 
            
            if (!ModelState.IsValid) { return View(domain); }

            domain.ModifyBy = this.UserId();
            
            int roleId = Svc.Role.Save(domain);
            this.SetStatusSuccess("儲存成功!!");
            return RedirectToAction(nameof(Edit), new { roleId });
        }


        [HttpGet]
        [UseViewPage("Form")]
        public IActionResult Edit(int roleId)
        {
            RoleDomain domain = Svc.Role.GetById(roleId);
             
            return View(domain);
        }



        [HttpPost]
        [UseViewPage("Form")]
        public IActionResult Edit(RoleDomain domain)
        {
            if (!ModelState.IsValid) { return View(domain); }

            domain.ModifyBy = this.UserId();

            int roleId = Svc.Role.Save(domain);
            this.SetStatusSuccess("儲存成功!!");
            return RedirectToAction(nameof(Edit), new { roleId });
        }


    }
}