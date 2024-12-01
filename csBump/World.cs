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

		private readonly List<Cell> getCellsTouchedBySegment_visited = new List<Cell>();
		public virtual List<Cell> GetCellsTouchedBySegment(Vector2 pt1, Vector2 pt2, List<Cell> result)
		{
			result.Clear();
			getCellsTouchedBySegment_visited.Clear();

			// use set
			List<Cell> visited = getCellsTouchedBySegment_visited;
			mGrid.Grid_traverse(mCellSize, pt1, pt2, new AnonymousTraverseCallback(this, pt1, visited, result));
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

		public virtual List<Cell> GetCellsTouchedByRay(Vector2 origin, Vector2 dir, List<Cell> result)
		{
			result.Clear();
			getCellsTouchedBySegment_visited.Clear();

			// use set
			List<Cell> visited = getCellsTouchedBySegment_visited;
			mGrid.Grid_traverseRay(mCellSize, origin, dir, new AnonymousTraverseCallback1(this, origin, visited, result));
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

		private List<Cell> info_cells = new List<Cell>();
		private Vector2 info_ti = new Vector2(0.0f, 0.0f);
		private Point info_normalX = new Point(0, 0);
		private Point info_normalY = new Point(0, 0);
		private List<Item> info_visited = new List<Item>();

		private List<ItemInfo> GetInfoAboutItemsTouchedBySegment(Vector2 pt1, Vector2 pt2, CollisionFilter filter, List<ItemInfo> infos)
		{
			info_visited.Clear();
			infos.Clear();
			GetCellsTouchedBySegment(pt1, pt2, info_cells);
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
							if (Rect2f.Rect_getSegmentIntersectionIndices(rect, pt1, pt2, 0, 1, out info_ti, out info_normalX, out info_normalY))
							{
								float ti1 = info_ti.X;
								float ti2 = info_ti.Y;
								if ((0 < ti1 && ti1 < 1) || (0 < ti2 && ti2 < 1))
								{
									Rect2f.Rect_getSegmentIntersectionIndices(rect, pt1, pt2, float.MinValue, float.MaxValue, out info_ti, out info_normalX, out info_normalY);
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

		private List<ItemInfo> GetInfoAboutItemsTouchedByRay(Vector2 origin, Vector2 dir, CollisionFilter filter, List<ItemInfo> infos)
		{
			info_visited.Clear();
			infos.Clear();
			GetCellsTouchedByRay(origin, dir, info_cells);
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
							if (Rect2f.Rect_getSegmentIntersectionIndices(rect, origin, origin + dir, 0, float.MaxValue, out info_ti, out info_normalX, out info_normalY))
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

		public virtual Collisions Project(Item item, Rect2f rect, Vector2 goal, Collisions collisions)
		{
			return Project(item, rect, goal, new DefaultFilter(), collisions);
		}


		private readonly List<Item> project_visited = new List<Item>();
		private Rect2f project_c = new Rect2f();
		private readonly LinkedHashSet<Item> project_dictItemsInCellRect = new LinkedHashSet<Item>();

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
			project_c = mGrid.Grid_toCellRect(mCellSize, new Rect2f(tl, tt, tw, th));
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


		private readonly Dictionary<Item, Rect2f> rects = new Dictionary<Item, Rect2f>();

		public Rect2f GetRect(Item item)
		{
			return rects[item];
		}

		private void SetRect(Item item, ref Rect2f rect)
		{
			rects[item] = rect;
		}

		public virtual Dictionary<Item,Rect2f>.KeyCollection GetItems()
		{
			return rects.Keys;
		}
		public virtual Dictionary<Item, Rect2f>.ValueCollection GetRects()
		{
			return rects.Values;
		}

		public virtual Dictionary<Vector2, Cell>.ValueCollection GetCells()
		{
			return mCellMap.Values;
		}

		public virtual int CountCells()
		{
			return mCellMap.Count;
		}

		public virtual bool HasItem(Item item)
		{
			return rects.ContainsKey(item);
		}

		public virtual int CountItems()
		{
			return rects.Count;
		}

		public virtual Vector2 ToWorld(Vector2 point)
		{
			return Grid.Grid_toWorld(mCellSize, point);
		}


		public Vector2 ToCell(Vector2 point)
		{
			return Grid.Grid_toCell(mCellSize, point); ;
		}


		private Rect2f add_c = new Rect2f();

		public virtual Item Add(Item item, float x, float y, float w, float h)
		{
			if (rects.ContainsKey(item))
			{
				return item;
			}

			Rect2f newRect = new Rect2f(x, y, w, h);

			rects.Add(item, newRect);
			add_c = mGrid.Grid_toCellRect(mCellSize, newRect);
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

		private Rect2f remove_c = new Rect2f();

		public virtual void Remove(Item item)
		{
			Rect2f rect = GetRect(item);

			rects.Remove(item);
			remove_c = mGrid.Grid_toCellRect(mCellSize, rect);
			float cl = remove_c.X, ct = remove_c.Y, cw = remove_c.Width, ch = remove_c.Height;
			for (float cy = ct; cy < ct + ch; cy++)
			{
				for (float cx = cl; cx < cl + cw; cx++)
				{
					RemoveItemFromCell(item, cx, cy);
				}
			}
		}

		public virtual void Reset()
		{
			rects.Clear();
			mCellMap.Clear();
			mNonEmptyCells.Clear();
		}

		public virtual void Update(Item item, float x2, float y2)
		{
			Rect2f rect = GetRect(item);
			Rect2f dest = new Rect2f(x2, y2, rect.Width, rect.Height);
			Update(item, dest);
		}

		private readonly Rect2f update_c1 = new Rect2f();
		private readonly Rect2f update_c2 = new Rect2f();

		public void Update(Item item, Rect2f dest)
		{
			Rect2f curr = GetRect(item);

			if (!curr.Equals(ref dest))
			{
				Rect2f c1 = mGrid.Grid_toCellRect(mCellSize, curr);
				Rect2f c2 = mGrid.Grid_toCellRect(mCellSize, dest);
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

				rects[item] = dest;
			}
		}

		private readonly List<Item> check_visited = new List<Item>();
		private readonly Collisions check_cols = new Collisions();
		private readonly Collisions check_projectedCols = new Collisions();
		private readonly IResponse.Result check_result = new IResponse.Result();

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

		public virtual IResponse.Result Move(Item item, Vector2 goal, CollisionFilter filter)
		{
			IResponse.Result result = Check(item, goal, filter);
			Update(item, result.mGoal.X, result.mGoal.Y);
			return result;
		}


		public virtual float GetCellSize()
		{
			return mCellSize;
		}


		private Rect2f query_c = new Rect2f();
		private readonly LinkedHashSet<Item> query_dictItemsInCellRect = new LinkedHashSet<Item>();



		/// <summary>
		/// A collision check of items that intersect the given rectangle.
		/// </summary>
		/// <param name="rect">Rect to check</param>
		/// <param name="filter">Defines what items will be checked for collision.</param>
		/// <param name="items">An empty list that will be filled with the {@link Item} instances that collide with the rectangle.</param>
		public virtual List<Item> QueryRect(Rect2f rect, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			query_c = mGrid.Grid_toCellRect(mCellSize, rect);
			float cl = query_c.X, ct = query_c.Y, cw = query_c.Width, ch = query_c.Height;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect2f otherRect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && rect.IsIntersecting(otherRect))
				{
					items.Add(item);
				}
			}

			return items;
		}


		private Vector2 query_point = new Vector2(0.0f, 0.0f);

		/// <summary>
		/// A collision check of items that intersect the given point.
		/// </summary>
		/// <param name="point">Point to check</param>
		/// <param name="filter">Defines what items will be checked for collision.</param>
		/// <param name="items">An empty list that will be filled with the {@link Item} instances that collide with the point.</param>
		public virtual List<Item> QueryPoint(Vector2 point, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			query_point = ToCell(point);
			float cx = query_point.X;
			float cy = query_point.Y;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cx, cy, 1, 1, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect2f rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && rect.ContainsPoint(point))
				{
					items.Add(item);
				}
			}

			return items;
		}


		private readonly List<ItemInfo> query_infos = new List<ItemInfo>();

		/// <summary>
		/// A collision check of items that intersect the given line segment.
		/// </summary>
		/// <param name="pt1">Segment start</param>
		/// <param name="pt2">Segment end</param>
		/// <param name="filter">Defines what items will be checked for collision.</param>
		/// <param name="items">An empty list that will be filled with the {@link Item} instances that intersect the segment.</param>
		/// <returns></returns>
		public virtual List<Item> QuerySegment(Vector2 pt1, Vector2 pt2, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			List<ItemInfo> infos = GetInfoAboutItemsTouchedBySegment(pt1, pt2, filter, query_infos);
			foreach (ItemInfo info in infos)
			{
				items.Add(info.mItem);
			}

			return items;
		}

		/// <summary>
		/// A collision check of items that intersect the given line segment. Returns more details about where the collision
		/// occurs compared to {@link World#querySegment(float, float, float, float, CollisionFilter, ArrayList)}
		/// </summary>
		/// <param name="pt1">Segment start</param>
		/// <param name="pt2">Segment end</param>
		/// <param name="filter">Defines what items will be checked for collision.</param>
		/// <param name="infos">An empty list that will be filled with the collision information.</param>
		/// <returns></returns>
		public virtual List<ItemInfo> QuerySegmentWithCoords(Vector2 pt1, Vector2 pt2, CollisionFilter filter, List<ItemInfo> infos)
		{
			infos.Clear();
			infos = GetInfoAboutItemsTouchedBySegment(pt1, pt2, filter, infos);
			float dx = pt2.X - pt1.X;
			float dy = pt2.Y - pt1.Y;
			foreach (ItemInfo info in infos)
			{
				float ti1 = info.mTI1;
				float ti2 = info.mTI2;
				info.mWeight = 0;
				info.mX1 = pt1.X + dx * ti1;
				info.mY1 = pt1.Y + dy * ti1;
				info.mX2 = pt1.X + dx * ti2;
				info.mY2 = pt1.Y + dy * ti2;
			}

			return infos;
		}



		/// <summary>
		/// A collision check of items that intersect the given ray.
		/// </summary>
		/// <param name="origin">The origin of the ray.</param>
		/// <param name="dir">Vector that defines the angle of the ray.</param>
		/// <param name="filter">Defines what items will be checked for collision.</param>
		/// <param name="items">An empty list that will be filled with the {@link Item} instances that intersect the ray</param>
		public virtual List<Item> QueryRay(Vector2 origin, Vector2 dir, CollisionFilter filter, List<Item> items)
		{
			items.Clear();
			List<ItemInfo> infos = GetInfoAboutItemsTouchedByRay(origin, dir, filter, query_infos);
			foreach (ItemInfo info in infos)
			{
				items.Add(info.mItem);
			}

			return items;
		}

		/// <summary>
		/// A collision check of items that intersect the given ray. Returns more details about where the collision
		/// occurs compared to {@link World#queryRay(float, float, float, float, CollisionFilter, ArrayList)}
		/// </summary>
		/// <param name="origin">The origin of the ray</param>
		/// <param name="dir">Vector that defines the angle of the ray.</param>
		/// <param name="filter">Defines what items will be checked for collision.s</param>
		/// <param name="infos">An empty list that will be filled with the {@link Item} instances that intersect the ray</param>
		public virtual List<ItemInfo> QueryRayWithCoords(Vector2 origin, Vector2 dir, CollisionFilter filter, List<ItemInfo> infos)
		{
			infos.Clear();
			infos = GetInfoAboutItemsTouchedByRay(origin, dir, filter, infos);
			foreach (ItemInfo info in infos)
			{
				float ti1 = info.mTI1;
				float ti2 = info.mTI2;
				info.mWeight = 0;
				info.mX1 = origin.X + dir.X * ti1;
				info.mY1 = origin.Y + dir.Y * ti1;
				info.mX2 = origin.X + dir.X * ti2;
				info.mY2 = origin.Y + dir.Y * ti2;
			}

			return infos;
		}
	}
}