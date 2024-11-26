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

namespace csBump
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class Collision
	{
		public bool overlaps;
		public float ti;
		public Point move = new Point();
		public IntPoint normal = new IntPoint();
		public Point touch = new Point();
		public Rect itemRect = new Rect();
		public Rect otherRect = new Rect();
		public Item item;
		public Item other;
		public Response type;
		
		public Collision()
		{
			item = new Item();
			other = new Item();
		}

		public virtual void Set(bool overlaps, float ti, float moveX, float moveY, int normalX, int normalY, float touchX, float touchY, float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			this.overlaps = overlaps;
			this.ti = ti;
			this.move.Set(moveX, moveY);
			this.normal.Set(normalX, normalY);
			this.touch.Set(touchX, touchY);
			this.itemRect.Set(x1, y1, w1, h1);
			this.otherRect.Set(x2, y2, w2, h2);
		}
	}
}