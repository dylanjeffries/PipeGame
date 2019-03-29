using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace PipeGame_v2
{
    class Pipe
    {
        protected List<Texture2D> emptyTextures;
        protected List<Texture2D> fullTextures;
        protected SoundEffect sound;
        protected Rectangle rect;

        public bool filled;
        protected int rotation;
        public bool isSource;
        public int[] conns;


        public void Update(MouseState msNow, MouseState msPrev)
        {
            if ((rect.Contains(msNow.X, msNow.Y)) && (msNow.LeftButton == ButtonState.Released && msPrev.LeftButton == ButtonState.Pressed))
            {
                if (rotation != 3)
                {
                    rotation++;
                    sound.Play();
                }
                else
                {
                    rotation = 0;
                    sound.Play();
                }
            }
            conns = updateConns(rotation);
        }

        protected virtual int[] updateConns(int _rotation)
        {
            return new int[4] { 0, 0, 0, 0 };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (filled)
            {
                spriteBatch.Draw(fullTextures[rotation], rect, Color.White);
            }
            else
            {
                spriteBatch.Draw(emptyTextures[rotation], rect, Color.White);
            }

        }

    }
}
