#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	/// <summary>
	/// Utility functions for rect
	/// TO DO: Clean this up.
	/// </summary>
	public class RectHelper
	{
		private Collision RectDetectCollisionGetSegmentIntersectionIndicesCol = new Collision();

		public virtual Collision? Rect_detectCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, float goalX, float goalY)
		{
			Collision col = RectDetectCollisionGetSegmentIntersectionIndicesCol;
			float dx = goalX - x1;
			float dy = goalY - y1;
			Rect2f diff = Rect2f.Rect_getDiff(x1, y1, w1, h1, x2, y2, w2, h2);

			bool overlaps = false;
			float? ti = null;
			int nx = 0, ny = 0;

			if (Rect2f.Rect_containsPoint(diff.X, diff.Y, diff.Width, diff.Height, 0, 0))
			{
				//item was intersecting other
				Vector2 nearestCorner = Rect2f.Rect_getNearestCorner(diff.X, diff.Y, diff.Width, diff.Height, 0, 0);

				//area of intersection
				float wi = MathF.Min(w1, MathF.Abs(nearestCorner.X));
				float hi = MathF.Min(h1, MathF.Abs(nearestCorner.Y));
				ti = -wi * hi; //ti is the negative area of intersection
				overlaps = true;
			}
			else
			{
				Vector2 tiVec;
				Point intersectionIndicesN1;
				Point intersectionIndicesN2; // TO DO: Unused?
				bool intersect = Rect2f.Rect_getSegmentIntersectionIndices(diff.X, diff.Y, diff.Width, diff.Height, 0, 0, dx, dy, float.MinValue, float.MaxValue, out tiVec, out intersectionIndicesN1, out intersectionIndicesN2);

				//item tunnels into other
				if (intersect && tiVec.X < 1 && MathF.Abs(tiVec.X - tiVec.Y) >= Extra.DELTA && (0 < tiVec.X + Extra.DELTA || 0 == tiVec.X && tiVec.Y > 0))
				{
					ti = tiVec.X; // TO DO: Why is this X centric? No Y case?
					nx = intersectionIndicesN1.X;
					ny = intersectionIndicesN1.Y;
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
					Vector2 nearestCorner = Rect2f.Rect_getNearestCorner(diff.X, diff.Y, diff.Width, diff.Height, 0, 0);

					if (MathF.Abs(nearestCorner.X) < MathF.Abs(nearestCorner.Y))
					{
						nearestCorner.Y = 0;
					}
					else
					{
						nearestCorner.X = 0;
					}

					nx = MathF.Sign(nearestCorner.X);
					ny = MathF.Sign(nearestCorner.Y);
					tx = x1 + nearestCorner.X;
					ty = y1 + nearestCorner.Y;
				}
				else
				{
					//intersecting and moving - move in the opposite direction
					Vector2 tiVec;
					Point intersectionIndicesN1;
					Point intersectionIndicesN2; // TO DO: Unused?
					bool intersect = Rect2f.Rect_getSegmentIntersectionIndices(diff.X, diff.Y, diff.Width, diff.Height, 0, 0, dx, dy, -float.MaxValue, 1, out tiVec, out intersectionIndicesN1, out intersectionIndicesN2);
					float ti1 = tiVec.X;
					if (!intersect)
					{
						return null;
					}

					tx = x1 + dx * ti1;
					ty = y1 + dy * ti1; // TO DO: Why is this X centric? No Y case?
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