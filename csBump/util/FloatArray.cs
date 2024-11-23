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
	public class FloatArray
	{
		public float[] items;
		public int size;
		public bool ordered;
		/// <summary>
		/// Creates an ordered array with a capacity of 16.
		/// </summary>
		public FloatArray() : this(true, 16)
		{
		}

		/// <summary>
		/// Creates an ordered array with the specified capacity.
		/// </summary>
		public FloatArray(int capacity) : this(true, capacity)
		{
		}

		public FloatArray(bool ordered, int capacity)
		{
			this.ordered = ordered;
			items = new float[capacity];
		}

		public FloatArray(FloatArray array)
		{
			this.ordered = array.ordered;
			size = array.size;
			items = new float[size];
			System.Arraycopy(array.items, 0, items, 0, size);
		}

		public FloatArray(float[] array) : this(true, array, 0, array.Length)
		{
		}

		public FloatArray(bool ordered, float[] array, int startIndex, int count) : this(ordered, count)
		{
			size = count;
			System.Arraycopy(array, startIndex, items, 0, count);
		}

		public virtual void Add(float value)
		{
			float[] items = this.items;
			if (size == items.Length)
				items = Resize(Math.Max(8, (int)(size * 1.75F)));
			items[size++] = value;
		}

		public virtual void AddAll(FloatArray array)
		{
			AddAll(array, 0, array.size);
		}

		public virtual void AddAll(FloatArray array, int offset, int length)
		{
			if (offset + length > array.size)
				throw new ArgumentException("offset + length must be <= size: " + offset + " + " + length + " <= " + array.size);
			AddAll(array.items, offset, length);
		}

		public virtual void AddAll(params float[] array)
		{
			AddAll(array, 0, array.Length);
		}

		public virtual void AddAll(float[] array, int offset, int length)
		{
			float[] items = this.items;
			int sizeNeeded = size + length;
			if (sizeNeeded > items.Length)
				items = Resize(Math.Max(8, (int)(sizeNeeded * 1.75F)));
			System.Arraycopy(array, offset, items, size, length);
			size += length;
		}

		public virtual float Get(int index)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			return items[index];
		}

		public virtual void Set(int index, float value)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			items[index] = value;
		}

		public virtual void Incr(int index, float value)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			items[index] += value;
		}

		public virtual void Mul(int index, float value)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			items[index] *= value;
		}

		public virtual void Insert(int index, float value)
		{
			if (index > size)
				throw new IndexOutOfBoundsException("index can't be > size: " + index + " > " + size);
			float[] items = this.items;
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
			float[] items = this.items;
			float firstValue = items[first];
			items[first] = items[second];
			items[second] = firstValue;
		}

		public virtual bool Contains(float value)
		{
			int i = size - 1;
			float[] items = this.items;
			while (i >= 0)
				if (items[i--] == value)
					return true;
			return false;
		}

		public virtual int IndexOf(float value)
		{
			float[] items = this.items;
			for (int i = 0, n = size; i < n; i++)
				if (items[i] == value)
					return i;
			return -1;
		}

		public virtual int LastIndexOf(char value)
		{
			float[] items = this.items;
			for (int i = size - 1; i >= 0; i--)
				if (items[i] == value)
					return i;
			return -1;
		}

		public virtual bool RemoveValue(float value)
		{
			float[] items = this.items;
			for (int i = 0, n = size; i < n; i++)
			{
				if (items[i] == value)
				{
					RemoveIndex(i);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Removes and returns the item at the specified index.
		/// </summary>
		public virtual float RemoveIndex(int index)
		{
			if (index >= size)
				throw new IndexOutOfBoundsException("index can't be >= size: " + index + " >= " + size);
			float[] items = this.items;
			float value = items[index];
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
			float[] items = this.items;
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

		public virtual bool RemoveAll(FloatArray array)
		{
			int size = this.size;
			int startSize = size;
			float[] items = this.items;
			for (int i = 0, n = array.size; i < n; i++)
			{
				float item = array[i];
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
		public virtual float Pop()
		{
			return items[--size];
		}

		/// <summary>
		/// Returns the last item.
		/// </summary>
		public virtual float Peek()
		{
			return items[size - 1];
		}

		/// <summary>
		/// Returns the first item.
		/// </summary>
		public virtual float First()
		{
			if (size == 0)
				throw new InvalidOperationException("Array is empty.");
			return items[0];
		}

		public virtual void Clear()
		{
			size = 0;
		}

		public virtual float[] Shrink()
		{
			if (items.Length != size)
				Resize(size);
			return items;
		}

		public virtual float[] EnsureCapacity(int additionalCapacity)
		{
			int sizeNeeded = size + additionalCapacity;
			if (sizeNeeded > items.Length)
				Resize(Math.Max(8, sizeNeeded));
			return items;
		}

		public virtual float[] SetSize(int newSize)
		{
			if (newSize > items.Length)
				Resize(Math.Max(8, newSize));
			size = newSize;
			return items;
		}

		protected virtual float[] Resize(int newSize)
		{
			float[] newItems = new float[newSize];
			float[] items = this.items;
			System.Arraycopy(items, 0, newItems, 0, Math.Min(size, newItems.Length));
			this.items = newItems;
			return newItems;
		}

		public virtual void Sort()
		{
			Arrays.Sort(items, 0, size);
		}

		public virtual void Reverse()
		{
			float[] items = this.items;
			for (int i = 0, lastIndex = size - 1, n = size / 2; i < n; i++)
			{
				int ii = lastIndex - i;
				float temp = items[i];
				items[i] = items[ii];
				items[ii] = temp;
			}
		}

		public virtual void Shuffle()
		{
			float[] items = this.items;
			for (int i = size - 1; i >= 0; i--)
			{
				int ii = MathUtils.Random(i);
				float temp = items[i];
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
		/// Returns a random item from the array, or zero if the array is empty.
		/// </summary>
		public virtual float Random()
		{
			if (size == 0)
				return 0;
			return items[MathUtils.Random(0, size - 1)];
		}

		public virtual float[] ToArray()
		{
			float[] array = new float[size];
			System.Arraycopy(items, 0, array, 0, size);
			return array;
		}

		public virtual int GetHashCode()
		{
			if (!ordered)
				return base.GetHashCode();
			float[] items = this.items;
			int h = 1;
			for (int i = 0, n = size; i < n; i++)
				h = h * 31 + Float.FloatToIntBits(items[i]);
			return h;
		}

		public virtual bool Equals(object @object)
		{
			if (@object == this)
				return true;
			if (!ordered)
				return false;
			if (!(@object is FloatArray))
				return false;
			FloatArray array = (FloatArray)@object;
			if (!array.ordered)
				return false;
			int n = size;
			if (n != array.size)
				return false;
			float[] items1 = this.items;
			float[] items2 = array.items;
			for (int i = 0; i < n; i++)
				if (items1[i] != items2[i])
					return false;
			return true;
		}

		public virtual bool Equals(object @object, float epsilon)
		{
			if (@object == this)
				return true;
			if (!(@object is FloatArray))
				return false;
			FloatArray array = (FloatArray)@object;
			int n = size;
			if (n != array.size)
				return false;
			if (!ordered)
				return false;
			if (!array.ordered)
				return false;
			float[] items1 = this.items;
			float[] items2 = array.items;
			for (int i = 0; i < n; i++)
				if (Math.Abs(items1[i] - items2[i]) > epsilon)
					return false;
			return true;
		}

		public virtual string ToString()
		{
			if (size == 0)
				return "[]";
			float[] items = this.items;
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
			float[] items = this.items;
			StringBuilder buffer = new StringBuilder(32);
			buffer.Append(items[0]);
			for (int i = 1; i < size; i++)
			{
				buffer.Append(separator);
				buffer.Append(items[i]);
			}

			return buffer.ToString();
		}

		/// <remarks>@see#FloatArray(float[])</remarks>
		public static FloatArray With(params float[] array)
		{
			return new FloatArray(array);
		}
	}
}