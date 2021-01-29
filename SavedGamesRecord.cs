using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class SavedGamesRecord
    {

        #region Fields
        private const string FILE_NAME_SAVED_GAMES = "saved.dat";
        private static SavedGamesRecord savedGamesInformaiton;

        private Dictionary<string, SudokuGrid> gamesDictionary;
        private BinaryFormatter formatter;

        #endregion

        #region Constructor
        /// <summary>
        /// Genaeral purpose constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sudokuGrid"></param>
        public SavedGamesRecord(string name, SudokuGrid sudokuGrid)
        {
            gamesDictionary = new Dictionary<string, SudokuGrid>();
            formatter = new BinaryFormatter();

            gamesDictionary.Add(name, sudokuGrid);
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public SavedGamesRecord()
        {
            gamesDictionary = new Dictionary<string, SudokuGrid>();
            formatter = new BinaryFormatter();
        }
        #endregion

        #region Public utillity methods
        public bool DeleteGame(string name)
        {
            if (!gamesDictionary.ContainsKey(name))
            {
                return false;
            }
            else
            {
                if (this.gamesDictionary.Remove(name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Save()
        {
            try
            {
                FileStream writeFileStream = new FileStream(FILE_NAME_SAVED_GAMES,FileMode.Append, FileAccess.Write);

                formatter.Serialize(writeFileStream, gamesDictionary);
                writeFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Error, whiele serializing..");
            }
        }
        public Dictionary<String, SudokuGrid> Load()
        {
            Deserialize();
            return gamesDictionary;
        }

        public SudokuGrid LoadByName(string selectedGame)
        {
            Load();
            SudokuGrid gameToLoad = new SudokuGrid(Difficulty.Easy);
            gamesDictionary.TryGetValue(selectedGame, out gameToLoad);
            return gameToLoad;
        }

        #endregion
        private void Deserialize()
        {
            if (File.Exists(FILE_NAME_SAVED_GAMES))
            {
                try
                {
                    FileStream readFileStream = new FileStream(FILE_NAME_SAVED_GAMES, FileMode.Open, FileAccess.Read);
                    //readFileStream.Seek(0, SeekOrigin.Begin);
                    //long len = readFileStream.Length;
                    //gamesDictionary = (Dictionary<String, SudokuGrid>)formatter.Deserialize(readFileStream);
                    //long pos = readFileStream.Position;
                    //gamesDictionary = (Dictionary<String, SudokuGrid>)formatter.Deserialize(readFileStream);
                    //pos = readFileStream.Position;


                    while (readFileStream.Position != readFileStream.Length)
                    {
                        Dictionary<String, SudokuGrid>saved = (Dictionary<String, SudokuGrid>)formatter.Deserialize(readFileStream);
                        var first = saved.First();
                        gamesDictionary.Add(first.Key,first.Value);
                    }

                    readFileStream.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error, while deserializing..");
                }
            }
        }
    }
}
