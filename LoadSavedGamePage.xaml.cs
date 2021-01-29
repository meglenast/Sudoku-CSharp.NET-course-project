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
    /// Interaction logic for LoadSavedGamePage.xaml
    /// </summary>
    public partial class LoadSavedGamePage : Page
    {
        string selectedGame;
        
        public LoadSavedGamePage()
        {
            InitializeComponent();
            LoadSavedGames();
        }

        
        public string SelectedGame
        {
            get { return selectedGame; }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            selectedGame = CmbBoxSavedGames.Text;
            LoadSelectedGame(selectedGame);
        }

        private void LoadSavedGames()
        {
            SavedGamesRecord savedGames = new SavedGamesRecord();
            Dictionary<string, SudokuGrid> games = savedGames.Load();

            foreach (var game in games)
            {
                CmbBoxSavedGames.Items.Add(game.Key);
            }
        }
        private void LoadSelectedGame(string selectedGame)
        {
            LaodSavedGamePageContent.Visibility = Visibility.Hidden;
            LoadSavedGameFrame.Content = new GridPage(selectedGame);
        }
    }
}
