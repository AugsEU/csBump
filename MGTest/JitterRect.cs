using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using csBump;
using static csBump.IResponse;

using TracyWrapper;
using System;

namespace MGTest
{
	internal class JitterRect
	{
		const float SPEED = 100.2f;

		static Random sRandom = new Random();

		Vector2 mPosition;
		Vector2 mBasePos;
		Vector2 mSize;

		public Item mBumpItem;
		public Color mColor = Color.Green;

		public JitterRect(World bumpWorld, Vector2 position, Vector2 size)
		{
			mPosition = position;
			mBasePos = position;
			mSize = size;

			mBumpItem = new Item();
			bumpWorld.Add(mBumpItem, mPosition.X, mPosition.Y, mSize.X, mSize.Y);
		}

		public void Update(GameTime gameTime, World bumpWorld)
		{
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Vector2 desirePos = mBasePos;
			desirePos += new Vector2((float)sRandom.NextDouble() * 2.0f - 1.0f, (float)sRandom.NextDouble() * 2.0f - 1.0f) * SPEED * dt;

			CollisionResult result = bumpWorld.Move(mBumpItem, desirePos, new DefaultFilter());

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
