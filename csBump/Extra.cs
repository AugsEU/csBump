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
	public static class Extra
	{
		public static float DELTA = 1E-05F;
		public static int Sign(float x)
		{
			if (x > 0)
			{
				return 1;
			}
			else if (x < 0)
			{
				return -1;
			}

			return 0;
		}

		public static float Nearest(float x, float a, float b)
		{
			if (Math.Abs(a - x) < Math.Abs(b - x))
			{
				return a;
			}

			return b;
		}

		public static void Swap<T>(IList<T> list, int index1, int index2)
		{
			T temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}
	}
}