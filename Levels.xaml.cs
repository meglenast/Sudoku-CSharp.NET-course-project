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
    /// Interaction logic for Levels.xaml
    /// </summary>
    public partial class Levels : Page
    {
        public Levels()
        {
            InitializeComponent();
        }

        #region Levels Commands Handlers
        //Easy Difficulty Command
        private void EasyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void EasyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.Easy);
        }

        //Medium Difficulty Command
        private void MediumCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void MediumCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.Medium);
        }

        //Hard Difficulty Command
        private void HardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HardCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.Hard);
        } 
        #endregion
    }
}
