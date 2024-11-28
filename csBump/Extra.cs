namespace csBump
{
	public static class Extra
	{
		public const float DELTA = 1E-05F;

		public static int Sign(float x)
		{
			if (x > 0)
			{
				return 1;
			}
			else if (x < 0)
			{
				return -1;
			}

			return 0;
		}

		public static float Nearest(float x, float a, float b)
		{
			if (Math.Abs(a - x) < Math.Abs(b - x))
			{
				return a;
			}

			return b;
		}

		public static void Swap<T>(IList<T> list, int index1, int index2)
		{
			T temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}
	}
}