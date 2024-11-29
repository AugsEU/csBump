#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	public class Grid
	{
		private Vector2 mGridTraverseC1 = new Vector2(0.0f, 0.0f);
		private Vector2 mGridTraverseC2 = new Vector2(0.0f, 0.0f);
		private Vector2 mGridTraverseInitStepX = new Vector2(0.0f, 0.0f);
		private Vector2 mGridTraverseInitStepY = new Vector2(0.0f, 0.0f);
		private Vector2 mGridToCellRectCXY = new Vector2(0.0f, 0.0f);

		public static void Grid_toWorld(float cellSize, float cx, float cy , out Vector2 point)
		{
			point = new Vector2((cx - 1) * cellSize,(cy - 1) * cellSize);
		}

		public static void Grid_toCell(float cellSize, float x, float y, out Vector2 point)
		{
			point = new Vector2(MathF.Floor(x / cellSize) + 1, MathF.Floor(y / cellSize) + 1);
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

		public virtual void Grid_traverse(float cellSize, float x1, float y1, float x2, float y2, TraverseCallback f)
		{
			Grid_toCell(cellSize, x1, y1, out mGridTraverseC1);
			float cx1 = mGridTraverseC1.X;
			float cy1 = mGridTraverseC1.Y;
			Grid_toCell(cellSize, x2, y2, out mGridTraverseC2);
			float cx2 = mGridTraverseC2.X;
			float cy2 = mGridTraverseC2.Y;
			int stepX = Grid_traverse_initStep(cellSize, cx1, x1, x2, out mGridTraverseInitStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, y1, y2, out mGridTraverseInitStepY);
			float dx = mGridTraverseInitStepX.X;
			float tx = mGridTraverseInitStepX.Y;
			float dy = mGridTraverseInitStepY.X;
			float ty = mGridTraverseInitStepY.Y;
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

		public virtual void Grid_traverseRay(float cellSize, float x1, float y1, float dirX, float dirY, TraverseCallback f)
		{
			Grid_toCell(cellSize, x1, y1, out mGridTraverseC1);
			float cx1 = mGridTraverseC1.X;
			float cy1 = mGridTraverseC1.Y;
			int stepX = Grid_traverse_initStep(cellSize, cx1, x1, x1 + dirX, out mGridTraverseInitStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, y1, y1 + dirY, out mGridTraverseInitStepY);
			float dx = mGridTraverseInitStepX.X;
			float tx = mGridTraverseInitStepX.Y;
			float dy = mGridTraverseInitStepY.X;
			float ty = mGridTraverseInitStepY.Y;
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

		public virtual Rect Grid_toCellRect(float cellSize, float x, float y, float w, float h, Rect rect)
		{
			Grid_toCell(cellSize, x, y, out mGridToCellRectCXY);
			float cx = mGridToCellRectCXY.X;
			float cy = mGridToCellRectCXY.Y;
			float cr = MathF.Ceiling((x + w) / cellSize);
			float cb = MathF.Ceiling((y + h) / cellSize);
			rect.Set(cx, cy, cr - cx + 1, cb - cy + 1);
			return rect;
		}
	}
}