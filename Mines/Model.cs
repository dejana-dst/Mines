using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines
{
    class Model
    {

        private int isFinished;
        private int[,] gameBoard; // vrijednosti i mine
        private int dimensionX=6;
        private int dimensionY = 6;
        private bool firstClickDone=false;
        private int remainingMineCount = 0;
        private int maxMineCount=15;
        private int[,] revealed;  // 0 nije, 1 jeste, -1 znak pitanja, 3 zastavica

        public int IsFinished { get => isFinished; set => isFinished = value; }
        public int[,] GameBoard { get => gameBoard; set => gameBoard = value; }
        public int DimensionX { get => dimensionX; set => dimensionX = value; }
        public int DimensionY { get => dimensionY; set => dimensionY = value; }
        public bool FirstClickDone { get => firstClickDone; set => firstClickDone = value; }
        public int RemainingMineCount { get => remainingMineCount; set => remainingMineCount = value; }
        public int MaxMineCount { get => maxMineCount; set => maxMineCount = value; }
        public int[,] Revealed { get => revealed; set => revealed = value; }

        public Model()
        {
            isFinished = 0;
            gameBoard = new int[dimensionX, dimensionY];
            firstClickDone = false;
            remainingMineCount = 0;
            maxMineCount = 0;
            revealed = new int[dimensionX, dimensionY];
        }
        public Model(int x,int y, int m)
        {
            isFinished = 0;
            dimensionX = x;
            dimensionY = y;
            maxMineCount = m;
            gameBoard = new int[dimensionX, dimensionY];
            firstClickDone = false;
            remainingMineCount = m;
            
            revealed = new int[dimensionX, dimensionY];
        }

        public void generateMines(int ex,int ey)
        {
            Random rnd = new Random();
            for(int i=0;i< maxMineCount; i++)
            {
                int x = rnd.Next(dimensionX);
                   
                int y = rnd.Next(dimensionY);
                while ((x >= (ex-1)) && (x<=ex+1) && (y>= ey-1) && (y <=(ey+1)) )
                {
                    x = rnd.Next(dimensionX); 
                        y = rnd.Next(dimensionY);
                }
                if (gameBoard[x,y]!=-1)
                {
                    gameBoard[x, y] = -1;
                    
                }
                else
                {
                    i--;
                }
            }
            remainingMineCount = maxMineCount;
        }

        public bool InBounds(int x, int y)
        {
            return y >= 0 && y < dimensionY && x >= 0 && x < dimensionX;
        }
        public void ForEachNeighbor(int i, int j, Action<int, int> action)
        {
            for (var i1 = i - 1; i1 <= i + 1; i1++)
                for (var j1 = j - 1; j1 <= j + 1; j1++)
                    if (InBounds(i1, j1) && !(i1 == i && j1 == j))
                        action(i1, j1);
        }
        public void generateNumbers()
        {
            for (int i = 0; i < dimensionX; i++)
            {
                for (int j = 0; j < dimensionY; j++)
                {

                    if (gameBoard[i, j] != -1)
                    {
                        int count = 0;
                        
                        ForEachNeighbor(i, j, (i1, j1) => {
                            if (gameBoard[i1, j1]==-1)
                                count++;
                        });

                        gameBoard[i, j] = count;
                    }


                }
            }
        }

        public bool checkDead(int x, int y)
        {
            if (gameBoard[x, y] == -1)
                return true;
            return false;
        }

        public bool checkVictory()
        {
            for (int i = 0; i < dimensionX; i++)
            {
                for (int j = 0; j < dimensionY; j++)
                {
                    if (revealed[i, j] == -1)
                        return false;
                    if (revealed[i, j] == 0 && gameBoard[i, j] != -1)
                        return false;
                    if ((revealed[i, j] == 1) && (gameBoard[i, j] == -1))
                        return false;
                    if (revealed[i, j] == 3 && gameBoard[i, j] != -1)
                        return false;
                }
            }
            return true;
        }

































    }
}
