using System;
using System.Drawing;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace csBump.Utils
{
	internal class BoolArray
	{
		#region rMembers

		public bool[] mItems;
		public int mSize;
		public readonly bool mOrderPreserved;

		#endregion rMembers




		#region rInit

		/// <summary>
		/// Creates an ordered array with a capacity of 16.
		/// </summary>
		public BoolArray() : this(true, 16)
		{
			
		}



		/// <summary>
		/// Creates an ordered array with the specified capacity.
		/// </summary>
		/// <param name="capacity">Any elements added beyond this will cause the backing array to be grown</param>
		public BoolArray(int capacity) : this(true, capacity)
		{
		}



		/// <summary>
		/// Create bool array with specified order capacity
		/// </summary>
		/// <param name="ordered">If false, methods that remove elements may change the order of other elements in the array, which avoids a memory copy.</param>
		/// <param name="capacity">Any elements added beyond this will cause the backing array to be grown.</param>
		public BoolArray(bool ordered, int capacity)
		{
			mOrderPreserved = ordered;
			mItems = new bool[capacity];
			mSize = 0;
		}



		/// <summary>
		/// Creates a new array containing the elements in the specific array. The new array will be ordered if the specific array is
		/// ordered. The capacity is set to the number of elements, so any subsequent elements added will cause the backing array to be
		/// grown.
		/// </summary>
		/// <param name="other">Array to copy.</param>
		public BoolArray(BoolArray other)
		{
			mOrderPreserved = other.mOrderPreserved;
			mSize = other.mSize;
			mItems = new bool[mSize];

			Array.Copy(other.mItems, mItems, mSize);
		}



		/// <summary>
		/// Creates a new ordered array containing the elements in the specified array. The capacity is set to the number of elements,
		/// so any subsequent elements added will cause the backing array to be grown.
		/// </summary>
		/// <param name="array">bool array to copy.</param>
		public BoolArray(bool[] array) : this(true, array, 0, array.Length)
		{
		}



		/// <summary>
		/// Creates a new array containing the elements in the specified array. The capacity is set to the number of elements, so any
		/// subsequent elements added will cause the backing array to be grown.
		/// </summary>
		/// <param name="ordered"> If false, methods that remove elements may change the order of other elements in the array, which avoids a memory copy.</param>
		/// <param name="array">Array to copy</param>
		/// <param name="startIndex">Start index to copy from</param>
		/// <param name="count">How many elements to copy.</param>
		public BoolArray(bool ordered, bool[] array, int startIndex, int count) : this(ordered, count)
		{
			mSize = count;
			Array.Copy(array, startIndex, mItems, 0, count);
		}

		#endregion rInit





		#region rAccess

		public void Add(bool value)
		{
			if (mSize == mItems.Length)
			{
				mItems = Resize(Math.Max(8, (int)(mSize * 1.75f)));
			}

			mItems[mSize++] = value;
		}

		public void AddAll(BoolArray other)
		{
			AddAll(other, 0, other.mSize);
		}

		public void AddAll(BoolArray array, int offset, int length)
		{
			if (offset + length > array.mSize)
			{
				throw new ArgumentOutOfRangeException("offset + length must be <= size: " + offset + " + " + length + " <= " + array.mSize);
			}

			AddAll(array.mItems, offset, length);
		}

		public void AddAll(params bool[] array)
		{
			AddAll(array, 0, array.Length);
		}

		public void AddAll(bool[] array, int offset, int length)
		{
			int sizeNeeded = mSize + length;
			if (sizeNeeded > mItems.Length)
			{
				mItems = Resize(Math.Max(8, (int)(sizeNeeded * 1.75f)));
			}

			Array.Copy(array, offset, mItems, mSize, length);
			
			mSize += length;
		}

		public bool Get(int index)
		{
			if (index >= mSize)
			{
				throw new IndexOutOfRangeException("index can't be >= size: " + index + " >= " + mSize);
			}

			return mItems[index];
		}

		public void Set(int index, bool value)
		{
			if (index >= mSize)
			{
				throw new IndexOutOfRangeException("index can't be >= size: " + index + " >= " + mSize);
			}

			mItems[index] = value;
		}

		public void Insert(int index, bool value)
		{
			if (index > mSize)
			{
				throw new IndexOutOfRangeException("index can't be > size: " + index + " > " + mSize);
			}

			if (mSize >= mItems.Length)
			{
				mItems = Resize(Math.Max(8, (int)(mSize * 1.75f)));
			}

			if (mOrderPreserved)
			{
				Array.Copy(mItems, index, mItems, index + 1, mSize - index);
			}
			else
			{
				mItems[mSize] = mItems[index];
			}
			
			mSize++;
			mItems[index] = value;
		}

		public void Swap(int first, int second)
		{
			if (first >= mSize)
			{
				throw new IndexOutOfRangeException("first can't be >= size: " + first + " >= " + mSize);
			}

			if (second >= mSize)
			{
				throw new IndexOutOfRangeException("second can't be >= size: " + second + " >= " + mSize);
			}

			bool temp = mItems[first];

			mItems[first] = mItems[second];
			mItems[second] = temp;
		}

		/** Removes and returns the item at the specified index. */
		public bool RemoveIndex(int index)
		{
			if (index >= mSize)
			{
				throw new IndexOutOfRangeException("index can't be >= size: " + index + " >= " + mSize);
			}

			bool value = mItems[index];
			mSize--;
			if (mOrderPreserved)
			{
				Array.Copy(mItems, index + 1, mItems, index, mSize - index);
			}
			else
			{
				mItems[index] = mItems[mSize];
			}

			return value;
		}

		/** Removes the items between the specified indices, inclusive. */
		public void RemoveRange(int start, int end)
		{
			if (end >= mSize)
			{
				throw new IndexOutOfRangeException("end can't be >= size: " + end + " >= " + mSize);
			}
			if (start > end)
			{
				throw new IndexOutOfRangeException("start can't be > end: " + start + " > " + end);
			}
			
			int count = end - start + 1;
			Array.Copy(mItems, start + count, mItems, start, mSize - (start + count));

			mSize -= count;
		}

		/** Removes and returns the last item. */
		public bool Pop()
		{
			return mItems[--mSize];
		}

		/** Returns the last item. */
		public bool Peek()
		{
			return mItems[mSize - 1];
		}

		/** Returns the first item. */
		public bool First()
		{
			if (mSize == 0)
			{
				throw new Exception("Array is empty.");
			}

			return mItems[0];
		}

		public void Clear()
		{
			mSize = 0;
		}

		/** Reduces the size of the backing array to the size of the actual items. This is useful to release memory when many items
		 * have been removed, or if it is known that more items will not be added.
		 * @return {@link #items} */
		public bool[] Shrink()
		{
			if (mItems.Length != mSize)
			{
				Resize(mSize);
			}

			return mItems;
		}

		/** Increases the size of the backing array to accommodate the specified number of additional items. Useful before adding many
		 * items to avoid multiple backing array resizes.
		 * @return {@link #items} */
		public bool[] EnsureCapacity(int additionalCapacity)
		{
			int sizeNeeded = mSize + additionalCapacity;
			if (sizeNeeded > mItems.Length)
			{
				Resize(Math.Max(8, sizeNeeded));
			}

			return mItems;
		}

		/** Sets the array size, leaving any values beyond the current size undefined.
		 * @return {@link #items} */
		public bool[] SetSize(int newSize)
		{
			if (newSize > mItems.Length)
			{
				Resize(Math.Max(8, newSize));
			}

			mSize = newSize;
			return mItems;
		}

		protected bool[] Resize(int newSize)
		{
			bool[] newItems = new bool[newSize];
			bool[] items = mItems;
			Array.Copy(items, 0, newItems, 0, Math.Min(mSize, newItems.Length));
			mItems = newItems;
			return newItems;
		}

		public void Reverse()
		{
			bool[] items = mItems;
			for (int i = 0, lastIndex = mSize - 1, n = mSize / 2; i < n; i++)
			{
				int ii = lastIndex - i;
				bool temp = items[i];
				items[i] = items[ii];
				items[ii] = temp;
			}
		}

		// TO DO
		//public void shuffle()
		//{
		//	bool[] items = mItems;
		//	for (int i = mSize - 1; i >= 0; i--)
		//	{
		//		int ii = MathUtils.random(i);
		//		bool temp = items[i];
		//		items[i] = items[ii];
		//		items[ii] = temp;
		//	}
		//}

		/** Reduces the size of the array to the specified size. If the array is already smaller than the specified size, no action is
		 * taken. */
		public void Truncate(int newSize)
		{
			if (mSize > newSize)
			{
				mSize = newSize;
			}
		}

		/** Returns a random item from the array, or false if the array is empty. */
		// TO DO
		//public bool random()
		//{
		//	if (size == 0) return false;
		//	return items[MathUtils.random(0, size - 1)];
		//}

		public bool[] ToArray()
		{
			bool[] array = new bool[mSize];
			Array.Copy(mItems, 0, array, 0, mSize);
			return array;
		}

		public override int GetHashCode()
		{
			if (!mOrderPreserved)
			{
				return 0;
			}

			bool[] items = mItems;
			int h = 1;
			for (int i = 0, n = mSize; i < n; i++)
			{
				h = h * 31 + (items[i] ? 1231 : 1237);
			}
			return h;
		}

		public override bool Equals(object? other)
		{
			if (other is null || other is not BoolArray)
			{
				return false;
			}

			if (object.ReferenceEquals(this, other))
			{
				return true;
			}

			BoolArray otherArray = (BoolArray)other;
			if (!otherArray.mOrderPreserved)
			{
				return false;
			}

			if (mSize != otherArray.mSize)
			{
				return false;
			}

			for (int i = 0; i < mSize; i++)
			{
				if (mItems[i] != otherArray.mItems[i]) return false;
			}

			return true;
		}

		public override string ToString()
		{
			string? ret = mItems.ToString();
			return ret is null ? "[]" : ret;
		}

		#endregion rAccess
	}
}
