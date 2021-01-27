using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuSolver
    {
        #region Fields
        private readonly int GRID_SIZE = 9;
        private int[,] solution;
        #endregion

        #region Constructors
        public SudokuSolver(int[,] grid)
        {
            Solution = grid;
        }
        #endregion

        #region Properties
        public int[,] Solution
        {
            set
            {
                if (value != null)
                {
                    solution = new int[GRID_SIZE, GRID_SIZE];

                    for (int row = 0; row < GRID_SIZE; row++)
                    {
                        for (int col = 0; col < GRID_SIZE; col++)
                        {
                            solution[row, col] = value[row, col];
                        }
                    }
                }
            }
        }

        #endregion
        private bool CheckFilled()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (solution[i, j] == 0)
                        return false;
                }
            }
            return true;
        }

        public int[,] GetSolution()
        {
            SolveSudoku1();
            int[,] res = new int[GRID_SIZE, GRID_SIZE];

            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    res[row, col] = solution[row, col];
                }
            }
            return res;
        }

        private bool SolveSudoku1()
        {
            int row = -1;
            int col = -1;
            bool isEmpty = true;

            for (int currRow = 0; currRow < GRID_SIZE; currRow++)
            {
                for (int currCol = 0; currCol < GRID_SIZE; currCol++)
                {
                    if (solution[currRow, currCol] == 0)
                    {
                        row = currRow;
                        col = currCol;

                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                {
                    break;
                }
            }
            if (isEmpty)
            {
                return true;
            }

            List<int> lst = new List<int>();
            Random rand = new Random();
            
            for (int value = 1; value <= GRID_SIZE; value++)
            {
                int val;
                while(true)
                {
                    val = rand.Next(1, 10);
                    if (!lst.Contains(val))
                    {
                        lst.Add(val);
                        break;
                    }
                    lst.Add(val);
                }

                if (solution[row, col] == 0 && CheckValidMove(row, col, val))
                {
                    solution[row, col] = val;

                    if (SolveSudoku1())
                    {
                        return true;
                    }
                    else
                    {
                        solution[row, col] = 0;
                    }
                }
            }
            return false;
        }

        private bool SolveSudoku()
        {
            int row = -1;
            int col = -1;
            bool isEmpty = true;

            for (int currRow = 0; currRow < GRID_SIZE; currRow++)
            {
                for (int currCol = 0; currCol < GRID_SIZE; currCol++)
                {
                    if (solution[currRow, currCol] == 0)
                    {
                        row = currRow;
                        col = currCol;

                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                {
                    break;
                }
            }
            if (isEmpty)
            {
                return true;
            }


            for (int value = 1; value <= GRID_SIZE; value++)
            {
                

                if (solution[row,col] == 0 && CheckValidMove(row,col,value))
                {
                    solution[row, col] = value;

                    if (SolveSudoku())
                    {
                        return true;
                    }
                    else
                    {
                        solution[row, col] = 0;
                    }
                }
            }
            return false;
        }

        private bool CheckValidMove(int rowInd, int colInd, int value)
        {
            return 
                    (!CheckRowContains(rowInd, value) &&
                    !CheckColContains(colInd, value) &&
                    !CheckSubgridContains(GetSubgridCoords(rowInd, colInd), value));
        }

        private bool CheckRowContains(int rowInd, int value)
            => GetRow(rowInd).Any(x => x == value);


        private bool CheckColContains(int colInd, int value)
            => GetCol(colInd).Any(x => x == value);

        private bool CheckSubgridContains((int, int) coords, int value)
            => GetSubgrid(coords).Any(x => x == value);

        private int[] GetCol(int colInd)
            => Enumerable.Range(0, GRID_SIZE).Select(x => solution[x, colInd]).ToArray();
        private int[] GetRow(int rowInd)
            => Enumerable.Range(0, GRID_SIZE).Select(x => solution[rowInd, x]).ToArray();

        private List<int> GetSubgrid((int rowInd, int colInd) subgridCoords)
        {
            List<int> subgrid = new List<int>();

            for (int i = subgridCoords.rowInd; i < subgridCoords.rowInd + 3; ++i)
            {
                for (int j = subgridCoords.colInd; j < subgridCoords.colInd + 3; j++)
                {
                    subgrid.Add(solution[i, j]);
                }
            }
            return subgrid;
        }

        private (int, int) GetSubgridCoords(int rowInd, int colInd)
        {
            //first row
            if (rowInd <= 2 && colInd <= 2)
                return (0, 0);
            else if (rowInd <= 2 && colInd >= 3 && colInd <= 5)
                return (0, 3);
            else if (rowInd <= 2 && colInd >= 6 && colInd <= 8)
                return (0, 6);
            //second row
            else if (rowInd >= 3 && rowInd <= 5 && colInd <= 2)
                return (3, 0);
            else if (rowInd >= 3 && rowInd <= 5 && colInd >= 3 && colInd <= 5)
                return (3, 3);
            else if (rowInd >= 3 && rowInd <= 5 && colInd >= 6 && colInd <= 8)
                return (3, 6);
            //third row
            else if (rowInd >= 6 && rowInd <= 8 && colInd <= 2)
                return (6, 0);
            else if (rowInd >= 6 && rowInd <= 8 && colInd >= 3 && colInd <= 5)
                return (6, 3);
            else
                return (6, 6);
        }

    }
}
