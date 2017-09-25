using System;
using System.Linq;

namespace Kernel.Extensions
{
    public static class TypeExtensions
	{
		//modified from: http://stackoverflow.com/questions/2448800/given-a-type-instance-how-to-get-generic-type-name-in-c
		private const char GenericTypeMarker = '`';

		/// <summary>
		/// Delegate for TryParse method of structure types such int, datetime, double etc
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		public delegate bool TryParseDelegate<T>(string value, out T result);

		/// <summary>
		/// Return a Type.String in the form "Type<T>"
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">type</exception>
		public static string ToGenericTypeString(this Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (!type.IsGenericType)
				return type.Name;

			string genericTypeName = type.GetGenericTypeDefinition().Name;
			genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf(TypeExtensions.GenericTypeMarker));
			string genericArgs = string.Join
			(
				",",
				type
				.GetGenericArguments()
				.Select(genericType => genericType.ToGenericTypeString())
				.ToArray()
			);
			return string.Format
			(
				"{0}<{1}>",
				genericTypeName,
				genericArgs
			);
		}

		
		/// <summary>
		/// Determines whether the given type or interface is assignable to generic type .
		/// </summary>
		/// <param name="givenType">Type of the given.</param>
		/// <param name="genericType">Type of the generic.</param>
		/// <returns><c>true</c> if [is assignable to generic type] true; otherwise, <c>false</c>.</returns>
		/// <exception cref="System.ArgumentNullException">type</exception>
		public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
		{
			if (givenType == null)
				throw new ArgumentNullException("type");
			if (genericType == null)
				throw new ArgumentNullException("genericType");

			if
			(
				genericType.IsInterface &&
				givenType
					.GetInterfaces()
					.Any(@interface => TypeExtensions.TypesMatch(@interface, genericType))
			)
				return true;

			if (TypeExtensions.TypesMatch(givenType, genericType))
				return true;

			if (givenType.BaseType == null)
				return false;

			return givenType.BaseType.IsAssignableToGenericType(genericType);
		}

		/// <summary>
		/// Create a delgate for TryParse Method if T suports the method.
		/// </summary>
		/// <typeparam name="T">Type that suports TryParse method</typeparam>
		/// <param name="type">Type for which the excension is called</param>
		/// <returns></returns>
		
		private static bool TypesMatch(Type compare, Type to)
		{
			if (to.IsAssignableFrom(compare))
				return true;

			if (compare.IsGenericType && to.IsGenericType && to.ContainsGenericParameters)
				compare = compare.GetGenericTypeDefinition();

			if (compare.IsGenericType && to.IsGenericType && compare.ContainsGenericParameters)
				to = to.GetGenericTypeDefinition();

			return to.IsAssignableFrom(compare);
		}
	}
}