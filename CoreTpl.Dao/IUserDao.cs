using System.Collections.Generic;
using CoreTpl.Domain;
using Orion.API.Models;


namespace CoreTpl.Dao
{
    public interface IUserDao
    {
        Pagination<UserDomain> GetPagination(WhereParams<UserDomain> findParam, PageParams<UserDomain> pageParams);
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
