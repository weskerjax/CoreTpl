﻿using Orion.API.Extensions;
using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Orion.API.Tests
{



	public class WhereBuilderTests
	{

		private OrionApiDbContext _dc;

		public WhereBuilderTests()
		{
			_dc = new OrionApiDbContext();
		}



		/*===========================================================================*/

		[Fact]		
		public void NullTest()
		{
			WhereParams<InvoiceIssueDomain> param = null;

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix);

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.NotEmpty(sql);
		}







		/*===========================================================================*/

		public static IEnumerable<object[]> StringValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, " IN (" };
				yield return new object[] { WhereOperator.NotIn, " NOT IN (" };
				yield return new object[] { WhereOperator.Equals, " = " };
				yield return new object[] { WhereOperator.NotEquals, " <> " };
				yield return new object[] { WhereOperator.Contains, " STRPOS" };
				yield return new object[] { WhereOperator.StartsWith, " LIKE " };
				yield return new object[] { WhereOperator.EndsWith, " LIKE " };
				yield return new object[] { WhereOperator.LessThan, " < " };
				yield return new object[] { WhereOperator.LessEquals, " <= " };
				yield return new object[] { WhereOperator.GreaterThan, " > " };
				yield return new object[] { WhereOperator.GreaterEquals, " >= " };
				yield return new object[] { WhereOperator.Between, " i\r\nORDER" };
			}
		}


		[Theory]
		[MemberData(nameof(StringValueTest_Data))]
		public void StringValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.InvoicePrefix, oper, "SS");

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix);

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.Contains(expected, sql);
		}




		/*===========================================================================*/

		public static IEnumerable<object[]> IntValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (" };
				yield return new object[] { WhereOperator.NotIn, " NOT IN (" };
				yield return new object[] { WhereOperator.Equals, " = " };
				yield return new object[] { WhereOperator.NotEquals, " <> " };
				yield return new object[] { WhereOperator.Contains, " i\r\nORDER" };
				yield return new object[] { WhereOperator.StartsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.EndsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.LessThan, " < " };
				yield return new object[] { WhereOperator.LessEquals, " <= " };
				yield return new object[] { WhereOperator.GreaterThan, " > " };
				yield return new object[] { WhereOperator.GreaterEquals, " >= " };
				yield return new object[] { WhereOperator.Between, " >= " };
			}
		}


		[Theory]
		[MemberData(nameof(IntValueTest_Data))]
		public void IntValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, oper, 1, 3);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.ProductQty, y => y.InvoiceNum);

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/



		public static IEnumerable<object[]> DateTimeValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, " IN (" };
				yield return new object[] { WhereOperator.NotIn, " NOT IN (" };
				yield return new object[] { WhereOperator.Equals, " = " };
				yield return new object[] { WhereOperator.NotEquals, " <> " };
				yield return new object[] { WhereOperator.Contains, " i\r\nORDER" };
				yield return new object[] { WhereOperator.StartsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.EndsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.LessThan, " < " };
				yield return new object[] { WhereOperator.LessEquals, " <= " };
				yield return new object[] { WhereOperator.GreaterThan, " > " };
				yield return new object[] { WhereOperator.GreaterEquals, " >= " };
				yield return new object[] { WhereOperator.Between, " >= " };
			}
		}


		[Theory]
		[MemberData(nameof(DateTimeValueTest_Data))]
		public void DateTimeValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ModifyDate, oper, DateTime.Today, DateTime.Now);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.ModifyDate, y => y.ModifyDate);

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.Contains(expected, sql);
		}


		[Fact]
		public void DateTimeRunTest()
		{
			Expression<Func<InvoiceIssue, bool>> predicate = x => x.ModifyDate == DateTime.Today;

			var values = new List<DateTime?> { DateTime.Today, DateTime.Now };
			var query = _dc.InvoiceIssue.Where(x => values .Contains( x.ModifyDate ));
			//var list = query.ToList();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.True(true);
		}



		/*===========================================================================*/

		public static IEnumerable<object[]> DecimalValueTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, " IN (" };
				yield return new object[] { WhereOperator.NotIn, " NOT IN (" };
				yield return new object[] { WhereOperator.Equals, " = " };
				yield return new object[] { WhereOperator.NotEquals, " <> " };
				yield return new object[] { WhereOperator.Contains, " i\r\nORDER" };
				yield return new object[] { WhereOperator.StartsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.EndsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.LessThan, " < " };
				yield return new object[] { WhereOperator.LessEquals, " <= " };
				yield return new object[] { WhereOperator.GreaterThan, " > " };
				yield return new object[] { WhereOperator.GreaterEquals, " >= " };
				yield return new object[] { WhereOperator.Between, " >= " };
			}
		}


		[Theory]
		[MemberData(nameof(DecimalValueTest_Data))]
		public void DecimalValueTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.Sum, oper, 1.0m, 3.0m);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind(x => x.Sum, y => y.Total);

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		public static IEnumerable<object[]> SubQueryTest_Data
		{
			get
			{
				yield return new object[] { WhereOperator.In, "IN (" };
				yield return new object[] { WhereOperator.NotIn, " IN (" };
				yield return new object[] { WhereOperator.Equals, "EXISTS (" };
				yield return new object[] { WhereOperator.NotEquals, "NOT (EXISTS (" };
				yield return new object[] { WhereOperator.Contains, " i\r\nORDER" };
				yield return new object[] { WhereOperator.StartsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.EndsWith, " i\r\nORDER" };
				yield return new object[] { WhereOperator.LessThan, " < " };
				yield return new object[] { WhereOperator.LessEquals, " <= " };
				yield return new object[] { WhereOperator.GreaterThan, " > " };
				yield return new object[] { WhereOperator.GreaterEquals, " >= " };
				yield return new object[] { WhereOperator.Between, " >= " };
			}
		}


		[Theory]
		[MemberData(nameof(SubQueryTest_Data))]
		public void SubQueryTest(WhereOperator oper, string expected)
		{
			var param = new WhereParams<InvoiceIssueDomain>();
			param.SetValues(x => x.ProductQty, oper, 1, 3);

			var build = new WhereBuilder<InvoiceIssue, InvoiceIssueDomain>(_dc.InvoiceIssue, param);

			build.WhereBind<int?>(x => x.ProductQty, y => y.InvoiceIssueItems.Select(z => (int?)z.Qty));

			var query = build.Build();
			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			Assert.Contains(expected, sql);
		}





		/*===========================================================================*/

		[Fact]
		public void FullRunTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.SetValues(x => x.UseStatus,	WhereOperator.Equals, UseStatus.Enable)
				.SetValues(x => x.InvoicePrefix, WhereOperator.Equals, "SS")
				.SetValues(x => x.ProductQty,	WhereOperator.Equals, 1)
				.SetValues(x => x.ModifyDate,	WhereOperator.Equals, DateTime.Today)
				.SetValues(x => x.Sum,			WhereOperator.Equals, 1.0m, 3.0m);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.UseStatus, y => y.InvoicePrefix)
				.WhereBind(x => x.InvoicePrefix,	y => y.InvoicePrefix)
				.WhereBind(x => x.ProductQty, y => y.InvoiceId)
				.WhereBind(x => x.ModifyDate,		y => y.ModifyDate)
				.WhereBind(x => x.Sum,			y => y.Total)
				.WhereBind(x => x.ProductQty,		y => y.InvoiceIssueItems.Select(z => (int?)z.Qty))
				.WhereBind(x => x.Sum,			y => y.InvoiceIssueItems.Select(z => z.Price))
				.Build();

			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			query.ToList();


			Assert.True(true);
		}





		/*===========================================================================*/

		[Fact]
		public void ListTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.SetValues(x => x.RoleIds, WhereOperator.Equals, 1);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.RoleIds, y => y.InvoiceIssueItems.Select(z => z.Qty))
				.Build();

			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			query.ToList();


			Assert.True(true);
		}





		/*===========================================================================*/

		[Fact]
		public void ConvertTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.SetValues(x => x.UseStatus, WhereOperator.Equals, UseStatus.Enable)
				.SetValues(x => x.ProductQty, WhereOperator.Equals, 1);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.UseStatus, y => y.InvoicePrefix)
				.WhereBind(x => x.ProductQty * 100, y => y.InvoiceId)
				.Build();

			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			query.ToList();

			Assert.True(true);
		}





		/*===========================================================================*/

		[Fact]
		public void StringTest()
		{
			var param = new WhereParams<InvoiceIssueDomain>()
				.Assign(x => x.InvoicePrefix == null);


			var query = _dc.InvoiceIssue.WhereBuilder(param)
				.WhereBind(x => x.UseStatus, y => y.InvoicePrefix)
				.WhereBind(x => x.InvoicePrefix, y => y.InvoicePrefix)
				.Build();

			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			query.ToList();


			Assert.True(true);
		}



        [Fact]
        public void DateTimeOffsetTest()
        {
            var param = new WhereParams<InvoiceIssueDomain>()
                .Assign(x => x.ModifyDate2 > DateTimeOffset.Now.AddMonths(-1));


            var query = _dc.InvoiceIssue.WhereBuilder(param)
                .WhereBind(x => x.ModifyDate2.DateTime, y => y.ModifyDate)
                .Build();

			var sql = query.OrderBy(x => x.CreateBy).ToSql();

			query.ToList();


            Assert.True(true);
        }



    }




    public enum UseStatus
	{
		Enable,
		Disable,

	}

	public class InvoiceIssueDomain
	{
		public int? ProductQty { get; set; }
		public decimal Sum { get; set; }
		public string InvoicePrefix { get; set; }
		public UseStatus UseStatus { get; set; }
		public int ModifyBy { get; set; }
		public DateTime ModifyDate { get; set; }
		public List<int> RoleIds { get; set; }

        public DateTimeOffset ModifyDate2 { get; set; }

    }

}
