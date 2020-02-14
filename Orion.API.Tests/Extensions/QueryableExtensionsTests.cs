using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Orion.API.Tests;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace Orion.API.Extensions.Tests
{
	public class QueryableExtensionsTests
	{
		private OrionApiDbContext _dc;

        public QueryableExtensionsTests()
        {
			_dc = OrionApiDbContext.CreateUseNpgsql();
        }


        [Fact]
		public void OrderBy_RunTest()
		{
			var list = new List<UserModel>
			{
				new UserModel{ UserId = 1 },
				new UserModel{ UserId = 2 },
			};

			list.AsQueryable()
                .OrderBy("UserId", true)
                .ToList();
		}

        [Fact]
        public void AdvancedOrderBy_RunTest()
        {
            var sqlA = _dc.InvoiceIssue
                .AdvancedOrderBy("InvoiceId", true)
                .ToSql();

            Assert.Contains("ORDER BY i.\"InvoiceId\" DESC", sqlA);


            var sqlB = _dc.InvoiceIssue
                .AdvancedOrderBy("InvoiceId,-InvoicePrefix,InvoiceDate", true)
                .ToSql();

            Assert.Contains("ORDER BY i.\"InvoiceId\", i.\"InvoicePrefix\" DESC, i.\"InvoiceDate\"", sqlB);
            
        }


        [Fact]
		public void MaxOrDefault_RunTest()
		{
			new List<UserModel> { }
                .AsQueryable()
                .MaxOrDefault(x => x.UserId);

			new List<UserModel> { new UserModel { UserId = 2 } }
                .AsQueryable()
                .MaxOrDefault(x => x.UserId);
		}

		[Fact]
		public void MinOrDefault_RunTest()
		{
			new List<UserModel> { }
                .AsQueryable()
                .MinOrDefault(x => x.UserId);

			new List<UserModel> { new UserModel{ UserId = 2 } }
                .AsQueryable()
                .MinOrDefault(x => x.UserId);
		}

	}

}
