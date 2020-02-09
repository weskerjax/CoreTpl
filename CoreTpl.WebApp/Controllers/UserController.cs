using System.Collections.Generic;
using System.Linq;
using CoreTpl.Domain;
using CoreTpl.Enums;
using CoreTpl.Service;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Extensions;
using Orion.API.Models;
using Orion.Mvc.Attributes;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{
    [ActAuthorize(ACT.UserSetting)]
    public class UserController : Controller
    {
        public IServiceContext Svc { private get; set; }


        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
        }



        public IActionResult List(WhereParams<UserDomain> findParam, PageParams<UserDomain> pageParams)
        {
            Pagination<UserDomain> domainPage = Svc.User.GetPagination(findParam, pageParams);
            ViewBag.Pagination = domainPage;

            return View();
        }


        public IActionResult Typeahead(string query)
        {
            var findParam = new WhereParams<UserDomain>().Assign(x => x.Account.Contains(query));

            Pagination<UserDomain> domainPage = Svc.User.GetPagination(findParam, new PageParams<UserDomain>
            {
                PageIndex = 1,
                PageSize = 40,
                OrderField = nameof(UserDomain.UserName),
                Descending = false,
            });

            var list = domainPage.List
                .Where(x => x.UserId != 1)
                .Select(x => new
                {
                    Value = x.UserId,
                    Label = x.UserName + " " + x.Account,
                });
            return Json(list);
        }




        [HttpGet]
        [UseViewPage("Form")]
        public IActionResult Create()
        {
            var domain = new UserDomain
            {
                RoleIds = new List<int>(),
            };

            return View(domain);
        }


        [HttpPost]
        [UseViewPage("Form")]
        public IActionResult Create(UserDomain domain)
        {
            if (!ModelState.IsValid) { return View(domain); }

            domain.ModifyBy = this.UserId();

            int userId = Svc.User.Save(domain);
            this.SetStatusSuccess("儲存成功!!");
            return RedirectToAction(nameof(Edit), new { userId });
        }



        public IActionResult IsAccountNotExist(string account)
        {
            bool notExists = false;
            try
            { Svc.User.GetByAccount(account); }
            catch (OrionException)
            { notExists = true; }

            return Json(notExists);
        }




        [HttpGet]
        [UseViewPage("Form")]
        public IActionResult Edit(int userId)
        {
            ViewBag.HoldActList = Svc.User.GetHoldActList(userId);

            UserDomain domain = Svc.User.GetById(userId);             

            return View(domain);
        }



        [HttpPost]
        [UseViewPage("Form")]
        public IActionResult Edit(UserDomain domain)
        {
            ViewBag.HoldActList = Svc.User.GetHoldActList(domain.UserId);

            if (!ModelState.IsValid) { return View(domain); }

            domain.ModifyBy = this.UserId();

            int userId = Svc.User.Save(domain);

            this.SetStatusSuccess("儲存成功!!");
            return RedirectToAction(nameof(Edit), new { userId });
        }




        public IActionResult SetPassword(UserSetPasswordViewModel vm)
        {
            if (Request.IsGetMethod()) { return View(vm); }
            if (!ModelState.IsValid) { return View(vm); }

            Svc.User.SetPassword(vm.UserId, vm.Password);
            this.SetStatusSuccess("儲存成功!!");
            return View(vm);
        }
        



        [HttpGet]
        public IActionResult ActEdit(int userId)
        {
            UserActDomain domain = Svc.User.GetSelfAct(userId);
            var list = domain.RoleActList.Concat(domain.AllowActList).Except(domain.DenyActList);

            var vm = new UserActViewModel
            {
                UserId = domain.UserId,
                UserName = domain.UserName,
                Account = domain.Account,
                RoleActList = domain.RoleActList,
                ActList = list.ToList(),
                CreateBy = domain.CreateBy,
                CreateDate = domain.CreateDate,
                ModifyBy = domain.ModifyBy,
                ModifyDate = domain.ModifyDate,
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult ActEdit(UserActViewModel vm)
        {
            UserActDomain domain = Svc.User.GetSelfAct(vm.UserId);

            domain.AllowActList = vm.ActList.Except(domain.RoleActList).ToList();
            domain.DenyActList = domain.RoleActList.Except(vm.ActList).ToList();
            domain.ModifyBy = this.UserId();

            Svc.User.SetSelfAct(domain);
            this.SetStatusSuccess("儲存成功!!");
            return RedirectToAction(nameof(ActEdit), new { vm.UserId });
        }



    }
}