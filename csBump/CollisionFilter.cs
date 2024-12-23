namespace csBump
{
	/// <summary>
	/// Takes two items and sees how they should
	/// </summary>
	public interface CollisionFilter
	{
		public IResponse? Filter(BumpID? item, BumpID? other);
	}

	public class DefaultFilter : CollisionFilter
	{
		public IResponse? Filter(BumpID? item, BumpID? other)
		{
			return new SlideResponse();
		}
	}
}