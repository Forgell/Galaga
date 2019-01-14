using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
	public class Stars
	{
		Star[] stars;
		Random r;
		int x;
		Rectangle screen;
		GraphicsDevice graphics;
		public class Star
		{
			private Rectangle starRect;
			private Texture2D starTex;
			private Color starColor;
			private Vector2 starVel, starPos;
			public static Texture2D defaultTex;
			public Star()
			{
				starRect = Rectangle.Empty;
				starTex = defaultTex;
				starColor = Color.White;
				starPos = Vector2.Zero;
				starVel = Vector2.Zero;
			}
			public Star(Rectangle sr, Texture2D st, Color sc, Vector2 sp, Vector2 sv)
			{
				starRect = sr;
				starTex = st;
				starColor = sc;
				starPos = sp;
				starVel = sv;
			}
			public Star(Rectangle sr, Color sc, Vector2 sp, Vector2 sv)
			{
				starRect = sr;
				starTex = defaultTex;
				starColor = sc;
				starPos = sp;
				starVel = sv;
			}
			public Rectangle Rect
			{
				get
				{
					return starRect;
				}
				set
				{
					this.starRect = value;
				}
			}
			public Texture2D Tex
			{
				get
				{
					return starTex;
				}
				set
				{
					this.starTex = value;
				}
			}
			public Color Col
			{
				get
				{
					return starColor;
				}
				set
				{
					this.starColor = value;
				}
			}
			public Vector2 Pos
			{
				get
				{
					return starPos;
				}
				set
				{
					this.starPos = value;
				}
			}
			public Vector2 Vel
			{
				get
				{
					return starVel;
				}
				set
				{
					this.starVel = value;
				}
			}
			public void move()
			{
				starPos.X += (int)starVel.X;
				starPos.Y += (int)starVel.Y;
			}
		}
		public Stars(GraphicsDevice graphics)
		{
			this.graphics = graphics;
			Texture2D star = new Texture2D(graphics, 1, 1);
			Color[] wht = { Color.White };
			star.SetData(wht);
			Star.defaultTex = star;
			screen = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
			r = new Random();
			x = 0;
			stars = new Star[graphics.Viewport.Width*graphics.Viewport.Height/2560];
		}
		public void Update(GameTime gameTime)
		{
			if (x < stars.Length)
			{
				for (int i = 0; i < 5 && i + x < stars.Length; i++)
					stars[x + i] = newStar();
				x += 5;
			}
			for (int i = 0; i < stars.Length; i++)
			{
				try
				{
					stars[i].move();
				}
				catch (NullReferenceException)
				{
					break;
				}
				if (!screen.Contains(new Point((int)stars[i].Pos.X, (int)stars[i].Pos.Y)))
				{
					stars[i] = newStar();
				}
			}
		}
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			graphics.Clear(Color.Black);
			for (int i = 0; i < stars.Length; i++)
			{
				try
				{
					spriteBatch.Draw(stars[i].Tex, stars[i].Pos, stars[i].Rect, stars[i].Col);
				}
				catch (ArgumentNullException)
				{
					stars[i].Tex = Star.defaultTex;
				}
				catch (NullReferenceException)
				{
					break;
				}
			}
		}
		protected Star newStar()
		{
			return new Star(new Rectangle(0, 0, 2, 2), new Color(r.Next(255), r.Next(255), r.Next(255), r.Next(255)), new Vector2(r.Next(screen.Width + 1), 0), new Vector2(0, r.Next(graphics.Viewport.Height / 40) + graphics.Viewport.Height / 96));
		}
	}
}
