using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace PipeGame_v2
{
    class menuScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        Button levelsButton;
        Button rndButton;

        public menuScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            levelsButton = new Button(new Rectangle(240,154,320,48),new List<Texture2D>() { textureLibrary[5], textureLibrary[6] , textureLibrary[7] });
            rndButton = new Button(new Rectangle(240, 250, 320, 48), new List<Texture2D>() { textureLibrary[42], textureLibrary[43], textureLibrary[44] });
        }


        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr)
        {
            if (levelsButton.isClicked(msNow, msPrev))
            {
                activeScr = screenState.levelScr;
            }

            if (rndButton.isClicked(msNow, msPrev))
            {
                activeScr = screenState.randScr;
            }

            if (new Rectangle(547,36,19,39).Contains(msNow.X, msNow.Y))
            {
                if (msNow.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed)
                {
                    //WRITE TO SAVE FILE
                    string _path = Environment.CurrentDirectory;
                    for (int i = 0; i < 6; i++)
                    {
                        _path = Convert.ToString(Directory.GetParent(_path));
                    }
                    _path += @"\save.txt";
                    if (File.Exists(_path))
                    {
                        using (var writer = new StreamWriter(_path))
                        {
                            writer.WriteLine("36");
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[4], new Vector2(0, 0), Color.White);
            levelsButton.Draw(spriteBatch);
            rndButton.Draw(spriteBatch);
        }



    }
}