#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	/// <summary>
	/// Represents a collision between two items.
	/// </summary>
	public class Collision
	{
		public bool mOverlaps;
		public float mTI;
		public Vector2 mMove = new Vector2(0.0f, 0.0f);
		public Point mNormal = new Point(0, 0);
		public Vector2 mTouch = new Vector2(0.0f, 0.0f);
		public Rect mItemRect = new Rect();
		public Rect mOtherRect = new Rect();
		public Item? mItem;
		public Item? mOther;
		public IResponse? mType;
		
		public Collision()
		{
		}

		public virtual void Set(bool overlaps, float ti, float moveX, float moveY, int normalX, int normalY, float touchX, float touchY, float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			mOverlaps = overlaps;
			mTI = ti;
			mMove = new Vector2(moveX, moveY);
			mNormal = new Point(normalX, normalY);
			mTouch = new Vector2(touchX, touchY);
			mItemRect.Set(x1, y1, w1, h1);
			mOtherRect.Set(x2, y2, w2, h2);
		}
	}
}