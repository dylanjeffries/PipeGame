using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PipeGame_v2
{
    class inputBox
    {
        int x, y;
        Rectangle Rect;
        public string text;
        public bool locked;
        SpriteFont font;
        Texture2D lockedTex, unlockedTex;
        Keys[] nowKeys, prevKeys;
        public inputBox(int _x, int _y, SpriteFont _font, Texture2D _lockedTex, Texture2D _unlockedTex)
        {
            x = _x;
            y = _y;
            Rect = new Rectangle(_x, _y, 320, 48);
            font = _font;
            lockedTex = _lockedTex;
            unlockedTex = _unlockedTex;
            locked = true;
            text = "";
            nowKeys = Keyboard.GetState().GetPressedKeys();
            prevKeys = nowKeys;
        }

        public void Update(MouseState msNow, MouseState msPrev)
        {
            if (Rect.Contains(msNow.X, msNow.Y) && (msNow.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed))
            {
                locked = !locked;
            }

            if(!locked)
            {
                foreach (Keys k in nowKeys)
                {
                    if ((Convert.ToString(k).Length == 1) && !prevKeys.Contains(k) && text.Length < 10)
                    {
                        text += k;
                    }

                    if (k == Keys.Back && !prevKeys.Contains(k) && text.Length > 0)
                    {
                        text = text.Remove(text.Length - 1);
                    }


                }
            }
            
            prevKeys = nowKeys;
            nowKeys = Keyboard.GetState().GetPressedKeys();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(locked)
            {
                spriteBatch.Draw(lockedTex, Rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(unlockedTex, Rect, Color.White);
            }

            spriteBatch.DrawString(font, text, new Vector2((x+160)-Convert.ToInt16(font.MeasureString(text).X / 2), y), Color.Black);
        }
    }
}
