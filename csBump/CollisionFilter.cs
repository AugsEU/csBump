namespace csBump
{
	/// <summary>
	/// Takes two items and sees how they should
	/// </summary>
	public interface CollisionFilter
	{
		public IResponse Filter(Item item, Item other);
	}

	public class DefaultFilter : CollisionFilter
	{
		public IResponse Filter(Item item, Item other)
		{
			return new SlideResponse();
		}
	}
}