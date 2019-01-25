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
    class Enemy
    {
        Texture2D tex;
        Rectangle hitbox, sheetRec, window;
        Vector2 origin, pos, fPos, velocity;
        float angle;
        Bullet bullet;
        int timer, lvl, fXV;
        bool moving;

        public Enemy(Texture2D t, Vector2 finalPos, int level, Rectangle window)
        {
            tex = t;
            fPos = finalPos;
            pos = new Vector2(-3200, 0);
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 32, 32);
            sheetRec = new Rectangle((level + 1) * 32, 0, 32, 32);
            origin = new Vector2(sheetRec.Width / 2, sheetRec.Height / 2);
            velocity = new Vector2();
            angle = 0;
            bullet = null;
            timer = 0;
            lvl = level;
            fXV = 1;
            moving = false;
            this.window = window;
        }

        public int Level
        {
            get { return lvl; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Bullet Bullet
        {
            get { return bullet; }
        }

        public bool IsMoving
        {
            get { return moving; }
        }

        public bool Intersects(Bullet bullet)
        {
            if (hitbox.Intersects(bullet.Hitbox))
            {
                if (lvl == 3)
                {
                    lvl = 4;
                    sheetRec.X += 32;
                }
                else if (lvl == 4)
                    lvl = 3;
                return true;
            }
            return false;
        }

        public void RemoveBullet()
        {
            bullet = null;
        }

        public void EnterScreenAt(int posNum)
        {
            switch (posNum)
            {
                case 0:
                    pos = new Vector2(-32, 3 * window.Height / 4);
                    break;
                case 1:
                    pos = new Vector2(-32, 2 * window.Height / 4);
                    break;
                case 2:
                    pos = new Vector2(-32, window.Height / 4);
                    break;
                case 3:
                    pos = new Vector2(-32, -32);
                    break;
                case 4:
                    pos = new Vector2(window.Width, -32);
                    break;
                case 5:
                    pos = new Vector2(window.Width, window.Height / 4);
                    break;
                case 6:
                    pos = new Vector2(window.Width, 2 * window.Height / 4);
                    break;
                case 7:
                    pos = new Vector2(window.Width, 3 * window.Height / 4);
                    break;
            }
            velocity = new Vector2(fPos.X - pos.X, fPos.Y - pos.Y);
            velocity.Normalize();
            FindAngle();
        }

        public void Move()
        {
            moving = true;
            velocity = new Vector2((float)new Random().NextDouble() * 3 - 1.5f, 2);
            FindAngle();
        }

        private void FindAngle()
        {
            if (velocity.X == 0)
                angle = (float)Math.PI;
            else
            {
                angle = (float)Math.Atan(velocity.Y / velocity.X);
                if (velocity.X > 0)
                    angle += (float)Math.PI / 2;
                else
                    angle -= (float)Math.PI / 2;
            }
        }

        public void Update(GameTime gameTime)
        {
            timer = (timer + 1) % 32;
            if (timer == 0)
            {
                sheetRec.Y = 0;
                fXV *= -1;
            }
            else if (timer == 16 )
                sheetRec.Y = 32;
            fPos.X += fXV;

            if (!moving)
            {
                velocity = new Vector2(fPos.X - pos.X, fPos.Y - pos.Y);
                if (Math.Abs(velocity.X) <= 2 && Math.Abs(velocity.Y) <= 2)
                    angle = 0;
                velocity.Normalize();
            }
            else if (moving && !hitbox.Intersects(window))
            {
                pos.X = new Random().Next(window.Width);
                pos.Y = -64;
                moving = false;
                velocity = new Vector2(fPos.X - pos.X, fPos.Y - pos.Y);
                velocity.Normalize();
                FindAngle();
            }

            pos.X += velocity.X;
            pos.Y += velocity.Y;
            hitbox.X = (int)pos.X;
            hitbox.Y = (int)pos.Y;
            if (bullet != null)
                bullet.Update(gameTime);
            else if (moving && new Random().Next(300 / lvl) == 1)
                bullet = new Bullet(tex, new Rectangle(hitbox.X + 12, hitbox.Y + 32, 8, 16), true);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, new Vector2(hitbox.X + origin.X, hitbox.Y + origin.Y), sheetRec, Color.White, angle, origin, 1, SpriteEffects.None, 0);
            if (bullet != null)
                bullet.Draw(spriteBatch, gameTime);
        }
    }
}
