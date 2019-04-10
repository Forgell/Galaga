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
        Rectangle rec, sheetRec;
        int timer;

        //Default enemy explosion
        public Explosion(Texture2D t, Rectangle pos, SoundEffect explosion)
        {
            tex = t;
            rec = new Rectangle(pos.X - 16, pos.Y - 16, 64, 64);
            sheetRec = new Rectangle(0, 128, 64, 64);
            timer = 0;
            explosion.Play();
        }

        //Player explosion
        public Explosion(Texture2D t, Rectangle pos, SoundEffect explosion, bool player)
        {
            tex = t;
            rec = new Rectangle(pos.X - 16, pos.Y - 16, 64, 64);
            if (player)
                sheetRec = new Rectangle(0, 64, 64, 64);
            else
                sheetRec = new Rectangle(0, 128, 64, 64);
            timer = 0;
            explosion.Play();
        }

        public int Timer
        {
            get { return timer; }
        }

        public void Update(GameTime gameTime)
        {
            sheetRec.X = timer / 8 * 64;
            timer++;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, rec, sheetRec, Color.White);
        }
    }
}
