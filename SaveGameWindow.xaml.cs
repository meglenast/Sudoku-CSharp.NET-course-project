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
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for SaveGameWindow.xaml
    /// </summary>
    public partial class SaveGameWindow : Window
    {
        public SaveGameWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method that returns the selected name for the game.
        /// </summary>
        public string SaveGameName 
        {
            get { return TxtSaveName.Text; }
        }

        /// <summary>
        /// Method that handles Save button click for the SaveGameWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
