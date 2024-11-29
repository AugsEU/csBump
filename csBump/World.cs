using csBump.util;

namespace csBump
{
	/// <summary>
	/// Represents the world in which collisions can take place.
	/// </summary>
	public class World
	{
		private readonly Dictionary<Point, Cell> mCellMap = new Dictionary<Point, Cell>();
		private readonly HashSet<Cell> mNonEmptyCells = new HashSet<Cell>();
		private float mCellMinX;
		private float mCellMinY;
		private float mCellMaxX;
		private float mCellMaxY;

		private readonly Grid mGrid = new Grid();

		private readonly RectHelper mRectHelper = new RectHelper();

		private bool mTileMode = true;
		private readonly float mCellSize;

		public World() : this(64F)
		{
		}

		public World(float cellSize)
		{
			this.mCellSize = cellSize;
		}

		public virtual void SetTileMode(bool tileMode)
		{
			this.mTileMode = tileMode;
		}

		public virtual bool IsTileMode()
		{
			return mTileMode;
		}

		private void AddItemToCell(Item item, float cx, float cy)
		{
			Point pt = new Point(cx, cy);
			Cell? cell = null;
			
			if (!mCellMap.TryGetValue(pt, out cell))
			{
				cell = new Cell();
				mCellMap.Add(pt, cell);
				if (cx < mCellMinX)
					mCellMinX = cx;
				if (cy < mCellMinY)
					mCellMinY = cy;
				if (cx > mCellMaxX)
					mCellMaxX = cx;
				if (cy > mCellMaxY)
					mCellMaxY = cy;
			}

			mNonEmptyCells.Add(cell);
			cell.mItems.Add(item);
		}

		private bool RemoveItemFromCell(Item item, float cx, float cy)
		{
			Point pt = new Point(cx, cy);
			Cell cell = mCellMap[pt];
			if (cell == null)
			{
				return false;
			}

			if (!cell.mItems.Remove(item))
			{
				return false;
			}

			if (cell.mItems.Count == 0)
			{
				mNonEmptyCells.Remove(cell);
			}

			return true;
		}

		private LinkedHashSet<Item> GetDictItemsInCellRect(float cl, float ct, float cw, float ch, LinkedHashSet<Item> result)
		{
			result.Clear();
			Point pt = new Point(cl, ct);
			for (float cy = ct; cy < ct + ch; cy++, pt.mY++)
			{
				for (float cx = cl; cx < cl + cw; cx++, pt.mX++)
				{
					Cell? cell = null;
					if (mCellMap.TryGetValue(pt, out cell) && !(cell.mItems.Count == 0))
					{
						// this is conscious of tunneling
						foreach(Item item in cell.mItems)
						{
							result.Add(item);
						}
					}
				}

				pt.mX = cl;
			}

			return result;
		}

		// this is conscious of tunneling
		private readonly List<Cell> getCellsTouchedBySegment_visited = new List<Cell>();
		// this is conscious of tunneling
		public virtual List<Cell> GetCellsTouchedBySegment(float x1, float y1, float x2, float y2, List<Cell> result)
		{
			result.Clear();
			getCellsTouchedBySegment_visited.Clear();

			// use set
			List<Cell> visited = getCellsTouchedBySegment_visited;
			Point pt = new Point(x1, y1);
			mGrid.Grid_traverse(mCellSize, x1, y1, x2, y2, new AnonymousTraverseCallback(this, pt, visited, result));
			return result;
		}

		private sealed class AnonymousTraverseCallback : Grid.TraverseCallback
		{

			private readonly World parent;
			private readonly Point pt;
			private readonly List<Cell> visited;
			private List<Cell> result;

			public AnonymousTraverseCallback(World parent, Point pt, List<Cell> visited, List<Cell> result)
			{
				this.parent = parent;
				this.pt = pt;
				this.visited = visited;
				this.result = result;
			}

			public bool OnTraverse(float cx, float cy, int stepX, int stepY)
			{
				//stop if cell coordinates are outside of the world.
				if (stepX == -1 && cx < parent.mCellMinX || stepX == 1 && cx > parent.mCellMaxX || stepY == -1 && cy < parent.mCellMinY || stepY == 1 && cy > parent.mCellMaxY)
					return false;
				pt.mX = cx;
				pt.mY = cy;
				Cell cell = parent.mCellMap[pt];
				if (cell == null || visited.Contains(cell))
				{
					return true;
				}

				visited.Add(cell);
				result.Add(cell);
				return true;
			}
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		public virtual List<Cell> GetCellsTouchedByRay(float originX, float originY, float dirX, float dirY, List<Cell> result)
		{
			result.Clear();
			getCellsTouchedBySegment_visited.Clear();

			// use set
			List<Cell> visited = getCellsTouchedBySegment_visited;
			Point pt = new Point(originX, originY);
			mGrid.Grid_traverseRay(mCellSize, originX, originY, dirX, dirY, new AnonymousTraverseCallback1(this, pt, visited, result));
			return result;
		}

		private sealed class AnonymousTraverseCallback1 : Grid.TraverseCallback
		{
			private readonly World parent;
			private readonly Point pt;
			private readonly List<Cell> visited;
			private List<Cell> result;

			public AnonymousTraverseCallback1(World parent, Point pt, List<Cell> visited, List<Cell> result)
			{
				this.parent = parent;
				this.pt = pt;
				this.visited = visited;
				this.result = result;
			}

			public bool OnTraverse(float cx, float cy, int stepX, int stepY)
			{
				//stop if cell coordinates are outside of the world.
				if (stepX == -1 && cx < parent.mCellMinX || stepX == 1 && cx > parent.mCellMaxX || stepY == -1 && cy < parent.mCellMinY || stepY == 1 && cy > parent.mCellMaxY)
					return false;
				pt.mX = cx;
				pt.mY = cy;
				Cell cell = parent.mCellMap[pt];
				if (cell == null || visited.Contains(cell))
				{
					return true;
				}

				visited.Add(cell);
				result.Add(cell);
				return true;
			}
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly List<Cell> info_cells = new List<Cell>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly Point info_ti = new Point();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly IntPoint info_normalX = new IntPoint();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly IntPoint info_normalY = new IntPoint();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly List<Item> info_visited = new List<Item>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private List<ItemInfo> GetInfoAboutItemsTouchedBySegment(float x1, float y1, float x2, float y2, CollisionFilter filter, List<ItemInfo> infos)
		{
			info_visited.Clear();
			infos.Clear();
			GetCellsTouchedBySegment(x1, y1, x2, y2, info_cells);
			foreach (Cell cell in info_cells)
			{
				foreach (Item item in cell.mItems)
				{
					if (!info_visited.Contains(item))
					{
						info_visited.Add(item);
						if (filter == null || filter.Filter(item, null) != null)
						{
							Rect rect = rects[item];
							float l = rect.mX;
							float t = rect.mY;
							float w = rect.mWidth;
							float h = rect.mHeight;
							if (Rect.Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, 0, 1, info_ti, info_normalX, info_normalY))
							{
								float ti1 = info_ti.mX;
								float ti2 = info_ti.mY;
								if ((0 < ti1 && ti1 < 1) || (0 < ti2 && ti2 < 1))
								{
									Rect.Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, float.MinValue, float.MaxValue, info_ti, info_normalX, info_normalY);
									float tii0 = info_ti.mX;
									float tii1 = info_ti.mY;
									infos.Add(new ItemInfo(item, ti1, ti2, Math.Min(tii0, tii1)));
								}
							}
						}
					}
				}
			}

			infos.Sort(ItemInfo.weightComparator);
			return infos;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private List<ItemInfo> GetInfoAboutItemsTouchedByRay(float originX, float originY, float dirX, float dirY, CollisionFilter filter, List<ItemInfo> infos)
		{
			info_visited.Clear();
			infos.Clear();
			GetCellsTouchedByRay(originX, originY, dirX, dirY, info_cells);
			foreach (Cell cell in info_cells)
			{
				foreach (Item item in cell.mItems)
				{
					if (!info_visited.Contains(item))
					{
						info_visited.Add(item);
						if (filter == null || filter.Filter(item, null) != null)
						{
							Rect rect = rects[item];
							float l = rect.mX;
							float t = rect.mY;
							float w = rect.mWidth;
							float h = rect.mHeight;
							if (Rect.Rect_getSegmentIntersectionIndices(l, t, w, h, originX, originY, originX + dirX, originY + dirY, 0, float.MaxValue, info_ti, info_normalX, info_normalY))
							{
								float ti1 = info_ti.mX;
								float ti2 = info_ti.mY;
								infos.Add(new ItemInfo(item, ti1, ti2, Math.Min(ti1, ti2)));
							}
						}
					}
				}
			}

			infos.Sort(ItemInfo.weightComparator);
			return infos;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		public virtual Collisions Project(Item item, float x, float y, float w, float h, float goalX, float goalY, Collisions collisions)
		{
			return Project(item, x, y, w, h, goalX, goalY, new DefaultFilter(), collisions);
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly List<Item> project_visited = new List<Item>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly Rect project_c = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private readonly LinkedHashSet<Item> project_dictItemsInCellRect = new LinkedHashSet<Item>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		public virtual Collisions Project(Item item, float x, float y, float w, float h, float goalX, float goalY, CollisionFilter filter, Collisions collisions)
		{
			collisions.Clear();
			List<Item> visited = project_visited;
			visited.Clear();
			if (item != null)
			{
				visited.Add(item);
			}

			/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
			float tl = MathF.Min(goalX, x);
			float tt = MathF.Min(goalY, y);
			float tr = MathF.Max(goalX + w, x + w);
			float tb = MathF.Max(goalY + h, y + h);
			float tw = tr - tl;
			float th = tb - tt;
			mGrid.Grid_toCellRect(mCellSize, tl, tt, tw, th, project_c);
			float cl = project_c.mX, ct = project_c.mY, cw = project_c.mWidth, ch = project_c.mHeight;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, project_dictItemsInCellRect);
			foreach (Item other in dictItemsInCellRect)
			{
				if (!visited.Contains(other))
				{
					visited.Add(other);
					IResponse response = filter.Filter(item, other);
					if (response != null)
					{
						Rect o = GetRect(other);
						float ox = o.mX, oy = o.mY, ow = o.mWidth, oh = o.mHeight;
						Collision col = mRectHelper.Rect_detectCollision(x, y, w, h, ox, oy, ow, oh, goalX, goalY);
						if (col != null)
						{
							collisions.Add(col.mOverlaps, col.mTI, col.mMove.mX, col.mMove.mY, col.mNormal.mX, col.mNormal.mY, col.mTouch.mX, col.mTouch.mY, col.mItemRect.mX, col.mItemRect.mY, col.mItemRect.mWidth, col.mItemRect.mHeight, col.mOtherRect.mX, col.mOtherRect.mY, col.mOtherRect.mWidth, col.mOtherRect.mHeight, item, other, response);
						}
					}
				}
			}

			if (mTileMode)
			{
				collisions.Sort();
			}

			return collisions;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Dictionary<Item, Rect> rects = new Dictionary<Item, Rect>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Rect GetRect(Item item)
		{
			return rects[item];
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Dictionary<Item,Rect>.KeyCollection GetItems()
		{
			return rects.Keys;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Dictionary<Item, Rect>.ValueCollection GetRects()
		{
			return rects.Values;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Dictionary<Point, Cell>.ValueCollection GetCells()
		{
			return mCellMap.Values;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual int CountCells()
		{
			return mCellMap.Count;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual bool HasItem(Item item)
		{
			return rects.ContainsKey(item);
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual int CountItems()
		{
			return rects.Count;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Point ToWorld(float cx, float cy, Point result)
		{
			Grid.Grid_toWorld(mCellSize, cx, cy, result);
			return result;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Point ToCell(float x, float y, Point result)
		{
			Grid.Grid_toCell(mCellSize, x, y, result);
			return result;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect add_c = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Item Add(Item item, float x, float y, float w, float h)
		{
			if (rects.ContainsKey(item))
			{
				return item;
			}

			rects.Add(item, new Rect(x, y, w, h));
			mGrid.Grid_toCellRect(mCellSize, x, y, w, h, add_c);
			float cl = add_c.mX, ct = add_c.mY, cw = add_c.mWidth, ch = add_c.mHeight;
			for (float cy = ct; cy < ct + ch; cy++)
			{
				for (float cx = cl; cx < cl + cw; cx++)
				{
					AddItemToCell(item, cx, cy);
				}
			}

			return item;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect remove_c = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Remove(Item item)
		{
			Rect rect = GetRect(item);
			float x = rect.mX, y = rect.mY, w = rect.mWidth, h = rect.mHeight;
			rects.Remove(item);
			mGrid.Grid_toCellRect(mCellSize, x, y, w, h, remove_c);
			float cl = remove_c.mX, ct = remove_c.mY, cw = remove_c.mWidth, ch = remove_c.mHeight;
			for (float cy = ct; cy < ct + ch; cy++)
			{
				for (float cx = cl; cx < cl + cw; cx++)
				{
					RemoveItemFromCell(item, cx, cy);
				}
			}
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Reset()
		{
			rects.Clear();
			mCellMap.Clear();
			mNonEmptyCells.Clear();
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Update(Item item, float x2, float y2)
		{
			Rect rect = GetRect(item);
			float x = rect.mX, y = rect.mY, w = rect.mWidth, h = rect.mHeight;
			Update(item, x2, y2, w, h);
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect update_c1 = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect update_c2 = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Update(Item item, float x2, float y2, float w2, float h2)
		{
			Rect rect = GetRect(item);
			float x1 = rect.mX, y1 = rect.mY, w1 = rect.mWidth, h1 = rect.mHeight;
			if (x1 != x2 || y1 != y2 || w1 != w2 || h1 != h2)
			{
				Rect c1 = mGrid.Grid_toCellRect(mCellSize, x1, y1, w1, h1, update_c1);
				Rect c2 = mGrid.Grid_toCellRect(mCellSize, x2, y2, w2, h2, update_c2);
				float cl1 = c1.mX, ct1 = c1.mY, cw1 = c1.mWidth, ch1 = c1.mHeight;
				float cl2 = c2.mX, ct2 = c2.mY, cw2 = c2.mWidth, ch2 = c2.mHeight;
				if (cl1 != cl2 || ct1 != ct2 || cw1 != cw2 || ch1 != ch2)
				{
					float cr1 = cl1 + cw1 - 1, cb1 = ct1 + ch1 - 1;
					float cr2 = cl2 + cw2 - 1, cb2 = ct2 + ch2 - 1;
					bool cyOut;
					for (float cy = ct1; cy <= cb1; cy++)
					{
						cyOut = cy < ct2 || cy > cb2;
						for (float cx = cl1; cx <= cr1; cx++)
						{
							if (cyOut || cx < cl2 || cx > cr2)
							{
								RemoveItemFromCell(item, cx, cy);
							}
						}
					}

					for (float cy = ct2; cy <= cb2; cy++)
					{
						cyOut = cy < ct1 || cy > cb1;
						for (float cx = cl2; cx <= cr2; cx++)
						{
							if (cyOut || cx < cl1 || cx > cr1)
							{
								AddItemToCell(item, cx, cy);
							}
						}
					}
				}

				rect.Set(x2, y2, w2, h2);
			}
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly List<Item> check_visited = new List<Item>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Collisions check_cols = new Collisions();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Collisions check_projectedCols = new Collisions();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly IResponse.Result check_result = new IResponse.Result();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual IResponse.Result Check(Item item, float goalX, float goalY, CollisionFilter filter)
		{
			List<Item> visited = check_visited;
			visited.Clear();
			visited.Add(item);
			CollisionFilter visitedFilter = new AnonymousCollisionFilter(this, visited, filter);
			Rect rect = GetRect(item);
			float x = rect.mX, y = rect.mY, w = rect.mWidth, h = rect.mHeight;
			Collisions cols = check_cols;
			cols.Clear();
			Collisions projectedCols = Project(item, x, y, w, h, goalX, goalY, filter, check_projectedCols);
			IResponse.Result result = check_result;
			while (projectedCols != null && !projectedCols.IsEmpty())
			{
				Collision col = projectedCols.Get(0);
				cols.Add(col.mOverlaps, col.mTI, col.mMove.mX, col.mMove.mY, col.mNormal.mX, col.mNormal.mY, col.mTouch.mX, col.mTouch.mY, col.mItemRect.mX, col.mItemRect.mY, col.mItemRect.mWidth, col.mItemRect.mHeight, col.mOtherRect.mX, col.mOtherRect.mY, col.mOtherRect.mWidth, col.mOtherRect.mHeight, col.mItem, col.mOther, col.mType);
				visited.Add(col.mOther);
				IResponse response = col.mType;
				response.Response(this, col, x, y, w, h, goalX, goalY, visitedFilter, result);
				goalX = result.mGoalX;
				goalY = result.mGoalY;
				projectedCols = result.mProjectedCollisions;
			}

			result.Set(goalX, goalY);
			result.mProjectedCollisions.Clear();
			for (int i = 0; i < cols.Size(); i++)
			{
				result.mProjectedCollisions.Add(cols.Get(i));
			}

			return result;
		}

		private sealed class AnonymousCollisionFilter : CollisionFilter
		{
			private readonly World parent;
			List<Item> visited;
			CollisionFilter? filter;

			public AnonymousCollisionFilter(World parent, List<Item> visited, CollisionFilter? filter)
			{
				this.parent = parent;
				this.visited = visited;
				this.filter = filter;
			}

			public IResponse Filter(Item item, Item other)
			{
				if (visited.Contains(other))
				{
					return null;
				}

				if (filter is null)
				{
					return new DefaultFilter().Filter(item, other);
				}

				return filter.Filter(item, other);
			}
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual IResponse.Result Move(Item item, float goalX, float goalY, CollisionFilter filter)
		{
			IResponse.Result result = Check(item, goalX, goalY, filter);
			Update(item, result.mGoalX, result.mGoalY);
			return result;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual float GetCellSize()
		{
			return mCellSize;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect query_c = new Rect();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly LinkedHashSet<Item> query_dictItemsInCellRect = new LinkedHashSet<Item>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<Item> QueryRect(float x, float y, float w, float h, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			mGrid.Grid_toCellRect(mCellSize, x, y, w, h, query_c);
			float cl = query_c.mX, ct = query_c.mY, cw = query_c.mWidth, ch = query_c.mHeight;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && Rect.Rect_isIntersecting(x, y, w, h, rect.mX, rect.mY, rect.mWidth, rect.mHeight))
				{
					items.Add(item);
				}
			}

			return items;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Point query_point = new Point();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<Item> QueryPoint(float x, float y, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			ToCell(x, y, query_point);
			float cx = query_point.mX;
			float cy = query_point.mY;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cx, cy, 1, 1, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && Rect.Rect_containsPoint(rect.mX, rect.mY, rect.mWidth, rect.mHeight, x, y))
				{
					items.Add(item);
				}
			}

			return items;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly List<ItemInfo> query_infos = new List<ItemInfo>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<Item> QuerySegment(float x1, float y1, float x2, float y2, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			List<ItemInfo> infos = GetInfoAboutItemsTouchedBySegment(x1, y1, x2, y2, filter, query_infos);
			foreach (ItemInfo info in infos)
			{
				items.Add(info.mItem);
			}

			return items;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<ItemInfo> QuerySegmentWithCoords(float x1, float y1, float x2, float y2, CollisionFilter filter, List<ItemInfo> infos)
		{
			infos.Clear();
			infos = GetInfoAboutItemsTouchedBySegment(x1, y1, x2, y2, filter, infos);
			float dx = x2 - x1;
			float dy = y2 - y1;
			foreach (ItemInfo info in infos)
			{
				float ti1 = info.mTI1;
				float ti2 = info.mTI2;
				info.mWeight = 0;
				info.mX1 = x1 + dx * ti1;
				info.mY1 = y1 + dy * ti1;
				info.mX2 = x1 + dx * ti2;
				info.mY2 = y1 + dy * ti2;
			}

			return infos;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<Item> QueryRay(float originX, float originY, float dirX, float dirY, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			List<ItemInfo> infos = GetInfoAboutItemsTouchedByRay(originX, originY, dirX, dirY, filter, query_infos);
			foreach (ItemInfo info in infos)
			{
				items.Add(info.mItem);
			}

			return items;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual List<ItemInfo> QueryRayWithCoords(float originX, float originY, float dirX, float dirY, CollisionFilter filter, List<ItemInfo> infos)
		{
			infos.Clear();
			infos = GetInfoAboutItemsTouchedByRay(originX, originY, dirX, dirY, filter, infos);
			foreach (ItemInfo info in infos)
			{
				float ti1 = info.mTI1;
				float ti2 = info.mTI2;
				info.mWeight = 0;
				info.mX1 = originX + dirX * ti1;
				info.mY1 = originY + dirY * ti1;
				info.mX2 = originX + dirX * ti2;
				info.mY2 = originY + dirY * ti2;
			}

			return infos;
		}
	}
}