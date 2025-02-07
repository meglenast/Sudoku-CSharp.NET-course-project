﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sudoku
{
    public class CustomCommands
    {

        #region Game Commands
        public static readonly RoutedUICommand Quit = new RoutedUICommand
            (
                "Quit",
                "Quit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand LoadNewGame = new RoutedUICommand
            (
                "LoadNewGame",
                "LoadNewGame",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand LoadSavedGame = new RoutedUICommand
            (
                "LoadSavedGame",
                "LoadSavedGame",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.M, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Save = new RoutedUICommand
            (
                "Save",
                "Save",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Control)
                }
            );


        #endregion

        #region Edit Commands
    

        public static readonly RoutedUICommand Undo = new RoutedUICommand
            (
                "Undo",
                "Undo",
                typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Z, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand Redo = new RoutedUICommand
            (
                "Redo",
                "Redo",
                typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Y, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand Reset = new RoutedUICommand
            (
                "Reset",
                "Reset",
                typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand Solve = new RoutedUICommand
            (
                "Solve",
                "Solve",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.X, ModifierKeys.Control)
                }
            );

        #endregion

        #region Level Difficulty Commands
        public static readonly RoutedUICommand Easy = new RoutedUICommand
            (
                "Easy",
                "Easy",
                typeof(CustomCommands)
            );

        public static readonly RoutedUICommand Medium = new RoutedUICommand
            (
                "Medium",
                "Medium",
                typeof(CustomCommands)
            );

        public static readonly RoutedUICommand Hard = new RoutedUICommand
            (
                "Hard",
                "Hard",
                typeof(CustomCommands)
            );
        #endregion

        public static readonly RoutedUICommand Statistic = new RoutedUICommand
           (
               "Statistic",
               "Statistic",
               typeof(CustomCommands)
           );
    }
}
