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
    class Enemy
    {
        Texture2D tex;
        Rectangle hitbox, sheetRec;
        Vector2 origin, pos, circleCenter;
        float angle;
        Bullet bullet;
        int timer, xV, lvl;
        bool moving;

        public Enemy(Texture2D t, Rectangle rec, int level)
        {
            tex = t;
            hitbox = rec;
            sheetRec = new Rectangle((level + 1) * 32, 0, 32, 32);
            origin = new Vector2(sheetRec.Width / 2, sheetRec.Height / 2);
            pos = new Vector2(hitbox.X, hitbox.Y);
            circleCenter = new Vector2();
            angle = 0;
            bullet = null;
            timer = 0;
            xV = 2;
            lvl = level;
            moving = false;
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

        public void removeBullet()
        {
            bullet = null;
        }

        public void Update(GameTime gameTime)
        {
            timer = (timer + 1) % 32;
            if (timer == 0)
            {
                sheetRec.Y = 0;
                xV *= -1;
            }
            else if (timer == 16)
                sheetRec.Y = 32;
            if (timer % 2 == 0)
                hitbox.X += xV;
            if (bullet != null)
                bullet.Update(gameTime);
            else if (moving && new Random().Next(300) == 1)
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
