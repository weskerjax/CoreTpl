using CoreTpl.Domain;
using Orion.API.Models;

namespace CoreTpl.Dao
{
    public interface IRoleDao
    {
        Pagination<RoleDomain> GetPagination(WhereParams<RoleDomain> findParam, PageParams<RoleDomain> pageParams);
        RoleDomain GetById(int roleId);
        int Save(RoleDomain domain);
    }
}
