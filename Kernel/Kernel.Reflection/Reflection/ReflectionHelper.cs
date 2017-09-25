namespace Kernel.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Kernel.Reflection.Extensions;

    public class ReflectionHelper
	{
		private static Func<Type, bool> _whereFuncDefault = t => true;

		/// <summary>
		/// Try to parse a string to type T and return either parsed value or default if not successful
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool TryParseOrDefault<T>(string value, out T result, T defaultValue = default(T))
		{
			result = defaultValue;

			var isEnum = typeof(T).IsEnum;

			var tryParseMethod = isEnum ? TypeExtensions.CreateDelegateForStaticMethodTryParseEnum<T>(typeof(T)) : TypeExtensions.CreateDelegateForStaticMethodTryParse<T>(typeof(T));

			if (tryParseMethod == null)
				return false;

			if (tryParseMethod.Invoke(value, out result))
				return true;

			result = defaultValue;

			return false;
		}

		/// <summary>
		/// Builds expression tree for propreties path for given instance
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Expression GetPathAsExpression(Expression instance, string path)
		{
			if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			if (instance == null)
				throw new ArgumentNullException("instance");

			var splitPath = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

			var result = splitPath.Aggregate(instance, (expression, nextMember) =>
			{
				var memberExpression = Expression.Property(expression, nextMember);

				return (Expression)memberExpression;
			});

			return result;
		}

		/// <summary>
		/// Gets the property represented by the lambda expression.
		/// </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="property"/> is null.</exception>
		/// <exception cref="ArgumentException">The <paramref name="property"/> is not a lambda expression or it does not represent a property access.</exception>
		public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> property, bool checkForSingleDot = false)
		{
			var memberInfo = ReflectionHelper.GetPropertyOrField(property, checkForSingleDot);

			var info = memberInfo as PropertyInfo;

			if (info == null)
				throw new ArgumentException("Member is not a property");

			return info;
		}

		/// <summary>
		/// Gets the property represented by the lambda expression.
		/// </summary>
		/// <exception cref="ArgumentNullException">The <paramref name="member"/> is null.</exception>
		/// <exception cref="ArgumentException">The <paramref name="member"/> is not a lambda expression or it does not represent a property access.</exception>
		public static MemberInfo GetPropertyOrField<T>(Expression<Func<T, object>> member, bool checkForSingleDot = false)
		{
			if (member == null)
				throw new ArgumentNullException("member");

			var info = ReflectionHelper.GetMemberInfo(member, checkForSingleDot) as MemberInfo;

			return info;
		}

		/// <summary>
		/// Gets types from assembly collection. Pass a predicate function to filter given types
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		/// <param name="func">The function.</param>
		/// <returns></returns>
		public static IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies, Func<Type, bool> func = null)
		{
			if (func == null)
				func = _whereFuncDefault;

			var allTypes = new List<Type>();

			foreach (var a in assemblies)
			{
				try
				{
					var types = a.GetTypes().Where(func);

					allTypes.AddRange(types);
				}
				catch (ReflectionTypeLoadException typeLoadException)
				{
					//throw typeLoadException;
					//Uncomment below to log all loader exception
					//foreach (var e in typeLoadException.LoaderExceptions)
					//{
					//	//Log here
					//	//LoggerManager.WriteExceptionToEventLog(e);
					//}
				}
			}

			return allTypes;
		}

		/// <summary>
		/// Gets all types. Discover all assemblies and returns all types in them. A function could be passed to filter given types
		/// </summary>
		/// <param name="func">The function.</param>
		/// <returns>All types filtered by func</returns>
		public static IEnumerable<Type> GetAllTypes(Func<Type, bool> func = null)
		{
			var assemblies = AssemblyScanner.ScannableAssemblies;

			return ReflectionHelper.GetAllTypes(assemblies, func);
		}

		/// <summary>
		/// Returns a MemberInfo for an expression containing a call to a property.
		/// </summary>
		/// <param name="member">Property expression such as t => t.Property1</param>
		/// <param name="checkForSingleDot">Checks that the member expression doesn't have more than one dot like a.Prop.Val</param>
		/// <returns></returns>
		private static MemberInfo GetMemberInfo(LambdaExpression member, bool checkForSingleDot)
		{
			MemberExpression memberExpr = null;

			switch (member.Body.NodeType)
			{
				case ExpressionType.Convert:
					memberExpr = ((UnaryExpression)member.Body).Operand as MemberExpression;
					break;
				case ExpressionType.MemberAccess:
					memberExpr = member.Body as MemberExpression;
					break;
				default:
					throw new ArgumentException("Not a member access", "member");
			}

			if (!checkForSingleDot)
				return memberExpr.Member;

			if (memberExpr.Expression is ParameterExpression)
				return memberExpr.Member;

			throw new ArgumentException("Argument passed contains more than a single dot which is not allowed: " + member,
				"member");
		}
	}
}