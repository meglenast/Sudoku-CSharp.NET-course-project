﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuGrid
    {
        public static readonly int GRID_SIZE = 9;
        private int[,] grid;
        private Stack<int[,]> undoStackHistory;
        private bool initialize;
        private int numInitValues;
        public SudokuGrid()
        {
            //grid = new int[9][];

            //for (int row = 0; row < GRID_SIZE; row++)
            //{
            //    grid[row] = new int[9];

            //    //for (int col = 0; col < GRID_SIZE; col++)
            //    //{
            //    //    grid[row][col] = 1;
            //    //}
            //}
            initialize = true;
            grid = new int[,]
            {
                    {5,3,0,0,7,0,0,0,0},
                    {6,0,0,1,9,5,0,0,0},
                    {0,9,8,0,0,0,0,6,0},
                    {8,0,0,0,6,0,0,0,3},
                    {4,0,0,8,0,3,0,0,1},
                    {7,0,0,0,2,0,0,0,6},
                    {0,6,0,0,0,0,2,8,0},
                    {0,0,0,4,1,9,0,0,5},
                    {0,0,0,0,8,0,0,7,9}

                //{5,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0},
                //{0,0,0,0,0,0,0,0,0}
            };
           // numInitValues = SetNumInitValues();
            undoStackHistory = new Stack<int[,]>();
        }

        #region Properties


        public bool Initialize { set { initialize = value; } }

        public int this[int indexRow, int indexCol]
        {
            get
            {
                if ((indexRow >= 0 && indexRow <= GRID_SIZE - 1) && (indexCol >= 0 && indexCol <= GRID_SIZE - 1))
                {
                    return grid[indexRow, indexCol];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if ((indexRow >= 0 && indexRow <= GRID_SIZE - 1) && (indexCol >= 0 && indexCol <= GRID_SIZE - 1))
                {
                    grid[indexRow, indexCol] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion

        #region ValidityChecks
        public bool AcceptableValue(int row, int col)
        {
            return (CheckRow(row, col) && CheckColumn(row, col) && CheckSquare(row, col));
        }

        private bool CheckRow(int row, int col)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if ((j != col) && (grid[row, j] == grid[row, col]))
                {
                    return false; // Row concurrence
                }
            }
            return true; //No row concurrence
        }

        private bool CheckColumn(int row, int col)
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                if ((i != row) && (grid[i, col] == grid[row, col]))
                {
                    return false; //Column concurrence
                }
            }
            return true; //No column concurrence
        }
        private bool CheckSquare(int row, int col)
        {
            //First row
            if (row >= 0 && row <= 2 && col >= 0 && col <= 2) // square [0,0]
                return CheckSqaureDefinedByRowCol(0, 0, row, col);
            else if (row >= 0 && row <= 2 && col >= 3 && col <= 5) // square [0,1]
                return CheckSqaureDefinedByRowCol(0, 3, row, col);
            else if (row >= 0 && row <= 2 && col >= 6 && col <= 8) // square [0,2]
                return CheckSqaureDefinedByRowCol(0, 6, row, col);
            //Second row
            else if (row >= 3 && row <= 5 && col >= 0 && col <= 2) //square [1,0]
                return CheckSqaureDefinedByRowCol(3, 0, row, col);
            else if (row >= 3 && row <= 5 && col >= 3 && col <= 5) //square [1,1]
                return CheckSqaureDefinedByRowCol(3, 3, row, col);
            else if (row >= 3 && row <= 5 && col >= 6 && col <= 8) //square [1,2]
                return CheckSqaureDefinedByRowCol(3, 6, row, col);
            //Third row
            else if (row >= 6 && row <= 8 && col >= 0 && col <= 2) //square [2,0]
                return CheckSqaureDefinedByRowCol(6, 0, row, col);
            else if (row >= 6 && row <= 8 && col >= 3 && col <= 5) //square [2,1]
                return CheckSqaureDefinedByRowCol(6, 3, row, col);
            else                                                   //square [2,2]                
                return CheckSqaureDefinedByRowCol(6, 6, row, col);
        }

        private bool CheckSqaureDefinedByRowCol(int row, int col, int val_row, int val_col)
        {
            for (int i = row; i < row + 3; i++)
            {
                for (int j = col; j < col + 3; j++)
                {
                    if ((i != val_row) && (j != val_col) && (grid[i, j] == grid[val_row, val_col]))
                        return false;
                }
            }
            return true;
        }
        #endregion

        #region UndoMethods
        public void addToHistory()
        {
            if (!initialize)
            {
                int[,] tmp = new int[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        tmp[i, j] = grid[i, j];
                    }
                }
                undoStackHistory.Push(tmp);
            }
        }

        public bool Undoable()
        {
            if (undoStackHistory.Count < 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int[,] getPreviousGrid()
        {
            int[,] temp = new int[9, 9];
            undoStackHistory.Pop();
            temp = undoStackHistory.Peek();
            return temp;

        }
        #endregion
    }
}
