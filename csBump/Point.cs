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

		public void Set(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object? o)
		{
			if (ReferenceEquals(this, o))
			{
				return true;
			}

			if (o == null || GetType() != o.GetType())
			{
				return false;
			}

			Point point = (Point)o;

			return point.x == x && point.y == y;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// Combine the hash codes of x and y
				int hash = 1471;
				hash = hash * 2647 + x.GetHashCode();
				hash = hash * 6091 + y.GetHashCode();
				return hash;
			}
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ')';
		}
	}
}