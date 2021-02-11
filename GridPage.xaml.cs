using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for SudokuGrid.xaml
    /// </summary>
    public partial class GridPage : Page
    {
        #region Fields

        private bool onRedo;
        private bool onSolver;
        private SudokuGrid grid;
        private TimeTracker timeTracker;
        internal static GridPage page;

        #endregion

        #region Constructors
        /// <summary>
        /// General purpose constructor.
        /// </summary>
        /// <param name="selectedGame"></param>
        public GridPage(string selectedGame)
        {
            SavedGamesRecord savedGames = new SavedGamesRecord();
            grid = savedGames.LoadByName(selectedGame);

            page = this;
            timeTracker = new TimeTracker();
            onRedo = false;
            onSolver = false;
            InitializeComponent();

            Loaded += MainPageLoaded;
        }

        /// <summary>
        /// General purpose constructor.
        /// </summary>
        /// <param name="difficulty"></param>
        public GridPage(Difficulty difficulty)
        {
            grid = new SudokuGrid(difficulty);
            page = this;
            timeTracker = new TimeTracker();
            onRedo = false;
            onSolver = false;

            InitializeComponent();

            Loaded += MainPageLoaded;
        }
        #endregion

        #region EventHandlers

        /// <summary>
        /// Method that handles the time tracker change.
        /// </summary>
        public string TimeTracker
        {
            set { Dispatcher.Invoke(new Action(() => { LblTimer.Text = value; })); }
        }

        /// <summary>
        /// Method that handles the chages in the grid by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                SetUsersValue(textBox, 0);
                grid.NumInitValues += 1;
                return;
            }
            else if (!int.TryParse(textBox.Text, out int res) || !(res >= 1 && res <= 9))
            {
                MessageBox.Show("Invalid value. You should enter a value between 1 and 9.");
                textBox.Text = string.Empty;
            }
            else
            {
                if (!onRedo)
                    grid.ClearRedoHistory();
                SetUsersValue(textBox, res);
                grid.addToHistory();

                if (grid.NumInitValues == 81 && textBox.Background != Brushes.IndianRed && onSolver == false)
                {
                    Statistic.AddToStatistic(grid.Difficulty, true);
                    MessageBox.Show("Won");
                }

                if (grid.NumInitValues == 81 && onSolver)
                {
                    Statistic.AddToStatistic(grid.Difficulty, false);
                }
            }
        }

        void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            int row = 0;
            int col = 0;
            List<TextBox> lst = AllTextBoxes(this);
            foreach (var textBox in lst)
            {
                if (col == 9)
                {
                    col = 0;
                    row++;
                }
                if (row == 9)
                {
                    return;
                }
                if (grid[row, col] != 0)
                {
                    if (grid.OriginalValue(row,col) != grid[row,col])
                    {
                        textBox.Text = grid[row, col].ToString();
                        textBox.FontFamily = new FontFamily("SettedValue");
                        textBox.Background = Brushes.Transparent;
                    }
                    else
                    {
                        textBox.Text = grid[row, col].ToString();
                        textBox.FontFamily = new FontFamily("SettedValue");
                        textBox.FontWeight = FontWeights.Bold;
                        textBox.Background = Brushes.LightSlateGray;
                    }
                }
                else
                {
                    textBox.Text = string.Empty;
                }
                ++col;
            }
            grid.Initialize = false;
            grid.addToHistory();
        }

        #endregion

        #region Commands Handlers

        /// <summary>
        /// Method that checks whether the Undo command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (grid.Undoable() && onSolver == false);
        }

        /// <summary>
        /// Method exccutes the Undo command. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// Method that checks whether the Redo commmand can be exucuted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (grid.Redoable() && onSolver == false);
        }

        /// <summary>
        /// Method exccutes the Redo command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Redo();
        }

        /// <summary>
        /// Method that checks whether the Reset commmand can be exucuted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (grid.Undoable() && onSolver == false);
        }

        /// <summary>
        /// Method executes the Reset command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Method that checks whether the Solve commmand can be exucuted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsSudokuInValidState() && onSolver == false)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        /// <summary>
        /// Method executes the Solve command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSolver = true;
            Solve();
        }

        /// <summary>
        ///  Method that checks whether the Save commmand can be exucuted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsSudokuInValidState() && onSolver == false)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        /// <summary>
        /// Method executes the Save command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Method that executes the Quit Command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Statistic.AddToStatistic(grid.Difficulty, false);
            Application.Current.Shutdown();
        }
        #endregion

        #region Undo, Redo, Save utillity methods

        /// <summary>
        /// Method that saves the game with a given name. 
        /// </summary>
        private void Save()
        {
            SaveGameWindow sw = new SaveGameWindow();
            sw.ShowDialog();

            string name = sw.SaveGameName;

            if (name is null || name == string.Empty)
            {
                MessageBox.Show("Invalid name");
                return;
            }

            SavedGamesRecord savedGames = new SavedGamesRecord(name, grid);
            savedGames.Save();
        }

        /// <summary>
        /// Method that shows the sudoku solution on screen. 
        /// </summary>
        private void Solve()
        {
            SudokuSolver solver = new SudokuSolver(grid.Grid);
            int[,] solution = solver.GetSolution();

            List<TextBox> curr = AllTextBoxes(this);

            int col = 0;
            int row = 0;

            foreach (var textBox in curr)
            {
                if (col == 9)
                {
                    col = 0;
                    row++;
                }
                if (row == 9)
                {
                    return;
                }
                if ((int.TryParse(textBox.Text, out int res) && res != solution[row, col]) || (textBox.Text == string.Empty && solution[row, col] != 0))
                {
                    SetSolutionValue(textBox, solution[row, col].ToString());
                }
                ++col;
            }
        }

        /// <summary>
        /// Methodthat shows he result of the Undo command on screen.
        /// </summary>
        private void Undo()
        {
            int[,] prev = grid.GetPreviousGrid();
            List<TextBox> curr = AllTextBoxes(this);

            int col = 0;
            int row = 0;

            foreach (var textBox in curr)
            {
                if (col == 9)
                {
                    col = 0;
                    row++;
                }
                if (row == 9)
                {
                    return;
                }
                if (int.TryParse(textBox.Text, out int res) && res != prev[row, col] && prev[row, col] == 0)
                {
                    textBox.Text = string.Empty;
                }
                ++col;
            }
        }

        /// <summary>
        /// Methodthat shows he result of the Redo command on screen.
        /// </summary>
        private void Redo()
        {
            int[,] redoGrid = grid.GetRedoGrid();
            List<TextBox> curr = AllTextBoxes(this);

            int col = 0;
            int row = 0;

            foreach (var textBox in curr)
            {
                if (col == 9)
                {
                    col = 0;
                    row++;
                }
                if (row == 9)
                {
                    return;
                }
                if ((int.TryParse(textBox.Text, out int res) && res != redoGrid[row, col]) || (textBox.Text == string.Empty && redoGrid[row, col] != 0))
                {
                    textBox.Text = redoGrid[row, col].ToString();
                }
                ++col;
            }
        }

        /// <summary>
        /// Methodthat shows he result of the Reset command on screen.
        /// </summary>
        private void Reset()
        {
            
            grid.Reset();

            int[,] originalGrid = grid.OriginalSudokuGrid;
            List<TextBox> curr = AllTextBoxes(this);

            int col = 0;
            int row = 0;

            foreach (var textBox in curr)
            {
                if (col == 9)
                {
                    col = 0;
                    row++;
                }
                if (row == 9)
                {
                    return;
                }
                if ((int.TryParse(textBox.Text, out int res) && res != originalGrid[row, col]) || (textBox.Text == string.Empty && originalGrid[row, col] != 0))
                {
                    SetUsersValue(textBox, originalGrid[row, col]);
                }
                ++col;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Utillity method that sets solution value in TextBox.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="value"></param>
        private void SetSolutionValue(TextBox textBox, string value)
        {
            textBox.Text = value;
            textBox.Background = Brushes.MediumPurple;
        }

        /// <summary>
        /// Utillity method that sets a value in TextBox. 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="res"></param>
        private void SetUsersValue(TextBox textBox, int res)
        {
            (int row, int col) coordinates = GetTextBoxCoordinates(textBox);
     
            grid[coordinates.row, coordinates.col] = res;

            if (res == 0)
            {
                textBox.Text = string.Empty;
                textBox.IsReadOnly = false;
                textBox.Background = Brushes.Transparent;
                grid[coordinates.row, coordinates.col] = 0;
                return;
            }
            if (grid.AcceptableValue(coordinates.row, coordinates.col))
            {
                textBox.Text = res.ToString();
                textBox.IsReadOnly = true;
            }
            else
            {
                textBox.Text = res.ToString();
                textBox.IsReadOnly = true;
                textBox.Background = Brushes.IndianRed;
            }
        }

        /// <summary>
        /// Utility method that returns the coordinated of a textbox referred by the WPF object. 
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        private static (int, int) GetTextBoxCoordinates(TextBox textBox)
            => (Int32.Parse((textBox.Name[7]).ToString()), Int32.Parse((textBox.Name[8]).ToString()));

        /// <summary>
        ///  Utility method that returns a List<TextBox> representing the sudoku grid.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<TextBox> AllTextBoxes(DependencyObject parent)
        {
            var list = new List<TextBox>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox)
                    list.Add(child as TextBox);
                list.AddRange(AllTextBoxes(child));
            }
            return list;
        }

        /// <summary>
        ///  Utility method that checks whether the sudoku is in a valid state.
        /// </summary>
        /// <returns></returns>
        private bool IsSudokuInValidState()
        {
            List<TextBox> sudokuGrid = AllTextBoxes(this);
            foreach (TextBox textBox in sudokuGrid)
            {
                if (textBox.Background == Brushes.IndianRed)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
