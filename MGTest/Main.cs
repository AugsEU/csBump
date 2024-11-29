using csBump;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGTest
{
	public class Main : Game
	{
		static Texture2D mBlankTexture;

		private GraphicsDeviceManager mGraphics;
		private SpriteBatch mSpriteBatch;

		World mWorld;

		MovingRect mPlayer;
		MovingRect mOtherPlayer;

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
