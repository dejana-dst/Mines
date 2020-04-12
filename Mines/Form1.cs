using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines
{
    public partial class Form1 : Form
    {
        Model game;
        Graphics g;
        Pen p;
        int tempX=15;
        int tempY=8;
        int tempM=26;
        int ttX = 15;
        int ttY = 8;
        int ttM = 26;
        int padX = 0, padY = 0;
        private bool firstClick=true;

        static int cell = 32;
        static Image flag = Mines.Properties.Resources.Flag;
        Bitmap flagBmp = new Bitmap(flag, new Size(cell-1, cell - 1));
        static Image mine = Mines.Properties.Resources.Mine;
        Bitmap mineBmp = new Bitmap(mine, new Size(cell - 1, cell - 1));
        static Image unpressed = Mines.Properties.Resources.unpressed;
        Bitmap unpressedBmp = new Bitmap(unpressed, new Size(cell - 1, cell - 1));
        static Image pressed = Mines.Properties.Resources.pressed;
        Bitmap pressedBmp = new Bitmap(pressed, new Size(cell - 1, cell - 1));
        Font fonty = new Font("Verdana", 15, FontStyle.Bold);
        

        public Form1()
        {
            InitializeComponent();
            
            g = this.CreateGraphics();
            p = new Pen(Brushes.Black, 1);
            newGameToolStripMenuItem_Click(null, null);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
          //  g = this.CreateGraphics();
          //  p = new Pen(Brushes.Black, 1);
        
           // game = new Model(tempX, tempY, tempM);
            
        }

        private void DrawGrid()
        {
            g.Clear(BackColor);

            if (tempX * cell < 400)

            {
                padX = (400 - (tempX * cell)) / 2;
                this.ClientSize= new Size(400, this.ClientSize.Height);
               // this.Size = new System.Drawing.Size(400, this.ClientSize.Height);
            }
            else
            {
                padX = 0;
                this.ClientSize = new Size(tempX * cell, this.ClientSize.Height);
               // this.Size = new System.Drawing.Size(tempX * 24, this.ClientSize.Height);
            }
            if (tempY * cell < (400))
            {
                padY = ((400 - (tempY * cell)) / 2) + 27;
                this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, 427);
            }

            else
            {
                padY = 27;
                this.ClientSize = new System.Drawing.Size(this.ClientSize.Width, tempY * cell + 27);
                
            }
            for (int i = 0; i <= tempX; i++)
            {
                //stubici
                g.DrawLine(p, i* cell + padX, padY, i* cell + padX, (cell * tempY) + padY);
                //Console.WriteLine("this is a pole: " + (i * 24 + padX) + " " + padY + " " + (i * 24 + padX) + " " + ((24 * tempY) + padY));
                
            }
            for (int i = 0; i <= tempY; i++)
            {
                // horizontale
                g.DrawLine(p, 0 + padX, (i* cell) + padY, (cell * tempX) + padX, (i* cell) + padY);
                //Console.WriteLine("this is a bed: " + padX + " " + ((i * 24) + padY) + " " + ((24 * tempX) + padX) + " " + ((i * 24) + padY));
            }
        }


        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempM = ttM;
            tempY = ttY;
            tempX = ttX;
            game = new Model(tempX,tempY,tempM);
            toolStripTextBox1.Text = game.RemainingMineCount.ToString();
            this.Form1_Paint(null, null);

            firstClick = true;



        }

        private void gameSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsBox dlg1 = new SettingsBox();
            dlg1.ShowDialog();
            this.ttX = SettingsBox.column;
            this.ttM = SettingsBox.mines;
            this.ttY=SettingsBox.row;
            
        }

        private bool WithinBounds(int x, int y)
        {
            if(x>padX && x<padX+tempX* cell && y > padY && y < padY + tempY * cell)
             return true;
            return false;
        }

        private void Reveal(int fx, int fy)
        {
            if (game.Revealed[fx, fy] == 1 || game.Revealed[fx, fy] == 3)
                return;
            game.Revealed[fx, fy] = 1;
            g.FillRectangle(new SolidBrush(this.BackColor), padX + fx * cell + 1, padY + fy * cell + 1, cell - 1, cell - 1);
            if(!game.GameBoard[fx, fy].ToString().Equals("0"))
               // g.DrawString(game.GameBoard[fx, fy].ToString(), fonty, new SolidBrush(Color.Blue), padX + fx * cell + 7, padY + fy * cell + 4);
            {

                int val = game.GameBoard[fx, fy];
                if (val > 0)
                    g.DrawString(val.ToString(), fonty, new SolidBrush(Color.FromArgb(7 + val * 31, 0, 255 - (val - 1) * 36)), padX + fx * cell + 7, padY + fy * cell + 4);
            }
            else 
            {
                for (var i1 = fx - 1; i1 <= fx + 1; i1++)
                    for (var j1 = fy - 1; j1 <= fy + 1; j1++)
                        if (game.InBounds(i1, j1) && !(i1 == fx && j1 == fy))
                            Reveal(i1, j1);
            }
        }

      

        private void Won()
        {
            if (game.checkVictory())
            {
                MessageBox.Show("You won!");
                newGameToolStripMenuItem_Click(null, null);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            { 
                if (WithinBounds(e.X, e.Y))
                {
                    int fx = (e.X - padX) / cell;
                    int fy = (e.Y - padY) / cell;
                    if (firstClick)
                    {
                        game.generateMines(fx,fy);
                        game.generateNumbers();
                        
                        firstClick = false;
                       // this.Form1_Paint(null, null);
                    }
                    
                     if(game.Revealed[fx,fy]!=0) 
                        return;

                    //game.Revealed[fx, fy] = 1;
                    if(game.checkDead(fx,fy))
                    {

                    for (int i = 0; i < game.DimensionX; i++)
                    {
                        for (int j = 0; j < game.DimensionY; j++)
                        {
                            if(game.GameBoard[i,j]==-1)
                            {
                                g.FillRectangle(new SolidBrush(this.BackColor), padX + i * cell + 1, padY +j * cell + 1, cell-1, cell-1);
                                g.DrawImage(mineBmp, padX + i * cell + 1, padY + j * cell + 1);
                            }
                                
                        }
                    }
                                
                        MessageBox.Show("You lost.");
                        newGameToolStripMenuItem_Click(null, null);
                        return;
                    }
                    Reveal(fx, fy);
                    //this.Form1_Paint(null, null);
                    Won();

                }
            }
            if (e.Button == MouseButtons.Right)
            {
                int fx = (e.X - padX) / cell;
                int fy = (e.Y - padY) / cell;
                if (game.Revealed[fx,fy]==0 && game.RemainingMineCount>0 && !firstClick)
                {
                    game.Revealed[fx, fy] = 3;
                    //g.FillRectangle(new SolidBrush(this.BackColor), padX + fx * cell + 1, padY + fy * cell + 1, cell-1, cell - 1);
                   
                    g.DrawImage(flagBmp, padX + fx * cell + 1, padY + fy * cell + 1);
                    game.RemainingMineCount--;
                    toolStripTextBox1.Text = game.RemainingMineCount.ToString();
                    //this.Form1_Paint(null,null);
                    Won();
                    return;
                }
                if (game.Revealed[fx, fy] == 3)
                {
                    game.RemainingMineCount++;
                    toolStripTextBox1.Text = game.RemainingMineCount.ToString();
                    game.Revealed[fx, fy] = -1;
                     g.FillRectangle(new SolidBrush(this.BackColor), padX + fx * cell + 1, padY + fy * cell + 1, cell-1, cell-1);
                    g.DrawImage(unpressedBmp, padX + fx * cell + 1, padY + fy * cell + 1);
                    g.DrawString("?", fonty, new SolidBrush(Color.Maroon), padX + fx * cell + 7, padY + fy * cell + 4);
                    //this.Form1_Paint(null, null);
                    return;
                }
                if (game.Revealed[fx, fy] == -1)
                {
                    game.Revealed[fx, fy] = 0;
                    g.FillRectangle(new SolidBrush(this.BackColor), padX + fx * cell + 1, padY + fy * cell + 1, cell - 1, cell - 1);
                    g.DrawImage(unpressedBmp, padX + fx * cell + 1, padY + fy * cell + 1);
                    //this.Form1_Paint(null, null);
                    return;
                }
            }


        }

        private void Form1_Shown(object sender, EventArgs e)
        {
           // DrawGrid();
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
           // DrawGrid();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid();
            for (int i = 0; i < game.DimensionX; i++)
            {
                for (int j = 0; j < game.DimensionY; j++)
                {
                    if (game.Revealed[i, j] == 0)
                    {
                        //g.FillRectangle(new SolidBrush(this.BackColor), padX + i * 24 + 1, padY + j * 24 + 1, 22, 22);

                        g.DrawImage(unpressedBmp, padX + i * cell + 1, padY + j * cell + 1);
                    }
                    if (game.Revealed[i, j] == 1)
                    {
                        g.FillRectangle(new SolidBrush(this.BackColor), padX + i * cell + 1, padY + j * cell + 1, cell-1, cell-1);
                        if(game.GameBoard[i,j]!=0)
                        {
                           int val=game.GameBoard[i, j];
                            if(val>0)
                                g.DrawString(val.ToString(), fonty, new SolidBrush(Color.FromArgb(7+val*31,0,255-(val-1)*36)), padX + i * cell + 7, padY + j * cell + 4);
                        }
                            
                        
                    }
                    if (game.Revealed[i, j] == -1)
                    {
                        g.DrawImage(unpressedBmp, padX + i * cell + 1, padY + j * cell + 1);
                        g.DrawString("?", fonty, new SolidBrush(Color.Maroon), padX + i * cell + 7, padY + j * cell + 4);
                    }
                    if (game.Revealed[i, j] == 3)
                    {
                        g.DrawImage(unpressedBmp, padX + i * cell + 1, padY + j * cell + 1);
                        g.DrawImage(flagBmp, padX + i * cell + 1, padY + j * cell + 1);
                    }
                }
            }
            //g.FillRectangle(new SolidBrush(Color.Crimson), 500, 500+ 24 + 1, 22, 22);
        }

   

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WithinBounds(e.X, e.Y) && e.Button==MouseButtons.Left)
            {
                int fx = (e.X - padX) / cell;
                int fy = (e.Y - padY) / cell;
                if (!firstClick && game.Revealed[fx,fy]==1)
                {

                    
                    int countF = 0, countQ=0;

                    game.ForEachNeighbor(fx, fy, (i1, j1) => {
                        if (game.Revealed[i1, j1] == 3)
                            countF++;
                        if (game.Revealed[i1, j1] == -1)
                            countQ++;
                    });

                    if(countQ==0 && countF==game.GameBoard[fx,fy])
                    {
                        // Reveal(fx, fy);
                        for (var i1 = fx - 1; i1 <= fx + 1; i1++)
                            for (var j1 = fy - 1; j1 <= fy + 1; j1++)
                                if (game.InBounds(i1, j1) && !(i1 == fx && j1 == fy))
                                {

                                    if (game.Revealed[i1, j1] == 0)
                                    {
                                        Reveal(i1, j1);
                                        //this.Form1_Paint(null, null);
                                        if (game.checkDead(i1, j1))
                                        {

                                            for (int i = 0; i < game.DimensionX; i++)
                                            {
                                                for (int j = 0; j < game.DimensionY; j++)
                                                {
                                                    if (game.GameBoard[i, j] == -1)
                                                    {
                                                        g.FillRectangle(new SolidBrush(this.BackColor), padX + i * cell + 1, padY + j * cell + 1, cell-1, cell-1);
                                                        
                                                        g.DrawImage(mineBmp, padX + i * cell + 1, padY + j * cell + 1);
                                                    }

                                                }
                                            }
                                            //g.FillRectangle(new SolidBrush(this.BackColor), padX + i1 * 24 + 1, padY + j1 * 24 + 1, 22, 22);
                                            //g.DrawImage(flagBmp, padX + i1 * 24 + 1, padY + j1 * 24 + 1);
                                            //g.DrawImage(mineBmp, padX + i1 * 24 + 1, padY + j1 * 24 + 1);
                                            //game = new Model(tempX, tempY, tempM);
                                            //toolStripTextBox1.Text = game.RemainingMineCount.ToString();

                                            //firstClick = true;
                                            MessageBox.Show("You lost.");
                                            newGameToolStripMenuItem_Click(null, null);
                                            return;
                                        
                                        
                                        }
                                    }
                                }
                                     



                                
                    }



                    Won();




                }
            }

        }





    }
}
