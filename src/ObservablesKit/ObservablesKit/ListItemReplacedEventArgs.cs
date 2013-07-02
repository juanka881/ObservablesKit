using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// event data to represent and item replace in a list
	/// </summary>
	/// <typeparam name="T">the type of the item in the list</typeparam>
	public class ListItemReplacedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets the index of the item that was replaced
		/// </summary>
		/// <value>
		/// The index.
		/// </value>
		public int Index { get; private set; }

		/// <summary>
		/// Gets the old item.
		/// </summary>
		/// <value>
		/// The old item.
		/// </value>
		public T OldItem { get; private set; }

		/// <summary>
		/// Gets the new item.
		/// </summary>
		/// <value>
		/// The new item.
		/// </value>
		public T NewItem { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ListItemReplacedEventArgs{T}"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldItem">The old item.</param>
		/// <param name="newItem">The new item.</param>
		public ListItemReplacedEventArgs(int index, T oldItem, T newItem)
		{
			this.Index = index;
			this.OldItem = oldItem;
			this.NewItem = NewItem;
		}
	}
}