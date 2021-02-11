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

        #region Command Handlers
        /// <summary>
        /// Mehod that checks whether EasyCommmand can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Method that handles EasyCommmand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.EASY);
        }

        /// <summary>
        /// Mehod that checks whether MediumCommmand can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediumCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Method that handles MediumCommmand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediumCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.MEDIUM);
        }

        /// <summary>
        /// Mehod that checks whether HardCommmand can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Method that handles HardCommmand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsLevels.Visibility = Visibility.Hidden;
            LevelsFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            LevelsFrame.Content = new GridPage(Difficulty.HARD);
        } 
        #endregion
    }
}
