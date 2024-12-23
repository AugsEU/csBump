using System.Runtime.CompilerServices;

namespace csBump
{
	public class BumpID
	{
		protected readonly int mIdentityHash;

		public BumpID()
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