#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	public class Grid
	{
		public static Vector2 Grid_toWorld(float cellSize, Vector2 point)
		{
			return new Vector2((point.X - 1) * cellSize,(point.Y - 1) * cellSize);
		}

		public static Vector2 Grid_toCell(float cellSize, Vector2 point)
		{
			return new Vector2(MathF.Floor(point.X / cellSize) + 1, MathF.Floor(point.Y / cellSize) + 1);
		}

		public static int Grid_traverse_initStep(float cellSize, float ct, float t1, float t2, out Vector2 point)
		{
			float v = t2 - t1;
			if (v > 0)
			{
				point = new Vector2(cellSize / v, ((ct + v) * cellSize - t1) / v);
				return 1;
			}
			else if (v < 0)
			{
				point = new Vector2(-cellSize / v, ((ct + v - 1) * cellSize - t1) / v);
				return -1;
			}
			else
			{
				point = new Vector2(float.MaxValue, float.MaxValue);
				return 0;
			}
		}

		public interface TraverseCallback
		{
			bool OnTraverse(float cx, float cy, int stepX, int stepY);
		}

		public virtual void Grid_traverse(float cellSize, Vector2 pt1, Vector2 pt2, TraverseCallback f)
		{
			Vector2 c1 = Grid_toCell(cellSize, pt1);
			float cx1 = c1.X;
			float cy1 = c1.Y;
			Vector2 c2 = Grid_toCell(cellSize, pt2);
			float cx2 = c2.X;
			float cy2 = c2.Y;

			Vector2 initStepX, initStepY;

			int stepX = Grid_traverse_initStep(cellSize, cx1, pt1.X, pt2.X, out initStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, pt1.Y, pt2.Y, out initStepY);
			float dx = initStepX.X;
			float tx = initStepX.Y;
			float dy = initStepY.X;
			float ty = initStepY.Y;
			float cx = cx1, cy = cy1;
			f.OnTraverse(cx, cy, stepX, stepY);
			/*The default implementation had an infinite loop problem when
				approaching the last cell in some occasions. We finish iterating
				when we are *next* to the last cell*/
			bool cont = true; //stop iterating if TraverseCallback reports that cell coordinates are outside of the world.
			while (Math.Abs(cx - cx2) + Math.Abs(cy - cy2) > 1 && cont)
			{
				if (tx < ty)
				{
					tx = tx + dx;
					cx = cx + stepX;
					cont = f.OnTraverse(cx, cy, stepX, stepY);
				}
				else
				{

					//Addition: include both cells when going through corners
					if (tx == ty)
					{
						f.OnTraverse(cx + stepX, cy, stepX, stepY);
					}

					ty = ty + dy;
					cy = cy + stepY;
					cont = f.OnTraverse(cx, cy, stepX, stepY);
				}
			}


			//If we have not arrived to the last cell, use it
			if (cx != cx2 || cy != cy2)
			{
				f.OnTraverse(cx2, cy2, stepX, stepY);
			}
		}

		public virtual void Grid_traverseRay(float cellSize, Vector2 point, Vector2 dir, TraverseCallback f)
		{
			Vector2 c1 = Grid_toCell(cellSize, point);
			float cx1 = c1.X;
			float cy1 = c1.Y;

			Vector2 initStepX, initStepY;
			int stepX = Grid_traverse_initStep(cellSize, cx1, point.X, point.X + dir.X, out initStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, point.Y, point.Y + dir.Y, out initStepY);
			float dx = initStepX.X;
			float tx = initStepX.Y;
			float dy = initStepY.X;
			float ty = initStepY.Y;
			float cx = cx1, cy = cy1;
			f.OnTraverse(cx, cy, stepX, stepY);
			bool cont = true; //stop iterating if TraverseCallback reports that cell coordinates are outside of the world.
			while (cont)
			{
				if (tx < ty)
				{
					cx = cx + stepX;
					cont = f.OnTraverse(cx, cy, stepX, stepY);
					tx = tx + dx;
				}
				else
				{

					//Addition: include both cells when going through corners
					if (tx == ty)
					{
						f.OnTraverse(cx + stepX, cy, stepX, stepY);
					}

					cy = cy + stepY;
					cont = f.OnTraverse(cx, cy, stepX, stepY);
					ty = ty + dy;
				}
			}
		}

		public virtual Rect2f Grid_toCellRect(float cellSize, Rect2f rect)
		{
			Vector2 toCellRectCXY = Grid_toCell(cellSize, rect.Position);
			float cx = toCellRectCXY.X;
			float cy = toCellRectCXY.Y;
			float cr = MathF.Ceiling((rect.X + rect.Width) / cellSize);
			float cb = MathF.Ceiling((rect.Y + rect.Height) / cellSize);

			return new Rect2f(cx, cy, cr - cx + 1, cb - cy + 1);
		}
	}
}