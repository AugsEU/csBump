using System;
using System.Numerics;

namespace csBump.Utils
{
	internal class MathUtils
	{
		public const float nanoToSec = 1 / 1000000000f;

		// ---
		public const float FLOAT_ROUNDING_ERROR = 0.000001f; // 32 bits
		public const float PI = 3.1415927f;
		public const float PI2 = PI * 2;

		public const float E = 2.7182818f;

		private const float radFull = PI * 2;
		private const float degFull = 360;

		/** multiply by this to convert from radians to degrees */
		public const float radiansToDegrees = 180f / PI;
		public const float radDeg = radiansToDegrees;
		/** multiply by this to convert from degrees to radians */
		public const float degreesToRadians = PI / 180;
		public const float degRad = degreesToRadians;

		static public Random mRandom = new Random();

		/** Returns the sine in radians from a lookup table. */
		static public float sinDeg(float degrees)
		{
			return MathF.Sin(degrees * degRad);
		}

		/** Returns the cosine in radians from a lookup table. */
		static public float cosDeg(float degrees)
		{
			return MathF.Cos(degrees * degRad);
		}



		/** Returns a random number between 0 (inclusive) and the specified value (inclusive). */
		static public int random(int range)
		{
			return mRandom.Next(range + 1);
		}

		/** Returns a random number between start (inclusive) and end (inclusive). */
		static public int random(int start, int end)
		{
			return start + mRandom.Next(end - start + 1);
		}

		/** Returns a random number between 0 (inclusive) and the specified value (inclusive). */
		static public long random(long range)
		{
			return (long)(mRandom.NextDouble() * range);
		}

		/** Returns a random number between start (inclusive) and end (inclusive). */
		static public long random(long start, long end)
		{
			return start + (long)(mRandom.NextDouble() * (end - start));
		}

		/** Returns a random bool value. */
		static public bool randomBoolean()
		{
			return mRandom.Next() % 2 == 0;
		}

		/** Returns true if a random value between 0 and 1 is less than the specified value. */
		static public bool randomBoolean(float chance)
		{
			return random() < chance;
		}

		/** Returns random number between 0.0 (inclusive) and 1.0 (exclusive). */
		static public float random()
		{
			return (float)mRandom.NextDouble();
		}

		/** Returns a random number between 0 (inclusive) and the specified value (exclusive). */
		static public float random(float range)
		{
			return random() * range;
		}

		/** Returns a random number between start (inclusive) and end (exclusive). */
		static public float random(float start, float end)
		{
			return start + random() * (end - start);
		}

		/** Returns -1 or 1, randomly. */
		static public int randomSign()
		{
			return mRandom.Next() % 2 == 0 ? -1 : 1;
		}

		/** Returns a triangularly distributed random number between -1.0 (exclusive) and 1.0 (exclusive), where values around zero are
		 * more likely.
		 * <p>
		 * This is an optimized version of {@link #randomTriangular(float, float, float) randomTriangular(-1, 1, 0)} */
		public static float randomTriangular()
		{
			return random() - random();
		}

		/** Returns a triangularly distributed random number between {@code -max} (exclusive) and {@code max} (exclusive), where values
		 * around zero are more likely.
		 * <p>
		 * This is an optimized version of {@link #randomTriangular(float, float, float) randomTriangular(-max, max, 0)}
		 * @param max the upper limit */
		public static float randomTriangular(float max)
		{
			return randomTriangular() * max;
		}

		/** Returns a triangularly distributed random number between {@code min} (inclusive) and {@code max} (exclusive), where the
		 * {@code mode} argument defaults to the midpoint between the bounds, giving a symmetric distribution.
		 * <p>
		 * This method is equivalent of {@link #randomTriangular(float, float, float) randomTriangular(min, max, (min + max) * .5f)}
		 * @param min the lower limit
		 * @param max the upper limit */
		public static float randomTriangular(float min, float max)
		{
			return randomTriangular(min, max, (min + max) * 0.5f);
		}

		/** Returns a triangularly distributed random number between {@code min} (inclusive) and {@code max} (exclusive), where values
		 * around {@code mode} are more likely.
		 * @param min the lower limit
		 * @param max the upper limit
		 * @param mode the point around which the values are more likely */
		public static float randomTriangular(float min, float max, float mode)
		{
			float u = random();
			float d = max - min;
			if (u <= (mode - min) / d) return min + (float)MathF.Sqrt(u * d * (mode - min));
			return max - (float)MathF.Sqrt((1 - u) * d * (max - mode));
		}

		// ---

		/** Returns the next power of two. Returns the specified value if the value is already a power of two. */
		public static int nextPowerOfTwo(int value)
		{
			if (value <= 0)
				throw new ArgumentException("Value must be positive.", nameof(value));

			uint uValue = (uint)value;

			// If the value is already a power of two, return it
			if ((uValue & (uValue - 1)) == 0)
			{
				return value;
			}

			// Use bit manipulation to find the next power of two
			return 1 << (32 - BitOperations.LeadingZeroCount(Math.Max(uValue, 2)));
		}

		static public bool isPowerOfTwo(int value)
		{
			return value != 0 && (value & value - 1) == 0;
		}

		// ---

		static public short clamp(short value, short min, short max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		static public int clamp(int value, int min, int max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		static public long clamp(long value, long min, long max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		static public float clamp(float value, float min, float max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		static public double clamp(double value, double min, double max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		// ---

		/** Linearly interpolates between fromValue to toValue on progress position. */
		static public float lerp(float fromValue, float toValue, float progress)
		{
			return fromValue + (toValue - fromValue) * progress;
		}

		/** Linearly interpolates between two angles in radians. Takes into account that angles wrap at two pi and always takes the
		 * direction with the smallest delta angle.
		 * 
		 * @param fromRadians start angle in radians
		 * @param toRadians target angle in radians
		 * @param progress interpolation value in the range [0, 1]
		 * @return the interpolated angle in the range [0, PI2[ */
		public static float lerpAngle(float fromRadians, float toRadians, float progress)
		{
			float delta = ((toRadians - fromRadians + PI2 + PI) % PI2) - PI;
			return (fromRadians + delta * progress + PI2) % PI2;
		}

		/** Linearly interpolates between two angles in degrees. Takes into account that angles wrap at 360 degrees and always takes
		 * the direction with the smallest delta angle.
		 * 
		 * @param fromDegrees start angle in degrees
		 * @param toDegrees target angle in degrees
		 * @param progress interpolation value in the range [0, 1]
		 * @return the interpolated angle in the range [0, 360[ */
		public static float lerpAngleDeg(float fromDegrees, float toDegrees, float progress)
		{
			float delta = ((toDegrees - fromDegrees + 360 + 180) % 360) - 180;
			return (fromDegrees + delta * progress + 360) % 360;
		}

		// ---

		private const int BIG_ENOUGH_INT = 16 * 1024;
		private const double BIG_ENOUGH_FLOOR = BIG_ENOUGH_INT;
		private const double CEIL = 0.9999999;
		private const double BIG_ENOUGH_CEIL = 16384.999999999996;
		private const double BIG_ENOUGH_ROUND = BIG_ENOUGH_INT + 0.5f;

		/** Returns the largest integer less than or equal to the specified float. This method will only properly floor floats from
		 * -(2^14) to (Float.MAX_VALUE - 2^14). */
		static public int floor(float value)
		{
			return (int)(value + BIG_ENOUGH_FLOOR) - BIG_ENOUGH_INT;
		}

		/** Returns the largest integer less than or equal to the specified float. This method will only properly floor floats that are
		 * positive. Note this method simply casts the float to int. */
		static public int floorPositive(float value)
		{
			return (int)value;
		}

		/** Returns the smallest integer greater than or equal to the specified float. This method will only properly ceil floats from
		 * -(2^14) to (Float.MAX_VALUE - 2^14). */
		static public int ceil(float value)
		{
			return BIG_ENOUGH_INT - (int)(BIG_ENOUGH_FLOOR - value);
		}

		/** Returns the smallest integer greater than or equal to the specified float. This method will only properly ceil floats that
		 * are positive. */
		static public int ceilPositive(float value)
		{
			return (int)(value + CEIL);
		}

		/** Returns the closest integer to the specified float. This method will only properly round floats from -(2^14) to
		 * (Float.MAX_VALUE - 2^14). */
		static public int round(float value)
		{
			return (int)(value + BIG_ENOUGH_ROUND) - BIG_ENOUGH_INT;
		}

		/** Returns the closest integer to the specified float. This method will only properly round floats that are positive. */
		static public int roundPositive(float value)
		{
			return (int)(value + 0.5f);
		}

		/** Returns true if the value is zero (using the default tolerance as upper bound) */
		static public bool isZero(float value)
		{
			return Math.Abs(value) <= FLOAT_ROUNDING_ERROR;
		}

		/** Returns true if the value is zero.
		 * @param tolerance represent an upper bound below which the value is considered zero. */
		static public bool isZero(float value, float tolerance)
		{
			return Math.Abs(value) <= tolerance;
		}

		/** Returns true if a is nearly equal to b. The function uses the default floating error tolerance.
		 * @param a the first value.
		 * @param b the second value. */
		static public bool isEqual(float a, float b)
		{
			return Math.Abs(a - b) <= FLOAT_ROUNDING_ERROR;
		}

		/** Returns true if a is nearly equal to b.
		 * @param a the first value.
		 * @param b the second value.
		 * @param tolerance represent an upper bound below which the two values are considered equal. */
		static public bool isEqual(float a, float b, float tolerance)
		{
			return Math.Abs(a - b) <= tolerance;
		}

		/** @return the logarithm of value with base a */
		static public float log(float a, float value)
		{
			return MathF.Log(value, a);
		}

		/** @return the logarithm of value with base 2 */
		static public float log2(float value)
		{
			return MathF.Log2(value);
		}
	}
}
