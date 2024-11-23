using csBump;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace csBump
{
	public class ItemInfo
	{
		public Item item;
		/// <summary>
		/// The x coordinate where the line segment intersects the {@link Rect} of the {@link Item}.
		/// </summary>
		public float x1;
		/// <summary>
		/// The y coordinate where the line segment intersects the {@link Rect} of the {@link Item}.
		/// </summary>
		public float y1;
		public float x2;
		public float y2;
		/// <summary>
		/// A value from 0 to 1 indicating how far from the starting point of the segment did the impact happen horizontally.
		/// </summary>
		public float ti1;
		/// <summary>
		/// A value from 0 to 1 indicating how far from the starting point of the segment did the impact happen vertically.
		/// </summary>
		public float ti2;
		public float weight;
		public ItemInfo(Item item, float ti1, float ti2, float weight)
		{
			this.item = item;
			this.ti1 = ti1;
			this.ti2 = ti2;
			this.weight = weight;
		}

		public static readonly Comparator<ItemInfo> weightComparator = new AnonymousComparator(this);
		private sealed class AnonymousComparator : Comparator
		{
			public AnonymousComparator(ItemInfo parent)
			{
				this.parent = parent;
			}

			private readonly ItemInfo parent;
			public int Compare(ItemInfo o1, ItemInfo o2)
			{
				return Float.Compare(o1.weight, o2.weight);
			}
		}
	}
}