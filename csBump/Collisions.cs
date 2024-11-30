namespace csBump
{
	/// <summary>
	///  Represents a list of collisions
	///  TO DO: Get rid of this.
	/// </summary>
	public class Collisions : IComparer<int>
	{
		private readonly List<bool> mOverlaps = new List<bool>();
		private readonly List<float> mTIs = new List<float>();
		private readonly List<float> mMoveXs = new List<float>();
		private readonly List<float> mMoveYs = new List<float>();
		private readonly List<int> mNormalXs = new List<int>();
		private readonly List<int> mNormalYs = new List<int>();
		private readonly List<float> mTouchXs = new List<float>();
		private readonly List<float> mTouchYs = new List<float>();
		private readonly List<float> mX1s = new List<float>();
		private readonly List<float> mY1s = new List<float>();
		private readonly List<float> mW1s = new List<float>();
		private readonly List<float> mH1s = new List<float>();
		private readonly List<float> mX2s = new List<float>();
		private readonly List<float> mY2s = new List<float>();
		private readonly List<float> mW2s = new List<float>();
		private readonly List<float> mH2s = new List<float>();
		public List<Item> mItems = new List<Item>();
		public List<Item> mOthers = new List<Item>();
		public List<IResponse> mTypes = new List<IResponse>();
		private int mSize = 0;

		public Collisions()
		{
		}

		public Collisions(Collisions other)
		{
			mOverlaps = new List<bool>(other.mOverlaps);
			mTIs = new List<float>(other.mTIs);
			mMoveXs = new List<float>(other.mMoveXs);
			mMoveYs = new List<float>(other.mMoveYs);
			mNormalXs = new List<int>(other.mNormalXs);
			mNormalYs = new List<int>(other.mNormalYs);
			mTouchXs = new List<float>(other.mTouchXs);
			mTouchYs = new List<float>(other.mTouchYs);
			mX1s = new List<float>(other.mX1s);
			mY1s = new List<float>(other.mY1s);
			mW1s = new List<float>(other.mW1s);
			mH1s = new List<float>(other.mH1s);
			mX2s = new List<float>(other.mX2s);
			mY2s = new List<float>(other.mY2s);
			mW2s = new List<float>(other.mW2s);
			mH2s = new List<float>(other.mH2s);
			mItems = new List<Item>(other.mItems);
			mOthers = new List<Item>(other.mOthers);
			mTypes = new List<IResponse>(other.mTypes);
			mSize = other.mSize;
		}

		public virtual void Add(Collision col)
		{
			Add(col.mOverlaps, col.mTI, col.mMove.X, col.mMove.Y, col.mNormal.X, col.mNormal.Y, col.mTouch.X, col.mTouch.Y, col.mItemRect.mX, col.mItemRect.mY, col.mItemRect.mWidth, col.mItemRect.mHeight, col.mOtherRect.mX, col.mOtherRect.mY, col.mOtherRect.mWidth, col.mOtherRect.mHeight, col.mItem, col.mOther, col.mType);
		}

		public virtual void Add(bool overlap, float ti, float moveX, float moveY, int normalX, int normalY, float touchX, float touchY, float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, Item item, Item other, IResponse type)
		{
			mSize++;
			mOverlaps.Add(overlap);
			mTIs.Add(ti);
			mMoveXs.Add(moveX);
			mMoveYs.Add(moveY);
			mNormalXs.Add(normalX);
			mNormalYs.Add(normalY);
			mTouchXs.Add(touchX);
			mTouchYs.Add(touchY);
			mX1s.Add(x1);
			mY1s.Add(y1);
			mW1s.Add(w1);
			mH1s.Add(h1);
			mX2s.Add(x2);
			mY2s.Add(y2);
			mW2s.Add(w2);
			mH2s.Add(h2);
			mItems.Add(item);
			mOthers.Add(other);
			mTypes.Add(type);
		}

		private readonly Collision collision = new Collision();
		public virtual Collision Get(int index)
		{
			if (index >= mSize)
			{
				return null;
			}

			collision.Set(mOverlaps[index], mTIs[index], mMoveXs[index], mMoveYs[index], mNormalXs[index], mNormalYs[index], mTouchXs[index], mTouchYs[index], mX1s[index], mY1s[index], mW1s[index], mH1s[index], mX2s[index], mY2s[index], mW2s[index], mH2s[index]);
			collision.mItem = mItems[index];
			collision.mOther = mOthers[index];
			collision.mType = mTypes[index];
			return collision;
		}

		public virtual void Remove(int index)
		{
			if (index < mSize)
			{
				mSize--;
				mOverlaps.RemoveAt(index);
				mTIs.RemoveAt(index);
				mMoveXs.RemoveAt(index);
				mMoveYs.RemoveAt(index);
				mNormalXs.RemoveAt(index);
				mNormalYs.RemoveAt(index);
				mTouchXs.RemoveAt(index);
				mTouchYs.RemoveAt(index);
				mX1s.RemoveAt(index);
				mY1s.RemoveAt(index);
				mW1s.RemoveAt(index);
				mH1s.RemoveAt(index);
				mX2s.RemoveAt(index);
				mY2s.RemoveAt(index);
				mW2s.RemoveAt(index);
				mH2s.RemoveAt(index);
				mItems.RemoveAt(index);
				mOthers.RemoveAt(index);
				mTypes.RemoveAt(index);
			}
		}

		public virtual int Size()
		{
			return mSize;
		}

		public virtual bool IsEmpty()
		{
			return mSize == 0;
		}

		public virtual void Clear()
		{
			mSize = 0;
			mOverlaps.Clear();
			mTIs.Clear();
			mMoveXs.Clear();
			mMoveYs.Clear();
			mNormalXs.Clear();
			mNormalYs.Clear();
			mTouchXs.Clear();
			mTouchYs.Clear();
			mX1s.Clear();
			mY1s.Clear();
			mW1s.Clear();
			mH1s.Clear();
			mX2s.Clear();
			mY2s.Clear();
			mW2s.Clear();
			mH2s.Clear();
			mItems.Clear();
			mOthers.Clear();
			mTypes.Clear();
		}

		private readonly List<int> order = new List<int>();
		private readonly Dictionary<int, int> swapMap = new Dictionary<int, int>();
		public virtual void KeySort<T>(IList<int> indices, IList<T> list)
		{
			swapMap.Clear();
			for (int i = 0; i < indices.Count; i++)
			{
				int k = indices[i];
				while (swapMap.ContainsKey(k))
				{
					int findKey;
					if(swapMap.TryGetValue(k, out findKey))
					{
						k = findKey;
					}
					else
					{
						k = 0;
					}
				}

				swapMap.Add(i, k);
			}

			foreach (KeyValuePair<int, int> e in swapMap)
			{
				Extra.Swap(list, e.Key, e.Value);
			}
		}

		public virtual void Sort()
		{
			order.Clear();
			for (int i = 0; i < mSize; i++)
			{
				order.Add(i);
			}

			order.Sort(this);
			KeySort(order, mOverlaps);
			KeySort(order, mTIs);
			KeySort(order, mMoveXs);
			KeySort(order, mMoveYs);
			KeySort(order, mNormalXs);
			KeySort(order, mNormalYs);
			KeySort(order, mTouchXs);
			KeySort(order, mTouchYs);
			KeySort(order, mX1s);
			KeySort(order, mY1s);
			KeySort(order, mW1s);
			KeySort(order, mH1s);
			KeySort(order, mX2s);
			KeySort(order, mY2s);
			KeySort(order, mW2s);
			KeySort(order, mH2s);
			KeySort(order, mItems);
			KeySort(order, mOthers);
			KeySort(order, mTypes);
		}

		public int Compare(int a, int b)
		{
			if (mTIs[a] == (mTIs[b]))
			{
				float ad = Rect2f.Rect_getSquareDistance(mX1s[a], mY1s[a], mW1s[a], mH1s[a], mX2s[a], mY2s[a], mW2s[a], mH2s[a]);
				float bd = Rect2f.Rect_getSquareDistance(mX1s[a], mY1s[a], mW1s[a], mH1s[a], mX2s[b], mY2s[b], mW2s[b], mH2s[b]);
				return ad.CompareTo(bd);
			}

			return mTIs[a].CompareTo(mTIs[b]);
		}
	}
}