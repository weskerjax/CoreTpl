using System.Collections.Generic;
using System.ServiceModel;
using Orion.API.Models;
using CoreTpl.Domain;

namespace CoreTpl.Service
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        Pagination<UserDomain> GetPagination(string keyword, PageParams<UserDomain> pageParams);

        [OperationContract]
        UserDomain GetById(int userId);

        [OperationContract]
        UserDomain GetByAccount(string account);

        [OperationContract]
        int Save(UserDomain domain);



        [OperationContract]
        int CheckPassword(string account, string password);

        [OperationContract]
        void SetPassword(int userId, string password);

        [OperationContract]
        List<string> GetHoldActList(int userId);



        [OperationContract]
        UserActDomain GetSelfAct(int userId);

        [OperationContract]
        void SetSelfAct(UserActDomain domain);


        [OperationContract]
        void AddSignInRecord(string account, string signInIp, string signInType, string statusCode, string statusMsg);

        [OperationContract]
        int GetCurrentSignInErrors(string signInIp);

        [OperationContract]
        Dictionary<string, string> GetPreferenceItems(int userId);

        [OperationContract]
        string GetPreference(int userId, string name);

        [OperationContract]
        void SavePreference(int userId, string name, string value);

    }
}
