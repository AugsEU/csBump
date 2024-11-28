namespace csBump
{
	public class ItemInfo
	{
		public Item mItem;

		/// <summary>
		/// The x coordinate where the line segment intersects the {@link Rect} of the {@link Item}.
		/// </summary>
		public float mX1;
		/// <summary>
		/// The y coordinate where the line segment intersects the {@link Rect} of the {@link Item}.
		/// </summary>
		public float mY1;
		public float mX2;
		public float mY2;
		/// <summary>
		/// A value from 0 to 1 indicating how far from the starting point of the segment did the impact happen horizontally.
		/// </summary>
		public float mTI1;
		/// <summary>
		/// A value from 0 to 1 indicating how far from the starting point of the segment did the impact happen vertically.
		/// </summary>
		public float mTI2;
		public float mWeight;

		public ItemInfo(Item item, float ti1, float ti2, float weight)
		{
			mItem = item;
			mTI1 = ti1;
			mTI2 = ti2;
			mWeight = weight;
		}

		public static readonly IComparer<ItemInfo> weightComparator = new AnonymousComparator();

		private sealed class AnonymousComparator : IComparer<ItemInfo>
		{
			public AnonymousComparator()
			{
			}

			public int Compare(ItemInfo? o1, ItemInfo? o2)
			{
				if (o1 == null || o2 == null)
				{
					throw new NullReferenceException();
				}

				if (o1.mWeight == o2.mWeight)
				{
					return 0;
				}

				return o1.mWeight.CompareTo(o2.mWeight);
			}
		}
	}
}