using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// event data to represents a list item event
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListItemEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets the index of the item in the list
		/// </summary>
		/// <value>
		/// The index.
		/// </value>
		public int Index { get; private set; }

		/// <summary>
		/// Gets the item in the list
		/// </summary>
		/// <value>
		/// The item.
		/// </value>
		public T Item { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ListItemEventArgs{T}"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public ListItemEventArgs(int index, T item)
		{
			this.Index = index;
			this.Item = item;
		}
	}
}