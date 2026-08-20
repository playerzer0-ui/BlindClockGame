using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NodeTesting.models;
using System;

namespace BlindClockGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int bgState = 2;
        private float opacity = 0f;
        private CollisionRect blackout;

        private float fadeTimer = 3f;
        private float fadeTimerMax = 3f;
        private Random rand;

        private MouseState previousMouseState;
        Canvas canvas;
        Button button;
        Timer timer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Window.AllowUserResizing = true;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Content = Content;
            Globals.spriteBatch = _spriteBatch;
            Globals.graphics = _graphics;

            // TODO: use this.Content to load your game content here
            canvas = new Canvas(_graphics.GraphicsDevice, Window, 1280, 720);
            button = new Button();
            timer = new Timer();
            blackout = new CollisionRect(640, 360, 1280, 720);
            rand = new Random();

            fadeTimerMax = (float)rand.NextDouble() * 3 + 2; // Random value between 2 and 5
            fadeTimer = fadeTimerMax;

            button.SetText("START");
            timer.Randomize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            MouseState currentMouseState = Mouse.GetState();
            Point clickPoint = Point.Zero;

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 canvasPos = canvas.ScreenToCanvas(new Vector2(currentMouseState.X, currentMouseState.Y));
                clickPoint = new Point((int)canvasPos.X, (int)canvasPos.Y);

                if (button.Hitbox.Contains(clickPoint)) 
                {
                    switch (Globals.state)
                    {
                        case 0:
                            Globals.state = 1;
                            button.SetText("STOP");
                            break;
                        case 1:
                            Globals.state = 2;
                            bgState = timer.CheckTime();
                            button.SetText("RESET");
                            break;
                        case 2:
                            Globals.state = 0;
                            bgState = 2;
                            opacity = 0f;
                            fadeTimerMax = (float)rand.NextDouble() * 3 + 2; // Random value between 2 and 5
                            fadeTimer = fadeTimerMax;
                            timer.Randomize();
                            button.SetText("START");
                            break;
                    }
                }
            }

            if(Globals.state == 1)
            {
                timer.Update(gameTime);
                if(opacity <= 1f)
                {
                    fadeTimer -= deltaTime;
                    if(fadeTimer < 0) {
                        opacity += 0.05f;
                    }
                }
            }


            previousMouseState = currentMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            canvas.Activate();
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            switch(bgState)
            {
                case -1: //too late
                    GraphicsDevice.Clear(PicoPallete.light_grey);
                    break;
                case 0: //too early
                    GraphicsDevice.Clear(PicoPallete.dark_grey);
                    break;
                case 1: //perfect
                    GraphicsDevice.Clear(PicoPallete.green);
                    break;
                case 2: //default
                    GraphicsDevice.Clear(PicoPallete.blue);
                    break;
            }
            timer.Draw();
            if (Globals.state == 1)
            {
                blackout.Draw(new Color(0, 0, 0, opacity));
            }
            button.Draw();
            _spriteBatch.End();
            canvas.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
