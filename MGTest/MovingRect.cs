using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using csBump;
using static csBump.IResponse;

using TracyWrapper;

namespace MGTest
{
	internal class MovingRect
	{
		const float SPEED = 100.2f;

		Vector2 mPosition;
		Vector2 mSize;

		public Item mBumpItem;
		public Color mColor = Color.Gray;

		public Keys mUpKey = Keys.W;
		public Keys mDownKey = Keys.S;
		public Keys mLeftKey = Keys.A;
		public Keys mRightKey = Keys.D;

		public MovingRect(World bumpWorld, Vector2 position, Vector2 size)
		{
			mPosition = position;
			mSize = size;

			mBumpItem = new Item();
			bumpWorld.Add(mBumpItem, mPosition.X, mPosition.Y, mSize.X, mSize.Y);
		}

		public void Update(GameTime gameTime, World bumpWorld)
		{
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 desirePos = mPosition;
			KeyboardState keyState = Keyboard.GetState();

			float effSpeed = keyState.IsKeyDown(Keys.LeftShift) ? SPEED * 400.0f : SPEED;

			if(keyState.IsKeyDown(mUpKey))
			{
				desirePos += new Vector2(0.0f, -1.0f) * effSpeed * dt;
			}
			if (keyState.IsKeyDown(mDownKey))
			{
				desirePos += new Vector2(0.0f, 1.0f) * effSpeed * dt;
			}
			if (keyState.IsKeyDown(mLeftKey))
			{
				desirePos += new Vector2(-1.0f, 0.0f) * effSpeed * dt;
			}
			if (keyState.IsKeyDown(mRightKey))
			{
				desirePos += new Vector2(1.0f, 0.0f) * effSpeed * dt;
			}

			Profiler.PushProfileZone("Rect Move");
			CollisionResult result = bumpWorld.Move(mBumpItem, desirePos, new DefaultFilter());
			Profiler.PopProfileZone();

			Rect2f rect = bumpWorld.GetRect(mBumpItem);
			mPosition.X = rect.X;
			mPosition.Y = rect.Y;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Main.DrawRect(spriteBatch, mPosition, mSize, mColor);
		}
	}
}
