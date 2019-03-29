using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace PipeGame_v2
{
    class howtoScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        Button playButton;

        public howtoScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            playButton = new Button(new Rectangle(240, 526, 320, 48), new List<Texture2D>() { textureLibrary[1], textureLibrary[2], textureLibrary[3] });
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr, ref DateTime levelStart)
        {
            if (playButton.isClicked(msNow, msPrev))
            {
                levelStart = new DateTime();
                activeScr = screenState.gameScr;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[41], new Vector2(0, 0), Color.White);
            playButton.Draw(spriteBatch);
        }



    }
}