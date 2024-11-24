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
	public class IntPoint
	{
		public int x, y;

		public IntPoint()
		{
		}

		public IntPoint(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public virtual void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object? o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			IntPoint intPoint = (IntPoint)o;
			return (x == intPoint.x && y == intPoint.y);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// Combine the hash codes of x and y
				int hash = 1471;
				hash = hash * 2647 + x;
				hash = hash * 6091 + y;
				return hash;
			}
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ')';
		}
	}
}