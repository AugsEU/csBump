using csBump;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TracyWrapper;

namespace MGTest
{
	internal class TileMap
	{
		Point mTileSize;
		BumpID[,] mSolidTiles;

		public TileMap(World world, Point tileSize, string tiles)
		{
			mTileSize = tileSize;

			string[] lines = tiles.Split('\n');

			mSolidTiles = new BumpID[lines[0].Length, lines.Length];

			for(int y = 0; y < lines.Length; y++)
			{
				string line = lines[y];
				for(int x = 0; x < line.Length; x++)
				{
					bool isSolid = line[x] != ' ';
					if (isSolid)
					{
						mSolidTiles[x, y] = new BumpID();
						world.Add(mSolidTiles[x, y], x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y);
					}
				}
			}
		}

		public void Draw(SpriteBatch sb, World world)
		{
			Profiler.PushProfileZone("Draw tilemap");
			for (int x = 0; x < mSolidTiles.GetLength(0); x++)
			{
				for (int y = 0; y < mSolidTiles.GetLength(1); y++)
				{
					if (mSolidTiles[x, y] is null)
					{
						continue;
					}
					Rect2f rect = world.GetRect(mSolidTiles[x, y]);
					Main.DrawRect(sb, new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height), Color.DarkRed);
				}
			}
			Profiler.PopProfileZone();
		}
	}
}
