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
using csBump.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace csBump.util
{
	public class IntIntMap : Iterable<IntIntMap.Entry>
	{
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		private static readonly int PRIME2 = 0xb50d9; //0xb4b82e39;
													  //	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
													  //0xb4b82e39;
		private static readonly int PRIME3 = 0xc21f1; //0xced1c241;
													  //	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
													  //0xb4b82e39;
													  //0xced1c241;
		private static readonly int EMPTY = 0;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		public int size;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		int[] keyTable, valueTable;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		int capacity, stashSize;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		int zeroValue;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		bool hasZeroValue;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private readonly float loadFactor;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private int hashShift, mask, threshold;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private int stashCapacity;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private int pushIterations;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private Entries entries1, entries2;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private Values values1, values2;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private Keys keys1, keys2;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		private int randomState = 0xbe1f14b1;
		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		public IntIntMap() : this(51, 0.8F)
		{
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		public IntIntMap(int initialCapacity) : this(initialCapacity, 0.8F)
		{
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		public IntIntMap(int initialCapacity, float loadFactor)
		{
			if (initialCapacity < 0)
				throw new ArgumentException("initialCapacity must be >= 0: " + initialCapacity);
			initialCapacity = MathUtils.NextPowerOfTwo((int)Math.Ceil(initialCapacity / loadFactor));
			if (initialCapacity > 1 << 30)
				throw new ArgumentException("initialCapacity is too large: " + initialCapacity);
			capacity = initialCapacity;
			if (loadFactor <= 0)
				throw new ArgumentException("loadFactor must be > 0: " + loadFactor);
			this.loadFactor = loadFactor;
			threshold = (int)(capacity * loadFactor);
			mask = capacity - 1;
			hashShift = 31 - Integer.NumberOfTrailingZeros(capacity);
			stashCapacity = Math.Max(3, (int)Math.Ceil(Math.Log(capacity)) * 2);
			pushIterations = Math.Max(Math.Min(capacity, 8), (int)Math.Sqrt(capacity) / 8);
			keyTable = new int[capacity + stashCapacity];
			valueTable = new int[keyTable.Length];
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		public IntIntMap(IntIntMap map) : this((int)Math.Floor(map.capacity * map.loadFactor), map.loadFactor)
		{
			stashSize = map.stashSize;
			System.Arraycopy(map.keyTable, 0, keyTable, 0, map.keyTable.Length);
			System.Arraycopy(map.valueTable, 0, valueTable, 0, map.valueTable.Length);
			size = map.size;
			zeroValue = map.zeroValue;
			hasZeroValue = map.hasZeroValue;
			randomState = map.randomState;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		public virtual void Put(int key, int value)
		{
			if (key == 0)
			{
				zeroValue = value;
				if (!hasZeroValue)
				{
					hasZeroValue = true;
					size++;
				}

				return;
			}

			int[] keyTable = this.keyTable;

			// Check for existing keys.
			int index1 = key & mask;
			int key1 = keyTable[index1];
			if (key == key1)
			{
				valueTable[index1] = value;
				return;
			}

			int index2 = Hash2(key);
			int key2 = keyTable[index2];
			if (key == key2)
			{
				valueTable[index2] = value;
				return;
			}

			int index3 = Hash3(key);
			int key3 = keyTable[index3];
			if (key == key3)
			{
				valueTable[index3] = value;
				return;
			}


			// Update key in the stash.
			for (int i = capacity, n = i + stashSize; i < n; i++)
			{
				if (key == keyTable[i])
				{
					valueTable[i] = value;
					return;
				}
			}


			// Check for empty buckets.
			if (key1 == EMPTY)
			{
				keyTable[index1] = key;
				valueTable[index1] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			if (key2 == EMPTY)
			{
				keyTable[index2] = key;
				valueTable[index2] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			if (key3 == EMPTY)
			{
				keyTable[index3] = key;
				valueTable[index3] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			Push(key, value, index1, key1, index2, key2, index3, key3);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		public virtual void PutAll(IntIntMap map)
		{
			foreach (Entry entry in map.Entries())
				Put(entry.key, entry.value);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		private void PutResize(int key, int value)
		{
			if (key == 0)
			{
				zeroValue = value;
				hasZeroValue = true;
				return;
			}


			// Check for empty buckets.
			int index1 = key & mask;
			int key1 = keyTable[index1];
			if (key1 == EMPTY)
			{
				keyTable[index1] = key;
				valueTable[index1] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			int index2 = Hash2(key);
			int key2 = keyTable[index2];
			if (key2 == EMPTY)
			{
				keyTable[index2] = key;
				valueTable[index2] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			int index3 = Hash3(key);
			int key3 = keyTable[index3];
			if (key3 == EMPTY)
			{
				keyTable[index3] = key;
				valueTable[index3] = value;
				if (size++ >= threshold)
					Resize(capacity << 1);
				return;
			}

			Push(key, value, index1, key1, index2, key2, index3, key3);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		private void Push(int insertKey, int insertValue, int index1, int key1, int index2, int key2, int index3, int key3)
		{
			int[] keyTable = this.keyTable;
			int[] valueTable = this.valueTable;
			int mask = this.mask;

			// Push keys until an empty bucket is found.
			int evictedKey;
			int evictedValue;
			int i = 0, pushIterations = this.pushIterations;
			do
			{

				// Replace the key and value for one of the hashes.
				switch (Random(3))
				{
					case 0:
						evictedKey = key1;
						evictedValue = valueTable[index1];
						keyTable[index1] = insertKey;
						valueTable[index1] = insertValue;
						break;
					case 1:
						evictedKey = key2;
						evictedValue = valueTable[index2];
						keyTable[index2] = insertKey;
						valueTable[index2] = insertValue;
						break;
					default:
						evictedKey = key3;
						evictedValue = valueTable[index3];
						keyTable[index3] = insertKey;
						valueTable[index3] = insertValue;
						break;
				}


				// If the evicted key hashes to an empty bucket, put it there and stop.
				index1 = evictedKey & mask;
				key1 = keyTable[index1];
				if (key1 == EMPTY)
				{
					keyTable[index1] = evictedKey;
					valueTable[index1] = evictedValue;
					if (size++ >= threshold)
						Resize(capacity << 1);
					return;
				}

				index2 = Hash2(evictedKey);
				key2 = keyTable[index2];
				if (key2 == EMPTY)
				{
					keyTable[index2] = evictedKey;
					valueTable[index2] = evictedValue;
					if (size++ >= threshold)
						Resize(capacity << 1);
					return;
				}

				index3 = Hash3(evictedKey);
				key3 = keyTable[index3];
				if (key3 == EMPTY)
				{
					keyTable[index3] = evictedKey;
					valueTable[index3] = evictedValue;
					if (size++ >= threshold)
						Resize(capacity << 1);
					return;
				}

				if (++i == pushIterations)
					break;
				insertKey = evictedKey;
				insertValue = evictedValue;
			}
			while (true);
			PutStash(evictedKey, evictedValue);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		private void PutStash(int key, int value)
		{
			if (stashSize == stashCapacity)
			{

				// Too many pushes occurred and the stash is full, increase the table size.
				Resize(capacity << 1);
				Put(key, value);
				return;
			}


			// Store key in the stash.
			int index = capacity + stashSize;
			keyTable[index] = key;
			valueTable[index] = value;
			stashSize++;
			size++;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		public virtual int Get(int key, int defaultValue)
		{
			if (key == 0)
			{
				if (!hasZeroValue)
					return defaultValue;
				return zeroValue;
			}

			int index = key & mask;
			if (keyTable[index] != key)
			{
				index = Hash2(key);
				if (keyTable[index] != key)
				{
					index = Hash3(key);
					if (keyTable[index] != key)
						return GetStash(key, defaultValue);
				}
			}

			return valueTable[index];
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		private int GetStash(int key, int defaultValue)
		{
			int[] keyTable = this.keyTable;
			for (int i = capacity, n = i + stashSize; i < n; i++)
				if (key == keyTable[i])
					return valueTable[i];
			return defaultValue;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		public virtual int GetAndIncrement(int key, int defaultValue, int increment)
		{
			if (key == 0)
			{
				if (hasZeroValue)
				{
					int value = zeroValue;
					zeroValue += increment;
					return value;
				}
				else
				{
					hasZeroValue = true;
					zeroValue = defaultValue + increment;
					++size;
					return defaultValue;
				}
			}

			int index = key & mask;
			if (key != keyTable[index])
			{
				index = Hash2(key);
				if (key != keyTable[index])
				{
					index = Hash3(key);
					if (key != keyTable[index])
						return GetAndIncrementStash(key, defaultValue, increment);
				}
			}

			int value = valueTable[index];
			valueTable[index] = value + increment;
			return value;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		private int GetAndIncrementStash(int key, int defaultValue, int increment)
		{
			int[] keyTable = this.keyTable;
			for (int i = capacity, n = i + stashSize; i < n; i++)
				if (key == keyTable[i])
				{
					int value = valueTable[i];
					valueTable[i] = value + increment;
					return value;
				}

			Put(key, defaultValue + increment);
			return defaultValue;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		public virtual int Remove(int key, int defaultValue)
		{
			if (key == 0)
			{
				if (!hasZeroValue)
					return defaultValue;
				hasZeroValue = false;
				size--;
				return zeroValue;
			}

			int index = key & mask;
			if (key == keyTable[index])
			{
				keyTable[index] = EMPTY;
				int oldValue = valueTable[index];
				size--;
				return oldValue;
			}

			index = Hash2(key);
			if (key == keyTable[index])
			{
				keyTable[index] = EMPTY;
				int oldValue = valueTable[index];
				size--;
				return oldValue;
			}

			index = Hash3(key);
			if (key == keyTable[index])
			{
				keyTable[index] = EMPTY;
				int oldValue = valueTable[index];
				size--;
				return oldValue;
			}

			return RemoveStash(key, defaultValue);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		virtual int RemoveStash(int key, int defaultValue)
		{
			int[] keyTable = this.keyTable;
			for (int i = capacity, n = i + stashSize; i < n; i++)
			{
				if (key == keyTable[i])
				{
					int oldValue = valueTable[i];
					RemoveStashIndex(i);
					size--;
					return oldValue;
				}
			}

			return defaultValue;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		virtual void RemoveStashIndex(int index)
		{

			// If the removed location was not last, move the last tuple to the removed location.
			stashSize--;
			int lastIndex = capacity + stashSize;
			if (index < lastIndex)
			{
				keyTable[index] = keyTable[lastIndex];
				valueTable[index] = valueTable[lastIndex];
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		public virtual void Shrink(int maximumCapacity)
		{
			if (maximumCapacity < 0)
				throw new ArgumentException("maximumCapacity must be >= 0: " + maximumCapacity);
			if (size > maximumCapacity)
				maximumCapacity = size;
			if (capacity <= maximumCapacity)
				return;
			maximumCapacity = MathUtils.NextPowerOfTwo(maximumCapacity);
			Resize(maximumCapacity);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual void Clear(int maximumCapacity)
		{
			if (capacity <= maximumCapacity)
			{
				Clear();
				return;
			}

			hasZeroValue = false;
			size = 0;
			Resize(maximumCapacity);
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual void Clear()
		{
			if (size == 0)
				return;
			int[] keyTable = this.keyTable;
			for (int i = capacity + stashSize; i-- > 0;)
				keyTable[i] = EMPTY;
			size = 0;
			stashSize = 0;
			hasZeroValue = false;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual bool ContainsValue(int value)
		{
			if (hasZeroValue && zeroValue == value)
				return true;
			int[] keyTable = this.keyTable, valueTable = this.valueTable;
			for (int i = capacity + stashSize; i-- > 0;)
				if (keyTable[i] != 0 && valueTable[i] == value)
					return true;
			return false;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual bool ContainsKey(int key)
		{
			if (key == 0)
				return hasZeroValue;
			int index = key & mask;
			if (keyTable[index] != key)
			{
				index = Hash2(key);
				if (keyTable[index] != key)
				{
					index = Hash3(key);
					if (keyTable[index] != key)
						return ContainsKeyStash(key);
				}
			}

			return true;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private bool ContainsKeyStash(int key)
		{
			int[] keyTable = this.keyTable;
			for (int i = capacity, n = i + stashSize; i < n; i++)
				if (key == keyTable[i])
					return true;
			return false;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual int FindKey(int value, int notFound)
		{
			if (hasZeroValue && zeroValue == value)
				return 0;
			int[] keyTable = this.keyTable, valueTable = this.valueTable;
			for (int i = capacity + stashSize; i-- > 0;)
				if (keyTable[i] != 0 && valueTable[i] == value)
					return keyTable[i];
			return notFound;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual void EnsureCapacity(int additionalCapacity)
		{
			int sizeNeeded = size + additionalCapacity;
			if (sizeNeeded >= threshold)
				Resize(MathUtils.NextPowerOfTwo((int)Math.Ceil(sizeNeeded / loadFactor)));
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private void Resize(int newSize)
		{
			int oldEndIndex = capacity + stashSize;
			capacity = newSize;
			threshold = (int)(newSize * loadFactor);
			mask = newSize - 1;
			hashShift = 31 - Integer.NumberOfTrailingZeros(newSize);
			stashCapacity = Math.Max(3, (int)Math.Ceil(Math.Log(newSize)) * 2);
			pushIterations = Math.Max(Math.Min(newSize, 8), (int)Math.Sqrt(newSize) / 8);
			int[] oldKeyTable = keyTable;
			int[] oldValueTable = valueTable;
			keyTable = new int[newSize + stashCapacity];
			valueTable = new int[newSize + stashCapacity];
			int oldSize = size;
			size = hasZeroValue ? 1 : 0;
			stashSize = 0;
			if (oldSize > 0)
			{
				for (int i = 0; i < oldEndIndex; i++)
				{
					int key = oldKeyTable[i];
					if (key != EMPTY)
						PutResize(key, oldValueTable[i]);
				}
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private int Hash2(int h)
		{
			h *= PRIME2;
			return (h ^ h >>> hashShift) & mask;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private int Hash3(int h)
		{
			h *= PRIME3;
			return (h ^ h >>> hashShift) & mask;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private int Random(int bound)
		{
			return ((randomState = randomState * 0x6ebd3 ^ 0x9E3779BD) >>> 24) * bound >>> 24;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual int GetHashCode()
		{
			int h = 0;
			if (hasZeroValue)
			{
				h += Float.FloatToIntBits(zeroValue);
			}

			int[] keyTable = this.keyTable;
			int[] valueTable = this.valueTable;
			for (int i = 0, n = capacity + stashSize; i < n; i++)
			{
				int key = keyTable[i];
				if (key != EMPTY)
				{
					h += key * 31;
					int value = valueTable[i];
					h += value;
				}
			}

			return h;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual bool Equals(object obj)
		{
			if (obj == this)
				return true;
			if (!(obj is IntIntMap))
				return false;
			IntIntMap other = (IntIntMap)obj;
			if (other.size != size)
				return false;
			if (other.hasZeroValue != hasZeroValue)
				return false;
			if (hasZeroValue && other.zeroValue != zeroValue)
			{
				return false;
			}

			int[] keyTable = this.keyTable;
			int[] valueTable = this.valueTable;
			for (int i = 0, n = capacity + stashSize; i < n; i++)
			{
				int key = keyTable[i];
				if (key != EMPTY)
				{
					int otherValue = other.Get(key, 0);
					if (otherValue == 0 && !other.ContainsKey(key))
						return false;
					int value = valueTable[i];
					if (otherValue != value)
						return false;
				}
			}

			return true;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual string ToString()
		{
			if (size == 0)
				return "{}";
			StringBuilder buffer = new StringBuilder(32);
			buffer.Append('{');
			int[] keyTable = this.keyTable;
			int[] valueTable = this.valueTable;
			int i = keyTable.Length;
			if (hasZeroValue)
			{
				buffer.Append("0=");
				buffer.Append(zeroValue);
			}
			else
			{
				while (i-- > 0)
				{
					int key = keyTable[i];
					if (key == EMPTY)
						continue;
					buffer.Append(key);
					buffer.Append('=');
					buffer.Append(valueTable[i]);
					break;
				}
			}

			while (i-- > 0)
			{
				int key = keyTable[i];
				if (key == EMPTY)
					continue;
				buffer.Append(", ");
				buffer.Append(key);
				buffer.Append('=');
				buffer.Append(valueTable[i]);
			}

			buffer.Append('}');
			return buffer.ToString();
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual IEnumerator<Entry> Iterator()
		{
			return Entries();
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual Entries Entries()
		{
			if (entries1 == null)
			{
				entries1 = new Entries(this);
				entries2 = new Entries(this);
			}

			if (!entries1.valid)
			{
				entries1.Reset();
				entries1.valid = true;
				entries2.valid = false;
				return entries1;
			}

			entries2.Reset();
			entries2.valid = true;
			entries1.valid = false;
			return entries2;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual Values Values()
		{
			if (values1 == null)
			{
				values1 = new Values(this);
				values2 = new Values(this);
			}

			if (!values1.valid)
			{
				values1.Reset();
				values1.valid = true;
				values2.valid = false;
				return values1;
			}

			values2.Reset();
			values2.valid = true;
			values1.valid = false;
			return values2;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public virtual Keys Keys()
		{
			if (keys1 == null)
			{
				keys1 = new Keys(this);
				keys2 = new Keys(this);
			}

			if (!keys1.valid)
			{
				keys1.Reset();
				keys1.valid = true;
				keys2.valid = false;
				return keys1;
			}

			keys2.Reset();
			keys2.valid = true;
			keys1.valid = false;
			return keys2;
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public class Entry
		{
			public int key;
			public int value;
			public virtual string ToString()
			{
				return key + "=" + value;
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		private class MapIterator
		{
			static readonly int INDEX_ILLEGAL = -2;
			static readonly int INDEX_ZERO = -1;
			public bool hasNext;
			readonly IntIntMap map;
			int nextIndex, currentIndex;
			bool valid = true;
			public MapIterator(IntIntMap map)
			{
				this.map = map;
				Reset();
			}

			public virtual void Reset()
			{
				currentIndex = INDEX_ILLEGAL;
				nextIndex = INDEX_ZERO;
				if (map.hasZeroValue)
					hasNext = true;
				else
					FindNextIndex();
			}

			virtual void FindNextIndex()
			{
				hasNext = false;
				int[] keyTable = map.keyTable;
				for (int n = map.capacity + map.stashSize; ++nextIndex < n;)
				{
					if (keyTable[nextIndex] != EMPTY)
					{
						hasNext = true;
						break;
					}
				}
			}

			public virtual void Remove()
			{
				if (currentIndex == INDEX_ZERO && map.hasZeroValue)
				{
					map.hasZeroValue = false;
				}
				else if (currentIndex < 0)
				{
					throw new InvalidOperationException("next must be called before remove.");
				}
				else if (currentIndex >= map.capacity)
				{
					map.RemoveStashIndex(currentIndex);
					nextIndex = currentIndex - 1;
					FindNextIndex();
				}
				else
				{
					map.keyTable[currentIndex] = EMPTY;
				}

				currentIndex = INDEX_ILLEGAL;
				map.size--;
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		public class Entries : MapIterator, Iterable<Entry>, IEnumerator<Entry>
		{
			private readonly Entry entry = new Entry();
			public Entries(IntIntMap map) : base(map)
			{
			}

			/// <summary>
			/// Note the same entry instance is returned each time this method is called.
			/// </summary>
			public virtual Entry Next()
			{
				int[] keyTable = map.keyTable;
				if (nextIndex == INDEX_ZERO)
				{
					entry.key = 0;
					entry.value = map.zeroValue;
				}
				else
				{
					entry.key = keyTable[nextIndex];
					entry.value = map.valueTable[nextIndex];
				}

				currentIndex = nextIndex;
				FindNextIndex();
				return entry;
			}

			/// <summary>
			/// Note the same entry instance is returned each time this method is called.
			/// </summary>
			public virtual bool HasNext()
			{
				return hasNext;
			}

			/// <summary>
			/// Note the same entry instance is returned each time this method is called.
			/// </summary>
			public virtual IEnumerator<Entry> Iterator()
			{
				return this;
			}

			/// <summary>
			/// Note the same entry instance is returned each time this method is called.
			/// </summary>
			public virtual void Remove()
			{
				base.Remove();
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		/// <summary>
		/// Note the same entry instance is returned each time this method is called.
		/// </summary>
		public class Values : MapIterator
		{
			public Values(IntIntMap map) : base(map)
			{
			}

			public virtual bool HasNext()
			{
				return hasNext;
			}

			public virtual int Next()
			{
				int value;
				if (nextIndex == INDEX_ZERO)
					value = map.zeroValue;
				else
					value = map.valueTable[nextIndex];
				currentIndex = nextIndex;
				FindNextIndex();
				return value;
			}

			/// <summary>
			/// Returns a new array containing the remaining values.
			/// </summary>
			public virtual IntArray ToArray()
			{
				IntArray array = new IntArray(true, map.size);
				while (hasNext)
					array.Add(Next());
				return array;
			}
		}

		//	private static final int PRIME1 = 0xbec61;//0xbe1f14b1;
		//0xb4b82e39;
		//0xced1c241;
		/// <summary>
		/// Creates a new map with an initial capacity of 51 and a load factor of 0.8.
		/// </summary>
		/// <summary>
		/// Creates a new map identical to the specified map.
		/// </summary>
		// Check for existing keys.
		// Update key in the stash.
		// Check for empty buckets.
		/// <summary>
		/// Skips checks for existing keys.
		/// </summary>
		// Check for empty buckets.
		// Push keys until an empty bucket is found.
		// Replace the key and value for one of the hashes.
		// If the evicted key hashes to an empty bucket, put it there and stop.
		// Too many pushes occurred and the stash is full, increase the table size.
		// Store key in the stash.
		/// <param name="defaultValue">Returned if the key was not associated with a value.</param>
		// If the removed location was not last, move the last tuple to the removed location.
		/// <summary>
		/// Clears the map and reduces the size of the backing arrays to be the specified capacity if they are larger.
		/// </summary>
		/// <summary>
		/// Note the same entry instance is returned each time this method is called.
		/// </summary>
		/// <summary>
		/// Returns a new array containing the remaining values.
		/// </summary>
		public class Keys : MapIterator
		{
			public Keys(IntIntMap map) : base(map)
			{
			}

			public virtual bool HasNext()
			{
				return hasNext;
			}

			public virtual int Next()
			{
				int key = nextIndex == INDEX_ZERO ? 0 : map.keyTable[nextIndex];
				currentIndex = nextIndex;
				FindNextIndex();
				return key;
			}

			/// <summary>
			/// Returns a new array containing the remaining keys.
			/// </summary>
			public virtual IntArray ToArray()
			{
				IntArray array = new IntArray(true, map.size);
				while (hasNext)
					array.Add(Next());
				return array;
			}
		}
	}
}