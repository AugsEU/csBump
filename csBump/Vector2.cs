#if !MONOGAME_BUILD
namespace csBump
{
	/// <summary>
	/// Represents a 2D point with float
	/// </summary>
	public struct Vector2
	{
		public float X { get; set; }
		public float Y { get; set; }

		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}

			if(obj is Vector2 otherVec)
			{
				return X == otherVec.X && Y == otherVec.Y;
			}

			return false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public override string ToString()
		{
			return "{X:" + X + " Y:" + Y + "}";
		}
	}
}
#endif