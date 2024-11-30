using csBump;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGTest
{
	public class Main : Game
	{
		const string TILES = "                              \n" +
							 "                              \n" +
							 "  XXX         X X             \n" +
							 "    X           X             \n" +
							 "    X  XXX  XX  X             \n" +
							 "                              \n" +
							 "                 XXXX         \n" +
							 "                 X  XXXXX     \n" +
							 "                 X    XXX     \n" +
							 "XXXXXXXX         XX   X X     \n" +
							 "                      XXXXX   \n";

		static Texture2D mBlankTexture;

		private GraphicsDeviceManager mGraphics;
		private SpriteBatch mSpriteBatch;

		World mWorld;

		MovingRect mPlayer;
		MovingRect mOtherPlayer;
		TileMap mTileMap;

		public Main()
		{
			mGraphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
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

			base.Initialize();
		}

		protected override void LoadContent()
		{
			mSpriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			mPlayer.Update(gameTime, mWorld);
			mOtherPlayer.Update(gameTime, mWorld);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			mSpriteBatch.Begin();

			mPlayer.Draw(mSpriteBatch);
			mOtherPlayer.Draw(mSpriteBatch);

			mTileMap.Draw(mSpriteBatch, mWorld);

			mSpriteBatch.End();

			base.Draw(gameTime);
		}

		public static void DrawRect(SpriteBatch sb, Vector2 pos, Vector2 size, Color color)
		{
			Rectangle intRect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

			sb.Draw(mBlankTexture, intRect, color);
		}
	}
}
