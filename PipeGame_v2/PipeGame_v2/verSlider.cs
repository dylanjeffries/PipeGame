using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PipeGame_v2
{
    class verSlider
    {

        Texture2D barTexture;
        Texture2D sliderTexture;
        int x, y, value;
        Rectangle barRect, sliderRect;
        SpriteFont font;
        bool active;

        public verSlider(Texture2D _barTexture, Texture2D _sliderTexture, int _x, int _y, SpriteFont _font)
        {
            barTexture = _barTexture;
            sliderTexture = _sliderTexture;
            x = _x;
            y = _y;
            value = 2;
            barRect = new Rectangle(x, y, 50, 320);
            sliderRect = new Rectangle(x, y + 10, 50, 20);
            active = false;
            font = _font;
        }

        public int Update(MouseState msNow, MouseState msPrev)
        {
            if (sliderRect.Contains(msNow.X, msNow.Y) && msNow.LeftButton == ButtonState.Pressed)
            {
                active = true;
            }
            else if (active && msNow.LeftButton == ButtonState.Released)
            {
                active = false;
            }

            if (active)
            {
                if (msNow.Y >= (barRect.Y + 20) && msNow.Y <= (barRect.Y + 300))//WITHIN BAR
                {
                    sliderRect.Y = msNow.Y - 10;
                    value = ((sliderRect.Y - (barRect.Y + 10)) / 32)+2;
                }
                else if (msNow.Y < (barRect.Y + 20))//LEFT OF BAR
                {
                    sliderRect.Y = barRect.Y + 10;
                    value = 2;
                }
                else//RIGHT OF BAR
                {
                    sliderRect.Y = barRect.Y + 290;
                    value = 10;
                }
            }
            return value;

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(barTexture, barRect, Color.White);
            spriteBatch.Draw(sliderTexture, sliderRect, Color.White);
            
        }
    }
}
