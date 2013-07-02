using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents a registration for the property observer, 
	/// it contains a list of actions to execute when 
	/// the observer notifies.
	/// </summary>
	public class ObjectObserverRegistration : IDisposable
	{
		private PropertyPathObserver observer;

		/// <summary>
		/// Gets the actions to execute
		/// </summary>
		/// <value>
		/// The actions.
		/// </value>
		public IList<PropertyPathChangedAction> Actions { get; private set; }

		/// <summary>
		/// Gets the observed object.
		/// </summary>
		/// <value>
		/// The observed object.
		/// </value>
		public object ObservedObject 
		{ 
			get
			{
				return this.observer.ObservableObject;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectObserverRegistration"/> class.
		/// </summary>
		/// <param name="observer">The observer.</param>
		/// <exception cref="System.ArgumentNullException">observer cannot be null</exception>
		public ObjectObserverRegistration(PropertyPathObserver observer)
		{
			if (observer == null)
				throw new ArgumentNullException("observer");

			this.observer = observer;
			this.observer.PropertyPathChanged += Observer_PropertyPathChanged;
			this.Actions = new List<PropertyPathChangedAction>();
		}

		private void Observer_PropertyPathChanged(object sender, PropertyPathObserverChangedEventArgs e)
		{
			foreach (var action in Actions)
				action(e);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.observer.PropertyPathChanged -= Observer_PropertyPathChanged;
			this.observer.Dispose();
			this.observer = null;
		}
	}

	/// <summary>
	/// represents a registration that is typed to the observers
	/// observed's object type.
	/// </summary>
	/// <typeparam name="T">the type of the observed object</typeparam>
	public class ObjectObserverRegistration<T> : ObjectObserverRegistration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectObserverRegistration{T}"/> class.
		/// </summary>
		/// <param name="observer">The observer.</param>
		public ObjectObserverRegistration(PropertyPathObserver observer)
			: base(observer)
		{

		}

		/// <summary>
		/// Does the specified action when the observer notifies
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		public ObjectObserverRegistration<T> Do(PropertyPathChangedAction action)
		{
			this.Actions.Add(action);
			return this;
		}
	}
}