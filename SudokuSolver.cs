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
        /// <summary>
        /// General purpose constructor
        /// </summary>
        /// <param name="grid"></param>
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


        /// <summary>
        /// Method that returns the sudoku solution.
        /// </summary>
        /// <returns></returns>
        public int[,] GetSolution()
        {
            SolveSudoku();
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

        #region Private utillity methods implementing the sudoku solver algorithm
        /// <summary>
        /// Method tha timplements the sudoku oslver algorithm.
        /// </summary>
        /// <returns></returns>
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

            List<int> lst = new List<int>();
            Random rand = new Random();

            for (int value = 1; value <= GRID_SIZE; value++)
            {
                int val;
                while (true)
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

        /// <summary>
        /// Method that checks whether the change made to the grid is a valid one.
        /// </summary>
        /// <param name="rowInd"></param>
        /// <param name="colInd"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckValidMove(int rowInd, int colInd, int value)
        {
            return
                    (!CheckRowContains(rowInd, value) &&
                    !CheckColContains(colInd, value) &&
                    !CheckSubgridContains(GetSubgridCoords(rowInd, colInd), value));
        }

        /// <summary>
        /// Helper method that checks whether a number is containt in a row.
        /// </summary>
        /// <param name="rowInd"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckRowContains(int rowInd, int value)
            => GetRow(rowInd).Any(x => x == value);

        /// <summary>
        /// Helper method that checks whether a number is containt in a column.
        /// </summary>
        /// <param name="colInd"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckColContains(int colInd, int value)
            => GetCol(colInd).Any(x => x == value);

        /// <summary>
        /// Helper method that checks whether a number is containt in a subgrid.
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckSubgridContains((int, int) coords, int value)
            => GetSubgrid(coords).Any(x => x == value);

        /// <summary>
        /// Helper method that returns the column refered by an index.
        /// </summary>
        /// <param name="colInd"></param>
        /// <returns></returns>
        private int[] GetCol(int colInd)
            => Enumerable.Range(0, GRID_SIZE).Select(x => solution[x, colInd]).ToArray();

        /// <summary>
        /// Helper method that returns the row refered by an index.
        /// </summary>
        /// <param name="rowInd"></param>
        /// <returns></returns>
        private int[] GetRow(int rowInd)
            => Enumerable.Range(0, GRID_SIZE).Select(x => solution[rowInd, x]).ToArray();


        /// <summary>
        /// Helper method that returns the subgrid by indexes.
        /// </summary>
        /// <param name="subgridCoords"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper method that gives the starting indexes of the subgrid refered by indexes.
        /// </summary>
        /// <param name="rowInd"></param>
        /// <param name="colInd"></param>
        /// <returns></returns>
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
        #endregion

    }
}
