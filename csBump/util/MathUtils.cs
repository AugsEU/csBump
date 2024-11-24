/*
 * Copyright 2017 DongBat.
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
using System.Numerics;
using System.Text;

namespace csBump.util
{
	/// <summary>
	///  * @author tao
	/// </summary>
	public sealed class MathUtils
	{
		public static readonly float nanoToSec = 1 / 1E+09F;
		// ---
		public static readonly float FLOAT_ROUNDING_ERROR = 1E-06F; // 32 bits
		public static readonly float PI = 3.1415927F;
		public static readonly float PI2 = PI * 2;
		public static readonly float E = 2.7182817F;
		private static readonly int SIN_BITS = 14; // 16KB. Adjust for accuracy.
		private static readonly int SIN_MASK = ~(-1 << SIN_BITS);
		private static readonly int SIN_COUNT = SIN_MASK + 1;
		private static readonly float radFull = PI * 2;
		private static readonly float degFull = 360;
		private static readonly float radToIndex = SIN_COUNT / radFull;
		private static readonly float degToIndex = SIN_COUNT / degFull;
		/// <summary>
		/// multiply by this to convert from radians to degrees
		/// </summary>
		public static readonly float radiansToDegrees = 180F / PI;
		public static readonly float radDeg = radiansToDegrees;
		/// <summary>
		/// multiply by this to convert from degrees to radians
		/// </summary>
		public static readonly float degreesToRadians = PI / 180;
		public static readonly float degRad = degreesToRadians;
		private class SinTable
		{
			public static readonly float[] table = new float[SIN_COUNT];
			static SinTable()
			{
				for (int i = 0; i < SIN_COUNT; i++)
				{
					table[i] = (float)Math.Sin((i + 0.5F) / SIN_COUNT * radFull);
				}

				for (int i = 0; i < 360; i += 90)
				{
					table[(int)(i * degToIndex) & SIN_MASK] = (float)Math.Sin(i * degreesToRadians);
				}
			}
		}

		/// <summary>
		/// Returns the sine in radians from a lookup table.
		/// </summary>
		public static float Sin(float radians)
		{
			return SinTable.table[(int)(radians * radToIndex) & SIN_MASK];
		}

		/// <summary>
		/// Returns the cosine in radians from a lookup table.
		/// </summary>
		public static float Cos(float radians)
		{
			return SinTable.table[(int)((radians + PI / 2) * radToIndex) & SIN_MASK];
		}

		/// <summary>
		/// Returns the sine in radians from a lookup table.
		/// </summary>
		public static float SinDeg(float degrees)
		{
			return SinTable.table[(int)(degrees * degToIndex) & SIN_MASK];
		}

		/// <summary>
		/// Returns the cosine in radians from a lookup table.
		/// </summary>
		public static float CosDeg(float degrees)
		{
			return SinTable.table[(int)((degrees + 90) * degToIndex) & SIN_MASK];
		}

		// ---
		public static float Atan2(float y, float x)
		{
			if (x == 0F)
			{
				if (y > 0F)
					return PI / 2;
				if (y == 0F)
					return 0F;
				return -PI / 2;
			}

			float atan, z = y / x;
			if (Math.Abs(z) < 1F)
			{
				atan = z / (1F + 0.28F * z * z);
				if (x < 0F)
					return atan + (y < 0F ? -PI : PI);
				return atan;
			}

			atan = PI / 2 - z / (z * z + 0.28F);
			return y < 0F ? atan - PI : atan;
		}

		// ---
		public static Random random = new Random();
		/// <summary>
		/// Returns a random number between 0 (inclusive) and the specified value (inclusive).
		/// </summary>
		public static int Random(int range)
		{
			return random.Next(range + 1);
		}

		/// <summary>
		/// Returns a random number between start (inclusive) and end (inclusive).
		/// </summary>
		public static int Random(int start, int end)
		{
			return start + random.Next(end - start + 1);
		}

		/// <summary>
		/// Returns a random number between 0 (inclusive) and the specified value (inclusive).
		/// </summary>
		public static long Random(long range)
		{
			return (long)(random.NextDouble() * range);
		}

		/// <summary>
		/// Returns a random number between start (inclusive) and end (inclusive).
		/// </summary>
		public static long Random(long start, long end)
		{
			return start + (long)(random.NextDouble() * (end - start));
		}

		/// <summary>
		/// Returns a random boolean value.
		/// </summary>
		public static bool RandomBoolean()
		{
			return random.Next() % 2 == 0;
		}

		/// <summary>
		/// Returns true if a random value between 0 and 1 is less than the specified value.
		/// </summary>
		public static bool RandomBoolean(float chance)
		{
			return MathUtils.Random() < chance;
		}

		/// <summary>
		/// Returns random number between 0.0 (inclusive) and 1.0 (exclusive).
		/// </summary>
		public static float Random()
		{
			return (float)random.NextDouble();
		}

		/// <summary>
		/// Returns a random number between 0 (inclusive) and the specified value (exclusive).
		/// </summary>
		public static float Random(float range)
		{
			return Random() * range;
		}

		/// <summary>
		/// Returns a random number between start (inclusive) and end (exclusive).
		/// </summary>
		public static float Random(float start, float end)
		{
			return start + Random() * (end - start);
		}

		/// <summary>
		/// Returns -1 or 1, randomly.
		/// </summary>
		public static int RandomSign()
		{
			return 1 | (random.Next() >> 31);
		}

		public static float RandomTriangular()
		{
			return Random() - Random();
		}

		public static float RandomTriangular(float max)
		{
			return (Random() - Random()) * max;
		}

		public static float RandomTriangular(float min, float max)
		{
			return RandomTriangular(min, max, (min + max) * 0.5F);
		}

		public static float RandomTriangular(float min, float max, float mode)
		{
			float u = Random();
			float d = max - min;
			if (u <= (mode - min) / d)
				return min + (float)Math.Sqrt(u * d * (mode - min));
			return max - (float)Math.Sqrt((1 - u) * d * (max - mode));
		}

		// ---
		/// <summary>
		/// Returns the next power of two. Returns the specified value if the value is already a power of two.
		/// </summary>
		public static int NextPowerOfTwo(int value)
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

		public static bool IsPowerOfTwo(int value)
		{
			return value != 0 && (value & value - 1) == 0;
		}

		// ---
		public static short Clamp(short value, short min, short max)
		{
			if (value < min)
				return min;
			if (value > max)
				return max;
			return value;
		}

		public static int Clamp(int value, int min, int max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		public static long Clamp(long value, long min, long max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		public static float Clamp(float value, float min, float max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		public static double Clamp(double value, double min, double max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		// ---
		/// <summary>
		/// Linearly interpolates between fromValue to toValue on progress position.
		/// </summary>
		public static float Lerp(float fromValue, float toValue, float progress)
		{
			return fromValue + (toValue - fromValue) * progress;
		}

		public static float LerpAngle(float fromRadians, float toRadians, float progress)
		{
			float delta = ((toRadians - fromRadians + PI2 + PI) % PI2) - PI;
			return (fromRadians + delta * progress + PI2) % PI2;
		}

		public static float LerpAngleDeg(float fromDegrees, float toDegrees, float progress)
		{
			float delta = ((toDegrees - fromDegrees + 360 + 180) % 360) - 180;
			return (fromDegrees + delta * progress + 360) % 360;
		}

		// ---
		private static readonly int BIG_ENOUGH_INT = 16 * 1024;
		private static readonly double BIG_ENOUGH_FLOOR = BIG_ENOUGH_INT;
		private static readonly double CEIL = 0.9999999;
		private static readonly double BIG_ENOUGH_CEIL = 16384.999999999996;
		private static readonly double BIG_ENOUGH_ROUND = BIG_ENOUGH_INT + 0.5F;
		public static int Floor(float value)
		{
			return (int)(value + BIG_ENOUGH_FLOOR) - BIG_ENOUGH_INT;
		}

		public static int FloorPositive(float value)
		{
			return (int)value;
		}

		public static int Ceil(float value)
		{
			return BIG_ENOUGH_INT - (int)(BIG_ENOUGH_FLOOR - value);
		}

		public static int CeilPositive(float value)
		{
			return (int)(value + CEIL);
		}

		public static int Round(float value)
		{
			return (int)(value + BIG_ENOUGH_ROUND) - BIG_ENOUGH_INT;
		}

		/// <summary>
		/// Returns the closest integer to the specified float. This method will only properly round floats that are positive.
		/// </summary>
		public static int RoundPositive(float value)
		{
			return (int)(value + 0.5F);
		}

		/// <summary>
		/// Returns true if the value is zero (using the default tolerance as upper bound)
		/// </summary>
		public static bool IsZero(float value)
		{
			return Math.Abs(value) <= FLOAT_ROUNDING_ERROR;
		}

		public static bool IsZero(float value, float tolerance)
		{
			return Math.Abs(value) <= tolerance;
		}

		public static bool IsEqual(float a, float b)
		{
			return Math.Abs(a - b) <= FLOAT_ROUNDING_ERROR;
		}

		public static bool IsEqual(float a, float b, float tolerance)
		{
			return Math.Abs(a - b) <= tolerance;
		}

		/// <returns>the logarithm of value with base a</returns>
		public static float Log(float a, float value)
		{
			return (float)(Math.Log(value) / Math.Log(a));
		}

		/// <returns>the logarithm of value with base 2</returns>
		public static float Log2(float value)
		{
			return Log(2, value);
		}
	}
}