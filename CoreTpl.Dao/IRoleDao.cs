using Orion.API.Models;
using CoreTpl.Domain;
using CoreTpl.Enums;
using System.Collections.Generic;

namespace CoreTpl.Dao
{
    public interface IRoleDao
    {
        Pagination<RoleDomain> GetPagination(string keyword, PageParams<RoleDomain> pageParams);
        RoleDomain GetById(int roleId);
        int Save(RoleDomain domain);
    }
}
