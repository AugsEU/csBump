#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	public class CollisionResult
	{
		public Vector2 mGoal;
		public List<Collision> mProjectedCollisions = new List<Collision>();
		public virtual void Set(Vector2 goal)
		{
			mGoal = goal;
		}
	}
}
