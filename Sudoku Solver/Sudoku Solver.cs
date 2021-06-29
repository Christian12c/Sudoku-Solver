using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TextBox[,] grid = new TextBox[9,9];
        int[,] numberGrid = new int[9, 9];
        int[,] numberGrid2 = new int[9, 9];

        /// <summary>
        /// Once the form is loaded, the grid is created and the KeyDown event is added to solveButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGrid();
            solveButton.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        /// <summary>
        /// Creates a grid of TextBoxes programmatically
        /// </summary>
        public void CreateGrid()
        {
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = new TextBox();
                    grid[i, j].Location = new Point(25 * i + 100, 20 * j + 100);
                    grid[i, j].Height = 50;
                    grid[i, j].Width = 25;
                    grid[i, j].KeyDown += new KeyEventHandler(Form1_KeyDown);
                    this.Controls.Add(grid[i, j]);
                }
            }
        }

        /// <summary>
        /// Adds the contents of the grid into the 2D arrays: numberGrid and numberGrid2
        /// </summary>
        public void AddToGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(grid[i, j].Text != "")
                    {
                        numberGrid[i, j] = Convert.ToInt32(grid[i, j].Text);
                        numberGrid2[i, j] = Convert.ToInt32(grid[i, j].Text);
                    }
                }
            }
        }
        
        /// <summary>
        /// Checks whether n is a valid move in position [y, x]
        /// The method checks the row, the column and the 9 x 9 grid
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool CheckValidPosition(int y, int x, int n)
        {
            //Check row
            for (int i = 0; i < 9; i++)
            {
                if (numberGrid[y, i] == n)
                {
                    return false;
                }
            }
            //Check column
            for (int i = 0; i < 9; i++)
            {
                if (numberGrid[i, x] == n)
                {
                    return false;
                }
            }
            //Check minigrid
            int y0 = (y / 3) * 3;
            int x0 = (x / 3) * 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (numberGrid[y0 + i, x0 + j] == n)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solveButton_Click(object sender, EventArgs e)
        {
            AddToGrid();
            Solve();
        }

        /// <summary>
        /// Uses backtracking to solve the Sudoku in numberGrid
        /// If when "Solve!" was clicked there was a 0 inside a cell in numbeGrid2...
        /// that means that it was not an inputted number and it should be red.
        /// Otherwise, the number should be black.
        /// These numbers are then put back into the grid of TextBoxes
        /// A message shows that the Sudoku is solved and all the cells are set to black for the next puzzle
        /// </summary>
        public void Solve()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (numberGrid[y, x] == 0)
                    {
                        for (int n = 1; n < 10; n++)
                        {
                            if (CheckValidPosition(y, x, n))
                            {
                                numberGrid[y, x] = n;
                                Solve();
                                numberGrid[y, x] = 0;
                            }
                        }
                        return;
                    }
                }
            }
            
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {                 
                    if(numberGrid2[y, x] == 0)
                    {
                        grid[y, x].ForeColor = Color.Red;
                    }
                    else
                    {
                        grid[y, x].ForeColor = Color.Black;
                    }
                    grid[y, x].Text = numberGrid[y, x].ToString();
                    this.Controls.Add(grid[y, x]);
                }
            }
            
            MessageBox.Show("Solved");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j].ForeColor = Color.Black;
                }
            }
            return;
        }

        /// <summary>
        /// If the "DEL" key is pressed, the whole program is cleared
        /// This Event Handler operates for the form, the cells in the grid and the "Solve!" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        numberGrid[i, j] = 0;
                        numberGrid2[i, j] = 0;
                        grid[i,j].Text = "";
                    }
                }
            }
        }
    }
}
