﻿using System;
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
    class Player
    {
        Texture2D tex;
        Rectangle hitbox, sheetRec;
        Vector2 origin;
        float angle;
        List<Bullet> bullets;
        KeyboardState oldKb;

        public Player(Texture2D t, Rectangle rec)
        {
            tex = t;
            hitbox = rec;
            sheetRec = new Rectangle(0, 0, 16, 16);
            origin = new Vector2(sheetRec.Width / 2, sheetRec.Height / 2);
            angle = 0;
            bullets = new List<Bullet>();
            oldKb = Keyboard.GetState();
        }

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public void RemoveBulletAt(int index)
        {
            bullets.RemoveAt(index);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Right) && hitbox.X <= 128)
                hitbox.X++;
            if (kb.IsKeyDown(Keys.Left) && hitbox.X >= 0)
                hitbox.X--;
            if (kb.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space) && bullets.Count < 2)
                bullets.Add(new Bullet(tex, new Rectangle(hitbox.X + 6, hitbox.Y - 8, 4, 8)));
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Update(gameTime);
            oldKb = kb;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, new Vector2(hitbox.X + 8, hitbox.Y + 8), sheetRec, Color.White, angle, origin, 1, SpriteEffects.None, 0);
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Draw(spriteBatch, gameTime);
        }
    }
}