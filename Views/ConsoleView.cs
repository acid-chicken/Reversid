using System;
using System.Collections.Generic;
using System.Linq;
using AcidChicken.Reversid.Models;

namespace AcidChicken.Reversid.Views
{
    public static class ConsoleView
    {
        private const uint _cf = 0b11;
        private const char _half = '\u2584';
        private const char _full = '\u2588';

        private static object _lock = new object();

        private static ConsoleColor ReversiToConsoleColor(uint source) =>
            source == 0b00 ? ConsoleColor.DarkGreen :
            source == 0b01 ? ConsoleColor.Black :
            source == 0b10 ? ConsoleColor.Red :
            source == 0b11 ? ConsoleColor.White :
            throw new ArgumentOutOfRangeException(nameof(source));

        private static Cell DoubledReversiToChar(uint top, uint bottom) =>
            new Cell()
            {
                IsHalf = top != bottom,
                Background = ReversiToConsoleColor(top),
                Foreground = ReversiToConsoleColor(bottom)
            };

        private static IEnumerable<Cell> DoubledReversisToCharArray(uint top, uint bottom) =>
            Enumerable.Range(0, 8).Select(x => 0xe - x * 2).Select(x => DoubledReversiToChar(top >> x & _cf, bottom >> x & _cf));

        private static IEnumerable<Cell> DoubledReversisToCharArray((uint top, uint bottom) source) =>
            DoubledReversisToCharArray(source.top, source.bottom);

        public static void Show(ReversiBoard board, int x = default, int y = default)
        {
            lock (_lock)
            {
                var originBackground = Console.BackgroundColor;
                var originForeground = Console.ForegroundColor;
                foreach (var cells in board.GetRowsAsDoubled().Select(DoubledReversisToCharArray))
                {
                    Console.CursorLeft = x;
                    Console.CursorTop = y++;
                    foreach (var cell in cells)
                    {
                        Console.BackgroundColor = cell.Background;
                        Console.ForegroundColor = cell.Foreground;
                        Console.Write(cell.IsHalf ? _half : _full);
                    }
                }
                Console.BackgroundColor = originBackground;
                Console.ForegroundColor = originForeground;
            }
        }

        private struct Cell
        {
            public bool IsHalf { get; set; }
            public ConsoleColor Background { get; set; }
            public ConsoleColor Foreground { get; set; }
        }
    }
}
