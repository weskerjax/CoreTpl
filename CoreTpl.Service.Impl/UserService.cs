using Orion.API;
using Orion.API.Models;
using CoreTpl.Dao;
using CoreTpl.Domain;
using CoreTpl.Enums;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CoreTpl.Service.Impl
{
    public class UserService : IUserService
    {

        private readonly IUserDao _userDao;
        private readonly IPasswordHandle _passwordHandle;

        public UserService(IUserDao userDao, IPasswordHandle passwordHandle)
        {
            _userDao = userDao;
            _passwordHandle = passwordHandle;
        }


        public Pagination<UserDomain> GetPagination(WhereParams<UserDomain> findParam, PageParams<UserDomain> pageParams)
        {
            using (OrionUtils.TransactionReadUncommitted())
            {
                return _userDao.GetPagination(findParam, pageParams);
            }
        }


        public UserDomain GetById(int userId)
        {
            var domain = _userDao.GetById(userId);
            if (domain == null) { throw new OrionNoDataException("使用者不存在"); }

            return domain;
        }



        public UserDomain GetByAccount(string account)
        {
            var domain = _userDao.GetByAccount(account);
            if (domain == null) { throw new OrionNoDataException("使用者不存在"); }

            return domain;
        }



        public int Save(UserDomain domain)
        {
            Checker.Has(domain.Account, "帳號不可以為空");
            Checker.Has(domain.UserName, "姓名不可以為空");
            Checker.Has(domain.Department, "部門不可以為空");

            if (domain.UserId == 0)
            {
                UserDomain user = _userDao.GetByAccount(domain.Account);
                if (user != null) { throw new OrionException("帳號已經存在"); }
            }

            domain.UserType = UserType.Manager;

            return _userDao.Save(domain);
        }




        public int CheckPassword(string account, string password)
        {
            UserDomain user = _userDao.GetByAccount(account);
            Checker.Has(user, "帳號不存在！");
            Checker.Is(user.UseStatus, UseStatus.Enable, "帳號停用！");

            string orgPasswd = _userDao.GetPassword(user.UserId);
            string encrypt = _passwordHandle.Encrypt(password);
            Checker.Is(orgPasswd, encrypt, "密碼錯誤！");

            return user.UserId;
        }



        public void SetPassword(int userId, string password)
        {
            if (!_passwordHandle.Validate(password))
            {
                throw new OrionException("密碼必須是[英數大小寫]且不能低於 6 個字！");
            }

            string encrypt = _passwordHandle.Encrypt(password);
            _userDao.SetPassword(userId, encrypt);
        }



        public List<string> GetHoldActList(int userId)
        {
            UserActDomain act = _userDao.GetSelfAct(userId);
            var list = act.RoleActList.Concat(act.AllowActList).Except(act.DenyActList);
            return list.ToList();
        }




        public UserActDomain GetSelfAct(int userId)
        {
            return _userDao.GetSelfAct(userId);
        }

        public void SetSelfAct(UserActDomain domain)
        {
            _userDao.SetSelfAct(domain);
        }




        public void AddSignInRecord(string account, string signInIp, string signInType, string statusCode, string statusMsg)
        {
            _userDao.AddSignInRecord(account, signInIp, signInType, statusCode, statusMsg);
        }



        public int GetCurrentSignInErrors(string signInIp)
        {
            return _userDao.GetCurrentSignInErrors(signInIp);
        }



        public Dictionary<string, string> GetPreferenceItems(int userId)
        {
            using (OrionUtils.TransactionReadUncommitted())
            {
                return _userDao.GetPreferenceItems(userId);
            }
        }

        public string GetPreference(int userId, string name)
        {
            return _userDao.GetPreference(userId, name);
        }


        public void SavePreference(int userId, string name, string value)
        {
            _userDao.SavePreference(userId, name, value);
        }


    }
}
