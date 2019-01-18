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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Stars field;
        Texture2D tex;
        Rectangle window;
        string[] lvl1Data;
        Enemy[][] enemies;
        Player p1;
        int score;
        SpriteFont font1;
        // Qualans Code
        bool isGameOn;
        TitleScreen titleScreen;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 288;
            graphics.PreferredBackBufferHeight = 416;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            field = new Stars(GraphicsDevice);
			Console.Write(GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height);
            tex = Content.Load<Texture2D>("GalagaSprites");
            window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //Needs to be optimized - Nick
            //using (var stream = TitleContainer.OpenStream("Level1.txt"))
            //{
            //    using (var reader = new StreamReader(stream))
            //    {
            //        lvl1Data = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //    }
            //}
            lvl1Data = new string[] { "  3333", "22222222", "22222222", "11111111", "11111111" };

            enemies = new Enemy[lvl1Data.Length][];
            for (int r = 0; r < enemies.Length; r++)
            {
                enemies[r] = new Enemy[lvl1Data[r].Length];
                for (int c = 0; c < enemies[r].Length; c++)
                {
                    if (lvl1Data[r].Substring(c, 1).Equals(" "))
                        enemies[r][c] = null;
                    else
                        enemies[r][c] = new Enemy(tex, new Rectangle(c * 32, (r + 1) * 32, 32, 32), int.Parse(lvl1Data[r].Substring(c, 1)));
                }
            }

            p1 = new Player(tex, new Rectangle(128, 384, 32, 32));
            score = 0;

            // Qualans Code
            isGameOn = false;
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            SpriteFont qFont = Content.Load<SpriteFont>("QualansFont");
            Texture2D bannderText = Content.Load<Texture2D>("banner");
            titleScreen = new TitleScreen( bannderText, qFont , window.Width , window.Height, GraphicsDevice);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            field.Update(gameTime);
            // TODO: Add your update logic here
            // Qualans code
            if (isGameOn)
            {
                // other peaples code
                List<Bullet> bullets = p1.Bullets;
                for (int r = 0; r < enemies.Length; r++)
                    for (int c = 0; c < enemies[r].Length; c++)
                        if (enemies[r][c] != null)
                        {
                            enemies[r][c].Update(gameTime);
                            for (int i = bullets.Count - 1; i > -1; i--)
                                if (enemies[r][c].Intersects(bullets[i]))
                                {
                                    if (enemies[r][c].Level != 4)
                                    {
                                        score += enemies[r][c].Level * 50;
                                        enemies[r][c] = null;
                                    }
                                    p1.RemoveBulletAt(i);
                                    break;
                                }
                        }
                for (int i = bullets.Count - 1; i > -1; i--)
                    if (!bullets[i].Hitbox.Intersects(window))
                        p1.RemoveBulletAt(i);
                p1.Update(gameTime);
            }else
            {
                // Qualans Code
                // default to title screen
                
                isGameOn = titleScreen.update(Mouse.GetState().X, Mouse.GetState().Y, Mouse.GetState().LeftButton == ButtonState.Pressed, gameTime);
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            if (isGameOn)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                field.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font1, score + "", new Vector2(0, 0), Color.White);
                for (int r = 0; r < enemies.Length; r++)
                    for (int c = 0; c < enemies[r].Length; c++)
                        if (enemies[r][c] != null)
                            enemies[r][c].Draw(spriteBatch, gameTime);
                p1.Draw(spriteBatch, gameTime);
                spriteBatch.End();
            }else
            {
                GraphicsDevice.Clear(Color.Black);
                titleScreen.draw(spriteBatch , gameTime);
            }
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
