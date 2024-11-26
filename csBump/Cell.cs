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
using csBump.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace csBump
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public class Cell
	{
		public float x;
		public float y;
		/// <summary>
		/// This class is compared by identity, like {@link Item}, and it also caches its identityHashCode() result.
		/// </summary>
		protected readonly int identityHash;
		public HashSet<Item> items = new HashSet<Item>(11);
		public Cell()
		{
			identityHash = RuntimeHelpers.GetHashCode(this);
			x = 0;
			y = 0;
		}

		public override bool Equals(object? o)
		{
			if(ReferenceEquals(this, o))
			{
				return true;
			}

			if(o is null)
			{
				return false;
			}

			if(o is not Cell)
			{
				return false;
			}

			Cell otherCell = (Cell)o;

			return x == otherCell.x && y == otherCell.y;
		}

		public override int GetHashCode()
		{
			return identityHash;
		}
	}
}