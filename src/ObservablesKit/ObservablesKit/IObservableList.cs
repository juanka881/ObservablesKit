using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// Represents a list that notifies when changes are made to it
	/// </summary>
	/// <typeparam name="T">the type that the list contains</typeparam>
	public interface IObservableList<T> : IObservableObject, IList<T>, IObservableListChanged<T>
	{
		
	}
}