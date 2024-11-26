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
using System.Xml.Linq;

namespace csBump
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class Collisions : IComparer<int>
	{
		private readonly List<bool> overlaps = new List<bool>();
		private readonly List<float> tis = new List<float>();
		private readonly List<float> moveXs = new List<float>();
		private readonly List<float> moveYs = new List<float>();
		private readonly List<int> normalXs = new List<int>();
		private readonly List<int> normalYs = new List<int>();
		private readonly List<float> touchXs = new List<float>();
		private readonly List<float> touchYs = new List<float>();
		private readonly List<float> x1s = new List<float>();
		private readonly List<float> y1s = new List<float>();
		private readonly List<float> w1s = new List<float>();
		private readonly List<float> h1s = new List<float>();
		private readonly List<float> x2s = new List<float>();
		private readonly List<float> y2s = new List<float>();
		private readonly List<float> w2s = new List<float>();
		private readonly List<float> h2s = new List<float>();
		public List<Item> items = new List<Item>();
		public List<Item> others = new List<Item>();
		public List<Response> types = new List<Response>();
		private int size = 0;

		public Collisions()
		{
		}

		public Collisions(Collisions other)
		{
			overlaps = new List<bool>(other.overlaps);
			tis = new List<float>(other.tis);
			moveXs = new List<float>(other.moveXs);
			moveYs = new List<float>(other.moveYs);
			normalXs = new List<int>(other.normalXs);
			normalYs = new List<int>(other.normalYs);
			touchXs = new List<float>(other.touchXs);
			touchYs = new List<float>(other.touchYs);
			x1s = new List<float>(other.x1s);
			y1s = new List<float>(other.y1s);
			w1s = new List<float>(other.w1s);
			h1s = new List<float>(other.h1s);
			x2s = new List<float>(other.x2s);
			y2s = new List<float>(other.y2s);
			w2s = new List<float>(other.w2s);
			h2s = new List<float>(other.h2s);
			items = new List<Item>(other.items);
			others = new List<Item>(other.others);
			types = new List<Response>(other.types);
			size = other.size;
		}

		public virtual void Add(Collision col)
		{
			Add(col.overlaps, col.ti, col.move.x, col.move.y, col.normal.x, col.normal.y, col.touch.x, col.touch.y, col.itemRect.x, col.itemRect.y, col.itemRect.w, col.itemRect.h, col.otherRect.x, col.otherRect.y, col.otherRect.w, col.otherRect.h, col.item, col.other, col.type);
		}

		public virtual void Add(bool overlap, float ti, float moveX, float moveY, int normalX, int normalY, float touchX, float touchY, float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, Item item, Item other, Response type)
		{
			size++;
			overlaps.Add(overlap);
			tis.Add(ti);
			moveXs.Add(moveX);
			moveYs.Add(moveY);
			normalXs.Add(normalX);
			normalYs.Add(normalY);
			touchXs.Add(touchX);
			touchYs.Add(touchY);
			x1s.Add(x1);
			y1s.Add(y1);
			w1s.Add(w1);
			h1s.Add(h1);
			x2s.Add(x2);
			y2s.Add(y2);
			w2s.Add(w2);
			h2s.Add(h2);
			items.Add(item);
			others.Add(other);
			types.Add(type);
		}

		private readonly Collision collision = new Collision();
		public virtual Collision Get(int index)
		{
			if (index >= size)
			{
				return null;
			}

			collision.Set(overlaps[index], tis[index], moveXs[index], moveYs[index], normalXs[index], normalYs[index], touchXs[index], touchYs[index], x1s[index], y1s[index], w1s[index], h1s[index], x2s[index], y2s[index], w2s[index], h2s[index]);
			collision.item = items[index];
			collision.other = others[index];
			collision.type = types[index];
			return collision;
		}

		public virtual void Remove(int index)
		{
			if (index < size)
			{
				size--;
				overlaps.RemoveAt(index);
				tis.RemoveAt(index);
				moveXs.RemoveAt(index);
				moveYs.RemoveAt(index);
				normalXs.RemoveAt(index);
				normalYs.RemoveAt(index);
				touchXs.RemoveAt(index);
				touchYs.RemoveAt(index);
				x1s.RemoveAt(index);
				y1s.RemoveAt(index);
				w1s.RemoveAt(index);
				h1s.RemoveAt(index);
				x2s.RemoveAt(index);
				y2s.RemoveAt(index);
				w2s.RemoveAt(index);
				h2s.RemoveAt(index);
				items.RemoveAt(index);
				others.RemoveAt(index);
				types.RemoveAt(index);
			}
		}

		public virtual int Size()
		{
			return size;
		}

		public virtual bool IsEmpty()
		{
			return size == 0;
		}

		public virtual void Clear()
		{
			size = 0;
			overlaps.Clear();
			tis.Clear();
			moveXs.Clear();
			moveYs.Clear();
			normalXs.Clear();
			normalYs.Clear();
			touchXs.Clear();
			touchYs.Clear();
			x1s.Clear();
			y1s.Clear();
			w1s.Clear();
			h1s.Clear();
			x2s.Clear();
			y2s.Clear();
			w2s.Clear();
			h2s.Clear();
			items.Clear();
			others.Clear();
			types.Clear();
		}

		private readonly List<int> order = new List<int>();
		private readonly Dictionary<int, int> swapMap = new Dictionary<int, int>();
		public virtual void KeySort<T>(IList<int> indices, IList<T> list)
		{
			swapMap.Clear();
			for (int i = 0; i < indices.Count; i++)
			{
				int k = indices[i];
				while (swapMap.ContainsKey(k))
				{
					int findKey;
					if(swapMap.TryGetValue(k, out findKey))
					{
						k = findKey;
					}
					else
					{
						k = 0;
					}
				}

				swapMap.Add(i, k);
			}

			foreach (KeyValuePair<int, int> e in swapMap)
			{
				Extra.Swap(list, e.Key, e.Value);
			}
		}

		public virtual void Sort()
		{
			order.Clear();
			for (int i = 0; i < size; i++)
			{
				order.Add(i);
			}

			order.Sort(this);
			KeySort(order, overlaps);
			KeySort(order, tis);
			KeySort(order, moveXs);
			KeySort(order, moveYs);
			KeySort(order, normalXs);
			KeySort(order, normalYs);
			KeySort(order, touchXs);
			KeySort(order, touchYs);
			KeySort(order, x1s);
			KeySort(order, y1s);
			KeySort(order, w1s);
			KeySort(order, h1s);
			KeySort(order, x2s);
			KeySort(order, y2s);
			KeySort(order, w2s);
			KeySort(order, h2s);
			KeySort(order, items);
			KeySort(order, others);
			KeySort(order, types);
		}

		public int Compare(int a, int b)
		{
			if (tis[a] == (tis[b]))
			{
				float ad = Rect.Rect_getSquareDistance(x1s[a], y1s[a], w1s[a], h1s[a], x2s[a], y2s[a], w2s[a], h2s[a]);
				float bd = Rect.Rect_getSquareDistance(x1s[a], y1s[a], w1s[a], h1s[a], x2s[b], y2s[b], w2s[b], h2s[b]);
				return ad.CompareTo(bd);
			}

			return tis[a].CompareTo(tis[b]);
		}
	}
}