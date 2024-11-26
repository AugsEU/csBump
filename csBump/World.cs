/*
 * Copyright 2017 tao.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using csBump;
using static csBump.Grid;
using static csBump.Response;
using System.ComponentModel;
using System.Net;

namespace csBump
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class World
	{
		private readonly HashMap<Point, Cell> cellMap = new HashMap<Point, Cell>();
		private readonly HashSet<Cell> nonEmptyCells = new HashSet<Cell>();
		private float cellMinX, cellMinY, cellMaxX, cellMaxY;
		private readonly Grid grid = new Grid();
		private readonly RectHelper rectHelper = new RectHelper();
		private bool tileMode = true;
		private readonly float cellSize;
		public World() : this(64F)
		{
		}

		public World(float cellSize)
		{
			this.cellSize = cellSize;
		}

		public virtual void SetTileMode(bool tileMode)
		{
			this.tileMode = tileMode;
		}

		public virtual bool IsTileMode()
		{
			return tileMode;
		}

		private void AddItemToCell(Item item, float cx, float cy)
		{
			Point pt = new Point(cx, cy);
			Cell cell = cellMap[pt];
			if (cell == null)
			{
				cell = new Cell();
				cellMap.Put(pt, cell);
				if (cx < cellMinX)
					cellMinX = cx;
				if (cy < cellMinY)
					cellMinY = cy;
				if (cx > cellMaxX)
					cellMaxX = cx;
				if (cy > cellMaxY)
					cellMaxY = cy;
			}

			nonEmptyCells.Add(cell);
			cell.items.Add(item);
		}

		private bool RemoveItemFromCell(Item item, float cx, float cy)
		{
			Point pt = new Point(cx, cy);
			Cell cell = cellMap[pt];
			if (cell == null)
			{
				return false;
			}

			if (!cell.items.Remove(item))
			{
				return false;
			}

			if (cell.items.IsEmpty())
			{
				nonEmptyCells.Remove(cell);
			}

			return true;
		}

		private LinkedHashSet<Item> GetDictItemsInCellRect(float cl, float ct, float cw, float ch, LinkedHashSet<Item> result)
		{
			result.Clear();
			Point pt = new Point(cl, ct);
			for (float cy = ct; cy < ct + ch; cy++, pt.y++)
			{
				for (float cx = cl; cx < cl + cw; cx++, pt.x++)
				{
					Cell cell = cellMap[pt];
					if (cell != null && !cell.items.IsEmpty())
					{

						// this is conscious of tunneling
						result.AddAll(cell.items);
					}
				}

				pt.x = cl;
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
			grid.Grid_traverse(cellSize, x1, y1, x2, y2, new AnonymousTraverseCallback(this));
			return result;
		}

		private sealed class AnonymousTraverseCallback : TraverseCallback
		{
			public AnonymousTraverseCallback(World parent)
			{
				this.parent = parent;
			}

			private readonly World parent;
			public bool OnTraverse(float cx, float cy, int stepX, int stepY)
			{

				//stop if cell coordinates are outside of the world.
				if (stepX == -1 && cx < cellMinX || stepX == 1 && cx > cellMaxX || stepY == -1 && cy < cellMinY || stepY == 1 && cy > cellMaxY)
					return false;
				pt.x = cx;
				pt.y = cy;
				Cell cell = cellMap[pt];
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
			grid.Grid_traverseRay(cellSize, originX, originY, dirX, dirY, new AnonymousTraverseCallback1(this));
			return result;
		}

		private sealed class AnonymousTraverseCallback1 : TraverseCallback
		{
			public AnonymousTraverseCallback1(World parent)
			{
				this.parent = parent;
			}

			private readonly World parent;
			public bool OnTraverse(float cx, float cy, int stepX, int stepY)
			{

				//stop if cell coordinates are outside of the world.
				if (stepX == -1 && cx < cellMinX || stepX == 1 && cx > cellMaxX || stepY == -1 && cy < cellMinY || stepY == 1 && cy > cellMaxY)
					return false;
				pt.x = cx;
				pt.y = cy;
				Cell cell = cellMap[pt];
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
				foreach (Item item in cell.items)
				{
					if (!info_visited.Contains(item))
					{
						info_visited.Add(item);
						if (filter == null || filter.Filter(item, null) != null)
						{
							Rect rect = rects[item];
							float l = rect.x;
							float t = rect.y;
							float w = rect.w;
							float h = rect.h;
							if (Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, 0, 1, info_ti, info_normalX, info_normalY))
							{
								float ti1 = info_ti.x;
								float ti2 = info_ti.y;
								if ((0 < ti1 && ti1 < 1) || (0 < ti2 && ti2 < 1))
								{
									Rect_getSegmentIntersectionIndices(l, t, w, h, x1, y1, x2, y2, -Float.MAX_VALUE, Float.MAX_VALUE, info_ti, info_normalX, info_normalY);
									float tii0 = info_ti.x;
									float tii1 = info_ti.y;
									infos.Add(new ItemInfo(item, ti1, ti2, Math.Min(tii0, tii1)));
								}
							}
						}
					}
				}
			}

			Collections.Sort(infos, weightComparator);
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
				foreach (Item item in cell.items)
				{
					if (!info_visited.Contains(item))
					{
						info_visited.Add(item);
						if (filter == null || filter.Filter(item, null) != null)
						{
							Rect rect = rects[item];
							float l = rect.x;
							float t = rect.y;
							float w = rect.w;
							float h = rect.h;
							if (Rect_getSegmentIntersectionIndices(l, t, w, h, originX, originY, originX + dirX, originY + dirY, 0, Float.MAX_VALUE, info_ti, info_normalX, info_normalY))
							{
								float ti1 = info_ti.x;
								float ti2 = info_ti.y;
								infos.Add(new ItemInfo(item, ti1, ti2, Math.Min(ti1, ti2)));
							}
						}
					}
				}
			}

			Collections.Sort(infos, weightComparator);
			return infos;
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		public virtual Collisions Project(Item item, float x, float y, float w, float h, float goalX, float goalY, Collisions collisions)
		{
			return Project(item, x, y, w, h, goalX, goalY, CollisionFilter.defaultFilter, collisions);
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
			float tl = Min(goalX, x);
			float tt = Min(goalY, y);
			float tr = Max(goalX + w, x + w);
			float tb = Max(goalY + h, y + h);
			float tw = tr - tl;
			float th = tb - tt;
			grid.Grid_toCellRect(cellSize, tl, tt, tw, th, project_c);
			float cl = project_c.x, ct = project_c.y, cw = project_c.w, ch = project_c.h;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, project_dictItemsInCellRect);
			foreach (Item other in dictItemsInCellRect)
			{
				if (!visited.Contains(other))
				{
					visited.Add(other);
					Response response = filter.Filter(item, other);
					if (response != null)
					{
						Rect o = GetRect(other);
						float ox = o.x, oy = o.y, ow = o.w, oh = o.h;
						Collision col = rectHelper.Rect_detectCollision(x, y, w, h, ox, oy, ow, oh, goalX, goalY);
						if (col != null)
						{
							collisions.Add(col.overlaps, col.ti, col.move.x, col.move.y, col.normal.x, col.normal.y, col.touch.x, col.touch.y, col.itemRect.x, col.itemRect.y, col.itemRect.w, col.itemRect.h, col.otherRect.x, col.otherRect.y, col.otherRect.w, col.otherRect.h, item, other, response);
						}
					}
				}
			}

			if (tileMode)
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
		private readonly HashMap<Item, Rect> rects = new HashMap<Item, Rect>();
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
		public virtual HashSet<Item> GetItems()
		{
			return rects.KeySet();
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Collection<Rect> GetRects()
		{
			return rects.Values();
		}

		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Collection<Cell> GetCells()
		{
			return cellMap.Values();
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
			return cellMap.Count;
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
			return rects.KeySet().Count;
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
			Grid_toWorld(cellSize, cx, cy, result);
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
			Grid_toCell(cellSize, x, y, result);
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

			rects.Put(item, new Rect(x, y, w, h));
			grid.Grid_toCellRect(cellSize, x, y, w, h, add_c);
			float cl = add_c.x, ct = add_c.y, cw = add_c.w, ch = add_c.h;
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
			float x = rect.x, y = rect.y, w = rect.w, h = rect.h;
			rects.Remove(item);
			grid.Grid_toCellRect(cellSize, x, y, w, h, remove_c);
			float cl = remove_c.x, ct = remove_c.y, cw = remove_c.w, ch = remove_c.h;
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
			cellMap.Clear();
			nonEmptyCells.Clear();
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
			float x = rect.x, y = rect.y, w = rect.w, h = rect.h;
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
			float x1 = rect.x, y1 = rect.y, w1 = rect.w, h1 = rect.h;
			if (x1 != x2 || y1 != y2 || w1 != w2 || h1 != h2)
			{
				Rect c1 = grid.Grid_toCellRect(cellSize, x1, y1, w1, h1, update_c1);
				Rect c2 = grid.Grid_toCellRect(cellSize, x2, y2, w2, h2, update_c2);
				float cl1 = c1.x, ct1 = c1.y, cw1 = c1.w, ch1 = c1.h;
				float cl2 = c2.x, ct2 = c2.y, cw2 = c2.w, ch2 = c2.h;
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
		private readonly Response.Result check_result = new Result();
		// this is conscious of tunneling
		// use set
		//stop if cell coordinates are outside of the world.
		// use set
		//stop if cell coordinates are outside of the world.
		/*This could probably be done with less cells using a polygon raster over the cells instead of a
    bounding rect of the whole movement. Conditional to building a queryPolygon method*/
		public virtual Response.Result Check(Item item, float goalX, float goalY, CollisionFilter filter)
		{
			List<Item> visited = check_visited;
			visited.Clear();
			visited.Add(item);
			CollisionFilter visitedFilter = new AnonymousCollisionFilter(this);
			Rect rect = GetRect(item);
			float x = rect.x, y = rect.y, w = rect.w, h = rect.h;
			Collisions cols = check_cols;
			cols.Clear();
			Collisions projectedCols = Project(item, x, y, w, h, goalX, goalY, filter, check_projectedCols);
			Response.Result result = check_result;
			while (projectedCols != null && !projectedCols.IsEmpty())
			{
				Collision col = projectedCols[0];
				cols.Add(col.overlaps, col.ti, col.move.x, col.move.y, col.normal.x, col.normal.y, col.touch.x, col.touch.y, col.itemRect.x, col.itemRect.y, col.itemRect.w, col.itemRect.h, col.otherRect.x, col.otherRect.y, col.otherRect.w, col.otherRect.h, col.item, col.other, col.type);
				visited.Add(col.other);
				Response response = col.type;
				response.Response(this, col, x, y, w, h, goalX, goalY, visitedFilter, result);
				goalX = result.goalX;
				goalY = result.goalY;
				projectedCols = result.projectedCollisions;
			}

			result[goalX] = goalY;
			result.projectedCollisions.Clear();
			for (int i = 0; i < cols.Count; i++)
			{
				result.projectedCollisions.Add(cols[i]);
			}

			return result;
		}

		private sealed class AnonymousCollisionFilter : CollisionFilter
		{
			public AnonymousCollisionFilter(World parent)
			{
				this.parent = parent;
			}

			private readonly World parent;
			public Response Filter(Item item, Item other)
			{
				if (visited.Contains(other))
				{
					return null;
				}

				if (filter == null)
				{
					return defaultFilter.Filter(item, other);
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
		public virtual Response.Result Move(Item item, float goalX, float goalY, CollisionFilter filter)
		{
			Response.Result result = Check(item, goalX, goalY, filter);
			Update(item, result.goalX, result.goalY);
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
			return cellSize;
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
			grid.Grid_toCellRect(cellSize, x, y, w, h, query_c);
			float cl = query_c.x, ct = query_c.y, cw = query_c.w, ch = query_c.h;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cl, ct, cw, ch, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && rect.Rect_isIntersecting(x, y, w, h, rect.x, rect.y, rect.w, rect.h))
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
			float cx = query_point.x;
			float cy = query_point.y;
			LinkedHashSet<Item> dictItemsInCellRect = GetDictItemsInCellRect(cx, cy, 1, 1, query_dictItemsInCellRect);
			foreach (Item item in dictItemsInCellRect)
			{
				Rect rect = rects[item];
				if ((filter == null || filter.Filter(item, null) != null) && rect.Rect_containsPoint(rect.x, rect.y, rect.w, rect.h, x, y))
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
				items.Add(info.item);
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
				float ti1 = info.ti1;
				float ti2 = info.ti2;
				info.weight = 0;
				info.x1 = x1 + dx * ti1;
				info.y1 = y1 + dy * ti1;
				info.x2 = x1 + dx * ti2;
				info.y2 = y1 + dy * ti2;
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
				items.Add(info.item);
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
				float ti1 = info.ti1;
				float ti2 = info.ti2;
				info.weight = 0;
				info.x1 = originX + dirX * ti1;
				info.y1 = originY + dirY * ti1;
				info.x2 = originX + dirX * ti2;
				info.y2 = originY + dirY * ti2;
			}

			return infos;
		}
	}
}