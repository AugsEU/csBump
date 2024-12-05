#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

using System.Diagnostics.CodeAnalysis;

namespace csBump
{
	/// <summary>
	/// Represents a rect in space. With floats.
	/// </summary>
	public struct Rect2f
	{
		Vector2 mPosition;
		Vector2 mSize;

		public float X { get { return mPosition.X; } set { mPosition.X = value; } }
		public float Y { get { return mPosition.Y; } set { mPosition.Y = value; } }
		public float Width { get { return mSize.X; } set { mSize.X = value; } }
		public float Height { get { return mSize.Y; } set { mSize.Y = value; } }
		public Vector2 Position { get { return mPosition; } set { mPosition = value; } }
		public Vector2 Size { get { return mSize; } set { mSize = value; } }

		public Rect2f()
		{
			mPosition = Vector2.Zero;
			mSize = Vector2.Zero;
		}

		public Rect2f(float x, float y, float w, float h)
		{
			X = x;
			Y = y;
			Width = w;
			Height = h;
		}

		public void Set(float x, float y, float w, float h)
		{
			X = x;
			Y = y;
			Width = w;
			Height = h;
		}

		public Vector2 GetNearestCorner(Vector2 point)
		{
			return new Vector2(Extra.Nearest(point.X, X, X + Width), Extra.Nearest(point.Y, Y, Y + Height));
		}

		public bool ContainsPoint(Vector2 point)
		{
			return point.X - X > Extra.DELTA && point.Y - Y > Extra.DELTA && X + Width - point.X > Extra.DELTA && Y + Height - point.Y > Extra.DELTA;
		}

		public bool IsIntersecting(Rect2f other)
		{
			return X < other.X + other.Width && other.X < X + Width && Y < other.Y + other.Height && other.Y < Y + Height;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if(obj is Rect2f otherRect)
			{
				return Equals(ref otherRect);
			}

			return false;
		}

		public bool Equals(ref Rect2f otherRect)
		{
			return otherRect.Position == Position && otherRect.Size == Size;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = mPosition.GetHashCode();
				hash = (hash + 137) ^ mSize.GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// This is a generalized implementation of the Liang-Barsky algorithm, which also returns
		/// the normals of the sides where the segment intersects.
		/// Notice that normals are only guaranteed to be accurate when initially ti1 == float.MinValue, ti2 == float.MaxValue
		/// </summary>
		/// <returns>false if the segment never touches the rect</returns>
		public static bool Rect_getSegmentIntersectionIndices(Rect2f rect, Vector2 pt1, Vector2 pt2, float ti1, float ti2, out Vector2 ti, out Point n1, out Point n2)
		{
			ti = Vector2.Zero;
			n1 = Point.Zero;
			n2 = Point.Zero;

			float dx = pt2.X - pt1.X;
			float dy = pt2.Y - pt1.Y;
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
						q = pt1.X - rect.X;
						break;
					case 2: //right
						nx = 1;
						ny = 0;
						p = dx;
						q = rect.X + rect.Width - pt1.X;
						break;
					case 3: //top
						nx = 0;
						ny = -1;
						p = -dy;
						q = pt1.Y - rect.Y;
						break;
					default: //bottom
						nx = 0;
						ny = 1;
						p = dy;
						q = rect.Y + rect.Height - pt1.Y;
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
		public static Rect2f GetDiff(Rect2f rect1, Rect2f rect2)
		{
			return new Rect2f(rect2.X - rect1.X - rect1.Width, rect2.Y - rect1.Y - rect1.Height, rect1.Width + rect2.Width, rect1.Height + rect2.Height);
		}

		public static float GetSquareDistance(Rect2f rect1, Rect2f rect2)
		{
			float dx = rect1.X - rect2.X + (rect1.Width - rect2.Width) / 2;
			float dy = rect1.Y - rect2.Y + (rect1.Height - rect2.Height) / 2;
			return dx * dx + dy * dy;
		}
	}
}