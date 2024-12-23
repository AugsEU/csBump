#if MONOGAME_BUILD
using Microsoft.Xna.Framework;
#endif

using System.Runtime.CompilerServices;

namespace csBump
{
	/// <summary>
	/// A cell containing items.
	/// </summary>
	public class Cell : IEquatable<Cell>
	{
		private Vector2 mPosition;
		public HashSet<BumpID> mItems = new HashSet<BumpID>(16);

		public Cell(Vector2 pos)
		{
			mPosition = pos;
		}

		public override bool Equals(object? other)
		{
			if(ReferenceEquals(this, other))
			{
				return true;
			}

			if(other is not Cell)
			{
				return false;
			}

			return Equals((Cell)other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Equals(Cell? other)
		{
			if (other is null)
			{
				return false;
			}

			return mPosition == other.mPosition;
		}
	}
}