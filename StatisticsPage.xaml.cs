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
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();

            string[] statistic = Statistic.GetStatInformation();

            // Initializing the WPF components with  information from file
            TxtTotalNumGamesPlayed.Text = statistic[0];
            TxtTotalNumGamesWon.Text = statistic[1];
            TxtTotalNumGamesEASY.Text = statistic[2];
            TxtTotalNumGamesMEDIUM.Text = statistic[3];
            TxtTotalNumGamesHARD.Text = statistic[4];
        }
    }
}
