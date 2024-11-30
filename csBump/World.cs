#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

using csBump.util;

namespace csBump
{
	/// <summary>
	/// Represents the world in which collisions can take place.
	/// </summary>
	public class World
	{
		private readonly Dictionary<Vector2, Cell> mCellMap = new Dictionary<Vector2, Cell>();
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
			Vector2 pt = new Vector2(cx, cy);
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
			Vector2 pt = new Vector2(cx, cy);
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
			Vector2 pt = new Vector2(cl, ct);
			for (float cy = ct; cy < ct + ch; cy++, pt.Y++)
			{
				for (float cx = cl; cx < cl + cw; cx++, pt.X++)
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

				pt.X = cl;
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
			Vector2 pt = new Vector2(x1, y1);
			mGrid.Grid_traverse(mCellSize, x1, y1, x2, y2, new AnonymousTraverseCallback(this, pt, visited, result));
			return result;
		}

		private sealed class AnonymousTraverseCallback : Grid.TraverseCallback
		{
			private readonly World parent;
			private readonly List<Cell> visited;
			private List<Cell> result;
			private Vector2 pt; // TO DO: Remove this member.

			public AnonymousTraverseCallback(World parent, Vector2 pt, List<Cell> visited, List<Cell> result)
			{
				this.parent = parent;
				this.visited = visited;
				this.result = result;
			}

			public bool OnTraverse(float cx, float cy, int stepX, int stepY)
			{
				//stop if cell coordinates are outside of the world.
				if (stepX == -1 && cx < parent.mCellMinX || stepX == 1 && cx > parent.mCellMaxX || stepY == -1 && cy < parent.mCellMinY || stepY == 1 && cy > parent.mCellMaxY)
					return false;
				pt.X = cx;
				pt.Y = cy;
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
			Vector2 pt = new Vector2(originX, originY);
			mGrid.Grid_traverseRay(mCellSize, originX, originY, dirX, dirY, new AnonymousTraverseCallback1(this, pt, visited, result));
			return result;
		}

		private sealed class AnonymousTraverseCallback1 : Grid.TraverseCallback
		{
			private readonly World parent;
			private readonly List<Cell> visited;
			private List<Cell> result;
			private Vector2 pt; // TO DO: Remove this.

			public AnonymousTraverseCallback1(World parent, Vector2 pt, List<Cell> visited, List<Cell> result)
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
				pt.X = cx;
				pt.Y = cy;
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
		private List<Cell> info_cells = new List<Cell>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private Vector2 info_ti = new Vector2(0.0f, 0.0f);
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private Point info_normalX = new Point(0, 0);
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private Point info_normalY = new Point(0, 0);
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		private List<Item> info_visited = new List<Item>();
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
							Rect2f rect = rects[item];
							float l = rect.X;
							float t = rect.Y;
							float w = rect.Width;
							float h = rect.Height;
							if (Rect2f.Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, 0, 1, out info_ti, out info_normalX, out info_normalY))
							{
								float ti1 = info_ti.X;
								float ti2 = info_ti.Y;
								if ((0 < ti1 && ti1 < 1) || (0 < ti2 && ti2 < 1))
								{
									Rect2f.Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, float.MinValue, float.MaxValue, out info_ti, out info_normalX, out info_normalY);
									float tii0 = info_ti.X;
									float tii1 = info_ti.Y;
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
							Rect2f rect = rects[item];
							float l = rect.X;
							float t = rect.Y;
							float w = rect.Width;
							float h = rect.Height;
							if (Rect2f.Rect_getSegmentIntersectionIndices(l, t, w, h, originX, originY, originX + dirX, originY + dirY, 0, float.MaxValue, out info_ti, out info_normalX, out info_normalY))
							{
								float ti1 = info_ti.X;
								float ti2 = info_ti.Y;
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
		public virtual Collisions Project(Item item, Rect2f rect, Vector2 goal, Collisions collisions)
		{
			return Project(item, rect, goal, new DefaultFilter(), collisions);
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
		private Rect2f project_c = new Rect2f();
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
		public virtual Collisions Project(Item item, Rect2f rect, Vector2 goal, CollisionFilter filter, Collisions collisions)
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
			float tl = MathF.Min(goal.X, rect.X);
			float tt = MathF.Min(goal.Y, rect.Y);
			float tr = MathF.Max(goal.X + rect.Width, rect.X + rect.Width);
			float tb = MathF.Max(goal.Y + rect.Height, rect.Y + rect.Height);
			float tw = tr - tl;
			float th = tb - tt;
			project_c = mGrid.Grid_toCellRect(mCellSize, tl, tt, tw, th);
			float cl = project_c.X, ct = project_c.Y, cw = project_c.Width, ch = project_c.Height;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, project_dictItemsInCellRect);
			foreach (Item other in dictItemsInCellRect)
			{
				if (!visited.Contains(other))
				{
					visited.Add(other);
					IResponse response = filter.Filter(item, other);
					if (response != null)
					{
						Rect2f otherRect = GetRect(other);

						Collision? col = mRectHelper.Rect_detectCollision(rect, otherRect, goal);
						if (col != null)
						{
							collisions.Add(col.mOverlaps, col.mTI, col.mMove.X, col.mMove.Y, col.mNormal.X, col.mNormal.Y, col.mTouch.X, col.mTouch.Y, col.mItemRect.X, col.mItemRect.Y, col.mItemRect.Width, col.mItemRect.Height, col.mOtherRect.X, col.mOtherRect.Y, col.mOtherRect.Width, col.mOtherRect.Height, item, other, response);
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
		private readonly Dictionary<Item, Rect2f> rects = new Dictionary<Item, Rect2f>();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public Rect2f GetRect(Item item)
		{
			return rects[item];
		}

		private void SetRect(Item item, ref Rect2f rect)
		{
			rects[item] = rect;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Dictionary<Item,Rect2f>.KeyCollection GetItems()
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
		public virtual Dictionary<Item, Rect2f>.ValueCollection GetRects()
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
		public virtual Dictionary<Vector2, Cell>.ValueCollection GetCells()
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
		public virtual Vector2 ToWorld(float cx, float cy, Vector2 result)
		{
			Grid.Grid_toWorld(mCellSize, cx, cy, out result);
			return result;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Vector2 ToCell(float x, float y, Vector2 result)
		{
			Grid.Grid_toCell(mCellSize, x, y, out result);
			return result;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private Rect2f add_c = new Rect2f();
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

			rects.Add(item, new Rect2f(x, y, w, h));
			add_c = mGrid.Grid_toCellRect(mCellSize, x, y, w, h);
			float cl = add_c.X, ct = add_c.Y, cw = add_c.Width, ch = add_c.Height;
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
		private Rect2f remove_c = new Rect2f();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Remove(Item item)
		{
			Rect2f rect = GetRect(item);
			float x = rect.X, y = rect.Y, w = rect.Width, h = rect.Height;
			rects.Remove(item);
			remove_c = mGrid.Grid_toCellRect(mCellSize, x, y, w, h);
			float cl = remove_c.X, ct = remove_c.Y, cw = remove_c.Width, ch = remove_c.Height;
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
			Rect2f rect = GetRect(item);
			float x = rect.X, y = rect.Y, w = rect.Width, h = rect.Height;
			Update(item, x2, y2, w, h);
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect2f update_c1 = new Rect2f();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		private readonly Rect2f update_c2 = new Rect2f();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual void Update(Item item, float x2, float y2, float w2, float h2)
		{
			Rect2f rect = GetRect(item);
			float x1 = rect.X, y1 = rect.Y, w1 = rect.Width, h1 = rect.Height;
			if (x1 != x2 || y1 != y2 || w1 != w2 || h1 != h2)
			{
				Rect2f c1 = mGrid.Grid_toCellRect(mCellSize, x1, y1, w1, h1);
				Rect2f c2 = mGrid.Grid_toCellRect(mCellSize, x2, y2, w2, h2);
				float cl1 = c1.X, ct1 = c1.Y, cw1 = c1.Width, ch1 = c1.Height;
				float cl2 = c2.X, ct2 = c2.Y, cw2 = c2.Width, ch2 = c2.Height;
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

				rects[item] = new Rect2f(x2, y2, w2, h2);
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
		public virtual IResponse.Result Check(Item item, Vector2 goal, CollisionFilter filter)
		{
			List<Item> visited = check_visited;
			visited.Clear();
			visited.Add(item);
			CollisionFilter visitedFilter = new AnonymousCollisionFilter(this, visited, filter);
			Rect2f rect = GetRect(item);
			Collisions cols = check_cols;
			cols.Clear();
			Collisions projectedCols = Project(item, rect, goal, filter, check_projectedCols);
			IResponse.Result result = check_result;
			while (projectedCols != null && !projectedCols.IsEmpty())
			{
				Collision col = projectedCols.Get(0);
				cols.Add(col.mOverlaps, col.mTI, col.mMove.X, col.mMove.Y, col.mNormal.X, col.mNormal.Y, col.mTouch.X, col.mTouch.Y, col.mItemRect.X, col.mItemRect.Y, col.mItemRect.Width, col.mItemRect.Height, col.mOtherRect.X, col.mOtherRect.Y, col.mOtherRect.Width, col.mOtherRect.Height, col.mItem, col.mOther, col.mType);
				visited.Add(col.mOther);
				IResponse response = col.mType;
				response.Response(this, col, rect, goal, visitedFilter, result);
				goal = result.mGoal;

				projectedCols = result.mProjectedCollisions;
			}

			result.Set(goal);
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
		public virtual IResponse.Result Move(Item item, Vector2 goal, CollisionFilter filter)
		{
			IResponse.Result result = Check(item, goal, filter);
			Update(item, result.mGoal.X, result.mGoal.Y);
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
		private Rect2f query_c = new Rect2f();
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
			query_c = mGrid.Grid_toCellRect(mCellSize, x, y, w, h);
			float cl = query_c.X, ct = query_c.Y, cw = query_c.Width, ch = query_c.Height;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect2f rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && Rect2f.Rect_isIntersecting(x, y, w, h, rect.X, rect.Y, rect.Width, rect.Height))
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
		private Vector2 query_point = new Vector2(0.0f, 0.0f);
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
			query_point = ToCell(x, y, query_point);
			float cx = query_point.X;
			float cy = query_point.Y;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cx, cy, 1, 1, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect2f rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && Rect2f.Rect_containsPoint(rect.X, rect.Y, rect.Width, rect.Height, x, y))
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