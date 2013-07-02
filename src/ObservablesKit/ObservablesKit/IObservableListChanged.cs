using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;

namespace ObservablesKit
{
	/// <summary>
	/// represents a method that will handle a list changed event
	/// </summary>
	/// <typeparam name="T">the type of item that the list contains</typeparam>
	/// <param name="sender">The sender.</param>
	/// <param name="e">The event.</param>
	public delegate void ListItemEventHandler<T>(object sender, ListItemEventArgs<T> e);

	/// <summary>
	/// represents a method that will handle a list item replace event
	/// </summary>
	/// <typeparam name="T">the type of item that the list contains</typeparam>
	/// <param name="sender">The sender.</param>
	/// <param name="e">The event.</param>
	public delegate void ListItemReplacedEventHandler<T>(object sender, ListItemReplacedEventArgs<T> e);

	/// <summary>
	/// represents a method that will handle a list cleared event
	/// </summary>
	/// <typeparam name="T">the type of item that the list contains</typeparam>
	/// <param name="sender">The sender.</param>
	/// <param name="e">The event.</param>
	public delegate void ListClearedEventHandler<T>(object sender, ListClearedEventArgs<T> e);

	/// <summary>
	/// represents an list that notifies when changes are made to it.
	/// </summary>
	/// <typeparam name="T">the type of item that the list contains</typeparam>
	public interface IObservableListChanged<T> : INotifyCollectionChanged
	{
		/// <summary>
		/// Occurs when an is item added.
		/// </summary>
		event ListItemEventHandler<T> ItemAdded;

		/// <summary>
		/// Occurs when an item is removed.
		/// </summary>
		event ListItemEventHandler<T> ItemRemoved;

		/// <summary>
		/// Occurs when an item is replaced.
		/// </summary>
		event ListItemReplacedEventHandler<T> ItemReplaced;

		/// <summary>
		/// Occurs when the list is cleared.
		/// </summary>
		event ListClearedEventHandler<T> ItemsCleared;
	}
}