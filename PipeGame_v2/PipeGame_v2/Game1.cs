using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace PipeGame_v2
{

    enum screenState
    {
        openScr,
        menuScr,
        levelScr,
        gameScr,
        compScr,
        randScr,
        howtoScr
    };


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState msNow;
        MouseState msPrev;

        SpriteFont buttonFont;
        List<Texture2D> textureLibrary = new List<Texture2D>();
        List<SoundEffect> soundLibrary = new List<SoundEffect>();
        List<Texture2D> animationLibrary = new List<Texture2D>();

        screenState activeScr;
        screenState prevScr;
        openScr OpeningScreen;
        menuScr MenuScreen;
        gameScr GameScreen;
        levelScr LevelScreen;
        compScr CompleteScreen;
        randomScr RandomScreen;
        howtoScr HowToScreen;
        int currentLevel;
        int rndWidth;
        int rndLength;

        DateTime levelStart = new DateTime();
        DateTime levelEnd = new DateTime();
        TimeSpan levelTime = new TimeSpan();
        
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            Window.Title = "Pipe Game";
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

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);
            activeScr = screenState.openScr;
            prevScr = screenState.openScr;
            currentLevel = 1;
            rndWidth = 0;
            rndLength = 0;

            buttonFont = Content.Load<SpriteFont>("butFont");

            //ANIMATION LOADING
            for (int i = 0; i < 80; i++)
            {
                animationLibrary.Add(Content.Load<Texture2D>("Textures/OpenAnimation/ani" + (i + 1)));
            }

            //SAVE FILE LOADING
            string _path = Environment.CurrentDirectory;
            for (int i = 0; i < 6; i++)
            {
                _path = Convert.ToString(Directory.GetParent(_path));
            }
            _path += @"\save.txt";
            if (!File.Exists(_path))
            {
                using (var writer = new StreamWriter(_path))
                {
                    writer.WriteLine(0);
                }
            }

            //TEXTURE LOADING
            textureLibrary.Add(Content.Load<Texture2D>("Textures/OpenScr/bcg"));//0
            textureLibrary.Add(Content.Load<Texture2D>("Textures/OpenScr/playButton1"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/OpenScr/playButton2"));//2
            textureLibrary.Add(Content.Load<Texture2D>("Textures/OpenScr/playButton3"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/bcg"));//4
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/levelsButton1"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/levelsButton2"));//6
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/levelsButton3"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe1"));//8
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe3"));//10
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe4"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe5"));//12
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe6"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe7"));//14
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Corner/corPipe8"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe1"));//16
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe3"));//18
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe4"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe5"));//20
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe6"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe7"));//22
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Cutoff/cutPipe8"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Straight/strPipe1"));//24
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Straight/strPipe2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Straight/strPipe3"));//26
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/Straight/strPipe4"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/levelScr/lvlButton1"));//28
            textureLibrary.Add(Content.Load<Texture2D>("Textures/levelScr/lvlButton2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/levelScr/lvlButton3"));//30
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/bcg"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/backButton1"));//32
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/backButton2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/backButton3"));//34
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/horBar"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/horSlider"));//36
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/verBar"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/verSlider"));//38
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/blob"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/RandomScr/canvas"));//40
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/howto"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/rndButton1"));//42
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/rndButton2"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/MenuScr/rndButton3"));//44
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/input1"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/CompScr/input2"));//46
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/crossButton1"));
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/crossButton2"));//48
            textureLibrary.Add(Content.Load<Texture2D>("Textures/GameScr/crossButton3"));

            //SOUND LOADING
            soundLibrary.Add(Content.Load<SoundEffect>("Sounds/pipeShift1"));//0
            soundLibrary.Add(Content.Load<SoundEffect>("Sounds/pipeShift2"));

            OpeningScreen = new openScr(textureLibrary, soundLibrary, animationLibrary, buttonFont);
            MenuScreen = new menuScr(textureLibrary, soundLibrary);
            GameScreen = new gameScr(textureLibrary, soundLibrary,currentLevel,rndWidth,rndLength);
            LevelScreen = new levelScr(textureLibrary, soundLibrary, buttonFont);
            CompleteScreen = new compScr(textureLibrary, soundLibrary, buttonFont, levelTime.Seconds, currentLevel);
            RandomScreen = new randomScr(textureLibrary, soundLibrary, buttonFont);
            HowToScreen = new howtoScr(textureLibrary, soundLibrary);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            msNow = Mouse.GetState();

            if (gameTime.TotalGameTime != TimeSpan.FromTicks(0))
            {
                prevScr = activeScr;
                switch (activeScr)
                {
                    case screenState.openScr:
                        {
                            OpeningScreen.Update(msNow, msPrev, ref activeScr);
                            break;
                        }
                    case screenState.menuScr:
                        {
                            MenuScreen.Update(msNow, msPrev, ref activeScr);
                            break;
                        }
                    case screenState.levelScr:
                        {
                            LevelScreen.Update(msNow, msPrev, ref activeScr, ref currentLevel, ref levelStart);                           
                            break;
                        }
                    case screenState.gameScr:
                        {
                            GameScreen.Update(msNow, msPrev, ref activeScr, ref levelStart, ref levelEnd);
                            break;
                        }
                    case screenState.compScr:
                        {
                            CompleteScreen.Update(msNow, msPrev, ref activeScr);
                            break;
                        }
                    case screenState.randScr:
                        {
                            RandomScreen.Update(msNow, msPrev, ref activeScr, ref currentLevel, ref rndWidth, ref rndLength, ref levelStart);
                            break;
                        }
                    case screenState.howtoScr:
                        {
                            HowToScreen.Update(msNow, msPrev, ref activeScr, ref levelStart);
                            break;
                        }
                }

                levelTime = levelEnd - levelStart;
                if(activeScr != prevScr)
                {
                    if(activeScr == screenState.gameScr)
                    {
                        GameScreen = new gameScr(textureLibrary, soundLibrary, currentLevel,rndWidth,rndLength);
                    }
                    else if (activeScr == screenState.compScr)
                    {
                        CompleteScreen = new compScr(textureLibrary, soundLibrary, buttonFont, levelTime.Seconds, currentLevel);
                    }
                }
            }

            msPrev = msNow;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();


            switch (activeScr)
            {
                case screenState.openScr:
                    {
                        OpeningScreen.Draw(spriteBatch);
                        break;
                    }
                case screenState.menuScr:
                    {
                        MenuScreen.Draw(spriteBatch);
                        break;
                    }
                case screenState.levelScr:
                    {
                        LevelScreen.Draw(spriteBatch);
                        break;
                    }
                case screenState.gameScr:
                    {

                        GameScreen.Draw(spriteBatch);

                        break;
                    }
                case screenState.compScr:
                    {
                        CompleteScreen.Draw(spriteBatch);
                        break;
                    }
                case screenState.randScr:
                    {
                        RandomScreen.Draw(spriteBatch);
                        break;
                    }
                case screenState.howtoScr:
                    {
                        HowToScreen.Draw(spriteBatch);
                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
