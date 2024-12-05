namespace csBump.util
{
	/// <summary>
	/// A comparer that is also a singleton. Doesn't hold any state.
	/// </summary>
	abstract class StatelessComparer<T, C>  where T : StatelessComparer<T, C>, IComparer<C>, new()
	{
		static T mInstance = new T();
		
		public static T Comparer { get { return mInstance; } }
	}
}
