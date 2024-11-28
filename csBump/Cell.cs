using System.Runtime.CompilerServices;

namespace csBump
{
	/// <summary>
	/// A cell containing items.
	/// </summary>
	public class Cell
	{
		/// <summary>
		/// This class is compared by identity, like {@link Item}, and it also caches its identityHashCode() result.
		/// </summary>
		protected readonly int mIdentityHash;

		public float mX;
		public float mY;
		public HashSet<Item> mItems = new HashSet<Item>(11);

		public Cell()
		{
			mIdentityHash = RuntimeHelpers.GetHashCode(this);
			mX = 0;
			mY = 0;
		}

		public override bool Equals(object? o)
		{
			if(ReferenceEquals(this, o))
			{
				return true;
			}

			if(o is null)
			{
				return false;
			}

			if(o is not Cell)
			{
				return false;
			}

			Cell otherCell = (Cell)o;

			return mX == otherCell.mX && mY == otherCell.mY;
		}

		public override int GetHashCode()
		{
			return mIdentityHash;
		}
	}
}