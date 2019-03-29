using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace PipeGame_v2
{
    class randomScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;
        horSlider widthSlider;
        verSlider heightSlider;
        SpriteFont font;
        Button backButton;
        Button playButton;

        int gridWidth, gridHeight;

        public randomScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary, SpriteFont _font)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;
            font = _font;
            widthSlider = new horSlider(textureLibrary[35],textureLibrary[36],240,90,_font);
            heightSlider = new verSlider(textureLibrary[37], textureLibrary[38], 190, 140, _font);
            gridWidth = 0;
            gridHeight = 0;
            playButton = new Button(new Rectangle(240, 494, 320, 48), new List<Texture2D>() { textureLibrary[1], textureLibrary[2], textureLibrary[3] });
            backButton = new Button(new Rectangle(240, 546, 320, 48), new List<Texture2D>() { textureLibrary[32], textureLibrary[33], textureLibrary[34] });
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr, ref int currentLevel, ref int width, ref int length, ref DateTime levelStart)
        {
            gridWidth = widthSlider.Update(msNow, msPrev);
            gridHeight = heightSlider.Update(msNow, msPrev);
            if(backButton.isClicked(msNow,msPrev))
            {
                activeScr = screenState.menuScr;
            }

            if(playButton.isClicked(msNow,msPrev))
            {
                width = gridWidth;
                length = gridHeight;
                levelStart = new DateTime();
                currentLevel = 99;
                activeScr = screenState.gameScr;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[0], new Vector2(0, 0), Color.White);
            widthSlider.Draw(spriteBatch);
            heightSlider.Draw(spriteBatch);

            
            gridDraw(spriteBatch);
            spriteBatch.DrawString(font, Convert.ToString(gridWidth), new Vector2(388, 150), Color.Black);
            spriteBatch.DrawString(font, Convert.ToString(gridHeight), new Vector2(250, 280), Color.Black);
            backButton.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
        }

        private void gridDraw(SpriteBatch spriteBatch)
        {
            Rectangle blobRect = new Rectangle(0, 0, 12, 12);
            Rectangle White = new Rectangle(300, 200, 200, 200);
            spriteBatch.Draw(textureLibrary[40], White, Color.White);

            int x, y;
            x = 300 + (100 - (10 * gridWidth));
            y = 200 + (100 - (10 * gridHeight));

            for (int b = 0;b<gridHeight;b++)
            {
                for(int a = 0;a<gridWidth;a++)
                {
                    blobRect.X = x + (a * 20) + 4;
                    blobRect.Y = y + (b * 20) + 4;
                    spriteBatch.Draw(textureLibrary[39], blobRect, Color.White);
                }
            }

        }



    }
}
