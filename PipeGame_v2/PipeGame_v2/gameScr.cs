using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO;

namespace PipeGame_v2
{
    public struct Tile
    {
        //TITLE TYPES, CORNER = 0, STRAIGHT = 1, CUTOFF = 2, RANDOM = 3
        public int tileType;
        public int valid;
    }

    struct levelInfo
    {
        public int _xLength;
        public int _yLength;
        public string _seed;
    }

    class gameScr
    {
        List<Texture2D> textureLibrary;
        List<SoundEffect> soundLibrary;

        //SEPARATE PIPE TEXTURES      
        List<Texture2D> emptyCor;
        List<Texture2D> fullCor;
        List<Texture2D> emptyCut;
        List<Texture2D> fullCut;
        List<Texture2D> emptyStr;
        List<Texture2D> fullStr;

        Pipe[,] pipeGrid;
        List<Pipe> waterList;
        Tile[,] tileGrid;

        Button backButton;

        public gameScr(List<Texture2D> _textureLibrary, List<SoundEffect> _soundLibrary, int _level, int _w, int _l)
        {
            textureLibrary = _textureLibrary;
            soundLibrary = _soundLibrary;

            backButton = new Button(new Rectangle(720, 20, 60, 60), new List<Texture2D> { textureLibrary[47], textureLibrary[48], textureLibrary[49] });

            pipeTextures(textureLibrary);

            waterList = new List<Pipe>();

            //INIT PIPES
            if(_level == 99)
            {
                tileGrid = new Tile[_w,_l];
                tileGrid = createPathGrid(tileGrid, _w, _l);
                tileGrid = finishPathGrid(tileGrid,_w,_l);
                pipeGrid = new Pipe[_w,_l];
                pipeGrid = createGrid(pipeGrid, tileGrid, _w, _l);

            }
            else
            {
                levelInfo gameLevel = new levelInfo();
                gameLevel = fetchFromDatabase(_level);
                pipeGrid = new Pipe[gameLevel._xLength, gameLevel._yLength];
                pipeGrid = infoToGrid(gameLevel);
                
            }
            
        }

        public void Update(MouseState msNow, MouseState msPrev, ref screenState activeScr, ref DateTime levelStart, ref DateTime levelEnd)
        {

            if(levelStart.Second == 0)
            {
                levelStart = DateTime.Now;
            }

            foreach(Pipe p in pipeGrid)
            {
                p.Update(msNow, msPrev);
            }

            waterList.Clear();
            waterList = waterTree(4, 0, 0, pipeGrid, waterList);

            //FILLER
            int s = 0;
            int t = 0;
            for (s = 0; s < pipeGrid.GetLength(0); s++)
            {
                for (t = 0; t < pipeGrid.GetLength(1); t++)
                {
                    if (waterList.Contains(pipeGrid[s,t]))
                    {
                        pipeGrid[s, t].filled = true;
                    }
                    else
                    {
                        if (!pipeGrid[s, t].isSource)
                        {
                            pipeGrid[s, t].filled = false;
                        }


                    }
                }
            }

            if(pipeGrid[pipeGrid.GetLength(0)-1,pipeGrid.GetLength(1)-1].filled)
            {
                levelEnd = DateTime.Now;
                activeScr = screenState.compScr;
            }
            if (backButton.isClicked(msNow, msPrev))
            {
                levelEnd = DateTime.Now;
                activeScr = screenState.menuScr;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureLibrary[0], new Vector2(0, 0), Color.White);
            foreach (Pipe p in pipeGrid)
            {
                p.Draw(spriteBatch);
            }

            backButton.Draw(spriteBatch);
        }

        private void pipeTextures(List<Texture2D> _textureLibrary)
        {
            emptyCor = new List<Texture2D> { _textureLibrary[8], _textureLibrary[9], _textureLibrary[10], _textureLibrary[11] };
            fullCor = new List<Texture2D> { _textureLibrary[12], _textureLibrary[13], _textureLibrary[14], _textureLibrary[15] };
            emptyCut = new List<Texture2D> { _textureLibrary[16], _textureLibrary[17], _textureLibrary[18], _textureLibrary[19] };
            fullCut = new List<Texture2D> { _textureLibrary[20], _textureLibrary[21], _textureLibrary[22], _textureLibrary[23] };
            emptyStr = new List<Texture2D> { _textureLibrary[24], _textureLibrary[25], _textureLibrary[24], _textureLibrary[25] };
            fullStr = new List<Texture2D> { _textureLibrary[26], _textureLibrary[27], _textureLibrary[26], _textureLibrary[27] };
        }

        private levelInfo fetchFromDatabase(int level)
        {
            string _path = Environment.CurrentDirectory;
            for(int i = 0;i<6;i++)
            {
                _path = Convert.ToString(Directory.GetParent(_path));
            }
            levelInfo thisLevel = new levelInfo();
            OleDbConnection seedConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+_path+@"\LevelSeeds.accdb");
            seedConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM tableLevels WHERE Level=[_Level]", seedConn);
            cmd.Parameters.AddWithValue("_Level", level);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            thisLevel._xLength = reader.GetInt32(1);
            thisLevel._yLength = reader.GetInt32(2);
            thisLevel._seed = reader.GetString(3);
            seedConn.Close();
            return thisLevel;
        }

        private Pipe[,] infoToGrid(levelInfo gameLevel)
        {
            Pipe[,] _pipeGrid;
            Random r = new Random();
            int width, height;
            string seed;
            int quantity;
            int lowerBound;
            int charCode;
            char rawChar;
            int rowCurrent = 0;
            int rowQuantity = 0;
            width = gameLevel._xLength;
            height = gameLevel._yLength;
            seed = gameLevel._seed;
            _pipeGrid = new Pipe[width, height];
            string[] rawArray = new string[height];

            foreach (char a in seed)
            {
                charCode = (int)a;
                if (charCode < 75)//Straight
                {
                    lowerBound = 65;
                    rawChar = '1';
                }
                else if (charCode > 84)//Cutoff
                {
                    lowerBound = 85;
                    rawChar = '2';
                }
                else//Corner
                {
                    lowerBound = 75;
                    rawChar = '0';
                }
                quantity = (charCode - lowerBound) + 1;
                rowQuantity += quantity;
                rawArray[rowCurrent] += new string(rawChar, quantity);
                if (rowQuantity == width)
                {
                    rowQuantity = 0;
                    rowCurrent += 1;
                }
            }
            int _row = 0;
            int _col = 0;
            int startX = 400 - (30 * width);
            int startY = 300 - (30 * height);
            SoundEffect _sound;  
            foreach (string str in rawArray)
            {
                _col = 0;
                foreach (char c in str)
                {
                    Thread.Sleep(20);
                    int rNum = r.Next(0, 3);
                    if(rNum == 0 || rNum == 2)
                    {
                        _sound = soundLibrary[0];
                    }
                    else
                    {
                        _sound = soundLibrary[1];
                    }


                    switch (c)
                    {
                        case '0':
                            {
                                _pipeGrid[_col, _row] = new corPipe(emptyCor, fullCor,_sound, new Rectangle(startX + (_col*60),startY + (_row*60),60,60),rNum);
                                break;
                            }
                        case '1':
                            {
                                _pipeGrid[_col, _row] = new strPipe(emptyStr, fullStr, _sound, new Rectangle(startX + (_col * 60), startY + (_row * 60), 60, 60), rNum);
                                break;
                            }
                        case '2':
                            {
                                if(_col == 0 && _row == 0)
                                {
                                    _pipeGrid[_col, _row] = new cutPipe(emptyCut, fullCut, _sound, new Rectangle(startX + (_col * 60), startY + (_row * 60), 60, 60), rNum,true);
                                }
                                else
                                {
                                    _pipeGrid[_col, _row] = new cutPipe(emptyCut, fullCut, _sound, new Rectangle(startX + (_col * 60), startY + (_row * 60), 60, 60), rNum,false);
                                }
                                break;
                            }
                    }                 
                    _col += 1;
                }
                _row += 1;
            }
            return _pipeGrid;
        }

        public List<Pipe> waterTree(int dir, int a, int b, Pipe[,] pipeGrid, List<Pipe> _waterList)
        {
            int[] newConn = pipeGrid[a, b].conns;
            int c = 0;
            int d = 0;
            switch (dir)
            {
                case 0:
                    {
                        newConn[2] = 0;
                        break;
                    }
                case 1:
                    {
                        newConn[3] = 0;
                        break;
                    }
                case 2:
                    {
                        newConn[0] = 0;
                        break;
                    }
                case 3:
                    {
                        newConn[1] = 0;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            for (int i = 0; i < 4; i++)
            {
                if (newConn[i] == 1)
                {
                    if (i == 0)//NORTH
                    {
                        c = a;
                        d = b - 1;
                        if (d >= 0)
                        {
                            if (pipeGrid[c, d].conns[2] == 1)
                            {
                                _waterList = waterTree(i, c, d, pipeGrid, _waterList);
                                if (_waterList.Contains(pipeGrid[c,d]) == false)
                                {
                                    _waterList.Add(pipeGrid[c,d]);
                                }
                            }
                        }
                    }

                    else if (i == 1)//EAST
                    {
                        c = a + 1;
                        d = b;
                        if (c <= pipeGrid.GetLength(0) - 1)
                        {
                            if (pipeGrid[c, d].conns[3] == 1)
                            {
                                _waterList = waterTree(i, c, d, pipeGrid, _waterList);
                                if (_waterList.Contains(pipeGrid[c, d]) == false)
                                {
                                    _waterList.Add(pipeGrid[c, d]);
                                }
                            }
                        }
                    }

                    else if (i == 2)//SOUTH
                    {
                        c = a;
                        d = b + 1;
                        if (d <= pipeGrid.GetLength(1) - 1)
                        {
                            if (pipeGrid[c, d].conns[0] == 1)
                            {
                                _waterList = waterTree(i, c, d, pipeGrid, _waterList);
                                if (_waterList.Contains(pipeGrid[c, d]) == false)
                                {
                                    _waterList.Add(pipeGrid[c, d]);
                                }
                            }
                        }
                    }

                    else if (i == 3)//WEST
                    {
                        c = a - 1;
                        d = b;
                        if (c >= 0)
                        {
                            if (pipeGrid[c, d].conns[1] == 1)
                            {
                                _waterList = waterTree(i, c, d, pipeGrid, _waterList);
                                if (_waterList.Contains(pipeGrid[c, d]) == false)
                                {
                                    _waterList.Add(pipeGrid[c, d]);
                                }
                            }
                        }
                    }
                }
            }
            return _waterList;
        }

        //CREATE PATH GRID
        private Tile[,] createPathGrid(Tile[,] pathGrid, int width, int length)
        {
            int x = 0;
            int y = 0;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < length; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        pathGrid[x, y].tileType = 2;
                        pathGrid[x, y].valid = 0;
                    }
                    else if (x == (width - 1) && y == (length - 1))
                    {
                        pathGrid[x, y].tileType = 2;
                        pathGrid[x, y].valid = 1;
                    }
                    else
                    {
                        pathGrid[x, y].tileType = 3;
                        pathGrid[x, y].valid = 1;
                    }
                }
            }
            return pathGrid;
        }

        private Tile[,] finishPathGrid(Tile[,] pathGrid, int width, int length)
        {
            int[] currentPos = new int[2];
            int[] prevPos = new int[2];
            int currentDir; // 0 = HOR, 1 = VER, 2 = EITHER
            int nextDir;
            int dirHolder; // 0 N, 1 E, 2 S, 3 W
            int[] possibleMoves = new int[4];
            int possibleSum;
            int rNum;
            int pipeType; // 0 = CORNER, 1 = STRAIGHT, 2 = CUTOFF

            while (true)
            {
                pathGrid = createPathGrid(pathGrid,width,length);
                currentPos[0] = 0;
                currentPos[1] = 0;
                prevPos[0] = 0;
                prevPos[1] = 0;
                currentDir = 2;
                nextDir = 0;
                dirHolder = 0;
                possibleMoves = new int[4] { 0, 0, 0, 0 };
                possibleSum = 1;
                rNum = 0;
                pipeType = 0;
                Random r = new Random();

                while (possibleSum != 0)
                {
                    Thread.Sleep(10);
                    if (currentPos[0] == (width - 1) && currentPos[1] == (length - 1))
                    {
                        break;
                    }
                    possibleMoves = checkValidMoves(possibleMoves, currentPos, pathGrid,width,length);
                    if (currentPos[1] == 0 && prevPos[1] != 0)
                    {
                        dirHolder = 1;
                    }
                    else if (currentPos[0] == 0 && prevPos[0] != 0)
                    {
                        dirHolder = 2;
                    }
                    else if (currentPos[1] == (length - 1) && prevPos[1] != (length - 1))
                    {
                        dirHolder = 2;
                    }
                    else if (currentPos[0] == (width - 1) && prevPos[0] != (width - 1))
                    {
                        dirHolder = 1;
                    }
                    else
                    {
                        possibleSum = sumPossible(possibleMoves);
                        rNum = r.Next(0, possibleSum);
                        dirHolder = checkDirection(possibleMoves, rNum);
                    }

                    if (dirHolder == 0 || dirHolder == 2)
                    {
                        nextDir = 1;
                    }
                    else
                    {
                        nextDir = 0;
                    }

                    if (currentDir == 2)
                    {
                        pipeType = 2;
                    }
                    else if (nextDir == currentDir)
                    {
                        pipeType = 1;
                    }
                    else
                    {
                        pipeType = 0;
                    }
                    pathGrid[currentPos[0], currentPos[1]].tileType = pipeType;
                    pathGrid[currentPos[0], currentPos[1]].valid = 0;

                    prevPos = currentPos;
                    if (dirHolder == 0)
                    {
                        currentPos[1] -= 1;
                    }
                    else if (dirHolder == 1)
                    {
                        currentPos[0] += 1;
                    }
                    else if (dirHolder == 2)
                    {
                        currentPos[1] += 1;
                    }
                    else
                    {
                        currentPos[0] -= 1;
                    }
                    currentDir = nextDir;
                }
                if (currentPos[0] == (width - 1) && currentPos[1] == (length - 1))
                {
                    break;
                }
            }
            return pathGrid;
        }

        //SUM OF POSSIBLE MOVES
        private int sumPossible(int[] possibleMoves)
        {
            int sum = possibleMoves[0] + possibleMoves[1] + possibleMoves[2] + possibleMoves[3];
            return sum;
        }

        //MATCH RANDOM NUMBER WITH DIRECTION
        private int checkDirection(int[] possibleMoves, int rNum)
        {
            int currentMove = 0;
            int finalDir = 0;
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                if (possibleMoves[i] == 1)
                {
                    if (currentMove == rNum)
                    {
                        finalDir = i;
                        break;
                    }
                    else
                    {
                        currentMove++;
                    }
                }
            }
            return finalDir;
        }

        //CHECK VALID POSITIONS
        private int[] checkValidMoves(int[] possibleMoves, int[] currentPos, Tile[,] pathGrid, int width, int length)
        {
            int x = currentPos[0];
            int y = currentPos[1];

            //NORTH
            if (y - 1 < 0)
            {
                possibleMoves[0] = 0;
            }
            else if (pathGrid[x, y - 1].valid == 0)
            {
                possibleMoves[0] = 0;
            }
            else
            {
                possibleMoves[0] = 1;
            }

            //EAST
            if (x + 1 > (width - 1))
            {
                possibleMoves[1] = 0;
            }
            else if (pathGrid[x + 1, y].valid == 0)
            {
                possibleMoves[1] = 0;
            }
            else
            {
                possibleMoves[1] = 1;
            }

            //SOUTH
            if (y + 1 > (length - 1))
            {
                possibleMoves[2] = 0;
            }
            else if (pathGrid[x, y + 1].valid == 0)
            {
                possibleMoves[2] = 0;
            }
            else
            {
                possibleMoves[2] = 1;
            }

            //WEST
            if (x - 1 < 0)
            {
                possibleMoves[3] = 0;
            }
            else if (pathGrid[x - 1, y].valid == 0)
            {
                possibleMoves[3] = 0;
            }
            else
            {
                possibleMoves[3] = 1;
            }

            return possibleMoves;

        }

        private Pipe[,] createGrid(Pipe[,] pipeGrid, Tile[,] pathGrid, int width, int length)
        {
            int x = 0;
            int y = 0;
            int x_ = 0;
            int y_ = 0;
            int _col = 0;
            int _row = 0;
            Random r = new Random();
            int rNum;
            SoundEffect _sound;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < length; y++)
                {
                    rNum = r.Next(0, 3);
                    if (rNum == 0 || rNum == 2)
                    {
                        _sound = soundLibrary[0];
                    }
                    else
                    {
                        _sound = soundLibrary[1];
                    }
                    x_ = 400 - (30 * width);
                    y_ = 300 - (30 * length);
                    Thread.Sleep(20);
                    if (pathGrid[x, y].tileType == 3)
                    {
                        rNum = r.Next(0, 3);
                        if (rNum == 0)
                        {
                            pathGrid[x, y].tileType = 0;
                        }
                        else if (rNum == 1)
                        {
                            pathGrid[x, y].tileType = 1;
                        }
                        else
                        {
                            pathGrid[x, y].tileType = 2;
                        }
                    }
                    Thread.Sleep(20);
                    if (pathGrid[x, y].tileType == 2)
                    {
                        rNum = r.Next(0, 3);
                        if (x == 0 && y == 0)
                        {
                            pipeGrid[x, y] = new cutPipe(emptyCut, fullCut, _sound, new Rectangle(x_ + (_col * 60), y_ + (_row * 60), 60, 60), rNum, true);
                        }
                        else
                        {
                            pipeGrid[x, y] = new cutPipe(emptyCut, fullCut, _sound, new Rectangle(x_ + (_col * 60), y_ + (_row * 60), 60, 60), rNum, false);
                        }

                    }
                    else if (pathGrid[x, y].tileType == 1)
                    {
                        rNum = r.Next(0, 3);
                        pipeGrid[x, y] = new strPipe(emptyStr, fullStr, _sound, new Rectangle(x_ + (_col * 60), y_ + (_row * 60), 60, 60), rNum);
                    }
                    else if (pathGrid[x, y].tileType == 0)
                    {
                        rNum = r.Next(0, 3);
                        pipeGrid[x, y] = new corPipe(emptyCor, fullCor, _sound, new Rectangle(x_ + (_col * 60), y_ + (_row * 60), 60, 60), rNum);
                    }
                    _row += 1;
                }
                _col += 1;
                _row = 0;
            }
            return pipeGrid;
        }


    }
}
