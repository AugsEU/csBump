using csBump;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TracyWrapper;

namespace MGTest
{
	public class Main : Game
	{
		const string TILES = "                              XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                              XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "  XXX         X X             XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "    X           X             XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "    X  XXX  XX  X             XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                              XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                 XXXX         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                 X  XXXXX     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                 X    XXX     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "XXXXXXXX         XX   X X     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                      XXXXX   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                      XXXXX   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                      XXXXX   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
							 "                      XXXXX   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n";

		static Texture2D mBlankTexture;

		private GraphicsDeviceManager mGraphics;
		private SpriteBatch mSpriteBatch;

		World mWorld;

		MovingRect mPlayer;
		MovingRect mOtherPlayer;
		List<JitterRect> mJitterRects;
		TileMap mTileMap;

		public Main()
		{
			mGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Profiler.InitThread();

			mBlankTexture = new Texture2D(mGraphics.GraphicsDevice, 1, 1);
			mBlankTexture.SetData<Color>(new Color[] { Color.White} );

			mWorld = new World();

			mPlayer = new MovingRect(mWorld, new Vector2(10.0f, 10.0f), new Vector2(16.0f, 16.0f));
			mOtherPlayer = new MovingRect(mWorld, new Vector2(30.0f, 10.0f), new Vector2(16.0f, 16.0f));
			mOtherPlayer.mUpKey = Keys.Up;
			mOtherPlayer.mDownKey = Keys.Down;
			mOtherPlayer.mLeftKey = Keys.Left;
			mOtherPlayer.mRightKey = Keys.Right;

			mTileMap = new TileMap(mWorld, new Point(16, 16), TILES);

			mJitterRects = new List<JitterRect>();
			SpawnJitterRects(new Vector2(16.0f, 200.0f), new Vector2(200.0f, 400.0f), 18.0f);


			base.Initialize();
		}

		void SpawnJitterRects(Vector2 start, Vector2 end, float spacing)
		{
			Vector2 rectSize = new Vector2(spacing, spacing) * 0.7f;

			for(float x = start.X; x <= end.X; x+= spacing)
			{
				for (float y = start.Y; y <= end.Y; y += spacing)
				{
					JitterRect newRect = new JitterRect(mWorld, new Vector2(x, y), rectSize);
					mJitterRects.Add(newRect);
				}
			}
		}

		protected override void LoadContent()
		{
			mSpriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			Profiler.PushProfileZone("Update Loop");
			mPlayer.Update(gameTime, mWorld);
			mOtherPlayer.Update(gameTime, mWorld);

			Profiler.PushProfileZone("Jitter Loop");
			foreach (JitterRect rect in mJitterRects)
			{
				rect.Update(gameTime, mWorld);
			}
			Profiler.PopProfileZone();

			base.Update(gameTime);
			Profiler.PopProfileZone();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			mSpriteBatch.Begin();

			mPlayer.Draw(mSpriteBatch);
			mOtherPlayer.Draw(mSpriteBatch);

			mTileMap.Draw(mSpriteBatch, mWorld);

			Profiler.PushProfileZone("Jitter Draw");
			foreach (JitterRect rect in mJitterRects)
			{
				rect.Draw(mSpriteBatch);
			}
			Profiler.PopProfileZone();

			mSpriteBatch.End();

			base.Draw(gameTime);

			Profiler.HeartBeat();
		}

		public static void DrawRect(SpriteBatch sb, Vector2 pos, Vector2 size, Color color)
		{
			Rectangle intRect = new Rectangle((int)MathF.Round(pos.X), (int)MathF.Round(pos.Y), (int)size.X, (int)size.Y);

			sb.Draw(mBlankTexture, intRect, color);
		}
	}
}
