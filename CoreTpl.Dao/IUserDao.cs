using Orion.API.Models;
using CoreTpl.Domain;
using CoreTpl.Enums;
using System;
using System.Collections.Generic;


namespace CoreTpl.Dao
{
    public interface IUserDao
    {
        Pagination<UserDomain> GetPagination(string keyword, PageParams<UserDomain> pageParams);
        UserDomain GetById(int userId);
        UserDomain GetByAccount(string account);
        int Save(UserDomain domain);

        string GetPassword(int userId);
        void SetPassword(int userId, string password);

        UserActDomain GetSelfAct(int userId);
        void SetSelfAct(UserActDomain domain);

        void AddSignInRecord(string account, string signInIp, string signInType, string statusCode, string statusMsg);

        int GetCurrentSignInErrors(string signInIp);

        Dictionary<string, string> GetPreferenceItems(int userId);

        string GetPreference(int userId, string name);

        void SavePreference(int userId, string name, string value);
    }
}
