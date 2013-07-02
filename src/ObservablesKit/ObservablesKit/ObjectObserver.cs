using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents an observer that can be use to register
	/// callbacks for property changes on an object. 
	/// </summary>
	/// <typeparam name="T">the type of the object to observe</typeparam>
	public class ObjectObserver<T> : IDisposable where T : INotifyPropertyChanged
	{
		private readonly T observedObject;
		private readonly IList<ObjectObserverRegistration> registrations;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectObserver{T}"/> class.
		/// </summary>
		/// <param name="observedObject">The observed object.</param>
		/// <exception cref="System.ArgumentNullException">observedObject cannot be null</exception>
		public ObjectObserver(T observedObject)
		{
			if (observedObject == null)
				throw new ArgumentNullException("observedObject");

			this.observedObject = observedObject;
			this.registrations = new List<ObjectObserverRegistration>();
		}

		/// <summary>
		/// registers a callback that will observer the given property path
		/// for changes
		/// </summary>
		/// <param name="selector">The property path selector.</param>
		/// <returns>a registration object</returns>
		/// <exception cref="System.ArgumentNullException">selector cannot be null</exception>
		/// <exception cref="System.InvalidOperationException">unable to get propery path from expression</exception>
		/// <exception cref="System.Exception">selector must start with a lamda expression</exception>
		public ObjectObserverRegistration<T> When(Expression<Func<T, object>> selector)
		{
			if (selector == null)
				throw new ArgumentNullException("selector");

			if (selector.NodeType != ExpressionType.Lambda)
				throw new InvalidOperationException("selector must start with a lamda expression");

			var path = PropertyHelper.GetPropertyPathStack(selector);

			if(path.Count == 0)
				throw new InvalidOperationException("unable to get propery path from expression");

			var obs = new PropertyPathObserver(this.observedObject, path);

			var reg = new ObjectObserverRegistration<T>(obs);

			this.registrations.Add(reg);

			return reg;
		}

		/// <summary>
		/// dettaches all registrations from the observed object
		/// </summary>
		public void Dispose()
		{
			foreach (var reg in registrations)
				reg.Dispose();

			this.registrations.Clear();
		}
	}
}