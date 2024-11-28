namespace csBump
{
	/// <summary>
	/// Represents a collision between two items.
	/// </summary>
	public class Collision
	{
		public bool mOverlaps;
		public float mTI;
		public Point mMove = new Point();
		public IntPoint mNormal = new IntPoint();
		public Point mTouch = new Point();
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
			mMove.Set(moveX, moveY);
			mNormal.Set(normalX, normalY);
			mTouch.Set(touchX, touchY);
			mItemRect.Set(x1, y1, w1, h1);
			mOtherRect.Set(x2, y2, w2, h2);
		}
	}
}