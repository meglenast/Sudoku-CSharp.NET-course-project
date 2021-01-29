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

        
        #region Commands Handlers

        /// Quit Command
        private void QuitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void QuitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        ///// Undo Command
        //private void EditCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = false;
        //}

        //private void EditCommand_Executed(object sender, ExecutedRoutedEventArgs e){ }

        //Solve Command
        private void SolveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void SolveCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        //Load New Game Command
        private void LoadNewGameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void LoadNewGameCommand_Executed(object sender, ExecutedRoutedEventArgs e) 
        {
            BtnsMenu.Visibility = Visibility.Hidden;
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Main.Content = new Levels();
        }
        //Load Saved Game Command
        private void LoadSavedGameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void LoadSavedGameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // BtnsMenu.Visibility = Visibility.Hidden;
            // Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Load();
        }
        //Save Game Command
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e){ }

        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e) { }

        
        #endregion

        private void Load()
        {
            BtnsMenu.Visibility = Visibility.Hidden;
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Main.Content = new LoadSavedGamePage();


        }

      
    }
}
