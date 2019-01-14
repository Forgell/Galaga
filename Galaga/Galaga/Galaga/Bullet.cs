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
    class Bullet
    {
        Texture2D tex;
        Rectangle hitbox, sheetRec;

        public Bullet(Texture2D t, Rectangle rec)
        {
            tex = t;
            hitbox = rec;
            sheetRec = new Rectangle(0, 64, 16, 32);
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public void Update(GameTime gameTime)
        {
            hitbox.Y -= 8;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(tex, hitbox, sheetRec, Color.White);
        }
    }
}
