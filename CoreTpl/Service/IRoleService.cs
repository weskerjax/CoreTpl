using System.ServiceModel;
using Orion.API.Models;
using CoreTpl.Domain;

namespace CoreTpl.Service
{
    [ServiceContract]
    public interface IRoleService
    {
                
        [OperationContract]
        Pagination<RoleDomain> GetPagination(WhereParams<RoleDomain> findParam, PageParams<RoleDomain> pageParams);
        
        [OperationContract]
        RoleDomain GetById(int roleId);
        
        [OperationContract]
        int Save(RoleDomain domain);


    }
}
