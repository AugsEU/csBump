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
			Point tch = collision.mTouch;
			Point move = collision.mMove;
			float sx = tch.mX, sy = tch.mY;
			if (move.mX != 0 || move.mY != 0)
			{
				if (collision.mNormal.mX == 0)
				{
					sx = goalX;
				}
				else
				{
					sy = goalY;
				}
			}

			x = tch.mX;
			y = tch.mY;
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
			result.Set(collision.mTouch.mX, collision.mTouch.mY);
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
			Point tch = collision.mTouch;
			Point move = collision.mMove;
			float bx = tch.mX, by = tch.mY;
			if (move.mX != 0 || move.mY != 0)
			{
				float bnx = goalX - tch.mX;
				float bny = goalY - tch.mY;
				if (collision.mNormal.mX == 0)
				{
					bny = -bny;
				}
				else
				{
					bnx = -bnx;
				}
				bx = tch.mX + bnx;
				by = tch.mY + bny;
			}

			x = tch.mX;
			y = tch.mY;
			goalX = bx;
			goalY = by;
			result.mProjectedCollisions.Clear();
			world.Project(collision.mItem, x, y, w, h, goalX, goalY, filter, result.mProjectedCollisions);
			result.Set(goalX, goalY);
			return result;
		}
	}
}