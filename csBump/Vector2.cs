#if !MONOGAME_BUILD
using System.Runtime.CompilerServices;

namespace csBump
{
	/// <summary>
	/// Represents a 2D point with float
	/// </summary>
	public struct Vector2 : IEquatable<Vector2>
	{
		#region rConsts

		private static readonly Vector2 ZERO = new Vector2(0f, 0f);
		private static readonly Vector2 UNIT_X = new Vector2(1f, 0f);
		private static readonly Vector2 UNIT_Y = new Vector2(0f, 1f);
		private static readonly Vector2 UNIT_XY = new Vector2(1f, 1f);

		public static Vector2 Zero { get { return ZERO; } }

		public static Vector2 UnitX { get { return UNIT_X; } }

		public static Vector2 UnitY { get { return UNIT_Y; } }

		public static Vector2 One { get { return UNIT_XY; } }

		#endregion rConsts





		#region rInit

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

		#endregion rInit



		#region rImpl

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

		public bool Equals(Vector2 other)
		{
			return (X == other.X) && (Y == other.Y);
		}

		#endregion rImpl



		#region rOperators

		public static Vector2 operator -(Vector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static Vector2 operator +(Vector2 value1, Vector2 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		public static Vector2 operator *(Vector2 value1, Vector2 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		public static Vector2 operator *(Vector2 value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 value1, Vector2 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 value1, float divider)
		{
			float factor = 1 / divider;
			value1.X *= factor;
			value1.Y *= factor;
			return value1;
		}

		public static bool operator ==(Vector2 value1, Vector2 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y;
		}

		public static bool operator !=(Vector2 value1, Vector2 value2)
		{
			return value1.X != value2.X || value1.Y != value2.Y;
		}

		#endregion rOperators
	}
}
#endif