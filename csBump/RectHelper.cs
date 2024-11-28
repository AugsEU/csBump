namespace csBump
{
	/// <summary>
	/// Utility functions for rect
	/// TO DO: Clean this up.
	/// </summary>
	public class RectHelper
	{
		private readonly Rect mRectDetectCollisionDiff = new Rect();
		private readonly Point mRectDetectCollisionNearestCorner = new Point();
		private readonly Point mRectDetectCollisionGetSegmentIntersectionIndicesTI = new Point();
		private readonly IntPoint mRectDetectCollisionGetSegmentIntersectionIndicesN1 = new IntPoint();
		private readonly IntPoint mRectDetectCollisionGetSegmentIntersectionIndicesN2 = new IntPoint();
		private readonly Collision RectDetectCollisionGetSegmentIntersectionIndicesCol = new Collision();


		public virtual Collision? Rect_detectCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, float goalX, float goalY)
		{
			Collision col = RectDetectCollisionGetSegmentIntersectionIndicesCol;
			float dx = goalX - x1;
			float dy = goalY - y1;
			Rect.Rect_getDiff(x1, y1, w1, h1, x2, y2, w2, h2, mRectDetectCollisionDiff);
			float x = mRectDetectCollisionDiff.mX;
			float y = mRectDetectCollisionDiff.mY;
			float w = mRectDetectCollisionDiff.mWidth;
			float h = mRectDetectCollisionDiff.mHeight;
			bool overlaps = false;
			float? ti = null;
			int nx = 0, ny = 0;
			if (Rect.Rect_containsPoint(x, y, w, h, 0, 0))
			{

				//item was intersecting other
				Rect.Rect_getNearestCorner(x, y, w, h, 0, 0, mRectDetectCollisionNearestCorner);
				float px = mRectDetectCollisionNearestCorner.mX;
				float py = mRectDetectCollisionNearestCorner.mY;

				//area of intersection
				float wi = MathF.Min(w1, MathF.Abs(px));
				float hi = MathF.Min(h1, MathF.Abs(py));
				ti = -wi * hi; //ti is the negative area of intersection
				overlaps = true;
			}
			else
			{
				bool intersect = Rect.Rect_getSegmentIntersectionIndices(x, y, w, h, 0, 0, dx, dy, float.MinValue, float.MaxValue, mRectDetectCollisionGetSegmentIntersectionIndicesTI, mRectDetectCollisionGetSegmentIntersectionIndicesN1, mRectDetectCollisionGetSegmentIntersectionIndicesN2);
				float ti1 = mRectDetectCollisionGetSegmentIntersectionIndicesTI.mX;
				float ti2 = mRectDetectCollisionGetSegmentIntersectionIndicesTI.mY;
				int nx1 = mRectDetectCollisionGetSegmentIntersectionIndicesN1.mX;
				int ny1 = mRectDetectCollisionGetSegmentIntersectionIndicesN1.mY;

				//item tunnels into other
				if (intersect && ti1 < 1 && MathF.Abs(ti1 - ti2) >= Extra.DELTA && (0 < ti1 + Extra.DELTA || 0 == ti1 && ti2 > 0))
				{
					ti = ti1;
					nx = nx1;
					ny = ny1;
					overlaps = false;
				}
			}

			if (!ti.HasValue)
			{
				return null;
			}

			float tx, ty;
			if (overlaps)
			{
				if (dx == 0 && dy == 0)
				{
					//intersecting and not moving - use minimum displacement vector
					Rect.Rect_getNearestCorner(x, y, w, h, 0, 0, mRectDetectCollisionNearestCorner);
					float px = mRectDetectCollisionNearestCorner.mX;
					float py = mRectDetectCollisionNearestCorner.mY;
					if (MathF.Abs(px) < MathF.Abs(py))
					{
						py = 0;
					}
					else
					{
						px = 0;
					}

					nx = MathF.Sign(px);
					ny = MathF.Sign(py);
					tx = x1 + px;
					ty = y1 + py;
				}
				else
				{

					//intersecting and moving - move in the opposite direction
					bool intersect = Rect.Rect_getSegmentIntersectionIndices(x, y, w, h, 0, 0, dx, dy, -float.MaxValue, 1, mRectDetectCollisionGetSegmentIntersectionIndicesTI, mRectDetectCollisionGetSegmentIntersectionIndicesN1, mRectDetectCollisionGetSegmentIntersectionIndicesN2);
					float ti1 = mRectDetectCollisionGetSegmentIntersectionIndicesTI.mX;
					nx = mRectDetectCollisionGetSegmentIntersectionIndicesN1.mX;
					ny = mRectDetectCollisionGetSegmentIntersectionIndicesN1.mY;
					if (!intersect)
					{
						return null;
					}

					tx = x1 + dx * ti1;
					ty = y1 + dy * ti1;
				}
			}
			else
			{
				//tunnel
				tx = x1 + dx * ti.Value;
				ty = y1 + dy * ti.Value;
			}

			col.Set(overlaps, ti.Value, dx, dy, nx, ny, tx, ty, x1, y1, w1, h1, x2, y2, w2, h2);
			return col;
		}
	}
}