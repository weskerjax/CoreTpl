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
            string mdfPath = Path.GetFullPath(@"..\..\OrionApi.mdf");
            string connection = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True";
            _dc = new OrionApiDbContext(new DbContextOptions<OrionApiDbContext>());
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
                .ToString();

            Assert.Contains("ORDER BY [t0].[InvoiceId] DESC", sqlA);


            var sqlB = _dc.InvoiceIssue
                .AdvancedOrderBy("InvoiceId,-InvoicePrefix,InvoiceDate", true)
                .ToString();

            Assert.Contains("ORDER BY [t0].[InvoiceId], [t0].[InvoicePrefix] DESC, [t0].[InvoiceDate]", sqlB);
            
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
