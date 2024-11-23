/*
 * Copyright 2017 DongBat.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace csBump.util
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class BooleanArray
	{
		public bool[] items;
		public int size;
		public bool ordered;
		/// <summary>
		/// Creates an ordered array with a capacity of 16.
		/// </summary>
		public BooleanArray() : this(true, 16)
		{
		}

		/// <summary>
		/// Creates an ordered array with the specified capacity.
		/// </summary>
		public BooleanArray(int capacity) : this(true, capacity)
		{
		}

		public BooleanArray(bool ordered, int capacity)
		{
			this.ordered = ordered;
			items = new bool[capacity];
		}

		public BooleanArray(BooleanArray array)
		{
			this.ordered = array.ordered;
			size = array.size;
			items = new bool[size];
			System.Arraycopy(array.items, 0, items, 0, size);
		}

		public BooleanArray(boolean[] array) : this(true, array, 0, array.Length)
		{
		}

		public BooleanArray(bool ordered, boolean[] array, int startIndex, int count) : this(ordered, count)
		{
			size = count;
			System.Arraycopy(array, startIndex, items, 0, count);
		}

		public virtual void Add(bool value)
		{
			bool[] items = this.items;
			if (size == items.Length)
				items = Resize(Math.Max(8, (int)(size * 1.75F)));
			items[size++] = value;
		}

		public virtual void AddAll(BooleanArray array)
		{
			AddAll(array, 0, array.size);
		}

		public virtual void AddAll(BooleanArray array, int offset, int length)
		{
			if (offset + length > array.size)
				throw new ArgumentException("offset + length must be <= size: " + offset + " + " + length + " <= " + array.size);
			AddAll(array.items, offset, length);
		}

		public virtual void AddAll(params bool[] array)
		{
			AddAll(array, 0, array.Length);
		}

		public virtual void AddAll(bool[] array, int offset, int length)
		{
			bool[] items = this.items;
			int sizeNeeded = size + length;
			if (sizeNeeded > items.Length)
				items = Resize(Math.Max(8, (int)(sizeNeeded * 1.75F)));
			System.Arraycopy(array, offset, items, size, length);
			size += length;
		}

		public virtual bool Get(int index)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			return items[index];
		}

		public virtual void Set(int index, bool value)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			items[index] = value;
		}

		public virtual void Insert(int index, bool value)
		{
			if (index > size)
				throw new IndexOutOfBoundsException("index can't be > size: " + index + " > " + size);
			bool[] items = this.items;
			if (size == items.Length)
				items = Resize(Math.Max(8, (int)(size * 1.75F)));
			if (ordered)
				System.Arraycopy(items, index, items, index + 1, size - index);
			else
				items[size] = items[index];
			size++;
			items[index] = value;
		}

		public virtual void Swap(int first, int second)
		{
			if (first >= size)
				throw new IndexOutOfBoundsException("first can't be >= size: " + first + " >= " + size);
			if (second >= size)
				throw new IndexOutOfBoundsException("second can't be >= size: " + second + " >= " + size);
			bool[] items = this.items;
			bool firstValue = items[first];
			items[first] = items[second];
			items[second] = firstValue;
		}

		/// <summary>
		/// Removes and returns the item at the specified index.
		/// </summary>
		public virtual bool RemoveIndex(int index)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			bool[] items = this.items;
			bool value = items[index];
			size--;
			if (ordered)
				System.Arraycopy(items, index + 1, items, index, size - index);
			else
				items[index] = items[size];
			return value;
		}

		/// <summary>
		/// Removes the items between the specified indices, inclusive.
		/// </summary>
		public virtual void RemoveRange(int start, int end)
		{
			if (end >= size)
				throw new IndexOutOfBoundsException("end can't be >= size: " + end + " >= " + size);
			if (start > end)
				throw new IndexOutOfBoundsException("start can't be > end: " + start + " > " + end);
			bool[] items = this.items;
			int count = end - start + 1;
			if (ordered)
				System.Arraycopy(items, start + count, items, start, size - (start + count));
			else
			{
				int lastIndex = this.size - 1;
				for (int i = 0; i < count; i++)
					items[start + i] = items[lastIndex - i];
			}

			size -= count;
		}

		public virtual bool RemoveAll(BooleanArray array)
		{
			int size = this.size;
			int startSize = size;
			bool[] items = this.items;
			for (int i = 0, n = array.size; i < n; i++)
			{
				bool item = array[i];
				for (int ii = 0; ii < size; ii++)
				{
					if (item == items[ii])
					{
						RemoveIndex(ii);
						size--;
						break;
					}
				}
			}

			return size != startSize;
		}

		/// <summary>
		/// Removes and returns the last item.
		/// </summary>
		public virtual bool Pop()
		{
			return items[--size];
		}

		/// <summary>
		/// Returns the last item.
		/// </summary>
		public virtual bool Peek()
		{
			return items[size - 1];
		}

		/// <summary>
		/// Returns the first item.
		/// </summary>
		public virtual bool First()
		{
			if (size == 0)
				throw new InvalidOperationException("Array is empty.");
			return items[0];
		}

		public virtual void Clear()
		{
			size = 0;
		}

		public virtual boolean[] Shrink()
		{
			if (items.Length != size)
				Resize(size);
			return items;
		}

		public virtual boolean[] EnsureCapacity(int additionalCapacity)
		{
			int sizeNeeded = size + additionalCapacity;
			if (sizeNeeded > items.Length)
				Resize(Math.Max(8, sizeNeeded));
			return items;
		}

		public virtual boolean[] SetSize(int newSize)
		{
			if (newSize > items.Length)
				Resize(Math.Max(8, newSize));
			size = newSize;
			return items;
		}

		protected virtual boolean[] Resize(int newSize)
		{
			bool[] newItems = new bool[newSize];
			bool[] items = this.items;
			System.Arraycopy(items, 0, newItems, 0, Math.Min(size, newItems.Length));
			this.items = newItems;
			return newItems;
		}

		public virtual void Reverse()
		{
			bool[] items = this.items;
			for (int i = 0, lastIndex = size - 1, n = size / 2; i < n; i++)
			{
				int ii = lastIndex - i;
				bool temp = items[i];
				items[i] = items[ii];
				items[ii] = temp;
			}
		}

		public virtual void Shuffle()
		{
			bool[] items = this.items;
			for (int i = size - 1; i >= 0; i--)
			{
				int ii = MathUtils.Random(i);
				bool temp = items[i];
				items[i] = items[ii];
				items[ii] = temp;
			}
		}

		public virtual void Truncate(int newSize)
		{
			if (size > newSize)
				size = newSize;
		}

		/// <summary>
		/// Returns a random item from the array, or false if the array is empty.
		/// </summary>
		public virtual bool Random()
		{
			if (size == 0)
				return false;
			return items[MathUtils.Random(0, size - 1)];
		}

		public virtual boolean[] ToArray()
		{
			bool[] array = new bool[size];
			System.Arraycopy(items, 0, array, 0, size);
			return array;
		}

		public virtual int GetHashCode()
		{
			if (!ordered)
				return base.GetHashCode();
			bool[] items = this.items;
			int h = 1;
			for (int i = 0, n = size; i < n; i++)
				h = h * 31 + (items[i] ? 1231 : 1237);
			return h;
		}

		public virtual bool Equals(object @object)
		{
			if (@object == this)
				return true;
			if (!ordered)
				return false;
			if (!(@object is BooleanArray))
				return false;
			BooleanArray array = (BooleanArray)@object;
			if (!array.ordered)
				return false;
			int n = size;
			if (n != array.size)
				return false;
			bool[] items1 = this.items;
			bool[] items2 = array.items;
			for (int i = 0; i < n; i++)
				if (items1[i] != items2[i])
					return false;
			return true;
		}

		public virtual string ToString()
		{
			if (size == 0)
				return "[]";
			bool[] items = this.items;
			StringBuilder buffer = new StringBuilder(32);
			buffer.Append('[');
			buffer.Append(items[0]);
			for (int i = 1; i < size; i++)
			{
				buffer.Append(", ");
				buffer.Append(items[i]);
			}

			buffer.Append(']');
			return buffer.ToString();
		}

		public virtual string ToString(string separator)
		{
			if (size == 0)
				return "";
			bool[] items = this.items;
			StringBuilder buffer = new StringBuilder(32);
			buffer.Append(items[0]);
			for (int i = 1; i < size; i++)
			{
				buffer.Append(separator);
				buffer.Append(items[i]);
			}

			return buffer.ToString();
		}

		/// <remarks>@see#BooleanArray(boolean[])</remarks>
		public static BooleanArray With(params bool[] array)
		{
			return new BooleanArray(array);
		}
	}
}