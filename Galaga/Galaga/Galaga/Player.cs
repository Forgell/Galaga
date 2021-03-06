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
        Rectangle hitbox, sheetRec, window;
        Vector2 origin;
        float angle;
        List<Bullet> bullets;
        int lives, timer, invTimer;
        SoundEffect laser;
        KeyboardState oldKb;

        public Player(Texture2D t, Rectangle rec, SoundEffect laser,  Rectangle window)
        {
            tex = t;
            hitbox = rec;
            sheetRec = new Rectangle(0, 0, 32, 32);
            origin = new Vector2(sheetRec.Width / 2, sheetRec.Height / 2);
            angle = 0;
            bullets = new List<Bullet>();
            lives = 2;
            timer = 0;
            invTimer = 0;
            this.laser = laser;
            oldKb = Keyboard.GetState();
            this.window = window;
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public int Timer
        {
            get { return timer; }
        }

        public int Lives
        {
            get { return lives; }
        }

        public bool IsInvincible
        {
            get { return invTimer > 0; }
        }

        public void RemoveBulletAt(int index)
        {
            bullets.RemoveAt(index);
        }

        public void RemoveLife()
        {
            lives--;
            timer = 300;
        }

        public void AddLife()
        {
            lives++;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (timer == 0)
            {
                if (kb.IsKeyDown(Keys.Right) && hitbox.X < window.Width - hitbox.Width)
                    hitbox.X += 2;
                if (kb.IsKeyDown(Keys.Left) && hitbox.X >= 0)
                    hitbox.X -= 2;
                if (kb.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space) && bullets.Count < 2)
                {
                    bullets.Add(new Bullet(tex, new Rectangle(hitbox.X + 12, hitbox.Y - 16, 8, 16)));
                    laser.Play();
                }
                if (invTimer > 0)
                    invTimer--;
            }
            else
            {
                if (timer == 1)
                {
                    hitbox.X = window.Width / 2 - (int)origin.X;
                    invTimer = 120;
                }
                else
                    hitbox.X = -96;
                timer--;
                if (kb.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space) && timer < 240)
                {
                    timer = 1;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Update(gameTime);
            oldKb = kb;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (timer == 0 && invTimer / 4 % 2 == 0)
                spriteBatch.Draw(tex, new Vector2(hitbox.X + origin.X, hitbox.Y + origin.Y), sheetRec, Color.White, angle, origin, 1, SpriteEffects.None, 0);
            for (int i = 0; i < bullets.Count; i++)
                bullets[i].Draw(spriteBatch, gameTime);
            for (int i = 0; i < lives; i++)
                spriteBatch.Draw(tex, new Rectangle(i * 32 - 2, window.Height - 32, 32, 32), sheetRec, Color.White);
        }
    }
}
