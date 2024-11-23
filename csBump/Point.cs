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
	public class Point
	{
		public float x, y;
		public Point()
		{
		}

		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public virtual void Set(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public virtual bool Equals(object o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			Point point = (Point)o;
			return Float.Compare(point.x, x) == 0 && Float.Compare(point.y, y) == 0;
		}

		public virtual int GetHashCode()
		{
			return (int)(Float.FloatToIntBits(x) * 0xC13FA9A902A6328F + Float.FloatToIntBits(y) * 0x91E10DA5C79E7B1D >>> 32);
		}

		public virtual string ToString()
		{
			return "(" + x + ", " + y + ')';
		}
	}
}