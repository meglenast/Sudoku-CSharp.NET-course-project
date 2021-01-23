using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private SudokuGrid grid;
        public GridPage()
        {
            grid = new SudokuGrid();
            InitializeComponent();
            Loaded += MainPageLoaded;
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
                SetUsersValue(textBox, res);
                grid.addToHistory();
            }
        }

        private void SetUsersValue(TextBox textBox, int res)
        {
            (int row, int col) coordinates = GetBoxCoordinatesByName(textBox);
            
            if (int.TryParse(textBox.Text,out int result))
            {
                grid[coordinates.row, coordinates.col] = result;
            }
            else
            {
                grid[coordinates.row, coordinates.col] = 0;
            }
            //grid[coordinates.row, coordinates.col] = Int32.Parse(textBox.Text);
            
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

        List<TextBox> AllTextBoxes(DependencyObject parent)
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

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (grid.Undoable())
                Undo();
            else
                MessageBox.Show("Cant be undo.");
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            if (grid.Redoable())
                Redo();
            else
                MessageBox.Show("Cant be redo.");
        }

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
                    SetUsersValue(textBox, 0);
                    return;
                }
                ++col;
            }
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
                if ((int.TryParse(textBox.Text, out int res) && res != redoGrid[row, col]) || (textBox.Text == string.Empty && redoGrid[row,col] != 0))
                {
                    SetUsersValue(textBox, redoGrid[row,col]);
                    return;
                }
                ++col;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            SudokuSolver solver = new SudokuSolver(grid.Grid);
            int[,] solution = solver.GetSolution();



        }
    }
}
