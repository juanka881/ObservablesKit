using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents a function that handles a path observers's notification
	/// </summary>
	/// <param name="e">The <see cref="PropertyPathObserverChangedEventArgs"/> instance containing the event data.</param>
	public delegate void PropertyPathChangedAction(PropertyPathObserverChangedEventArgs e);

	/// <summary>
	/// represents a function that handles a path observer's notification
	/// </summary>
	/// <param name="sender">The sender.</param>
	/// <param name="e">The <see cref="PropertyPathObserverChangedEventArgs"/> instance containing the event data.</param>
	public delegate void PropertyPathObserverChangedEventHandler(object sender, PropertyPathObserverChangedEventArgs e);

	/// <summary>
	/// represents the path observer changed notification data
	/// </summary>
	public class PropertyPathObserverChangedEventArgs
	{
		/// <summary>
		/// Gets the name of the property
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the value of the property
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyPathObserverChangedEventArgs"/> class.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The value of the property.</param>
		public PropertyPathObserverChangedEventArgs(string name, object value)
		{
			this.Name = name;
			this.Value = value;
		}
	}
}