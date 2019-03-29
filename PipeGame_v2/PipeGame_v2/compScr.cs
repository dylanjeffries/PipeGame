using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace PipeGame_v2
{
    class compScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        Button backButton;
        inputBox inpBox;
        SpriteFont font;
        string time;
        int a;
        int currentLevel;

        public compScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary, SpriteFont _font, int _time, int _currentLevel)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            font = _font;
            backButton = new Button(new Rectangle(240, 500, 320, 48), new List<Texture2D>() { textureLibrary[32], textureLibrary[33], textureLibrary[34] });
            inpBox = new inputBox(240, 436, font, textureLibrary[45], textureLibrary[46]);
            time = Convert.ToString(_time) + " seconds";
            currentLevel = _currentLevel;
            a = 400 - (Convert.ToInt16((font.MeasureString(time).X)) / 2);

            
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr)
        {
            if(backButton.isClicked(msNow,msPrev))
            {
                //WRITE TO SAVE FILE
                string _path = Environment.CurrentDirectory;
                for (int i = 0; i < 6; i++)
                {
                    _path = Convert.ToString(Directory.GetParent(_path));
                }
                _path += @"\save.txt";
                if (File.Exists(_path) && (currentLevel+1 > getUnlocked()))
                {
                    using (var writer = new StreamWriter(_path))
                    {
                        writer.WriteLine(currentLevel + 1);
                    }
                }

                activeScr = screenState.menuScr;

            }

            inpBox.Update(msNow, msPrev);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[31], new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, time, new Vector2(a, 320), Color.Black);
            backButton.Draw(spriteBatch);
            inpBox.Draw(spriteBatch);
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

            if (level == 0)
            {
                level = 1;
            }
            return level;
        }



    }
}