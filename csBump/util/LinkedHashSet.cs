using System;
using System.Collections;
using System.Collections.Generic;

namespace csBump.util
{
	public class LinkedHashSet<T> : ISet<T> where T : notnull
	{

		private readonly IDictionary<T, LinkedListNode<T>> mDict;
		private readonly LinkedList<T> mList;

		public LinkedHashSet(int initialCapacity)
		{
			this.mDict = new Dictionary<T, LinkedListNode<T>>(initialCapacity);
			this.mList = new LinkedList<T>();
		}

		public LinkedHashSet()
		{
			this.mDict = new Dictionary<T, LinkedListNode<T>>();
			this.mList = new LinkedList<T>();
		}

		public LinkedHashSet(IEnumerable<T> e) : this()
		{
			addEnumerable(e);
		}

		public LinkedHashSet(int initialCapacity, IEnumerable<T> e) : this(initialCapacity)
		{
			addEnumerable(e);
		}

		private void addEnumerable(IEnumerable<T> e)
		{
			foreach (T t in e)
			{
				Add(t);
			}
		}

		//
		// ISet implementation
		//

		public bool Add(T item)
		{
			if (this.mDict.ContainsKey(item))
			{
				return false;
			}
			LinkedListNode<T> node = this.mList.AddLast(item);
			this.mDict[item] = node;
			return true;
		}

		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			foreach (T t in other)
			{
				Remove(t);
			}
		}

		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			T[] ts = new T[Count];
			CopyTo(ts, 0);
			foreach (T t in ts)
			{
				if (!System.Linq.Enumerable.Contains(other, t))
				{
					Remove(t);
				}
			}
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			int contains = 0;
			int noContains = 0;
			foreach (T t in other)
			{
				if (Contains(t))
				{
					contains++;
				}
				else
				{
					noContains++;
				}
			}
			return contains == Count && noContains > 0;
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			int otherCount = System.Linq.Enumerable.Count(other);
			if (Count <= otherCount)
			{
				return false;
			}
			int contains = 0;
			int noContains = 0;
			foreach (T t in this)
			{
				if (System.Linq.Enumerable.Contains(other, t))
				{
					contains++;
				}
				else
				{
					noContains++;
				}
			}
			return contains == otherCount && noContains > 0;
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			foreach (T t in this)
			{
				if (!System.Linq.Enumerable.Contains(other, t))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			foreach (T t in other)
			{
				if (!Contains(t))
				{
					return false;
				}
			}
			return true;
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			foreach (T t in other)
			{
				if (Contains(t))
				{
					return true;
				}
			}
			return false;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			int otherCount = System.Linq.Enumerable.Count(other);
			if (Count != otherCount)
			{
				return false;
			}
			return IsSupersetOf(other);
		}

		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			T[] ts = new T[Count];
			CopyTo(ts, 0);
			HashSet<T> otherList = new HashSet<T>(other);
			foreach (T t in ts)
			{
				if (otherList.Contains(t))
				{
					Remove(t);
					otherList.Remove(t);
				}
			}
			foreach (T t in otherList)
			{
				Add(t);
			}
		}

		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other cannot be null");
			}
			foreach (T t in other)
			{
				Add(t);
			}
		}

		//
		// ICollection<T> implementation
		//

		public int Count
		{
			get
			{
				return this.mDict.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this.mDict.IsReadOnly;
			}
		}

		void ICollection<T>.Add(T item)
		{
			Add(item);
		}

		public void Clear()
		{
			this.mDict.Clear();
			this.mList.Clear();
		}

		public bool Contains(T item)
		{
			return this.mDict.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.mList.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			LinkedListNode<T> node;
			if (!this.mDict.TryGetValue(item, out node))
			{
				return false;
			}
			this.mDict.Remove(item);
			this.mList.Remove(node);
			return true;
		}

		//
		// IEnumerable<T> implementation
		//

		public IEnumerator<T> GetEnumerator()
		{
			return this.mList.GetEnumerator();
		}

		//
		// IEnumerable implementation
		//

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.mList.GetEnumerator();
		}

	}
}
