using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    
    class Statistic
    {
        private static readonly string statFileName = "stat.txt";

        /// <summary>
        /// Method that adds a game to statistic
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="isWin"></param>
        public static void AddToStatistic(Difficulty difficulty, bool isWin)
        {
            if (File.Exists(statFileName))
            {
                string[] data = File.ReadAllLines(statFileName);

                int numGamesPlayed = Convert.ToInt32(data[0]);
                ++numGamesPlayed;
                int numGamesWon = Convert.ToInt32(data[1]);
                if (isWin) ++numGamesWon;

                data[0] = numGamesPlayed.ToString();
                data[1] = numGamesWon.ToString();
                data[(int)difficulty] = (Convert.ToInt32(data[(int)difficulty]) + 1).ToString();

                File.WriteAllLines(statFileName, data);
            }
            else
            {
                string[] record = { "1", "0", "0", "0", "0"};
                if (isWin)
                {
                    record[1] = "1";
                    record[(int)difficulty] = "1";
                }
                else
                {
                    record[(int)difficulty] = "1";
                }
                File.WriteAllLines(statFileName, record);
            }
        }

        /// <summary>
        /// Method that reads games statisc 
        /// </summary>
        /// <returns></returns>
        public static string[] GetStatInformation() 
            => File.ReadAllLines(statFileName);
    }
}
