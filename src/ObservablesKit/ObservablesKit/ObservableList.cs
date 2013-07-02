using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit
{
	/// <summary>
	/// represents a list that provides change notifications
	/// </summary>
	/// <typeparam name="T">the type of item in the list</typeparam>
	public class ObservableList<T> : ObservableObject, IObservableList<T>
	{
		private readonly List<T> list;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
		/// </summary>
		public ObservableList()
		{
			this.list = new List<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
		/// </summary>
		/// <param name="capacity">The capacity of the list.</param>
		public ObservableList(int capacity)
		{
			this.list = new List<T>(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
		/// </summary>
		/// <param name="items">The items to place initially on the list.</param>
		public ObservableList(IEnumerable<T> items)
		{
			this.list = new List<T>(items);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		private void InsertCore(int index, T item)
		{
			this.list.Insert(index, item);
			this.NotifyItemAdded(index, item);
		}

		private void RemoveCore(int index)
		{
			var item = this.list[index];
			this.RemoveAt(index);
			this.NotifyItemRemoved(index, item);
		}

		private void ReplaceCore(int index, T item)
		{
			var oldItem = this.list[index];
			this.list[index] = item;
			this.NotifyItemReplace(index, oldItem, item);
		}

		private void ClearCore()
		{
			var items = Enumerable.Empty<T>();
			var handler = this.ItemsCleared;

			if (handler != null)
			{
				var copy = new T[this.list.Count];
				this.list.CopyTo(copy);
				items = copy;
			}
			
			this.list.Clear();
			this.NotifyItemsCleared(items);
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		public void Add(T item)
		{
			this.InsertCore(this.list.Count, item);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		public void Clear()
		{
			this.ClearCore();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		/// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return this.list.Contains(item);
		}

		/// <summary>
		/// Copies the list to an array
		/// </summary>
		/// <param name="array">The array to copy to.</param>
		/// <param name="arrayIndex">Index of the array where to start setting items at.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		/// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public bool Remove(T item)
		{
			var idx = this.IndexOf(item);

			if (idx == -1)
				return false;
			
			this.RemoveCore(idx);
			return true;
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		///   </returns>
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
		///   </returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <returns>
		/// The index of <paramref name="item" /> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(T item)
		{
			return this.list.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		public void Insert(int index, T item)
		{
			this.InsertCore(index, item);
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			this.RemoveCore(index);
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>the item at the specified index</returns>
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}

			set
			{
				this.ReplaceCore(index, value);
			}
		}

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Occurs when an item is added.
		/// </summary>
		public event ListItemEventHandler<T> ItemAdded;

		/// <summary>
		/// Occurs when an item is removed.
		/// </summary>
		public event ListItemEventHandler<T> ItemRemoved;

		/// <summary>
		/// Occurs when an item is replaced.
		/// </summary>
		public event ListItemReplacedEventHandler<T> ItemReplaced;

		/// <summary>
		/// Occurs when the list is cleared.
		/// </summary>
		public event ListClearedEventHandler<T> ItemsCleared;

		/// <summary>
		/// Notifies that an item has been added.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void NotifyItemAdded(int index, T item)
		{
			var collectionHandler = this.CollectionChanged;

			if (collectionHandler != null)
			{
				var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
				collectionHandler(this, e);

				this.NotifyCountChanged();
				this.NotifyItemsChanged();
			}

			var itemHandler = this.ItemAdded;

			if (itemHandler != null)
			{
				var e = new ListItemEventArgs<T>(index, item);
				itemHandler(this, e);
			}
		}

		/// <summary>
		/// Notifies that an item has been removed.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void NotifyItemRemoved(int index, T item)
		{
			var collectionHandler = this.CollectionChanged;

			if (collectionHandler != null)
			{
				var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
				collectionHandler(this, e);

				this.NotifyCountChanged();
				this.NotifyItemsChanged();
			}

			var itemHandler = this.ItemRemoved;

			if (itemHandler != null)
			{
				var e = new ListItemEventArgs<T>(index, item);
				itemHandler(this, e);
			}
		}

		/// <summary>
		/// Notifies that an item has been replaced.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldItem">The old item.</param>
		/// <param name="newItem">The new item.</param>
		public void NotifyItemReplace(int index, T oldItem, T newItem)
		{
			var collectionHandler = this.CollectionChanged;

			if (collectionHandler != null)
			{
				var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
				collectionHandler(this, e);

				this.NotifyItemsChanged();
			}

			var itemHandler = this.ItemReplaced;

			if (itemHandler != null)
			{
				var e = new ListItemReplacedEventArgs<T>(index, oldItem, newItem);
				itemHandler(this, e);
			}
		}

		/// <summary>
		/// Notifies that the list has been cleared.
		/// </summary>
		/// <param name="items">The items that were cleared off.</param>
		public void NotifyItemsCleared(IEnumerable<T> items)
		{
			var collectionHandler = this.CollectionChanged;

			if (collectionHandler != null)
			{
				var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				collectionHandler(this, e);

				this.NotifyCountChanged();
				this.NotifyItemsChanged();
			}

			var itemHandler = this.ItemsCleared;

			if (itemHandler != null)
			{
				var e = new ListClearedEventArgs<T>(items ?? Enumerable.Empty<T>());
				itemHandler(this, e);
			}
		}

		/// <summary>
		/// Notifies that the count property changed changed.
		/// </summary>
		public void NotifyCountChanged()
		{
			this.NotifyPropertyChanged("Count");
		}

		/// <summary>
		/// Notifies the items indexer property changed.
		/// </summary>
		public void NotifyItemsChanged()
		{
			this.NotifyPropertyChanged("Item[]");
		}
	}
}