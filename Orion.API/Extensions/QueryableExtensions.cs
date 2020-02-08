﻿using Orion.API.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Orion.API.Extensions
{
	/// <summary>定義 IQueryable&lt;TSource&gt; 的 Extension</summary>
	public static class QueryableExtensions
	{
		
		/// <summary>從 IQueryable 建立 List</summary>
		public static List<TResult> ToList<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			return source.Select(selector).ToList();
		}

		/// <summary>從 IQueryable 建立 List</summary>
		public static TResult[] ToArray<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			return source.Select(selector).ToArray();
		}




		/*====================================================*/
		 

		/// <summary> OrderBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool descending) 
		{
			return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
		}

		/// <summary> ThenBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool descending)
		{
			return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
		}


		/// <summary> OrderBy 擴充，可直接使用 Enum Field 排序，Field Name 必須與 Column Name 一致</summary>
		public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, Enum keySelector, bool descending)
		{
			return OrderBy(source, keySelector.ToString(), descending);
		}

		/// <summary> OrderBy 擴充</summary>
		/// <param name="source"></param>
		/// <param name="keySelector">field Name, propertyName</param>
		/// <param name="descending">true Asc; false Desc</param>
		/// <returns></returns>
		public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string keySelector, bool descending)
		{
			Type modelType = typeof(TSource);
			var prop = modelType.GetProperty(keySelector);
			if (prop == null) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }

			MethodInfo orderByMethod = Utils.QueryableOrderByMethod(modelType, prop.PropertyType);
			var keyExpression = Utils.KeyExpression(modelType, prop);

			return (IOrderedQueryable<TSource>)orderByMethod.Invoke(null, new object[] { source, keyExpression, descending });
		}




		/// <summary> OrderBy 擴充</summary>
		/// <param name="source"></param>
		/// <param name="keySelector">field Name, propertyName</param>
		/// <param name="descending">true Asc; false Desc</param>
		/// <returns></returns>
		public static IOrderedQueryable<TSource> AdvancedOrderBy<TSource>(this IQueryable<TSource> source, string keySelector, bool descending)
		{
			Type modelType = typeof(TSource);

			var columns = keySelector.Split(',')
				.Select(x => x.Trim())
				.Where(x => x.HasText())
				.Select(x => new
				{
					Prop = modelType.GetProperty(x.Trim('-')),
					Descending = x.StartsWith("-"),
				})
				.Where(x => x.Prop != null)
				.Distinct(x => x.Prop.Name)
				.ToList();

			if (columns.Count == 0) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }


			var first = columns.Shift();
			if (first.Descending || columns.Count > 0) { descending = first.Descending; }
			IOrderedQueryable<TSource> ordered = OrderBy(source, first.Prop.Name, descending);


			foreach (var column in columns)
			{
				Type propType = column.Prop.PropertyType;
				MethodInfo thenByMethod = Utils.QueryableThenByMethod(modelType, propType);
				var keyExpression = Utils.KeyExpression(modelType, column.Prop);

				ordered = (IOrderedQueryable<TSource>)thenByMethod.Invoke(null, new object[] { ordered, keyExpression, column.Descending });
			}

			return ordered;
		}





		/*====================================================*/

		/// <summary>避免 SqlParameter 參數數量超過 2100 個，將查詢 IN 分次執行。</summary>
		public static IEnumerable<TSource> WhereIn<TSource, T>(this IQueryable<TSource> source, Expression<Func<TSource, T>> selector, IEnumerable<T> values, int blockSize = 2000)
		{
			if (values == null || !values.Any()) { return Enumerable.Empty<TSource>(); }

			MethodInfo containsMethod = LambdaUtils.GetMethod(() => values.Contains(default(T)));

			IEnumerable<TSource> result = values
				.Distinct()
				.BulkToList(blockSize)
				.SelectMany(valueList => 
				{
					var containsExpr = Expression.Call(containsMethod, Expression.Constant(valueList), selector.Body);
					return source.Where(Expression.Lambda<Func<TSource, bool>>(containsExpr, selector.Parameters));
				});

			return result;
		}






		/*====================================================*/


		private static Expression<Func<TSource, TResult?>> convertToNullable<TSource, TResult>(Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			Type nType = typeof(Nullable<>).MakeGenericType(typeof(TResult));
			var convertExpr = Expression.Convert(selector.Body, nType);
			var selectorExpr = Expression.Lambda<Func<TSource, TResult?>>(convertExpr, selector.Parameters);

			return selectorExpr;
		}



		/// <summary>傳回最大的結果值；空集合則傳回預設值。</summary>
		public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source) where TSource : struct
		{
			return source.MaxOrDefault(x => x);
		}

		/// <summary>傳回最大的結果值；空集合則傳回預設值。</summary>
		public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.Max(convertToNullable(selector)) ?? default(TResult);
		}



		/// <summary>傳回最小的結果值；空集合則傳回預設值。</summary>
		public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source) where TSource : struct
		{
			return source.MinOrDefault(x => x);
		}

		/// <summary>傳回最小的結果值；空集合則傳回預設值。</summary>
		public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.Min(convertToNullable(selector)) ?? default(TResult);
		}



		/*====================================================*/

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault(this IQueryable<int> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault(this IQueryable<long> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault(this IQueryable<float> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault(this IQueryable<double> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault(this IQueryable<decimal> source) { return source.SumOrDefault(x => x); }



		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			return source.Sum(convertToNullable(selector)) ?? default(int);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			return source.Sum(convertToNullable(selector)) ?? default(long);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			return source.Sum(convertToNullable(selector)) ?? default(float);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			return source.Sum(convertToNullable(selector)) ?? default(double);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			return source.Sum(convertToNullable(selector)) ?? default(decimal);
		}




        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault(this IQueryable<int> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault(this IQueryable<long> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static float AverageOrDefault(this IQueryable<float> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault(this IQueryable<double> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static decimal AverageOrDefault(this IQueryable<decimal> source) { return source.AverageOrDefault(x => x); }



        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(int);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(long);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static float AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(float);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(double);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        public static decimal AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(decimal);
        }






        /*====================================================*/

        /// <summary>
        /// query = query.WhereBuilder(param)<para/>
        ///     .Mapping(x =&gt; x.InvoicePrefix,  y =&gt; y.InvoicePrefix)<para/>
        ///     .Mapping(x =&gt; x.Sum,            y =&gt; y.Total)<para/>
        ///     .Mapping(x =&gt; x.ProductQty,     y =&gt; y.InvoiceIssueItems.Select(z =&gt; (int?)z.Qty))<para/>
        ///     .Build();<para/>
        /// </summary>
        public static WhereBuilder<TSource, TParams> WhereBuilder<TSource, TParams>(this IQueryable<TSource> source, WhereParams<TParams> param) 
		{
			return new WhereBuilder<TSource, TParams>(source, param);
		}


	}
}
