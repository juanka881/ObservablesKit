using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace ObservablesKit
{
	/// <summary>
	/// provides helper methods to work with properties
	/// </summary>
	public class PropertyHelper
	{
		/// <summary>
		/// Gets property path stack from a expression selector.
		/// </summary>
		/// <typeparam name="T">type of the root object</typeparam>
		/// <typeparam name="V">type of the property selected</typeparam>
		/// <param name="selector">The selector.</param>
		/// <returns>a stack with the path to the property selected</returns>
		public static Stack<MemberExpression> GetPropertyPathStack<T, V>(Expression<Func<T, V>> selector)
		{
			var me = null as MemberExpression;

			switch (selector.Body.NodeType)
			{
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					var ue = selector.Body as UnaryExpression;
					if (ue != null)
						me = ue.Operand as MemberExpression;
					break;

				default:
					me = selector.Body as MemberExpression;
					break;
			}

			var stack = new Stack<MemberExpression>();

			if (me == null && selector.Body is ParameterExpression)
				return stack;

			if (me == null)
				throw new InvalidOperationException("not a property path selector");

			while (me != null)
			{
				stack.Push(me);
				me = me.Expression as MemberExpression;
			}

			return stack;
		}

		/// <summary>
		/// Gets the property value at a given depth in a propery path stack.
		/// </summary>
		/// <param name="obj">The object to use as the root to get the value from.</param>
		/// <param name="propertyPathStack">The property path stack.</param>
		/// <param name="depth">The depth.</param>
		/// <returns>the value or null if not found</returns>
		/// <exception cref="System.ArgumentNullException">obj cannot be null</exception>
		/// <exception cref="System.InvalidOperationException">unable to get property information from property path</exception>
		public static object GetPropertyValueAtDepth(object obj, Stack<MemberExpression> propertyPathStack, int depth)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			var currentDepth = 0;
			var currentObj = obj;
			
			foreach (var me in propertyPathStack)
			{
				var pi = me.Member as PropertyInfo;

				if(pi == null)
					throw new InvalidOperationException("unable to get property information from property path");

				var propertyValue = pi.GetValue(currentObj, null);

				if (currentDepth == depth)
					return propertyValue;

				if (propertyValue == null)
					return null;

				currentDepth += 1;
				currentObj = propertyValue;
			}

			return null;
		}
	}
}