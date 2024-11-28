namespace csBump
{
	public class Grid
	{
		private readonly Point mGridTraverseC1 = new Point();
		private readonly Point mGridTraverseC2 = new Point();
		private readonly Point mGridTraverseInitStepX = new Point();
		private readonly Point mGridTraverseInitStepY = new Point();
		private readonly Point mGridToCellRectCXY = new Point();

		public static void Grid_toWorld(float cellSize, float cx, float cy, Point point)
		{
			point.Set((cx - 1) * cellSize,(cy - 1) * cellSize);
		}

		public static void Grid_toCell(float cellSize, float x, float y, Point point)
		{
			point.Set(MathF.Floor(x / cellSize) + 1, MathF.Floor(y / cellSize) + 1);
		}

		public static int Grid_traverse_initStep(float cellSize, float ct, float t1, float t2, Point point)
		{
			float v = t2 - t1;
			if (v > 0)
			{
				point.Set(cellSize / v, ((ct + v) * cellSize - t1) / v);
				return 1;
			}
			else if (v < 0)
			{
				point.Set(-cellSize / v, ((ct + v - 1) * cellSize - t1) / v);
				return -1;
			}
			else
			{
				point.Set(float.MaxValue, float.MaxValue);
				return 0;
			}
		}

		public interface TraverseCallback
		{
			bool OnTraverse(float cx, float cy, int stepX, int stepY);
		}

		public virtual void Grid_traverse(float cellSize, float x1, float y1, float x2, float y2, TraverseCallback f)
		{
			Grid_toCell(cellSize, x1, y1, mGridTraverseC1);
			float cx1 = mGridTraverseC1.mX;
			float cy1 = mGridTraverseC1.mY;
			Grid_toCell(cellSize, x2, y2, mGridTraverseC2);
			float cx2 = mGridTraverseC2.mX;
			float cy2 = mGridTraverseC2.mY;
			int stepX = Grid_traverse_initStep(cellSize, cx1, x1, x2, mGridTraverseInitStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, y1, y2, mGridTraverseInitStepY);
			float dx = mGridTraverseInitStepX.mX;
			float tx = mGridTraverseInitStepX.mY;
			float dy = mGridTraverseInitStepY.mX;
			float ty = mGridTraverseInitStepY.mY;
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
			Grid_toCell(cellSize, x1, y1, mGridTraverseC1);
			float cx1 = mGridTraverseC1.mX;
			float cy1 = mGridTraverseC1.mY;
			int stepX = Grid_traverse_initStep(cellSize, cx1, x1, x1 + dirX, mGridTraverseInitStepX);
			int stepY = Grid_traverse_initStep(cellSize, cy1, y1, y1 + dirY, mGridTraverseInitStepY);
			float dx = mGridTraverseInitStepX.mX;
			float tx = mGridTraverseInitStepX.mY;
			float dy = mGridTraverseInitStepY.mX;
			float ty = mGridTraverseInitStepY.mY;
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
			Grid_toCell(cellSize, x, y, mGridToCellRectCXY);
			float cx = mGridToCellRectCXY.mX;
			float cy = mGridToCellRectCXY.mY;
			float cr = MathF.Ceiling((x + w) / cellSize);
			float cb = MathF.Ceiling((y + h) / cellSize);
			rect.Set(cx, cy, cr - cx + 1, cb - cy + 1);
			return rect;
		}
	}
}