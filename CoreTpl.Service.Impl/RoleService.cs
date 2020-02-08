using Orion.API;
using Orion.API.Models;
using CoreTpl.Dao;
using CoreTpl.Domain;
using CoreTpl.Enums;
using System.Collections.Generic;

namespace CoreTpl.Service.Impl
{
	public class RoleService : IRoleService
	{
		private readonly IRoleDao _roleDao;

		public RoleService(IRoleDao roleDao)
		{
			_roleDao = roleDao;
		}



		public Pagination<RoleDomain> GetPagination(string keyword, PageParams<RoleDomain> pageParams)
		{
            using (OrionUtils.TransactionReadUncommitted())
            {
                return _roleDao.GetPagination(keyword, pageParams);
            }
		}

		public RoleDomain GetById(int roleId)
		{
			var domain = _roleDao.GetById(roleId);
			if (domain == null) { throw new OrionNoDataException("角色資料不存在"); }

			return domain;
		}


		public int Save(RoleDomain domain)
		{
			Checker.Has(domain.RoleName, "角色名稱不可以為空");


			if (domain.UseStatus != UseStatus.Enable && (domain.UserIds != null && domain.UserIds.Count > 0))
			{
				throw new OrionException("角色還有使用者不可以關閉");
			}

			return _roleDao.Save(domain);
		}

	}
}
