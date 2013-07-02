using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// event data to represent that a list was cleared
	/// </summary>
	/// <typeparam name="T">the type of the item in the list</typeparam>
	public class ListClearedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets the items in the list before it was cleared
		/// </summary>
		/// <value>
		/// The items.
		/// </value>
		public IList<T> Items { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ListClearedEventArgs{T}"/> class.
		/// </summary>
		/// <param name="items">The items in the list before it was cleared.</param>
		public ListClearedEventArgs(IEnumerable<T> items)
		{
			this.Items = new List<T>(items);
		}
	}
}