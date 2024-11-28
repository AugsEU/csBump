using System.Runtime.CompilerServices;

namespace csBump
{
	public class Item
	{
		protected readonly int mIdentityHash;

		public Item()
		{
			mIdentityHash = RuntimeHelpers.GetHashCode(this);
		}

		public override bool Equals(object? o)
		{
			return ReferenceEquals(this, o);
		}

		public override int GetHashCode()
		{
			return mIdentityHash;
		}
	}
}