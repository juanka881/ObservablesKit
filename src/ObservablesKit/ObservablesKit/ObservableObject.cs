using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents an object that provides property change
	/// notifications
	/// </summary>
	public class ObservableObject : IObservableObject
	{
		/// <summary>
		/// Notifies that a property has changed
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void NotifyPropertyChanged(string propertyName)
		{
			var handler = this.PropertyChanged;

			if (handler == null)
				return;

			handler(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}