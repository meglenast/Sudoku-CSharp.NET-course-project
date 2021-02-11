using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SudokuGenerator
    {
        #region Fields
        private static readonly int SIZE_GRID = 9;
        private readonly int[,] sudokuPuzzle;
        private static Difficulty difficulty;
        #endregion

        #region Properties
        public int[,] SudokuPuzzle
        {
            get
            {
                int[,] res = new int[SIZE_GRID, SIZE_GRID];

                for (int i = 0; i < SIZE_GRID; i++)
                    for (int j = 0; j < SIZE_GRID; j++)
                        res[i, j] = sudokuPuzzle[i, j];

                return res;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// General purpose constructor
        /// </summary>
        /// <param name="diff"></param>
        public SudokuGenerator(Difficulty diff)
        {
            sudokuPuzzle = new int[SIZE_GRID, SIZE_GRID];
            difficulty = diff;

            SudokuSolver sudokuSolver = new SudokuSolver(sudokuPuzzle);
            sudokuPuzzle = sudokuSolver.GetSolution();
            GenerateSudoku();
        } 
        #endregion

        /// <summary>
        /// Method that generates a new sudoku puzzle based on sudoku solver algorithm.
        /// </summary>
        private void GenerateSudoku()
        {
            Random rand = new Random();

            int numToErase;

            switch (difficulty)
            {
                case Difficulty.EASY:
                    numToErase = rand.Next(27, 36);
                    //numToErase = rand.Next(2, 3);
                    break;
                case Difficulty.MEDIUM:
                    numToErase = rand.Next(35, 45);
                    break;
                case Difficulty.HARD:
                    numToErase = rand.Next(44, 60);
                    break;
                default:
                    numToErase = 10;
                    break;
            }

            for (int i = 0; i < numToErase; i++)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);

                if (sudokuPuzzle[row, col] == 0) --i;
                
                sudokuPuzzle[row, col] = 0;
            }
        }
    }
}
