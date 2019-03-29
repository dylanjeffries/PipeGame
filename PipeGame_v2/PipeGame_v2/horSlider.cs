using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PipeGame_v2
{
    class horSlider
    {

        Texture2D barTexture;
        Texture2D sliderTexture;
        int x, y, value;
        Rectangle barRect, sliderRect;
        SpriteFont font;
        bool active;

        public horSlider(Texture2D _barTexture, Texture2D _sliderTexture, int _x, int _y, SpriteFont _font)
        {
            barTexture = _barTexture;
            sliderTexture = _sliderTexture;
            x = _x;
            y = _y;
            value = 2;
            barRect = new Rectangle(x, y, 320, 50);
            sliderRect = new Rectangle(x+10, y, 20, 50);
            active = false;
            font = _font;
        }

        public int Update(MouseState msNow, MouseState msPrev)
        {
            if(sliderRect.Contains(msNow.X,msNow.Y) && msNow.LeftButton == ButtonState.Pressed)
            {
                active = true;
            }
            else if(active && msNow.LeftButton == ButtonState.Released)
            {
                active = false;
            }

            if(active)
            {
                if(msNow.X >= (barRect.X + 20) && msNow.X <= (barRect.X + 300))//WITHIN BAR
                {
                    sliderRect.X = msNow.X - 10;
                    value = ((sliderRect.X - (barRect.X + 10)) / 32)+2;
                }
                else if(msNow.X < (barRect.X + 20))//LEFT OF BAR
                {
                    sliderRect.X = barRect.X + 10;
                    value = 2;
                }
                else//RIGHT OF BAR
                {
                    sliderRect.X = barRect.X + 290;
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
