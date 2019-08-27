using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Snake : Form
    {
        struct tailListType
        {
            public int row;
            public int col;
        }
        const int numOfPixels = 441;
        int lastArray;
        Color notInUseColor = Color.DarkGray;
        Color snakeColor = Color.DarkGreen;
        Color[,] grid = new Color[(int)Math.Sqrt(numOfPixels), (int)Math.Sqrt(numOfPixels)];
        int tail = 5;
        char direction = ' ';
        char prevDirection = ' ';
        //char prevDirection = ' ';      
        int count = 0;
        int currentRow = 10;
        int currentCol = 10;
        int appleRow = 15;
        int appleCol = 15;
        Color appleColour = Color.Red;
        List<tailListType> tailList = new List<tailListType>();

        

        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            lastArray = (int)Math.Sqrt(numOfPixels);
            for (int row = 0; row < lastArray; row++)
            {
                for (int col = 0; col < lastArray; col++)
                {
                    Label lblPixel = new Label();
                    lblPixel.BackColor = notInUseColor;
                    lblPixel.Size = new Size(22, 22);
                    lblPixel.AutoSize = false;
                    lblPixel.Name = "lblPixel-" + row + "-" + col;
                    lblPixel.Margin = new Padding(2);
                    

                    flowLayoutPanel1.Controls.Add(lblPixel);
                    grid[row, col] = notInUseColor;

                }
            }
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "lblPixel-" + appleRow + "-" + appleCol)
                {
                    ctrl.BackColor = appleColour;
                }
            }


        }

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                prevDirection = direction;
                direction = 'L';
            }
            if (e.KeyCode == Keys.Right)
            {
                prevDirection = direction;
                direction = 'R';
            }
            if (e.KeyCode == Keys.Up)
            {
                prevDirection = direction;
                direction = 'U';
            }
            if (e.KeyCode == Keys.Down)
            {
                prevDirection = direction;
                direction = 'D';
            }
        }

        private void TmrFrame_Tick(object sender, EventArgs e)
        {
            //AppleCheck();
            
            CheckForApple();
            if (direction == ' ')
            {
                grid[currentRow, currentCol] = snakeColor;
                foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                {
                    if (ctrl.Name == "lblPixel-" + currentRow + "-" + currentCol)
                    {
                        ctrl.BackColor = snakeColor;
                    }
                }
            }
            else if (direction == 'L')
            {
                if (prevDirection == 'R')
                {
                    direction = 'R';
                    SnakeRight();
                }
                else
                {
                    SnakeLeft(); 
                }               
            }
            else if (direction == 'R')
            {
                if (prevDirection == 'L')
                {
                    direction = 'L';
                    SnakeLeft();
                }
                else
                {
                    SnakeRight();
                }
            }
            else if (direction == 'U')
            {
                if (prevDirection == 'D')
                {
                    direction = 'D';
                    SnakeDown();
                }
                else
                {
                    SnakeUp();
                }
            }
            else if (direction == 'D')
            {
                if (prevDirection == 'U')
                {
                    direction = 'U';
                    SnakeUp();
                }
                else
                {
                    SnakeDown();
                }
            }
            
        }

        private void CheckForApple()
        {
            bool valid = false;
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.BackColor == appleColour)
                {
                    valid = true;
                }
            }
            if (valid == false)
            {
                PlaceNewApple();
            }
        }

        private void SnakeDown()
        {
            tailListType item;
            item.row = currentRow;
            item.col = currentCol;
            tailList.Insert(0, item);
            currentRow++;
            count++;

            if (currentRow > 20)
            {
                currentRow = 0;
            }
            DrawSnake();
            CheckDeath();
            AppleCheck();
        }

        private void CheckDeath()
        {
            
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "lblPixel-" + currentRow + "-" + currentCol && ctrl.BackColor == snakeColor)
                {
                    
                    tail = 5;
                    restart();
                }
            }

        }

       

        private void restart()
        {
            currentRow = 10;
            currentCol = 10;
            appleRow = 15;
            appleCol = 15;
            tailList.Clear();
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                ctrl.BackColor = notInUseColor;
            }

            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "lblPixel-" + appleRow + "-" + appleCol)
                {
                    ctrl.BackColor = appleColour;
                }
            }
        }

        private void AppleCheck()
        {
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "lblPixel-" + currentRow + "-" + currentCol && ctrl.BackColor == appleColour)
                {
                    tail += 2;
                    PlaceNewApple();
                    break;
                }
            }
        }

        private void PlaceNewApple()
        {
            Random rnd = new Random();
            appleRow = rnd.Next(0, (int)Math.Sqrt(numOfPixels) + 1);
            appleCol = rnd.Next(0, (int)Math.Sqrt(numOfPixels) + 1);

            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "lblPixel-" + appleRow + "-" + appleCol)
                {
                    if (ctrl.BackColor == snakeColor)
                    {
                        PlaceNewApple();

                    }
                    else
                    {
                        ctrl.BackColor = appleColour;
                    }
                }
            }



        }

        private void SnakeUp()
        {
            grid[currentRow, currentCol] = snakeColor;
            tailListType item;
            item.row = currentRow;
            item.col = currentCol;
            tailList.Insert(0, item);
            currentRow--;
            count++;

            if (currentRow < 0)
            {
                currentRow = 20;
            }
            DrawSnake();
            CheckDeath();
            AppleCheck();
        }

        

        private void SnakeRight()
        {
            grid[currentRow, currentCol] = snakeColor;
            tailListType item;
            item.row = currentRow;
            item.col = currentCol;
            tailList.Insert(0, item);
            currentCol++;
            count++;

            if (currentCol > 20)
            {
                currentCol = 0;
            }
            DrawSnake();
            CheckDeath();
            AppleCheck();
        }

        private void SnakeLeft()
        {
            grid[currentRow, currentCol] = snakeColor;
            tailListType item;
            item.row = currentRow;
            item.col = currentCol;
            tailList.Insert(0, item);
            currentCol--;
            

            if (currentCol < 0)
            {
                currentCol = 20;
            }
            
            DrawSnake();            
            CheckDeath();
            AppleCheck();

        }

        private void DrawSnake()
        {                 
            if (tailList.Count > tail)
            {

                for (int i = tail; i < tailList.Count; i++)
                {
                    foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                    {
                        if (ctrl.Name == "lblPixel-" + tailList[i].row + "-" + tailList[i].col)
                        {
                            grid[tailList[i].row, tailList[i].col] = notInUseColor;
                            ctrl.BackColor = notInUseColor;
                        }
                    }
                    tailList.RemoveAt(i);

                }
            }
            foreach (var item in tailList)
            {
                
                foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                {
                    if (ctrl.Name == "lblPixel-" + item.row + "-" + item.col)
                    {
                        
                        
                        ctrl.BackColor = snakeColor; 
                        
                    }                    
                }
                
            }

            
        }        
    }
}

