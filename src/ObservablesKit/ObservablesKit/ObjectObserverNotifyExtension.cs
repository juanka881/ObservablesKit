using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservablesKit
{
	/// <summary>
	/// provides notification extensions to the <see cref="ObjectObserverRegistration"/> object
	/// </summary>
	public static class ObjectObserverNotifyExtension
	{
		/// <summary>
		/// Registers a callback that will send property changed notification when the registration fires.
		/// </summary>
		/// <typeparam name="T">type of the observed object on the registration</typeparam>
		/// <param name="reg">The regristation.</param>
		/// <param name="selector">The selector use to determine the property to notify on.</param>
		/// <returns>the registration object</returns>
		/// <exception cref="System.ArgumentNullException">reg cannot be null</exception>
		public static ObjectObserverRegistration<T> Notify<T>(this ObjectObserverRegistration<T> reg, Expression<Func<T, object>> selector)
		{
			if (reg == null)
				throw new ArgumentNullException("reg");

			var stack = PropertyHelper.GetPropertyPathStack(selector);

			reg.Actions.Add(_ =>
			{
				var parent = PropertyHelper.GetPropertyValueAtDepth(reg.ObservedObject, stack, stack.Count - 2);
				var property = stack.Last().Member as PropertyInfo;
				var obs = parent as IObservableObject;

				if(property != null && obs != null)
					obs.NotifyPropertyChanged(property.Name);
			});

			return reg;
		}
	}
}