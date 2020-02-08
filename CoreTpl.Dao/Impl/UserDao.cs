using Orion.API;
using Orion.API.Extensions;
using Orion.API.Models;
using CoreTpl.Dao.Database;
using CoreTpl.Domain;
using CoreTpl.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreTpl.Dao.Impl
{
    public class UserDao : IUserDao
    {
        private readonly TplDbContext _dc;

        public UserDao(TplDbContext dc)
        {
            _dc = dc;
        }



        private UserDomain toDomain(UserInfo data, Func<int, List<int>> getRoleIds)
        {
            if (data == null) { return null; }

            return new UserDomain
            {
                UserId = data.UserId,
                Account = data.Account,
                UserName = data.UserName,
                Email = data.Email,
                UseStatus = data.UseStatus.ToEnum<UseStatus>(),
                Department = data.Department,
                ExtensionNum = data.ExtensionNum,
                UserTitle = data.UserTitle,
                RemarkText = data.RemarkText,
                CreateBy = data.CreateBy,
                CreateDate = data.CreateDate,
                ModifyBy = data.ModifyBy,
                ModifyDate = data.ModifyDate,
                RoleIds = getRoleIds(data.UserId),
            };
        }



        private Func<int, List<int>> getRoleIdsProvider(int preUserId = 0)
        {
            return _dc.UserRole
                .WhereHas(x => x.UserId == preUserId)
                .OrderBy(x => x.RoleId)
                .PrepareList(x => x.UserId, x => x.RoleId);
        }



        public Pagination<UserDomain> GetPagination(string keyword, PageParams<UserDomain> pageParams)
        {
            IQueryable<UserInfo> query = _dc.UserInfo;


            if (keyword.HasText())
            {
                keyword = keyword.Trim();

                query = query.Where(q =>
                    q.Account.Contains(keyword) ||
                    q.UserName.Contains(keyword) ||
                    q.UserTitle.Contains(keyword)
                );
            }


            pageParams = pageParams.NullToUnlimited();

            bool isDesc = pageParams.Descending;
            try
            {
                query = query.AdvancedOrderBy(pageParams.OrderField, isDesc);
            }
            catch (Exception)
            {
                query = query.OrderBy(x => x.UserId, isDesc);
            }

            Func<int, List<int>> getRoleIds = getRoleIdsProvider();

            var result = query.AsPagination(pageParams.PageIndex, pageParams.PageSize);
            return result.As(x => toDomain(x, getRoleIds));
        }






        public UserDomain GetById(int userId)
        {
            UserInfo data = _dc.UserInfo.FirstOrDefault(x => x.UserId == userId);
            if (data == null) { return null; }

            Func<int, List<int>> getRoleIds = getRoleIdsProvider(data.UserId);
            UserDomain domain = toDomain(data, getRoleIds);
            return domain;
        }


        public UserDomain GetByAccount(string account)
        {
            UserInfo data = _dc.UserInfo.FirstOrDefault(x => x.Account == account.ToLower());
            if (data == null) { return null; }

            Func<int, List<int>> getRoleIds = getRoleIdsProvider(data.UserId);
            UserDomain domain = toDomain(data, getRoleIds);
            return domain;
        }





        public int Save(UserDomain domain)
        {
            UserInfo data;

            if (domain.UserId > 0)
            {
                data = _dc.UserInfo.FirstOrDefault(x => x.UserId == domain.UserId);
                Checker.Has(data, "使用者不存在無法修改");
            }
            else
            {
                data = new UserInfo
                {
                    Account = domain.Account.ToLower(),
                    CreateBy = domain.ModifyBy,
                    CreateDate = DateTime.Now,
                };
                _dc.UserInfo.Add(data);
            }


            data.UserName = domain.UserName;
            data.Department = domain.Department;
            data.ExtensionNum = domain.ExtensionNum;
            data.UserTitle = domain.UserTitle;
            data.Email = domain.Email;
            data.UseStatus = domain.UseStatus.ToString();
            data.RemarkText = domain.RemarkText;
            data.ModifyBy = domain.ModifyBy;
            data.ModifyDate = DateTime.Now;


            /*使用者角色對應處理*/
            _dc.UserRole.RemoveRange(data.UserRole.ToList());

            var roleList = domain.RoleIds.NullToEmpty().Select(x => new UserRole
            {
                RoleId = x,
                CreateBy = domain.ModifyBy,
                CreateDate = DateTime.Now,
                ModifyBy = domain.ModifyBy,
                ModifyDate = DateTime.Now,
            });
            data.UserRole.AddRange(roleList);


            _dc.SaveChanges();

            return data.UserId;
        }





        public string GetPassword(int userId)
        {
            return _dc.UserInfo.Where(x => x.UserId == userId).Select(x => x.Password).FirstOrDefault();
        }

        public void SetPassword(int userId, string password)
        {
            UserInfo data = _dc.UserInfo.FirstOrDefault(x => x.UserId == userId);
            if (data == null) { return; }

            data.Password = password;

            _dc.SaveChanges();
        }



        /// <summary>
        /// 使用者本身的權限
        /// </summary>
        public UserActDomain GetSelfAct(int userId)
        {
            UserInfo data = _dc.UserInfo.FirstOrDefault(x => x.UserId == userId);
            Checker.Has(data, "帳號不存在！");

            List<string> roleAct = _dc.UserInfo
                    .Where(x => x.UserId == userId)
                    .SelectMany(x => x.UserRole.Select(y => y.RoleInfo.AllowActList))
                    .AsEnumerable()
                    .SelectMany(x => OrionUtils.ToIdsList<string>(x))
                    .Distinct()
                    .ToList();

            List<string> allowActList = OrionUtils.ToIdsList<string>(data.AllowActList);
            List<string> denyActList = OrionUtils.ToIdsList<string>(data.DenyActList);

            var domain = new UserActDomain
            {
                UserId = data.UserId,
                UserName = data.UserName,
                Account = data.Account,
                CreateBy = data.ModifyBy,
                CreateDate = data.CreateDate,
                ModifyBy = data.ModifyBy,
                ModifyDate = data.ModifyDate,
                RoleActList = roleAct,
                AllowActList = allowActList,
                DenyActList = denyActList,
            };

            return domain;
        }



        public void SetSelfAct(UserActDomain domain)
        {

            UserInfo data = _dc.UserInfo.FirstOrDefault(x => x.UserId == domain.UserId);
            Checker.Has(data, "帳號不存在！");

            data.AllowActList = OrionUtils.ToIdsString(domain.AllowActList);
            data.DenyActList = OrionUtils.ToIdsString(domain.DenyActList);
            data.ModifyBy = domain.ModifyBy;
            data.ModifyDate = DateTime.Now;

            _dc.SaveChanges();
        }




        public void AddSignInRecord(string account, string signInIp, string signInType, string statusCode, string statusMsg)
        {
            var data = new UserSignInRecord
            {
                Account = account.ToLower().LimitLength(256),
                SignInIp = signInIp.LimitLength(48),
                SignInType = signInType.LimitLength(32),
                StatusCode = statusCode.LimitLength(32),
                StatusMsg = statusMsg.LimitLength(32),
                CreateDate = DateTime.Now,
            };
            _dc.UserSignInRecord.Add(data);

            var expiry = DateTime.Today.AddDays(-180);
            _dc.UserSignInRecord.RemoveRange(x => x.CreateDate < expiry);

            _dc.SaveChanges();
        }



        public int GetCurrentSignInErrors(string signInIp)
        {
            UserSignInRecord prevSuccess = _dc.UserSignInRecord
                .Where(x => x.SignInIp == signInIp)
                .Where(x => x.StatusCode == "success")
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            int searchStartId = prevSuccess == null ? 0 : prevSuccess.Id;
            int times = _dc.UserSignInRecord
                .Where(x => x.SignInIp == signInIp)
                .Where(x => x.StatusCode == "error")
                .Where(x => x.Id > searchStartId)
                .Count();

            return times;
        }



        public Dictionary<string, string> GetPreferenceItems(int userId)
        {
            Dictionary<string, string> items = _dc.UserPreference
                .Where(x => x.UserId == userId)
                .Select(x => new { x.Name, x.Value })
                .ToDictionary(x => x.Name, x => x.Value);

            return items;
        }


        public string GetPreference(int userId, string name)
        {
            return _dc.UserPreference
                .Where(x => x.UserId == userId)
                .Where(x => x.Name == name)
                .Select(x => x.Value)
                .FirstOrDefault();
        }


        public void SavePreference(int userId, string name, string value)
        {
            UserPreference data = _dc.UserPreference
                .Where(x => x.UserId == userId)
                .Where(x => x.Name == name)
                .FirstOrDefault();

            if (data == null)
            {
                data = new UserPreference
                {
                    UserId = userId,
                    Name = name,
                    CreateDate = DateTime.Now,
                };
                _dc.UserPreference.Add(data);
            }

            data.Value = value;
            data.ModifyDate = DateTime.Now;

            _dc.SaveChanges();
        }




    }
}
