using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kernel.Extensions;

namespace Kernel.Reflection.Extensions
{
	public static class TypeExtensions
	{
		private static IDictionary<Type, Delegate> tryParseDelegatesCahe = new Dictionary<Type, Delegate>();
        
		/// <summary>
		/// Delegate for TryParse method of structure types such int, datetime, double etc
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		public delegate bool TryParseDelegate<T>(string value, out T result);
        
		/// <summary>
		/// Creates a delegate that invokes a non-generic instance method with parametersType provided. 
		/// Usage:
		///		method = TypeExtensions.GetInstanceMethodDelegateWithParameters(TypeTheMethodBelongsTo, MethodName, typeof(param1)...typeof(paramN));
		///		method(instance, new [] {object1, object2, .... objectN} )
		/// </summary>
		/// <param name="type"></param>
		/// <param name="methodName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Action<object, object[]> GetInvoker(this Type type, string methodName, params Type[] parameters)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (string.IsNullOrWhiteSpace(methodName))
				throw new ArgumentNullException("methodName");

			var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);

			if (methodInfo == null)
				throw new MissingMethodException(string.Format("Could not find public instance method {0} in type {1}", methodName, type));

			var parametersParameter = Expression.Parameter(typeof(object[]));
			var targetParameter = Expression.Parameter(typeof(object));
			var castTarget = Expression.Convert(targetParameter, type);
			var methodParams = methodInfo.GetParameters();
			var paramExpressions = new List<Expression>();

			//if we have parameters extract them from the array with ElementAt method and create an expression converting to the method parameter type at the same position
			if (parameters.Count() > 0)
			{
				for (var i = 0; i < methodParams.Count(); i++)
				{
					//get the next element from the object[] by colling ArrayIndex
					var current = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));

					//convert the object to the method type at that position
					paramExpressions.Add(Expression.Convert(current, methodParams[i].ParameterType));
				}
			}

			var execute = Expression.Call(castTarget, methodInfo, paramExpressions);
			var lambda = Expression.Lambda<Action<object, object[]>>(execute, targetParameter, parametersParameter);
			return lambda.Compile();
		}

		/// <summary>
		/// Creates a delegate that invokes a non-generic instance method with parametersType provided. 
		/// Usage:
		///		method = TypeExtensions.GetInstanceMethodDelegateWithParameters(TypeTheMethodBelongsTo, MethodName, typeof(param1)...typeof(paramN));
		///		method(instance, new [] {object1, object2, .... objectN} )
		/// </summary>
		/// <param name="type"></param>
		/// <param name="methodName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static Func<object, object[], Task> GetAsyncInvoker(this Type type, string methodName, params Type[] parameters)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (string.IsNullOrWhiteSpace(methodName))
				throw new ArgumentNullException("methodName");

			var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, parameters, null);

			if (methodInfo == null)
				throw new MissingMethodException(string.Format("Could not find public instance method {0} in type {1}", methodName, type));

			var parametersParameter = Expression.Parameter(typeof(object[]));
			var targetParameter = Expression.Parameter(typeof(object));
			var castTarget = Expression.Convert(targetParameter, type);
			var methodParams = methodInfo.GetParameters();
			var paramExpressions = new List<Expression>();

			//if we have parameters extract them from the array with ElementAt method and create an expression converting to the method parameter type at the same position
			if (parameters.Count() > 0)
			{
				for (var i = 0; i < methodParams.Count(); i++)
				{
					//get the next element from the object[] by colling ArrayIndex
					var current = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));

					//convert the object to the method type at that position
					paramExpressions.Add(Expression.Convert(current, methodParams[i].ParameterType));
				}
			}

			var execute = Expression.Call(castTarget, methodInfo, paramExpressions);

			var block = Expression.Block(execute);
			//if the method retuen type is not a task make it return Task.FromResult(null) to be awaitable
			if (!Kernel.Extensions.TypeExtensions.IsAssignableToGenericType(methodInfo.ReturnType, typeof(Task)))
			{
				var methodInfo1 = typeof(TaskWrapper).GetMethod("WrapMethod", BindingFlags.Public | BindingFlags.Static);
				if (methodInfo1 == null)
					throw new MissingMethodException("WrapMethod");
				//Create a call expression for wrap method
				var callExp = Expression.Call(methodInfo1);
				//create a block expression with sync and wrapper method so return type is Task
				block = Expression.Block(execute, callExp);
			}

			var lambda = Expression.Lambda<Func<object, object[], Task>>(block, targetParameter, parametersParameter);
			return lambda.Compile();
		}

		/// <summary>
		/// Builds a delgate for one or more nested properties.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="path">path to the property in the object graph</param>
		/// <returns></returns>
		public static Func<object, TResult> GetInstancePropertyDelegate<TResult>(this Type type, string path)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (String.IsNullOrWhiteSpace(path))
				throw new ArgumentNullException("path");

			var parameter = Expression.Parameter(typeof(object));

			var convert = Expression.Convert(parameter, type) as Expression;

			var result = ReflectionHelper.GetPathAsExpression(convert, path);

			var resultConvert = Expression.Convert(result, typeof(TResult));

			var func = Expression.Lambda<Func<object, TResult>>(resultConvert, parameter).Compile();

			return func;
		}
        
		/// <summary>
		/// Create a delgate for TryParse Method if T suports the method.
		/// </summary>
		/// <typeparam name="T">Type that suports TryParse method</typeparam>
		/// <param name="type">Type for which the excension is called</param>
		/// <returns></returns>
		public static Delegate GetDelegateTryParse(this Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (tryParseDelegatesCahe.ContainsKey(type))
				return tryParseDelegatesCahe[type];

			var bindingFlags = BindingFlags.Public | BindingFlags.Static;

			var tryParseMethod = type.GetMethod("TryParse", bindingFlags, null, new Type[] { typeof(string), type.MakeByRefType() }, null);

			if (tryParseMethod == null)
				return null;

			var param1 = Expression.Parameter(typeof(string));

			var param2 = Expression.Parameter(type.MakeByRefType());

			var expession = Expression.Call(tryParseMethod, new Expression[] { param1, param2 });
			var genericDelegate = typeof(TryParseDelegate<>).MakeGenericType(type);
			var func = Expression.Lambda(genericDelegate, expession, param1, param2).Compile();
			tryParseDelegatesCahe[type] = func;
			return func;
		}

		/// <summary>
		/// Create a delegate for a static property of given type
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static Func<object, TResult> GetStaticPropertyDelegate<TResult>(this Type type, string propertyName)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var param = Expression.Parameter(typeof(object));

			var propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);

			if (propertyInfo == null)
				throw new InvalidOperationException(String.Format("Could not find property {0} in type {1}", propertyName, type));

			var expr = Expression.Property(null, propertyInfo);

			var convert = Expression.Convert(expr, typeof(TResult));

			return Expression.Lambda<Func<object, TResult>>(convert, param).Compile();
		}

		/// <summary>
		/// Build an assign delegate for instance property
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static Action<object, object> GetAssignPropertyDelegate(this Type type, string propertyName)
		{
			var parameterExpressionTarget = Expression.Parameter(typeof(object));
			var parameterExpressionValue = Expression.Parameter(typeof(object));

			var convert = Expression.Convert(parameterExpressionTarget, type);
			var left = ReflectionHelper.GetPathAsExpression(convert, propertyName);

			var assignExpression = Expression.Assign(left, Expression.Convert(parameterExpressionValue, left.Type));

			var lambda = Expression.Lambda<Action<object, object>>(assignExpression, parameterExpressionTarget, parameterExpressionValue);
			var assignDelegate = lambda.Compile();

			return assignDelegate;
		}

		/// <summary>
		/// Create a new instance of the given task and return it as T.
		/// Chooses the constructor with the least number of arugments greater than or equal to @params.Length
		/// and supplies Type.Missing for each missing one
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static T New<T>(this Type type, params object[] @params)
		{
			var constructor =
				type
				.GetConstructors()
				.Where(x => x.GetParameters().Length >= @params.Length)
				.OrderBy(x => x.GetParameters().Length)
				.First();
			var parameters =
				@params
				.Concat
				(
					Enumerable
					.Range(0, constructor.GetParameters().Length - @params.Length)
					.Select(x => Type.Missing)
				)
				.ToArray();
			var instance = constructor
				.Invoke
				(
					BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
					null,
					parameters,
					CultureInfo.CurrentCulture
				);
			return (T)instance;
		}

		/// <summary>
		/// Create a delegate for static Method TryParse of Enum type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		public static TryParseDelegate<T> CreateDelegateForStaticMethodTryParseEnum<T>(this Type type)
		{
			var t = typeof(T);

			var tryParseMethod = typeof(Enum).GetMethods().Single(m => m.Name == "TryParse" && m.IsGenericMethod && m.GetParameters().Count() == 2);

			if (tryParseMethod == null)
				return null;

			var param1 = Expression.Parameter(typeof(string));

			var param2 = Expression.Parameter(typeof(T).MakeByRefType());

			var genericMethos = tryParseMethod.MakeGenericMethod(new Type[] { t });

			var expession = Expression.Call(genericMethos, new Expression[] { param1, param2 });

			var func = Expression.Lambda<TryParseDelegate<T>>(expession, param1, param2).Compile();

			return func;
		}

		/// <summary>
		/// Create a delgate for TryParse Method if T suports the method.
		/// </summary>
		/// <typeparam name="T">Type that suports TryParse method</typeparam>
		/// <param name="type">Type for which the excension is called</param>
		/// <returns></returns>
		public static TryParseDelegate<T> CreateDelegateForStaticMethodTryParse<T>(this Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var bindingFlags = BindingFlags.Public | BindingFlags.Static;

			var tryParseMethod = typeof(T).GetMethod("TryParse", bindingFlags, null, new Type[] { typeof(string), typeof(T).MakeByRefType() }, null);

			if (tryParseMethod == null)
				return null;

			var param1 = Expression.Parameter(typeof(string));

			var param2 = Expression.Parameter(typeof(T).MakeByRefType());

			var expession = Expression.Call(tryParseMethod, new Expression[] { param1, param2 });

			var func = Expression.Lambda<TryParseDelegate<T>>(expession, param1, param2).Compile();

			return func;
		}
        
		#region nested classes
		/// <summary>
		/// Helper class to return a null task for sync methods returning void
		/// </summary>
		private class TaskWrapper
		{
			public static Task WrapMethod()
			{
				return Task.FromResult<object>(null);
			}
		}
		#endregion
		#region Not Implemented

		/// <summary>
		/// Create a delagete for static method of type with one argument
		/// </summary>
		/// <typeparam name="TArgument">The argument of the method</typeparam>
		/// <typeparam name="Result">The type the method returns</typeparam>
		/// <param name="type">The type for which the excention method is called.</param>
		/// <param name="methodName">The nama of the method.</param>
		/// <returns></returns>
		public static Func<TArgument, Result> GetStaticFunctionDelegate<TArgument, Result>(this Type type, string methodName)
		{
			throw new NotImplementedException();

			//if (type == null)
			//	throw new ArgumentNullException("type");

			//var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(TArgument) }, null);

			//if (methodInfo == null)
			//	throw new InvalidOperationException(String.Format("Could not find static method {0} in type {1}", methodName, type)); ;

			//var param1 = Expression.Parameter(typeof(TArgument));

			//var expression = Expression.Call(methodInfo, new Expression[] { param1 });

			//var func = Expression.Lambda<Func<TArgument, Result>>(expression, param1).Compile();

			//return func;
		}

		#endregion
	}
}