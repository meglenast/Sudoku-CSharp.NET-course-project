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
        private SudokuGrid grid;
        private TimeTracker timeTracker;
        internal static GridPage page;

        #endregion

        public GridPage(string selectedGame)
        {
            SavedGamesRecord savedGames = new SavedGamesRecord();
            grid = savedGames.LoadByName(selectedGame);
            //grid = new SudokuGrid(difficulty);

            page = this;
            timeTracker = new TimeTracker();
            onRedo = false;

            InitializeComponent();

            Loaded += MainPageLoaded;
        }

        public GridPage(Difficulty difficulty)
        {

            grid = new SudokuGrid(difficulty);
            page = this;
            timeTracker = new TimeTracker();
            onRedo = false;

            InitializeComponent();
            
            Loaded += MainPageLoaded;
        }

        #region EventHandlers
        
        public string TimeTracker
        {
            set { Dispatcher.Invoke(new Action(() => { LblTimer.Text = value; })); }
        }

        private void TxtBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                // grid.addToHistory();
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

                if (grid.NumInitValues == 81)
                    MessageBox.Show("Won");
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
                    textBox.Text = grid[row, col].ToString();
                    textBox.FontFamily = new FontFamily("SettedValue");
                    textBox.FontWeight = FontWeights.Bold;
                    textBox.Background = Brushes.Gray;
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

        //Undo Command
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = grid.Undoable();
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Undo();
        }

        //Redo Command
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = grid.Redoable();
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Redo();
        }
        
        //Reset Command
        private void ResetCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = grid.Undoable();
        }

        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Reset();
        }

        //Solve Command
        private void SolveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //check if valid state!
            e.CanExecute = true;
        }

        private void SolveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Solve();
        }

        //Save Command
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //check if valid state!
            e.CanExecute = true;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }


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
        #endregion

        #region Undo/Redo
        private void Undo()
        {
            int[,] prev = grid.getPreviousGrid();
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
                    onRedo = true;
                    SetUsersValue(textBox, 0);
                    onRedo = false;
                    return;
                }
                ++col;
            }

            grid.NumInitValues -= 1;
        }

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
                    onRedo = true;
                    SetUsersValue(textBox, redoGrid[row, col]);
                    onRedo = false;
                    //return;
                }
                ++col;
            }
            grid.NumInitValues += 1;
        }

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
                    onRedo = true;
                    SetUsersValue(textBox, originalGrid[row, col]);
                    onRedo = false;
                    //return;
                }
                ++col;
            }
            grid.NumInitValues += 1;

        }

        #endregion

        #region private
        private void SetSolutionValue(TextBox textBox, string value)
        {
            textBox.Text = value;
            textBox.Background = Brushes.PaleVioletRed;
        }

        private void SetUsersValue(TextBox textBox, int res)
        {
            (int row, int col) coordinates = GetBoxCoordinatesByName(textBox);

            //if (int.TryParse(textBox.Text, out int result))
            //{
            //    grid[coordinates.row, coordinates.col] = result;
            //}
            //else
            //{
            //    grid[coordinates.row, coordinates.col] = 0;
            //}

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
                //grid[coordinates.row, coordinates.col] = 0;
                //textBox.Text = string.Empty;
            }
        }
        private static (int, int) GetBoxCoordinatesByName(TextBox textBox)
        {
            int row = Int32.Parse((textBox.Name[7]).ToString());
            int col = Int32.Parse((textBox.Name[8]).ToString());
            return (row, col);
        }
        #endregion

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

    }
}
