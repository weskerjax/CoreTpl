﻿using System.Collections.Generic;
using System.Security.Principal;
using Xunit;
using System.Linq;
using Orion.API.Extensions;

namespace Orion.Mvc.UI.Tests
{
	public class MenuProviderTests
	{

		[Fact]
		public void RunTest()
		{
			IPrincipal user = new MockPrincipal();

			var provider = new MenuProvider("UI/Menu.config");
			var list = provider.GetAllowList(user, "/User/Create");
			Assert.NotEmpty(list);
		}


		class MockPrincipal : IPrincipal
		{
			public IIdentity Identity { get { return null; } }
			public bool IsInRole(string role) { return true; }
		}

	}
}
