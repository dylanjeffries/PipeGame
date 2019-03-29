using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace PipeGame_v2
{
    class levelScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        Button backButton;
        levelButton[,] levelGrid = new levelButton[6,6];
        int unlockedLevel;

        public levelScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary, SpriteFont _font)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            backButton = new Button(new Rectangle(240, 546, 320, 48), new List<Texture2D>() { textureLibrary[32], textureLibrary[33], textureLibrary[34] });
            levelGrid = initLevelButtons(levelGrid, _font, textureLibrary);
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr, ref int currentLevel, ref DateTime levelStart)
        {

            unlockedLevel = getUnlocked();

            foreach (levelButton f in levelGrid)
            {
                if (f.isClicked(msNow, msPrev) && (f.levelNum <= unlockedLevel))
                {
                    if(f.levelNum == 1)
                    {
                        activeScr = screenState.howtoScr;
                        currentLevel = f.levelNum;
                    }
                    else
                    {
                        levelStart = new DateTime();
                        activeScr = screenState.gameScr;
                        currentLevel = f.levelNum;
                    }
                    
                }
            }
            
            

            if(backButton.isClicked(msNow,msPrev))
            {
                activeScr = screenState.menuScr;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[0], new Vector2(0, 0), Color.White);
            backButton.Draw(spriteBatch);
            foreach(levelButton f in levelGrid)
            {
                if(f.levelNum <= unlockedLevel)
                {
                    f.Draw(spriteBatch);
                }
                
            }
            
            
            

        }

        public levelButton[,] initLevelButtons(levelButton[,] levelGrid, SpriteFont _font, List<Texture2D> textureLibrary)
        {
            int levelNum = 0;
            int x = 0;
            int y = 0;

            for (int b = 1; b < 7; b++)
            {
                for (int a = 1; a < 7; a++)
                {
                    levelNum = ((6 * b) - 6) + a;
                    x = 136 + (88 * a) - 74;
                    y = (88 * b) - 74;
                    levelGrid[a-1, b-1] = new levelButton(new Rectangle(x, y, 60, 60), new List<Texture2D>() { textureLibrary[28], textureLibrary[29], textureLibrary[30] }, _font, levelNum);
                }
            }

            return levelGrid;
        }

        public int getUnlocked()
        {
            int level = 1;
            string _path = Environment.CurrentDirectory;

            for (int i = 0; i < 6; i++)
            {
                _path = Convert.ToString(Directory.GetParent(_path));
            }
            _path += @"\save.txt";

            if (File.Exists(_path))
            {
                using (var reader = new StreamReader(_path))
                {
                    
                    level = int.Parse(reader.ReadLine());
                }
            }

            if(level == 0)
            {
                level = 1;
            }
            return level;
        }
    }
}