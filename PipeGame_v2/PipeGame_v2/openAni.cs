using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PipeGame_v2
{
    class openAni
    {
        List<Texture2D> animationLibrary;
        int frame, ticks;
        Rectangle rect;

        public openAni(List<Texture2D> _animationLibrary, int _x, int _y)
        {
            animationLibrary = _animationLibrary;
            rect = new Rectangle(_x, _y, 601, 520);
            frame = 0;
            ticks = 0;
        }

        public void Update()
        {
            ticks++;
            if(frame == 79 && ticks == 8)
            {
                frame = 0;
                ticks = 0;
            }
            else if((frame == 0 || frame == 40) && ticks == 80)
            {
                frame++;
                ticks = 0;
            }
            else if(!(frame == 0 || frame == 40) && ticks == 8)
            {
                frame++;
                ticks = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animationLibrary[frame],rect, Color.White);
        }
    }
}
