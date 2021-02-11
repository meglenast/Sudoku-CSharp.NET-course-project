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
        private readonly string FILE_NAME_SAVED_GAMES = "saved.dat";

        private Dictionary<string, SudokuGrid> gamesDictionary;
        private BinaryFormatter formatter;

        #endregion

        #region Constructors
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
        
        /// <summary>
        /// Method that deletes game from the dictionary with saved games.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteGame(string name)
        {
            if (!gamesDictionary.ContainsKey(name))
                return false;
            else
                return this.gamesDictionary.Remove(name);
        }

        /// <summary>
        /// Methode that serialize new saved game.
        /// </summary>
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
                Console.WriteLine("Error, while serializing..");
            }
        }
        
        /// <summary>
        /// Method that loads all of the saved games.
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, SudokuGrid> Load()
        {
            Deserialize();
            return gamesDictionary;
        }

        /// <summary>
        /// Method that a game reffered by it's name.
        /// </summary>
        /// <param name="selectedGame"></param>
        /// <returns></returns>
        public SudokuGrid LoadByName(string selectedGame)
        {
            Load();
            SudokuGrid gameToLoad = new SudokuGrid(Difficulty.LOADED_SUDOKU);
            gamesDictionary.TryGetValue(selectedGame, out gameToLoad);
            gameToLoad.NumInitValues = 0;
            return gameToLoad;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Methods that deserialize all games  from a file.
        /// </summary>
        private void Deserialize()
        {
            if (File.Exists(FILE_NAME_SAVED_GAMES))
            {
                try
                {
                    FileStream readFileStream = new FileStream(FILE_NAME_SAVED_GAMES, FileMode.Open, FileAccess.Read);

                    while (readFileStream.Position != readFileStream.Length)
                    {
                        Dictionary<String, SudokuGrid> saved = (Dictionary<String, SudokuGrid>)formatter.Deserialize(readFileStream);
                        var first = saved.First();
                        gamesDictionary.Add(first.Key, first.Value);
                    }

                    readFileStream.Close();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error, while deserializing..");
                }
            }
        } 
        #endregion
    }
}
