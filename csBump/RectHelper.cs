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
		public virtual Collision? Rect_detectCollision(BumpID item, Rect2f itemRect, BumpID other, Rect2f otherRect, Vector2 goal, IResponse response)
		{
			float dx = goal.X - itemRect.X;
			float dy = goal.Y - itemRect.Y;
			Rect2f diff = Rect2f.GetDiff(itemRect, otherRect);

			bool overlaps = false;
			float? ti = null;
			int nx = 0, ny = 0;

			if (diff.ContainsPoint(Vector2.Zero))
			{
				//item was intersecting other
				Vector2 nearestCorner = diff.GetNearestCorner(Vector2.Zero);

				//area of intersection
				float wi = MathF.Min(itemRect.Width, MathF.Abs(nearestCorner.X));
				float hi = MathF.Min(itemRect.Height, MathF.Abs(nearestCorner.Y));
				ti = -wi * hi; //ti is the negative area of intersection
				overlaps = true;
			}
			else
			{
				Vector2 tiVec;
				Point intersectionIndicesN1;
				Point intersectionIndicesN2; // TO DO: Unused?
				bool intersect = Rect2f.Rect_getSegmentIntersectionIndices(diff, Vector2.Zero, new Vector2(dx, dy), float.MinValue, float.MaxValue, out tiVec, out intersectionIndicesN1, out intersectionIndicesN2);

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
					Vector2 nearestCorner = diff.GetNearestCorner(Vector2.Zero);

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
					tx = itemRect.X + nearestCorner.X;
					ty = itemRect.Y + nearestCorner.Y;
				}
				else
				{
					//intersecting and moving - move in the opposite direction
					Vector2 tiVec;
					Point intersectionIndicesN1;
					Point intersectionIndicesN2; // TO DO: Unused?
					bool intersect = Rect2f.Rect_getSegmentIntersectionIndices(diff, Vector2.Zero, new Vector2(dx, dy), -float.MaxValue, 1, out tiVec, out intersectionIndicesN1, out intersectionIndicesN2);
					float ti1 = tiVec.X;
					if (!intersect)
					{
						return null;
					}

					tx = itemRect.X + dx * ti1;
					ty = itemRect.Y + dy * ti1; // TO DO: Why is this X centric? No Y case?
				}
			}
			else
			{
				//tunnel
				tx = itemRect.X + dx * ti.Value;
				ty = itemRect.Y + dy * ti.Value;
			}

			return new Collision(overlaps, ti.Value, new Vector2(dx, dy), new Point(nx, ny), new Vector2(tx, ty), itemRect, otherRect, item, other, response);
		}
	}
}