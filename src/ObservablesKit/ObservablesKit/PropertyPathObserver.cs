using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// monitors a property path for changes and notifies
	/// when a change is detected.
	/// </summary>
	public class PropertyPathObserver : IDisposable
	{
		private PropertyInfo propertyInfo;
		private object observableObject;
		private object propertyValue;
		
		/// <summary>
		/// Gets the parent of this path
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public PropertyPathObserver Parent { get; private set; }

		/// <summary>
		/// Gets the child for this path
		/// </summary>
		/// <value>
		/// The child.
		/// </value>
		public PropertyPathObserver Child { get; private set; }

		/// <summary>
		/// Gets the observable object that this path is monitoring
		/// </summary>
		/// <value>
		/// The observable object.
		/// </value>
		public object ObservableObject
		{
			get
			{
				return this.observableObject;
			}

			private set
			{
				if (this.observableObject == value)
					return;

				this.DetachFromObservableObject_PropertyChanged(this.ObservableObject as INotifyPropertyChanged);

				this.observableObject = value;

				this.AttachToObservableObject_PropertyChanged(this.ObservableObject as INotifyPropertyChanged);

				this.UpdatePropertyValue();
			}
		}

		/// <summary>
		/// Gets the property value for the object that is being
		/// monitored
		/// </summary>
		/// <value>
		/// The property value.
		/// </value>
		public object PropertyValue
		{
			get
			{
				return propertyValue;
			}

			private set
			{
				if (this.propertyValue == value)
					return;

				propertyValue = value;

				if (this.Child != null)
					this.Child.ObservableObject = value as INotifyPropertyChanged;
			}
		}

		/// <summary>
		/// Gets the name of the property being monitor on the 
		/// ObservableObject
		/// </summary>
		/// <value>
		/// The name of the property.
		/// </value>
		public string PropertyName
		{
			get
			{
				return this.propertyInfo.Name;
			}
		}

		/// <summary>
		/// Occurs when a change is detected in the property path chain.
		/// </summary>
		public event PropertyPathObserverChangedEventHandler PropertyPathChanged;

		/// <summary>
		/// Prevents a default instance of the <see cref="PropertyPathObserver"/> class from being created.
		/// </summary>
		private PropertyPathObserver()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyPathObserver"/> class.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="propertyPathStack">The property path stack.</param>
		public PropertyPathObserver(INotifyPropertyChanged obj, Stack<MemberExpression> propertyPathStack)
		{
			if (propertyPathStack == null)
				throw new ArgumentNullException("propertyPathStack");

			if (propertyPathStack.Count == 0)
				throw new ArgumentException("propertyPathStack must have at least one item");

			var copy = new Stack<MemberExpression>(propertyPathStack);

			this.Initialize(obj, copy, null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyPathObserver"/> class.
		/// </summary>
		/// <param name="obj">The object to monitor.</param>
		/// <param name="propertyPathStack">The property path stack, this stack will be consume
		/// after this function completes.</param>
		/// <param name="parent">The parent of this path if any, null if this is the root.</param>
		/// <exception cref="System.ArgumentNullException">propertyPathStack cannot be null</exception>
		/// <exception cref="System.ArgumentException">
		/// propertyPathStack must have at least one item
		/// or
		/// path contains a non property member
		/// </exception>
		private void Initialize(INotifyPropertyChanged obj, Stack<MemberExpression> propertyPathStack, PropertyPathObserver parent)
		{
			if (propertyPathStack == null)
				throw new ArgumentNullException("propertyPathStack");

			if (propertyPathStack.Count == 0)
				throw new ArgumentException("propertyPathStack must have at least one item");

			var path = propertyPathStack.Pop();

			if (!(path.Member is PropertyInfo))
				throw new ArgumentException("path contains a non property member");

			this.propertyInfo = (PropertyInfo)path.Member;
			this.ObservableObject = obj;
			this.Parent = parent;
			
			if (propertyPathStack.Count > 0)
			{
				this.Child = new PropertyPathObserver();
				this.Child.Initialize(this.PropertyValue as INotifyPropertyChanged, propertyPathStack, this);
			}
		}

		private void DetachFromObservableObject_PropertyChanged(INotifyPropertyChanged obj)
		{
			if (obj == null)
				return;

			obj.PropertyChanged -= ObservableObject_PropertyChanged;
		}

		private void AttachToObservableObject_PropertyChanged(INotifyPropertyChanged obj)
		{
			if (obj == null)
				return;

			obj.PropertyChanged += ObservableObject_PropertyChanged;
		}

		private void ObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.PropertyName != e.PropertyName && !string.IsNullOrWhiteSpace(e.PropertyName))
				return;

			this.UpdatePropertyValue();

			if (this.Child == null)
			{
				var ce = new PropertyPathObserverChangedEventArgs(this.PropertyName, this.PropertyValue);
				this.NotifyPropertyPathChanged(ce);
			}
		}

		private void UpdatePropertyValue()
		{
			var val = null as object;

			if(this.ObservableObject != null)
				val = this.propertyInfo.GetValue(this.ObservableObject, null);

			this.PropertyValue = val;
		}

		/// <summary>
		/// Notifies that the property path has changed.
		/// </summary>
		/// <param name="e">The <see cref="PropertyPathObserverChangedEventArgs"/> instance containing the event data.</param>
		public void NotifyPropertyPathChanged(PropertyPathObserverChangedEventArgs e)
		{
			if (this.Parent == null)
			{
				var handler = this.PropertyPathChanged;

				if (handler == null)
					return;

				handler(this, e);
			}
			else
			{
				this.Parent.NotifyPropertyPathChanged(e);
			}
		}

		/// <summary>
		/// dettaches change notifications from the observed object
		/// </summary>
		public void Dispose()
		{
			this.DetachFromObservableObject_PropertyChanged(this.ObservableObject as INotifyPropertyChanged);
			this.PropertyPathChanged = null;
		}
	}
}