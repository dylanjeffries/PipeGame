using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace PipeGame_v2
{
    class levelButton
    {
        Rectangle rect;
        List<Texture2D> textures;
        SpriteFont font;
        public int levelNum;
        int a, b, c, d;
        
        bool hold;
        bool clicked;
        bool hover;

        public levelButton(Rectangle _rect, List<Texture2D> _textures, SpriteFont _font, int _levelNum)//Texture Order: Normal, Hover, Hold
        {
            rect = _rect;
            textures = _textures;
            font = _font;
            levelNum = _levelNum;
            hold = false;
            clicked = false;
            hover = false;
            a = (27 - ((Convert.ToInt16((font.MeasureString(Convert.ToString(levelNum)).X)) / 2))) + rect.X+5;
            b = (27 - ((Convert.ToInt16((font.MeasureString(Convert.ToString(levelNum)).Y)) / 2))) + rect.Y+10;
            c = (27 - ((Convert.ToInt16((font.MeasureString(Convert.ToString(levelNum)).X)) / 2))) + rect.X;
            d = (27 - ((Convert.ToInt16((font.MeasureString(Convert.ToString(levelNum)).Y)) / 2))) + rect.Y+15;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hover)
            {
                if (hold)
                {
                    spriteBatch.Draw(textures[2], rect, Color.White);
                    spriteBatch.DrawString(font, Convert.ToString(levelNum), new Vector2(c, d), Color.Black);
                }
                else
                {
                    spriteBatch.Draw(textures[1], rect, Color.White);
                    spriteBatch.DrawString(font, Convert.ToString(levelNum), new Vector2(a, b), Color.Black);
                }
            }
            else
            {
                spriteBatch.Draw(textures[0], rect, Color.White);
                spriteBatch.DrawString(font, Convert.ToString(levelNum), new Vector2(a, b), Color.Black);
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
