using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
    class SudokuGrid
    {
        protected static readonly int GRID_SIZE = 9;
        private int[,] grid;
        private int[,] originalSudokuGrid;
        private Stack<int[,]> undoStackHistory;
        private Stack<int[,]> redoStackHistory;
        private bool initialize;
        protected int numInitValues;

        public SudokuGrid(Difficulty difficulty)
        {
            initialize = true;
            SudokuGenerator sudokuGenerator = new SudokuGenerator(difficulty);
            grid = sudokuGenerator.SudokuPuzzle;
            originalSudokuGrid = new int[GRID_SIZE, GRID_SIZE];
            Array.Copy(grid,originalSudokuGrid,81);
            SetNumInitValues();
            undoStackHistory = new Stack<int[,]>();
            redoStackHistory = new Stack<int[,]>();
        }

        #region Properties

        public int[,] Grid
        {
            get
            {
                int[,] res = new int[GRID_SIZE, GRID_SIZE];

                for (int row = 0; row < GRID_SIZE; row++)
                {
                    for (int col = 0; col < GRID_SIZE; col++)
                    {
                        res[row, col] = grid[row, col];

                    }
                }
                return res;
            }
        }
        public int NumInitValues
        {
            get => numInitValues;

            set
            {
                numInitValues = value;
            }
        }


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

                    if (!initialize)
                    {
                        if (value == 0)
                            --numInitValues;
                        else
                            ++numInitValues;
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public int[,] OriginalSudokuGrid
        {
            get 
            {
                int[,] res = new int[GRID_SIZE,GRID_SIZE];
                Array.Copy(originalSudokuGrid, res, GRID_SIZE * GRID_SIZE);
                return res;
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
            return undoStackHistory.Count < 2 ? false : true;
        }

        public bool Redoable()
        {
            return redoStackHistory.Count < 1 ? false : true;
        }

        public int[,] getPreviousGrid()
        {
            int[,] temp = new int[9, 9];
            redoStackHistory.Push(undoStackHistory.Pop());
            temp = undoStackHistory.Peek();
            return temp;

        }

        public int[,] GetRedoGrid()
        {
            int[,] temp = new int[9, 9];
            temp = redoStackHistory.Pop();
            return temp;
        }
        public void ClearRedoHistory()
        {
            redoStackHistory.Clear();
        }

        public void Reset()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    grid[i, j] = originalSudokuGrid[i, j];
                }
            }
            undoStackHistory.Clear();
            redoStackHistory.Clear();
            undoStackHistory.Push(originalSudokuGrid);
            SetNumInitValues();
        }
        #endregion

        private void SetNumInitValues()
        {
            int cnt = 0;

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (grid[i, j] != 0)
                    {
                        ++cnt;
                    }
                }
            }
            numInitValues = cnt;
        }


    }
}
