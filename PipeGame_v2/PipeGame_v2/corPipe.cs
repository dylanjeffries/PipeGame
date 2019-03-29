using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace PipeGame_v2
{
    class corPipe : Pipe
    {
        public corPipe(List<Texture2D> _emptyTextures, List<Texture2D> _fullTextures, SoundEffect _sound, Rectangle _rect, int _rotation)
        {
            emptyTextures = _emptyTextures;
            fullTextures = _fullTextures;
            sound = _sound;
            rect = _rect;

            filled = false;
            rotation = _rotation;
            isSource = false;
            conns = updateConns(rotation);
        }

        protected override int[] updateConns(int _rotation)
        {
            int[] _conns;

            switch (_rotation)
            {
                case 0://NORTH-EAST
                    {
                        _conns = new int[4] { 1, 1, 0, 0 };
                        break;
                    }
                case 1://EAST-SOUTH
                    {
                        _conns = new int[4] { 0, 1, 1, 0 };
                        break;
                    }
                case 2://SOUTH-WEST
                    {
                        _conns = new int[4] { 0, 0, 1, 1 };
                        break;
                    }
                case 3://WEST-NORTH
                    {
                        _conns = new int[4] { 1, 0, 0, 1 };
                        break;
                    }

                default:
                    {
                        _conns = new int[4] { 0, 0, 0, 0 };
                        break;
                    }
            }

            return _conns;
        }
    }
}
