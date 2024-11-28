namespace csBump
{
	/// <summary>
	/// Represents a 2D point with float
	/// </summary>
	public class Point
	{
		public float mX;
		public float mY;

		public Point()
		{
		}

		public Point(float x, float y)
		{
			this.mX = x;
			this.mY = y;
		}

		public void Set(float x, float y)
		{
			this.mX = x;
			this.mY = y;
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

			return point.mX == mX && point.mY == mY;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// Combine the hash codes of x and y
				int hash = 1471;
				hash = hash * 2647 + mX.GetHashCode();
				hash = hash * 6091 + mY.GetHashCode();
				return hash;
			}
		}

		public override string ToString()
		{
			return "(" + mX + ", " + mY + ')';
		}
	}
}