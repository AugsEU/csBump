#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

namespace csBump
{
	/// <summary>
	/// Represents a rect in space. With floats.
	/// </summary>
	public struct Rect2f
	{
		public float mX;
		public float mY;

		public float mWidth;
		public float mHeight;

		public Rect2f()
		{
		}

		public Rect2f(float x, float y, float w, float h)
		{
			mX = x;
			mY = y;
			mWidth = w;
			mHeight = h;
		}

		public void Set(float x, float y, float w, float h)
		{
			mX = x;
			mY = y;
			mWidth = w;
			mHeight = h;
		}

		public static void Rect_getNearestCorner(float x, float y, float w, float h, float px, float py, out Vector2 result)
		{
			result = new Vector2(Extra.Nearest(px, x, x + w), Extra.Nearest(py, y, y + h));
		}

		/// <summary>
		/// This is a generalized implementation of the Liang-Barsky algorithm, which also returns
		/// the normals of the sides where the segment intersects.
		/// Notice that normals are only guaranteed to be accurate when initially ti1 == float.MinValue, ti2 == float.MaxValue
		/// </summary>
		/// <returns>false if the segment never touches the rect</returns>
		public static bool Rect_getSegmentIntersectionIndices(float x, float y, float w, float h, float x1, float y1, float x2, float y2, float ti1, float ti2, out Vector2 ti, out Point n1, out Point n2)
		{
			ti = Vector2.Zero;
			n1 = Point.Zero;
			n2 = Point.Zero;

			float dx = x2 - x1;
			float dy = y2 - y1;
			int nx = 0, ny = 0;
			int nx1 = 0, ny1 = 0, nx2 = 0, ny2 = 0;
			float p, q, r;
			for (int side = 1; side <= 4; side++)
			{
				switch (side)
				{
					case 1: //left
						nx = -1;
						ny = 0;
						p = -dx;
						q = x1 - x;
						break;
					case 2: //right
						nx = 1;
						ny = 0;
						p = dx;
						q = x + w - x1;
						break;
					case 3: //top
						nx = 0;
						ny = -1;
						p = -dy;
						q = y1 - y;
						break;
					default: //bottom
						nx = 0;
						ny = 1;
						p = dy;
						q = y + h - y1;
						break;
				}

				if (p == 0)
				{
					if (q <= 0)
					{
						return false;
					}
				}
				else
				{
					r = q / p;
					if (p < 0)
					{
						if (r > ti2)
						{
							return false;
						}
						else if (r > ti1)
						{
							ti1 = r;
							nx1 = nx;
							ny1 = ny;
						}
					}
					else
					{
						if (r < ti1)
						{
							return false;
						}
						else if (r < ti2)
						{
							ti2 = r;
							nx2 = nx;
							ny2 = ny;
						}
					}
				}
			}

			ti = new Vector2(ti1, ti2);
			n1 = new Point(nx1, ny1);
			n2 = new Point(nx2, ny2);
			return true;
		}

		/// <summary>
		/// Calculates the Minkowsky difference between 2 rects, which is another rect
		/// </summary>
		public static Rect2f Rect_getDiff(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			return new Rect2f(x2 - x1 - w1, y2 - y1 - h1, w1 + w2, h1 + h2);
		}

		public static bool Rect_containsPoint(float x, float y, float w, float h, float px, float py)
		{
			return px - x > Extra.DELTA && py - y > Extra.DELTA && x + w - px > Extra.DELTA && y + h - py > Extra.DELTA;
		}

		public static bool Rect_isIntersecting(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			return x1 < x2 + w2 && x2 < x1 + w1 && y1 < y2 + h2 && y2 < y1 + h1;
		}

		public static float Rect_getSquareDistance(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
		{
			float dx = x1 - x2 + (w1 - w2) / 2;
			float dy = y1 - y2 + (h1 - h2) / 2;
			return dx * dx + dy * dy;
		}
	}
}