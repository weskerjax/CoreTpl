using System;
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

    [ActAuthorize(ACT.RoleSetting)]
    public class RoleController : Controller
    {
        public IServiceContext Svc { private get; set; }



        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }




        [TplSearchRemember]
        public IActionResult List(WhereParams<RoleDomain> findParam, PageParams<RoleDomain> pageParams, bool export = false)
        {
            if (export) { pageParams.PageIndex = 1; pageParams.PageSize = -1; }

            Pagination<RoleDomain> domainPage = Svc.Role.GetPagination(findParam, pageParams);
            ViewBag.Pagination = domainPage;

            if (!export) { return View(); }
            return this.ExcelView($"角色-{DateTime.Now:yyyyMMddHHmmss}.xls");
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