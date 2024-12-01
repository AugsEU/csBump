#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

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

		private Vector2 mPosition;
		public HashSet<Item> mItems = new HashSet<Item>(11);

		public float X { get { return mPosition.X; } set { mPosition.X = value; } }
		public float Y { get { return mPosition.Y; } set { mPosition.Y = value; } }

		public Cell()
		{
			mIdentityHash = RuntimeHelpers.GetHashCode(this);
			mPosition = Vector2.Zero;
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

			return mPosition == otherCell.mPosition;
		}

		public override int GetHashCode()
		{
			return mIdentityHash;
		}
	}
}