using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Custom commands handlers

        /// <summary>
        /// Method that checks whether the Quit command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        
        /// <summary>
        /// Method that handles the Quit command. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Method that checks whether the Solve command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void SolveCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        /// <summary>
        /// Method that checks  whether the "Load new game" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadNewGameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Method that handles the "Load new game" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadNewGameCommand_Executed(object sender, ExecutedRoutedEventArgs e) 
        {
            BtnsMenu.Visibility = Visibility.Hidden;
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Main.Content = new Levels();
        }

        /// <summary>
        /// Method that checks  whether the "Load saved game" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSavedGameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Mthod that handles the "Load saved game" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSavedGameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsMenu.Visibility = Visibility.Hidden;
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Main.Content = new LoadSavedGamePage();
        }

        /// <summary>
        /// Method that checks whether the "Save game" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        ///// <summary>
        ///// Method that handles the "Save game" command. 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }


        /// <summary>
        /// Method that checks whether the "Undo" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Method that handles the "Undo" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e){ }


        /// <summary>
        /// Method that checks whether the "Redo" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        /// <summary>
        /// Method that handles the "Redo" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }


        /// <summary>
        /// Method that checks whether the "Show statistic" command can be executed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        ///// <summary>
        ///// Method that handles the "Statistic" command. 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void StatisticCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnsMenu.Visibility = Visibility.Hidden;
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Main.Content = new StatisticsPage();
        }

        #endregion
    }
}
