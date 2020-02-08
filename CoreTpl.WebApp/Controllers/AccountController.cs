using System.Collections.Generic;
using System.Security.Claims;
using CoreTpl.Domain;
using CoreTpl.Service;
using CoreTpl.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Extensions;

namespace CoreTpl.WebApp.Controllers
{
    //[SessionState(SessionStateBehavior.Required)]
    public class AccountController : Controller
    {
        public IServiceContext Svc { private get; set; }



        public IActionResult Index()
        {
            return RedirectToAction(nameof(Login));
        }



        private int signInCheck(string account, string password)
        {
            int userId;
            string statusMsg = "";
            try
            {
                userId = Svc.User.CheckPassword(account, password);
            }
            catch (OrionException ex)
            {
                statusMsg = ex.Message;
                throw;
            }
            finally
            {
                string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                string status = statusMsg.HasText() ? "error" : "success";
                Svc.User.AddSignInRecord(account, remoteIp, "Web", status, statusMsg);
            }

            return userId;
        }

        private void signInStore(int userId)
        {
            UserDomain user = Svc.User.GetById(userId);
            List<string> actList = Svc.User.GetHoldActList(userId);

            List<Claim> claims = actList.ToList(x => new Claim(ClaimTypes.Role, x));

            claims.Add(new Claim(OrionUser.UserId, "Jax"));
            claims.Add(new Claim(OrionUser.UserName, "Jax"));
            claims.Add(new Claim(OrionUser.Account, "Jax"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity)).Wait();
        }





        /// <summary>判斷某IP登入錯誤超過幾次需要驗證</summary>
        private bool isUseCaptchaValid()
        {
            string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
            int errorTimes = Svc.User.GetCurrentSignInErrors(remoteIp);
            return errorTimes >= 3;
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            /*排除已經登入沒有權限*/
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Forbidden", "Error"); }

            /*排除 Ajax*/
            if (Request.IsAjaxRequest())
            {
                //Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 403;
                return Content("請登入系統!!");
            }

            ViewBag.UseCaptchaValid = isUseCaptchaValid();

            var vm = new UserLoginViewModel
            {
            };
            return View(vm);
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(UserLoginViewModel vm, string returnUrl)
        {
            ViewBag.UseCaptchaValid = isUseCaptchaValid();
            //if (ViewBag.UseCaptchaValid == true && !this.IsCaptchaValid("驗證碼無效!"))
            //{ throw new OrionException("驗證碼無效!"); }

            if (!ModelState.IsValid) { return View(vm); }

            try
            {
                int userId = signInCheck(vm.Account, vm.Password);
                signInStore(userId);
            }
            catch
            {
                ViewBag.UseCaptchaValid = isUseCaptchaValid();
                throw;
            }

            if (isInvalidUrl(returnUrl)) { returnUrl = Url.Content("~/"); }
            return Redirect(returnUrl);
        }


        /// <summary>是否為無效的返回網址</summary>
        private bool isInvalidUrl(string returnUrl)
        {
            if (returnUrl.NoText()) { return true; }
            if (returnUrl.Contains("/Home/Logout")) { return true; }
            return false;
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return Content("OK");
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult CheckLogon()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Content("True");
            }
            else
            {
                //Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 403;
                return Content("False");
            }
        }



        public IActionResult ChangePassword(UserChangePasswordViewModel vm)
        {
            if (Request.IsGetMethod()) { return View(vm); }
            if (!ModelState.IsValid) { return View(vm); }

            Svc.User.CheckPassword(User.GetAccount(), vm.OldPassword);
            Svc.User.SetPassword(this.UserId(), vm.Password);

            this.SetStatusSuccess("儲存成功!!");
            return View(vm);
        }



    }
}