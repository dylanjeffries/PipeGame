using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace PipeGame_v2
{
    class openScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        List<Texture2D> animationLibrary;
        Button playButton;
        openAni animation;

        public openScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary, List<Texture2D> _animationLibrary, SpriteFont _font)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            animationLibrary = _animationLibrary;
            playButton = new Button(new Rectangle(240, 526, 320, 48), new List<Texture2D>() { textureLibrary[1], textureLibrary[2], textureLibrary[3] });
            animation = new openAni(animationLibrary, 100, 0);
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr)
        {
            if (playButton.isClicked(msNow, msPrev))
            {
                activeScr = screenState.menuScr;
            }

            animation.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[0], new Vector2(0, 0), Color.White);
            playButton.Draw(spriteBatch);
            animation.Draw(spriteBatch);
        }



    }
}
