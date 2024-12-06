#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	/// <summary>
	/// Represents a collision between two items.
	/// </summary>
	public struct Collision : IComparable<Collision>
	{
		public bool mOverlaps;
		public float mTI;
		public Vector2 mMove = new Vector2(0.0f, 0.0f);
		public Point mNormal = new Point(0, 0);
		public Vector2 mTouch = new Vector2(0.0f, 0.0f);
		public Rect2f mItemRect = new Rect2f();
		public Rect2f mOtherRect = new Rect2f();
		public Item mItem;
		public Item mOther;
		public IResponse mType;

		public Collision(bool overlaps, float ti, Vector2 move, Point normal, Vector2 touch, Rect2f itemRect, Rect2f otherRect, Item item, Item other, IResponse type)
		{
			mOverlaps = overlaps;
			mTI = ti;
			mMove = move;
			mNormal = normal;
			mTouch = touch;
			mItemRect = itemRect;
			mOtherRect = otherRect;
			mItem = item;
			mOther = other;
			mType = type;
		}

		public int CompareTo(Collision other)
		{
			if (mTI == other.mTI)
			{
				float ad = Rect2f.GetSquareDistance(mItemRect, mOtherRect);
				float bd = Rect2f.GetSquareDistance(mItemRect, other.mOtherRect);
				return ad.CompareTo(bd);
			}

			return mTI.CompareTo(other.mTI);
		}
	}
}