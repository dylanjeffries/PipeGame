using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace PipeGame_v2
{
    class Button
    {
        Rectangle rect;
        List<Texture2D> textures;
        bool hold;
        bool clicked;
        bool hover;

        public Button(Rectangle _rect, List<Texture2D> _textures)//Texture Order: Normal, Hover, Hold
        {
            rect = _rect;
            textures = _textures;
            hold = false;
            clicked = false;
            hover = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hover)
            {
                if (hold)
                {
                    spriteBatch.Draw(textures[2], rect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(textures[1], rect, Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(textures[0], rect, Color.White);
            }
        }

        public bool isClicked(MouseState msNow, MouseState msPrev)
        {
            clicked = false;
            if (rect.Contains(msNow.X, msNow.Y))
            {
                hover = true;
                if (msNow.LeftButton == ButtonState.Pressed)
                {
                    hold = true;
                }
                else
                {
                    hold = false;
                }
                if (msNow.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed)
                {
                    clicked = true;
                }
            }
            else
            {
                hover = false;
            }
            return clicked;
        }
    }
}
