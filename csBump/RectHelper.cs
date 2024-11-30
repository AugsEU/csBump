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
		// TO DO: Remove this?
		private Collision mResultCollision = new Collision();

		public virtual Collision? Rect_detectCollision(Rect2f rect1, Rect2f rect2, Vector2 goal)
		{
			Collision col = mResultCollision;
			float dx = goal.X - rect1.X;
			float dy = goal.Y - rect1.Y;
			Rect2f diff = Rect2f.Rect_getDiff(rect1.X, rect1.Y, rect1.Width, rect1.Height, rect2.X, rect2.Y, rect2.Width, rect2.Height);

			bool overlaps = false;
			float? ti = null;
			int nx = 0, ny = 0;

			if (Rect2f.Rect_containsPoint(diff.X, diff.Y, diff.Width, diff.Height, 0, 0))
			{
				//item was intersecting other
				Vector2 nearestCorner = Rect2f.Rect_getNearestCorner(diff.X, diff.Y, diff.Width, diff.Height, 0, 0);

				//area of intersection
				float wi = MathF.Min(rect1.Width, MathF.Abs(nearestCorner.X));
				float hi = MathF.Min(rect1.Height, MathF.Abs(nearestCorner.Y));
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
					tx = rect1.X + nearestCorner.X;
					ty = rect1.Y + nearestCorner.Y;
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

					tx = rect1.X + dx * ti1;
					ty = rect1.Y + dy * ti1; // TO DO: Why is this X centric? No Y case?
				}
			}
			else
			{
				//tunnel
				tx = rect1.X + dx * ti.Value;
				ty = rect1.Y + dy * ti.Value;
			}

			col.Set(overlaps, ti.Value, dx, dy, nx, ny, tx, ty, rect1.X, rect1.Y, rect1.Width, rect1.Height, rect2.X, rect2.Y, rect2.Width, rect2.Height);
			return col;
		}
	}
}