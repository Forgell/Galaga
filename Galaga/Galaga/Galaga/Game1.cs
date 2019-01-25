using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        string[] lvlData;
        Enemy[][] enemies;
        Player p1;
        int score, timer, extraLifeScore;
        SoundEffect explosion, playerExplosion;
        List<Explosion> explosions;
        SpriteFont font1;
        string centerText;
        int levelNum;
        Rectangle[] levelRecs;
        int[] levelValues;
        Random ran;
        bool gameOver;
        Button replay;

        // Qualans Code
        TitleScreen titleScreen;
        bool isGameOn;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 352;
            graphics.PreferredBackBufferHeight = 480;
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
            ran = new Random();
            levelNum = 1;
            levelRecs = new Rectangle[] { new Rectangle(288, 192, 32, 32), new Rectangle(320, 192, 32, 32), new Rectangle(352, 192, 32, 32),
                new Rectangle(288, 224, 32, 32), new Rectangle(320, 224, 32, 32), new Rectangle(352, 224, 32, 32) };
            levelValues = new int[] { 1, 5, 10, 20, 30, 50 };
            ReadLevelData();

            p1 = new Player(tex, new Rectangle(window.Width / 2 - 16, window.Height - 96, 32, 32), window);
            score = 0;
            extraLifeScore = 10000;
            centerText = "";

            gameOver = false;
            replay = null;

            explosions = new List<Explosion>();
            isGameOn = false;
            IsMouseVisible = true;
            // test
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
            explosion = Content.Load<SoundEffect>("galaga_destroyed");
            playerExplosion = Content.Load<SoundEffect>("fighter_destroyed");
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            Texture2D bannder = Content.Load<Texture2D>("banner");

            titleScreen = new TitleScreen( bannder, font1 , window.Width , window.Height , GraphicsDevice);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            field.Update(gameTime);
            // TODO: Add your update logic here
            if (isGameOn)
            {
                if (timer != 0)
                {
                    centerText = "LEVEL " + levelNum;
                    if (timer % 30 == 0)
                        for (int c = 0; c < enemies[timer/ 30 - 1].Length; c++)
                            if (enemies[timer / 30 - 1][c] != null)
                                enemies[timer / 30 - 1][c].EnterScreenAt(c);
                    timer--;
                }
                List<Bullet> bullets = p1.Bullets;
                for (int r = 0; r < enemies.Length; r++)
                    for (int c = 0; c < enemies[r].Length; c++)
                        if (enemies[r][c] != null)
                        {
                            enemies[r][c].Update(gameTime);
                            if (enemies[r][c].Bullet != null)
                            {
                                if (p1.Hitbox.Intersects(enemies[r][c].Bullet.Hitbox))
                                {
                                    if (!p1.IsInvincible)
                                    {
                                        p1.RemoveLife();
                                        explosions.Add(new Explosion(tex, p1.Hitbox, playerExplosion, true));
                                    }
                                    enemies[r][c].RemoveBullet();
                                }
                                else if (!enemies[r][c].Bullet.Hitbox.Intersects(window))
                                    enemies[r][c].RemoveBullet();
                            }
                            if (p1.Hitbox.Intersects(enemies[r][c].Hitbox))
                            {
                                explosions.Add(new Explosion(tex, enemies[r][c].Hitbox, explosion));
                                enemies[r][c] = null;
                                if (!p1.IsInvincible)
                                {
                                    p1.RemoveLife();
                                    explosions.Add(new Explosion(tex, p1.Hitbox, playerExplosion, true));
                                }
                                break;
                            }
                            for (int i = bullets.Count - 1; i > -1; i--)
                                if (enemies[r][c].Intersects(bullets[i]))
                                {
                                    if (enemies[r][c].Level != 4)
                                    {
                                        score += enemies[r][c].Level * 50 + (levelNum - 1) * 5;
                                        explosions.Add(new Explosion(tex, enemies[r][c].Hitbox, explosion));
                                        enemies[r][c] = null;
                                    }
                                    p1.RemoveBulletAt(i);
                                    break;
                                }
                        }
                for (int i = bullets.Count - 1; i > -1; i--)
                    if (!bullets[i].Hitbox.Intersects(window))
                        p1.RemoveBulletAt(i);
                if (ran.Next(150) < levelNum)
                {
                    int r = ran.Next(enemies.Length);
                    int c = ran.Next(enemies[r].Length);
                    if (enemies[r][c] != null)
                        enemies[r][c].Move();
                }
                for (int i = explosions.Count - 1; i > -1; i--)
                {
                    explosions[i].Update(gameTime);
                    if (explosions[i].Timer > 40)
                        explosions.RemoveAt(i);
                }
                if (!gameOver)
                {
                    p1.Update(gameTime);
                    if (p1.Timer != 0)
                        centerText = "READY?";
                    else if (timer == 0)
                        centerText = "";
                    if (p1.Lives < 0)
                    {
                        gameOver = true;
                        replay = new Button("Main Menu", new Vector2(window.Width / 2 - 60, window.Height / 2 + 64), font1);
                        for (int i = p1.Bullets.Count - 1; i > -1; i--)
                            p1.RemoveBulletAt(i);
                    }
                }
                else
                {
                    centerText = " GAME\n OVER";
                    if (replay.preesed(Mouse.GetState().X, Mouse.GetState().Y) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Initialize();
                    }
                }
                if (score >= extraLifeScore)
                {
                    p1.AddLife();
                    extraLifeScore += 10000;
                }

                if (LevelOver())
                {
                    levelNum++;
                    ReadLevelData();
                }
            }
            else
            {
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
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                field.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font1, score + "", new Vector2(0, 0), Color.White);
                for (int r = 0; r < enemies.Length; r++)
                    for (int c = 0; c < enemies[r].Length; c++)
                        if (enemies[r][c] != null)
                            enemies[r][c].Draw(spriteBatch, gameTime);
                p1.Draw(spriteBatch, gameTime);
                for (int i = 0; i < explosions.Count; i++)
                    explosions[i].Draw(spriteBatch, gameTime);
                spriteBatch.DrawString(font1, centerText, new Vector2(window.Width / 2 - 40, window.Height / 2 - 8), Color.Red);
                if (replay != null)
                    replay.Draw(spriteBatch);
                int levelNumCopy = levelNum;
                int x = 0;
                for (int i = levelRecs.Length - 1; i > -1; i--)
                    while (levelNumCopy >= levelValues[i])
                    {
                        levelNumCopy -= levelValues[i];
                        x++;
                    }
                levelNumCopy = levelNum;
                for (int i = levelRecs.Length - 1; i > -1; i--)
                    while (levelNumCopy >= levelValues[i])
                    {
                        levelNumCopy -= levelValues[i];
                        spriteBatch.Draw(tex, new Vector2(window.Width - x * 32, window.Height - 32), levelRecs[i], Color.White);
                        x--;
                    }
                spriteBatch.End();
                // TODO: Add your drawing code here
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                titleScreen.draw(spriteBatch , gameTime);
            }
            base.Draw(gameTime);
        }

        public void ReadLevelData()
        {
            timer = 150;
            var lines = File.ReadAllLines("Level" + levelNum % 5 + ".txt");
            lvlData = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                lvlData[i] = lines[i];
            enemies = new Enemy[lvlData.Length][];
            for (int r = 0; r < enemies.Length; r++)
            {
                enemies[r] = new Enemy[lvlData[r].Length];
                for (int c = 0; c < enemies[r].Length; c++)
                {
                    if (lvlData[r].Substring(c, 1).Equals(" "))
                        enemies[r][c] = null;
                    else
                        enemies[r][c] = new Enemy(tex, new Vector2(c * 32 + 32, (r + 1) * 32), int.Parse(lvlData[r].Substring(c, 1)), window);
                }
            }
        }

        public bool LevelOver()
        {
            for (int r = 0; r < enemies.Length; r++)
                for (int c = 0; c < enemies[r].Length; c++)
                    if (enemies[r][c] != null)
                        return false;
            return true;
        }
    }
}
