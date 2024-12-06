#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	/// <summary>
	/// A response between two entities.
	/// </summary>
	public interface IResponse
	{
		CollisionResult Response(World world, Collision collision, Rect2f rect, Vector2 goal, CollisionFilter filter, CollisionResult result);
	}

	public class SlideResponse : IResponse
	{
		public CollisionResult Response(World world, Collision collision, Rect2f rect, Vector2 goal, CollisionFilter filter, CollisionResult result)
		{
			Vector2 tch = collision.mTouch;
			Vector2 move = collision.mMove;
			float sx = tch.X, sy = tch.Y;
			if (move.X != 0 || move.Y != 0)
			{
				if (collision.mNormal.X == 0)
				{
					sx = goal.X;
				}
				else
				{
					sy = goal.Y;
				}
			}

			rect.Position = tch;
			goal = new Vector2(sx, sy);
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, rect, goal, filter, result.mProjectedCollisions);
			result.Set(goal);
			return result;
		}
	}

	public class TouchResponse : IResponse
	{
		public CollisionResult Response(World world, Collision collision, Rect2f rect, Vector2 goal, CollisionFilter filter, CollisionResult result)
		{
			result.mProjectedCollisions.Clear();
			result.Set(collision.mTouch);
			return result;
		}
	}

	public class CrossResponse : IResponse
	{
		public CollisionResult Response(World world, Collision collision, Rect2f rect, Vector2 goal, CollisionFilter filter, CollisionResult result)
		{
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, rect, goal, filter, result.mProjectedCollisions);
			result.Set(goal);
			return result;
		}
	}

	public class BounceResponse : IResponse
	{
		public CollisionResult Response(World world, Collision collision, Rect2f rect, Vector2 goal, CollisionFilter filter, CollisionResult result)
		{
			Vector2 tch = collision.mTouch;
			Vector2 move = collision.mMove;
			float bx = tch.X, by = tch.Y;
			if (move.X != 0 || move.Y != 0)
			{
				float bnx = goal.X - tch.X;
				float bny = goal.Y - tch.Y;
				if (collision.mNormal.X == 0)
				{
					bny = -bny;
				}
				else
				{
					bnx = -bnx;
				}
				bx = tch.X + bnx;
				by = tch.Y + bny;
			}

			rect.Position = tch;
			goal.X = bx;
			goal.Y = by;
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, rect, goal, filter, result.mProjectedCollisions);
			result.Set(goal);
			return result;
		}
	}
}