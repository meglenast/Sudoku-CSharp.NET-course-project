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
        #region Fields
        protected static readonly int GRID_SIZE = 9;

        private int[,] grid;
        private readonly int[,] originalSudokuGrid;

        private readonly Difficulty difficulty;
        private bool initialize;
        protected int numInitValues;

        private Stack<int[,]> undoStackHistory;
        private Stack<int[,]> redoStackHistory;
        #endregion

        #region Constructors
        /// <summary>
        /// General purpose constuctor
        /// </summary>
        /// <param name="difficulty"></param>
        public SudokuGrid(Difficulty difficulty)
        {
            initialize = true;

            SudokuGenerator sudokuGenerator = new SudokuGenerator(difficulty);
            grid = sudokuGenerator.SudokuPuzzle;
            originalSudokuGrid = new int[GRID_SIZE, GRID_SIZE];

            Array.Copy(grid, originalSudokuGrid, 81);

            SetNumInitValues();
            this.difficulty = difficulty;

            undoStackHistory = new Stack<int[,]>();
            redoStackHistory = new Stack<int[,]>();
        }
        #endregion

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

        public Difficulty Difficulty
        {
            get => difficulty;
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
                int[,] res = new int[GRID_SIZE, GRID_SIZE];
                Array.Copy(originalSudokuGrid, res, GRID_SIZE * GRID_SIZE);
                return res;
            }
        }

        #endregion

        #region Methods that validate moves

        /// <summary>
        /// Method that checks whether the given value is a valid next move.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool AcceptableValue(int row, int col)
        {
            return (CheckRow(row, col) && CheckColumn(row, col) && CheckSquare(row, col));
        }

        /// <summary>
        /// Method that checks whether the given value is a valid next move for the row given.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method that checks whether the given value is a valid next move for the column given.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method that returns the coordinates of the subgrid refered by the indexes.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method that checks whether the given value is a valid next move for the subgrid given.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="val_row"></param>
        /// <param name="val_col"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Methods that adds the current state of the grid to the undo stack.
        /// </summary>
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

        /// <summary>
        /// Method that cheks whether the Undo operation can be done.
        /// </summary>
        /// <returns></returns>
        public bool Undoable()
        {
            return undoStackHistory.Count < 2 ? false : true;
        }

        /// <summary>
        /// Method that cheks whether the Redo operation can be done.
        /// </summary>
        /// <returns></returns>
        public bool Redoable()
        {
            return redoStackHistory.Count < 1 ? false : true;
        }

        /// <summary>
        /// Method that returns the grid from the Undo operation.
        /// </summary>
        /// <returns></returns>
        public int[,] GetPreviousGrid()
        {
            int[,] temp = new int[9, 9];
            redoStackHistory.Push(undoStackHistory.Pop());
            temp = undoStackHistory.Peek();
            return temp;
        }

        /// <summary>
        /// Medhod that returns the grid from the Redo operation.  
        /// </summary>
        /// <returns></returns>
        public int[,] GetRedoGrid()
        {
            int[,] temp = new int[9, 9];
            temp = redoStackHistory.Pop();
            return temp;
        }

        /// <summary>
        /// Method that clears the redo stack
        /// </summary>
        public void ClearRedoHistory()
        {
            redoStackHistory.Clear();
        }

        /// <summary>
        /// Method that resets to the original grid
        /// </summary>
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

        /// <summary>
        /// Method that returns the original value referred by the indexes.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int OriginalValue(int row, int col) => originalSudokuGrid[row, col];

        #endregion

        #region Uitllity methods
        /// <summary>
        /// Method that sets the number of initialized cells
        /// </summary>
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

        #endregion
    }
}
