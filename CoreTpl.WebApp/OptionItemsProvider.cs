using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreTpl.Domain;
using CoreTpl.Enums;
using CoreTpl.Service;
using Microsoft.AspNetCore.Http;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.UI;

namespace CoreTpl.WebApp
{

    public interface IOptionItemsProvider
    {
        TplConfig TplConfig { get; }

        List<IBreadcrumb> Breadcrumb { get; }
        List<MenuItem> MenuAllowList { get; }
        List<MenuItem> MenuList { get; }


        Dictionary<int, string> RoleName { get; }
        Dictionary<int, string> UserFullName { get; }
        Dictionary<int, string> UserName { get; }


        Dictionary<string, bool> GetColumnStatus(int userId, string name);
    }




    public class OptionItemsProvider : IOptionItemsProvider
    {
        private readonly IMenuProvider _menuProvider;
        private readonly IBreadcrumbProvider _breadcrumbProvider;

        private readonly IServiceContext _svc;
        private readonly TplConfig _tplConfig;
        private readonly HttpContext _httpContext;

        public OptionItemsProvider(
            IMenuProvider menuProvider,
            IBreadcrumbProvider breadcrumbProvider,
            IHttpContextAccessor httpContextAccessor,
            IServiceContext svc,
            TplConfig tplConfig
        )
        {
            _menuProvider = menuProvider;
            _breadcrumbProvider = breadcrumbProvider;
            _httpContext = httpContextAccessor.HttpContext;
            _svc = svc;
            _tplConfig = tplConfig;
        }


        public TplConfig TplConfig
        {
            get { return _tplConfig; }
        }



        /*===========================================================*/

             



        /*===========================================================*/

        /// <summary>角色名稱</summary>
        public Dictionary<int, string> RoleName
        {
            get
            {
                return _svc.Role.GetPagination(null, null).List
                    .OrderBy(x => x.RoleName)
                    .ToDictionary(x => x.RoleId, x => x.RoleName);
            }
        }

        /// <summary>使用者名稱</summary>
        public Dictionary<int, string> UserName
        {
            get
            {
                return _svc.User.GetPagination(null, null).List
                    .OrderBy(x => x.UserName)
                    .ToDictionary(x => x.UserId, x => x.UserName);
            }
        }

        /// <summary>使用者完整名稱</summary>
        public Dictionary<int, string> UserFullName
        {
            get
            {
                return _svc.User.GetPagination(null, null).List
                    .OrderBy(x => x.UserName)
                    .ToDictionary(x => x.UserId, x => x.UserName + " " + x.Account);
            }
        }





        /*===========================================================*/

        public List<MenuItem> MenuList
        {
            get
            {
                return _menuProvider.GetList(_httpContext.User, _httpContext.Request.Path.Value);
            }
        }

        public List<MenuItem> MenuAllowList
        {
            get
            {
                return _menuProvider.GetAllowList(_httpContext.User, _httpContext.Request.Path.Value);
            }
        }




        /*===========================================================*/

        public List<IBreadcrumb> Breadcrumb
        {
            get
            {
                List<IBreadcrumb> list = _breadcrumbProvider.GetPathList(_httpContext.Request.Path.Value);
                return list;
            }
        }



        /*===========================================================*/

        /// <summary>使用者欄位狀態</summary>
        public Dictionary<string, bool> GetColumnStatus(int userId, string name)
        {
            try
            {
                string columnStatusJson = _svc.User.GetPreference(userId, "#Orderable_" + name);
                return columnStatusJson.JsonToObject<Dictionary<string, bool>>();
            }
            catch (OrionException)
            {
                return null;
            }
        }

    }
}