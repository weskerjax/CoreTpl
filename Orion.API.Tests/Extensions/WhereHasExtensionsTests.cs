using Orion.API.Tests;
using System.Collections.Generic;
using Xunit;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Orion.API.Extensions.Tests
{
	public class WhereHasExtensionsTests
	{

		private OrionApiDbContext _dc;

		public WhereHasExtensionsTests()
		{
			string mdfPath = Path.GetFullPath(@"..\..\OrionApi.mdf");
			string connection = $@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True";
			_dc = new OrionApiDbContext(new DbContextOptions<OrionApiDbContext>());
		}



		public static IEnumerable<object[]> RunTest_Data
		{
			get
			{
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "FF" }, " = @p0" 
				};
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "" }, "Table(" 
				};
			}
		}

		[Theory]
		[MemberData(nameof(RunTest_Data))]
		public void RunTest(InvoiceIssueDomain domain, string expected)
		{

			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix);
			var sql = query.ToString();

			Assert.Contains(expected, sql);	
		}


        [Fact]
        public void RunTest2()
        {
            var len = 22;

            var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix.Length == len);
            var sql = query.ToString();
                       
        }


        [Fact]
		public void NullTest()
		{
			var domain = new InvoiceIssueDomain();

			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix.ToString());
			var sql = query.ToString();

			Assert.Contains("Table(", sql);
		}


	}



	public class InvoiceIssueDomain
	{
		/// <summary>發票字軌</summary>
		public string InvoicePrefix { get; set; }

	}

}
