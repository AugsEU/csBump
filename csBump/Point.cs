#if !MONOGAME_BUILD
namespace csBump
{
	public struct Point
	{
		public int X {  get; set; }
		public int Y { get; set; }

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

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
	}
}
#endif