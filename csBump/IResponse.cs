#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

using static csBump.IResponse;

namespace csBump
{
	/// <summary>
	/// A response between two entities.
	/// </summary>
	public interface IResponse
	{
		Result Response(World world, Collision collision, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Result result);
		class Result
		{
			public float mGoalX;
			public float mGoalY;
			public Collisions mProjectedCollisions = new Collisions();
			public virtual void Set(float goalX, float goalY)
			{
				mGoalX = goalX;
				mGoalY = goalY;
			}
		}
	}

	public class SlideResponse : IResponse
	{
		public Result Response(World world, Collision collision, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Result result)
		{
			Vector2 tch = collision.mTouch;
			Vector2 move = collision.mMove;
			float sx = tch.X, sy = tch.Y;
			if (move.X != 0 || move.Y != 0)
			{
				if (collision.mNormal.X == 0)
				{
					sx = goalX;
				}
				else
				{
					sy = goalY;
				}
			}

			x = tch.X;
			y = tch.Y;
			goalX = sx;
			goalY = sy;
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, x, y, w, h, goalX, goalY, filter, result.mProjectedCollisions);
			result.Set(goalX, goalY);
			return result;
		}
	}

	public class TouchResponse : IResponse
	{
		public Result Response(World world, Collision collision, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Result result)
		{
			result.mProjectedCollisions.Clear();
			result.Set(collision.mTouch.X, collision.mTouch.Y);
			return result;
		}
	}

	public class CrossResponse : IResponse
	{
		public Result Response(World world, Collision collision, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Result result)
		{
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, x, y, w, h, goalX, goalY, filter, result.mProjectedCollisions);
			result.Set(goalX, goalY);
			return result;
		}
	}

	public class BounceResponse : IResponse
	{
		public Result Response(World world, Collision collision, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Result result)
		{
			Vector2 tch = collision.mTouch;
			Vector2 move = collision.mMove;
			float bx = tch.X, by = tch.Y;
			if (move.X != 0 || move.Y != 0)
			{
				float bnx = goalX - tch.X;
				float bny = goalY - tch.Y;
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

			x = tch.X;
			y = tch.Y;
			goalX = bx;
			goalY = by;
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, x, y, w, h, goalX, goalY, filter, result.mProjectedCollisions);
			result.Set(goalX, goalY);
			return result;
		}
	}
}