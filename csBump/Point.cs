#if !MONOGAME_BUILD
namespace csBump
{
	public struct Point : IEquatable<Point>
	{
		#region rConsts

		private static readonly Point zeroPoint = new Point();

		public static Point Zero { get { return zeroPoint; } }

		#endregion rConsts





		#region rInit

		public int X {  get; set; }
		public int Y { get; set; }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		#endregion rInit





		#region rImpl

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}

			if (obj is Point otherPoint)
			{
				return X == otherPoint.X && Y == otherPoint.Y;
			}

			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}
		}

		public override string ToString()
		{
			return "(" + X + ", " + Y + ')';
		}

		public bool Equals(Point other)
		{
			return X == other.X && Y == other.Y;
		}

		#endregion rImpl





		#region rOperators

		public static Point operator +(Point value1, Point value2)
		{
			return new Point(value1.X + value2.X, value1.Y + value2.Y);
		}

		public static Point operator -(Point value1, Point value2)
		{
			return new Point(value1.X - value2.X, value1.Y - value2.Y);
		}

		public static Point operator *(Point value1, Point value2)
		{
			return new Point(value1.X * value2.X, value1.Y * value2.Y);
		}

		public static Point operator /(Point source, Point divisor)
		{
			return new Point(source.X / divisor.X, source.Y / divisor.Y);
		}

		public static bool operator ==(Point a, Point b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Point a, Point b)
		{
			return !a.Equals(b);
		}

		#endregion rOperators
	}
}
#endif