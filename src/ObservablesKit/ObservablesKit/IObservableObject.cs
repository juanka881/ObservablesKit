using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents an object that provides property change notifications when
	/// its properties changes
	/// </summary>
	public interface IObservableObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Notifies a property changed
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		void NotifyPropertyChanged(string propertyName);
	}
}