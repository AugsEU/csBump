/*
 * Copyright 2017 DongBat.
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
using csBump;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace csBump
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class RectHelper
	{
		private readonly Rect rect_detectCollision_diff = new Rect();
		private readonly Point rect_detectCollision_nearestCorner = new Point();
		private readonly Point rect_detectCollision_getSegmentIntersectionIndices_ti = new Point();
		private readonly IntPoint rect_detectCollision_getSegmentIntersectionIndices_n1 = new IntPoint();
		private readonly IntPoint rect_detectCollision_getSegmentIntersectionIndices_n2 = new IntPoint();
		private readonly Collision rect_detectCollision_getSegmentIntersectionIndices_col = new Collision();


		public virtual Collision? Rect_detectCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, float goalX, float goalY)
		{
			Collision col = rect_detectCollision_getSegmentIntersectionIndices_col;
			float dx = goalX - x1;
			float dy = goalY - y1;
			Rect.Rect_getDiff(x1, y1, w1, h1, x2, y2, w2, h2, rect_detectCollision_diff);
			float x = rect_detectCollision_diff.x;
			float y = rect_detectCollision_diff.y;
			float w = rect_detectCollision_diff.w;
			float h = rect_detectCollision_diff.h;
			bool overlaps = false;
			float? ti = null;
			int nx = 0, ny = 0;
			if (Rect.Rect_containsPoint(x, y, w, h, 0, 0))
			{

				//item was intersecting other
				Rect.Rect_getNearestCorner(x, y, w, h, 0, 0, rect_detectCollision_nearestCorner);
				float px = rect_detectCollision_nearestCorner.x;
				float py = rect_detectCollision_nearestCorner.y;

				//area of intersection
				float wi = MathF.Min(w1, MathF.Abs(px));
				float hi = MathF.Min(h1, MathF.Abs(py));
				ti = -wi * hi; //ti is the negative area of intersection
				overlaps = true;
			}
			else
			{
				bool intersect = Rect.Rect_getSegmentIntersectionIndices(x, y, w, h, 0, 0, dx, dy, float.MinValue, float.MaxValue, rect_detectCollision_getSegmentIntersectionIndices_ti, rect_detectCollision_getSegmentIntersectionIndices_n1, rect_detectCollision_getSegmentIntersectionIndices_n2);
				float ti1 = rect_detectCollision_getSegmentIntersectionIndices_ti.x;
				float ti2 = rect_detectCollision_getSegmentIntersectionIndices_ti.y;
				int nx1 = rect_detectCollision_getSegmentIntersectionIndices_n1.x;
				int ny1 = rect_detectCollision_getSegmentIntersectionIndices_n1.y;

				//item tunnels into other
				if (intersect && ti1 < 1 && MathF.Abs(ti1 - ti2) >= Extra.DELTA && (0 < ti1 + Extra.DELTA || 0 == ti1 && ti2 > 0))
				{
					ti = ti1;
					nx = nx1;
					ny = ny1;
					overlaps = false;
				}
			}

			if (!ti.HasValue)
			{
				return null;
			}

			float tx, ty;
			if (overlaps)
			{
				if (dx == 0 && dy == 0)
				{
					//intersecting and not moving - use minimum displacement vector
					Rect.Rect_getNearestCorner(x, y, w, h, 0, 0, rect_detectCollision_nearestCorner);
					float px = rect_detectCollision_nearestCorner.x;
					float py = rect_detectCollision_nearestCorner.y;
					if (MathF.Abs(px) < MathF.Abs(py))
					{
						py = 0;
					}
					else
					{
						px = 0;
					}

					nx = MathF.Sign(px);
					ny = MathF.Sign(py);
					tx = x1 + px;
					ty = y1 + py;
				}
				else
				{

					//intersecting and moving - move in the opposite direction
					bool intersect = Rect.Rect_getSegmentIntersectionIndices(x, y, w, h, 0, 0, dx, dy, -float.MaxValue, 1, rect_detectCollision_getSegmentIntersectionIndices_ti, rect_detectCollision_getSegmentIntersectionIndices_n1, rect_detectCollision_getSegmentIntersectionIndices_n2);
					float ti1 = rect_detectCollision_getSegmentIntersectionIndices_ti.x;
					nx = rect_detectCollision_getSegmentIntersectionIndices_n1.x;
					ny = rect_detectCollision_getSegmentIntersectionIndices_n1.y;
					if (!intersect)
					{
						return null;
					}

					tx = x1 + dx * ti1;
					ty = y1 + dy * ti1;
				}
			}
			else
			{
				//tunnel
				tx = x1 + dx * ti.Value;
				ty = y1 + dy * ti.Value;
			}

			col.Set(overlaps, ti.Value, dx, dy, nx, ny, tx, ty, x1, y1, w1, h1, x2, y2, w2, h2);
			return col;
		}
	}
}