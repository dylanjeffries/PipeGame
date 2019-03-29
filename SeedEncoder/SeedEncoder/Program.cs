using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace SeedEncoder
{
    struct levelInfo
    {
        public int _xLength;
        public int _yLength;
        public string _seed;
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Raw to Seed\n2. Seed to Raw");
                Console.Write("Entry: ");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        {
                            Console.Clear();
                            rawToSeed();
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            seedToRaw();
                            break;
                        }
                }
            }

        }

        static void rawToSeed()
        {
            string rowText;
            int quantity;
            int a;
            int lowerBound;
            string finalCode = "";
            Console.Write("Level: ");
            int Level = int.Parse(Console.ReadLine());
            Console.Write("xLength: ");
            int xLength = int.Parse(Console.ReadLine());
            Console.Write("yLength: ");
            int yLength = int.Parse(Console.ReadLine());
            for(int i = 0;i<yLength;i++)
            {
                Console.Write("Row " + (i + 1) + " :");
                rowText = Console.ReadLine();
                a = 0;
                quantity = 1;
                while(a < xLength)
                {
                    if (a == (xLength-1))
                    {
                        if (rowText[a] == 'S')
                        {
                            lowerBound = 65;
                        }
                        else if (rowText[a] == 'C')
                        {
                            lowerBound = 75;
                        }
                        else
                        {
                            lowerBound = 85;
                        }
                        finalCode = finalCode + ((char)((lowerBound + quantity) - 1));
                        quantity = 1;
                        break;
                    }
                    else if (rowText[a] != rowText[a+1])
                    {
                        if (rowText[a] == 'S')
                        {
                            lowerBound = 65;
                        }
                        else if(rowText[a] == 'C')
                        {
                            lowerBound = 75;
                        }
                        else
                        {
                            lowerBound = 85;
                        }
                        finalCode = finalCode + ((char)((lowerBound + quantity) - 1));
                        quantity = 1;
                        a += 1;
                    }
                    else
                    {
                        quantity += 1;
                        a += 1;
                    }
                }               
            }
            Console.WriteLine("\n"+finalCode+"\n");
            Console.Write("Save to database?(Y/N) ");
            if(Console.ReadLine() == "Y")
            {
                saveToDatabase(Level, xLength, yLength, finalCode);
            }
            Console.ReadKey();

        }

        static void seedToRaw()
        { 
            int quantity;
            int lowerBound;
            int charCode;
            char rawChar;
            int rowCurrent = 0;
            int rowQuantity = 0;   
            string seed;
            int xLength;
            int yLength;
            Console.Write("Load from database?(Y/N) ");
            if (Console.ReadLine() == "Y")
            {
                levelInfo thisLevel;
                Console.Write("Level: ");
                thisLevel = fetchFromDatabase(int.Parse(Console.ReadLine()));
                xLength = thisLevel._xLength;
                yLength = thisLevel._yLength;
                seed = thisLevel._seed;
            }
            else
            {
                Console.Write("Seed: ");
                seed = Console.ReadLine();
                Console.Write("xLength: ");
                xLength = int.Parse(Console.ReadLine());
                Console.Write("yLength: ");
                yLength = int.Parse(Console.ReadLine());
            }            
            string[] rawArray = new string[yLength];
            foreach(char a in seed)
            {
                charCode = (int)a;
                if(charCode < 75)//Straight
                {
                    lowerBound = 65;
                    rawChar = 'S';
                }
                else if(charCode > 84)//Cutoff
                {
                    lowerBound = 85;
                    rawChar = 'E';
                }
                else//Corner
                {
                    lowerBound = 75;
                    rawChar = 'C';
                }
                quantity = (charCode - lowerBound) + 1;
                rowQuantity += quantity;
                rawArray[rowCurrent] += new string(rawChar, quantity);
                if (rowQuantity == xLength)
                {
                    rowQuantity = 0;
                    rowCurrent += 1;
                }
            }
            Console.WriteLine("\n");
            foreach (string str in rawArray)
            {
                Console.WriteLine(str);
            }
            Console.ReadKey();  
        }

        static void saveToDatabase(int level, int xLength, int yLength, string seed)
        {
            string _path = Environment.CurrentDirectory;
            for (int i = 0; i < 4; i++)
            {
                _path = Convert.ToString(Directory.GetParent(_path));
            }
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+_path+@"\LevelSeeds.accdb");
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("INSERT INTO tableLevels([Level], [xLength], [yLength], [Seed]) VALUES (?,?,?,?)", conn);
            cmd.Parameters.AddWithValue("Level", level);
            cmd.Parameters.AddWithValue("xLength", xLength);
            cmd.Parameters.AddWithValue("yLength", yLength);
            cmd.Parameters.AddWithValue("Seed", seed);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        static levelInfo fetchFromDatabase(int level)
        {
            levelInfo thisLevel = new levelInfo();
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\user 1\Documents\Visual Studio 2015\Projects\SeedEncoder\LevelSeeds.accdb""");
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM tableLevels WHERE Level=[_Level]", conn);
            cmd.Parameters.AddWithValue("_Level", level);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            thisLevel._xLength = reader.GetInt32(1);
            thisLevel._yLength = reader.GetInt32(2);
            thisLevel._seed = reader.GetString(3);
            conn.Close();
            return thisLevel;
        }
       
    }
}
