namespace csBump
{
	public class IntPoint
	{
		public int mX;
		public int mY;

		public IntPoint()
		{
		}

		public IntPoint(int x, int y)
		{
			mX = x;
			mY = y;
		}

		public void Set(int x, int y)
		{
			mX = x;
			mY = y;
		}

		public override bool Equals(object? o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			IntPoint intPoint = (IntPoint)o;
			return (mX == intPoint.mX && mY == intPoint.mY);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// Combine the hash codes of x and y
				int hash = 1471;
				hash = hash * 2647 + mX;
				hash = hash * 6091 + mY;
				return hash;
			}
		}

		public override string ToString()
		{
			return "(" + mX + ", " + mY + ')';
		}
	}
}