﻿using Orion.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Orion.API.Extensions;

namespace Orion.API
{
	/// <summary>WhereBuild Tools</summary>
	public class WhereBuilder<TModel, TParams> 
	{

		private static readonly MethodInfo _containsMethod = LambdaUtils
			.GetGenericMethodDefinition<IEnumerable<int>>(list => list.Contains(1));

		private static readonly MethodInfo _anyMethod = LambdaUtils
			.GetGenericMethodDefinition<IEnumerable<int>>(list => list.Any(x => true));

		private static readonly MethodInfo _stringContains = LambdaUtils.GetMethod<string>(x => x.Contains(""));
		private static readonly MethodInfo _stringStartsWith = LambdaUtils.GetMethod<string>(x => x.StartsWith(""));
		private static readonly MethodInfo _stringEndsWith = LambdaUtils.GetMethod<string>(x => x.EndsWith(""));



		private IQueryable<TModel> _query;
		private WhereParams<TParams> _param;


		/// <summary></summary>
		public WhereBuilder(IQueryable<TModel> query, WhereParams<TParams> param)
		{
			_query = query;
			_param = param;
		}




		/*=====================================================================*/

		private PropertyInfo getProperty(LambdaExpression lambdaExpr)
		{
			PropertyInfo prop = lambdaExpr.GetProperty();
			if (prop != null) { return prop; }

			throw new Exception("無法取得 " + lambdaExpr + " 的 Property");
		}


		private T[] getValues<T>(Expression<Func<TParams, IEnumerable<T>>> find)
		{
			return _param.GetValues(find);
		}


		private T[] getValues<T>(Expression<Func<TParams, T>> find)
		{
			PropertyInfo prop = getProperty(find);

			Func<TParams, T> getter = find.Compile();
			var tempParam = (TParams)Activator.CreateInstance(typeof(TParams));

			object[] values = ((IWhereParams)_param).GetValues(prop.Name);

			var result = new List<T>();
			foreach (var value in values)
			{
				prop.SetValue(tempParam, value);
				result.Add(getter(tempParam));
			}

			return result.ToArray();
		}




		/*=====================================================================*/

		private Expression getStringCondition(Expression parameter, WhereOperator oper, string[] values)
		{
			var containsMethod = _containsMethod.MakeGenericMethod(typeof(string));
			var compareMethod = LambdaUtils.GetMethod<string>(x => x.CompareTo(""));
			var zeroExpr = Expression.Constant(0);
			var valueExpr = Expression.Constant(values.FirstOrDefault(), typeof(string));
			var valuesExpr = Expression.Constant(values, typeof(string[]));
			var compareExpr = Expression.Call(parameter, compareMethod, valueExpr);

			switch (oper)
			{
				case WhereOperator.Contains: /* x.Contains(value) */
					return Expression.Call(parameter, _stringContains, valueExpr);
				case WhereOperator.NotContains: /* x.Contains(value) */
					return Expression.Not(Expression.Call(parameter, _stringContains, valueExpr));
				case WhereOperator.StartsWith: /* x.StartsWith(value) */
					return Expression.Call(parameter, _stringStartsWith, valueExpr);
				case WhereOperator.NotStartsWith: /* x.StartsWith(value) */
					return Expression.Not(Expression.Call(parameter, _stringStartsWith, valueExpr));
				case WhereOperator.EndsWith: /* x.EndsWith(value) */
					return Expression.Call(parameter, _stringEndsWith, valueExpr);
				case WhereOperator.NotEndsWith: /* x.EndsWith(value) */
					return Expression.Not(Expression.Call(parameter, _stringEndsWith, valueExpr));
				case WhereOperator.Equals: /* x == value */
					return Expression.Equal(parameter, valueExpr);
				case WhereOperator.NotEquals: /* x != value */
					return Expression.NotEqual(parameter, valueExpr);

				case WhereOperator.LessThan: /* x < value */
					return Expression.LessThan(compareExpr, Expression.Constant(0));
				case WhereOperator.LessEquals: /* x <= value */
					return Expression.LessThanOrEqual(compareExpr, Expression.Constant(0));
				case WhereOperator.GreaterThan: /* x > value */
					return Expression.GreaterThan(compareExpr, Expression.Constant(0));
				case WhereOperator.GreaterEquals: /* x >= value */
					return Expression.GreaterThanOrEqual(compareExpr, Expression.Constant(0));

				case WhereOperator.In: /* values.Contains(x) */
					return Expression.Call(containsMethod, valuesExpr, parameter);
				case WhereOperator.NotIn: /* !values.Contains(x) */
					return Expression.Not(Expression.Call(containsMethod, valuesExpr, parameter));

			}

			return null;
		}



		private Expression getValueCondition<T>(Expression parameter, WhereOperator oper, T[] values)
		{
			var tempExpr = Expression.Constant(values.FirstOrDefault());
			var valueExpr = Expression.Convert(tempExpr, typeof(T));
			var valuesExpr = Expression.Constant(values, typeof(T[]));
			var containsMethod = _containsMethod.MakeGenericMethod(typeof(T));

			switch (oper)
			{
				case WhereOperator.Equals: /* x == value */
					return Expression.Equal(parameter, valueExpr);
				case WhereOperator.NotEquals: /* x != value */
					return Expression.NotEqual(parameter, valueExpr);
				case WhereOperator.LessThan: /* x < value */
					return Expression.LessThan(parameter, valueExpr);
				case WhereOperator.LessEquals: /* x <= value */
					return Expression.LessThanOrEqual(parameter, valueExpr);
				case WhereOperator.GreaterThan: /* x > value */
					return Expression.GreaterThan(parameter, valueExpr);
				case WhereOperator.GreaterEquals: /* x >= value */
					return Expression.GreaterThanOrEqual(parameter, valueExpr);
				case WhereOperator.In: /* values.Contains(x) */
					return Expression.Call(containsMethod, valuesExpr, parameter);
				case WhereOperator.NotIn: /* !values.Contains(x) */
					return Expression.Not(Expression.Call(containsMethod, valuesExpr, parameter));
				case WhereOperator.Between: /* x >= values[0] && x <= values[1] */
					if(values.Length < 2) { return Expression.GreaterThanOrEqual(parameter, valueExpr); } /* x >= value */

					var value1Expr = Expression.Convert(Expression.Constant(values[1]), typeof(T));
					return Expression.AndAlso(
						Expression.GreaterThanOrEqual(parameter, valueExpr),
						Expression.LessThanOrEqual(parameter, value1Expr)
					);
			}

			return null;
		}



		private Expression getCondition<T>(Expression parameter, WhereOperator oper, T[] values)
		{
			if (_param == null) { return null; }
			if (values == null || values.Length == 0) { return null; }

			var strValues = values as string[];
			if (strValues != null)
			{
				/*字串類型*/
				return getStringCondition(parameter, oper, strValues);
			}
			else
			{
				/*數值或日期類型*/
				return getValueCondition(parameter, oper, values);
			}
		}





		private void bindEnumerable<T>(Expression<Func<TModel, IEnumerable<T>>> columnSelector, WhereOperator oper, T[] values)
		{
			Type type = typeof(T);
			var parameter = Expression.Parameter(type);

			bool notOperator = true;

			switch (oper)
			{
				case WhereOperator.NotIn:           oper = WhereOperator.In; break;
				case WhereOperator.NotEquals:       oper = WhereOperator.Equals; break;
				case WhereOperator.NotContains:     oper = WhereOperator.Contains; break;
				case WhereOperator.NotStartsWith:   oper = WhereOperator.StartsWith; break;
				case WhereOperator.NotEndsWith:     oper = WhereOperator.EndsWith; break;
				default: notOperator = false; break;
			}

			
			Expression condition = getCondition(parameter, oper, values);
			if (condition == null) { return; }

			/* y => y == deliveryNum 
			 * x => x.Any(y => y == deliveryNum) 
			 * */
			MethodInfo anyMethod = _anyMethod.MakeGenericMethod(type);
			Expression conditionExpr = Expression.Lambda<Func<T, bool>>(condition, parameter);
			Expression invokedExpr = Expression.Call(anyMethod, columnSelector.Body, conditionExpr);
			if (notOperator) { invokedExpr = Expression.Not(invokedExpr); }

			var whereExpr = Expression.Lambda<Func<TModel, bool>>(invokedExpr, columnSelector.Parameters);

			_query = _query.Where(whereExpr);
		}






		/*=====================================================================*/


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, IEnumerable<T>>> find, Expression<Func<TModel, IEnumerable<T>>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = getValues(find);
			WhereOperator oper = _param.GetOperator(find);

			bindEnumerable(columnSelector, oper, values);
			return this;
		}


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, T>> find, Expression<Func<TModel, IEnumerable<T>>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = getValues(find);
			WhereOperator oper = _param.GetOperator(find);

			bindEnumerable(columnSelector, oper, values);
			return this;
		}


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind<T>(Expression<Func<TParams, T>> find, Expression<Func<TModel, T>> columnSelector)
		{
			if (_param == null) { return this; }
			T[] values = getValues(find);
			WhereOperator oper = _param.GetOperator(find);

			Expression condition = getCondition(columnSelector.Body, oper, values);
			if (condition == null) { return this; }

			/* x => x => x.name == find.CompanyCode
			 * */
			var whereExpr = Expression.Lambda<Func<TModel, bool>>(condition, columnSelector.Parameters);

			_query = _query.Where(whereExpr);
			return this;
		}
				 


		/// <summary>綁定查詢欄位</summary>
		public WhereBuilder<TModel, TParams> WhereBind(Expression<Func<TParams, Enum>> find, Expression<Func<TModel, string>> columnSelector)
		{
			if (_param == null) { return this; }

			string[] values = getValues(find).ToArray(x => x.ToString());
			WhereOperator oper = _param.GetOperator(find);

			Expression condition = getCondition(columnSelector.Body, oper, values);
			if (condition == null) { return this; }

			/* x => x => x.name == find.CompanyCode
			 * */
			var whereExpr = Expression.Lambda<Func<TModel, bool>>(condition, columnSelector.Parameters);

			_query = _query.Where(whereExpr);
			return this;
		}




		/// <summary></summary>
		public IQueryable<TModel> Build()
		{
			return _query;
		}


	}
}
