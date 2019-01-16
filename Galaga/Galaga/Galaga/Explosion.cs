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
    class Explosion
    {
        Texture2D tex;
        Rectangle rec, sheetPos;
        int timer;

        public Explosion(Texture2D t, Rectangle pos, SoundEffect explosion)
        {
            tex = t;
            rec = new Rectangle(pos.X - 16, pos.Y - 16, 64, 64);
            sheetPos = new Rectangle(0, 128, 64, 64);
            timer = 0;
            explosion.Play();
        }

        public int Timer
        {
            get { return timer; }
        }

        public void Update(GameTime gameTime)
        {
            sheetPos.X = timer / 8 * 64;
            timer++;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, rec, sheetPos, Color.White);
        }
    }
}
