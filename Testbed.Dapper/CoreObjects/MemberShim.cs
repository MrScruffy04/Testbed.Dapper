namespace Testbed.Dapper.CoreObjects
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public static class MemberShim
	{
		public static Action<TModel, TValue> GetReadOnlySetter<TModel, TValue>(Expression<Func<TModel, object>> expression, NamingConventions namingConvention = NamingConventions.UnderscoreAndCamelCase)
		{
			var targetExp = Expression.Parameter(typeof(TModel), "target");
			var valueExp = Expression.Parameter(typeof(TValue), "value");

			var memberExp = Expression.PropertyOrField(
				targetExp,
				GetBackingMemberName(GetMember(expression).Name, namingConvention));

			var assignExp = Expression.Assign(
				memberExp,
				valueExp);

			return Expression.Lambda<Action<TModel, TValue>>(assignExp, targetExp, valueExp).Compile();
		}

		private static MemberInfo GetMember<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
		{
			var memberExp = RemoveUnary(expression.Body);

			if (memberExp == null)
			{
				return null;
			}

			return memberExp.Member;
		}

		private static MemberExpression RemoveUnary(Expression toUnwrap)
		{
			if (toUnwrap is UnaryExpression)
			{
				return ((UnaryExpression)toUnwrap).Operand as MemberExpression;
			}

			return toUnwrap as MemberExpression;
		}

		private static string GetBackingMemberName(string originalName, NamingConventions namingConvention)
		{
			return new NamingConventionUtility(originalName).ApplyConventions(namingConvention);
		}
	}
}
