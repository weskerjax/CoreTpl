﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Orion.API.Extensions;

namespace Orion.Mvc.Attributes
{
	/// <summary>ACT 驗證使用者權限</summary>
	public class ActAuthorizeAttribute : AuthorizeAttribute
    {
		/// <summary>ACT 驗證使用者權限</summary>
		public ActAuthorizeAttribute() { }

		/// <summary>ACT 驗證使用者權限</summary>
		public ActAuthorizeAttribute(params object[] actLits)
        {
            if (actLits.Length == 0) { return; }
         
            Roles = actLits.Select(x => x.ToString()).JoinBy(",");
        }


	}
}