using System;
using System.Collections.Generic;
using System.Linq;

namespace AcidChicken.Reversid.Models
{
    public struct ReversiBoard
    {
        private const uint _cf0 = 0b1100000000000000u;
        private const uint _cf1 = _cf0 >> 0x2;
        private const uint _cf2 = _cf1 >> 0x2;
        private const uint _cf3 = _cf2 >> 0x2;
        private const uint _cf4 = _cf3 >> 0x2;
        private const uint _cf5 = _cf4 >> 0x2;
        private const uint _cf6 = _cf5 >> 0x2;
        private const uint _cf7 = _cf6 >> 0x2;
        private const uint _crf0 = ~_cf0;
        private const uint _crf1 = ~_cf1;
        private const uint _crf2 = ~_cf2;
        private const uint _crf3 = ~_cf3;
        private const uint _crf4 = ~_cf4;
        private const uint _crf5 = ~_cf5;
        private const uint _crf6 = ~_cf6;
        private const uint _crf7 = ~_cf7;

        private uint
            _0,
            _1,
            _2,
            _3,
            _4,
            _5,
            _6,
            _7;

        public static ReversiBoard GetInitial()
        {
            var result = new ReversiBoard();
            result.SetRow(3, 0b00_00_00_11_10_00_00_00u);
            result.SetRow(4, 0b00_00_00_10_11_00_00_00u);
            return result;
        }

        private static uint FromChar(char source) =>
            source == '-' ? 0u :
            source == ' ' ? 1u :
            source == 'b' ? 2u :
            source == 'w' ? 3u :
            throw new FormatException(nameof(source));

        private static Reversi FromCharAsReversi(char source) =>
            source == '-' ? Reversi.Empty :
            source == ' ' ? Reversi.Solid :
            source == 'b' ? Reversi.Black :
            source == 'w' ? Reversi.White :
            throw new FormatException(nameof(source));

        public static ReversiBoard FromString(string source)
            => FromStrings(source.Split('\n'));

        public static ReversiBoard FromStrings(IEnumerable<string> source)
        {
            var result = new ReversiBoard();
            foreach (var (row, index) in source.Select(x => x.Aggregate(0u, (a, c) => a << 2 | (uint)FromChar(c))).Select((x, i) => (x, i)))
                result.SetRow(index, row);
            return result;
        }

        internal static char ToChar(Reversi source) =>
            ToChar((uint)source);
        internal static char ToChar(uint source) =>
            source == 0u ? '-' :
            source == 1u ? ' ' :
            source == 2u ? 'b' :
            source == 3u ? 'w' :
            throw new FormatException(nameof(source));

        internal string FromRowToString(uint source) =>
            new string(Enumerable.Range(0, 8).Select(x => 0xe - x * 2).Select(x => ToChar(source >> x)).ToArray());

        public override string ToString() =>
            string.Join('\n', ToStrings());

        public IEnumerable<string> ToStrings() =>
            GetRows().Select(FromRowToString);

        public Reversi this[int index] =>
            (Reversi)Get(index % 8, index / 8);

        public uint Get(int row, int column) =>
            column == 0 ? GetRow(row) >> 0xe & _cf7 :
            column == 1 ? GetRow(row) >> 0xa & _cf7 :
            column == 2 ? GetRow(row) >> 0xc & _cf7 :
            column == 3 ? GetRow(row) >> 0x8 & _cf7 :
            column == 4 ? GetRow(row) >> 0x6 & _cf7 :
            column == 5 ? GetRow(row) >> 0x4 & _cf7 :
            column == 6 ? GetRow(row) >> 0x2 & _cf7 :
            column == 7 ? GetRow(row) & _cf7 :
            throw new ArgumentOutOfRangeException(nameof(column));

        public Reversi GetAsReversi(int row, int column) =>
            (Reversi)Get(row, column);

        /// <remarks>A little bit slower than <see cref="GetRow" />.</remarks>
        public uint GetColumn(int index) =>
            index == 0 ? _0 & _cf0 | _1 >> 0x2 & _cf1 | _2 >> 0x4 & _cf2 | _3 >> 0x6 & _cf3 | _4 >> 0x8 & _cf4 | _5 >> 0xa & _cf5 | _6 >> 0xc & _cf6 | _7 >> 0xe & _cf7 :
            index == 1 ? _0 << 0x2 & _cf0 | _1 & _cf1 | _2 >> 0x2 & _cf2 | _3 >> 0x4 & _cf3 | _4 >> 0x6 & _cf4 | _5 >> 0x8 & _cf5 | _6 >> 0xa & _cf6 | _7 >> 0xc & _cf7 :
            index == 2 ? _0 << 0x4 & _cf0 | _1 << 0x2 & _cf1 | _2 & _cf2 | _3 >> 0x2 & _cf3 | _4 >> 0x4 & _cf4 | _5 >> 0x6 & _cf5 | _6 >> 0x8 & _cf6 | _7 >> 0xa & _cf7 :
            index == 3 ? _0 << 0x6 & _cf0 | _1 << 0x4 & _cf1 | _2 << 0x2 & _cf2 | _3 & _cf3 | _4 >> 0x2 & _cf4 | _5 >> 0x4 & _cf5 | _6 >> 0x6 & _cf6 | _7 >> 0x8 & _cf7 :
            index == 4 ? _0 << 0x8 & _cf0 | _1 << 0x6 & _cf1 | _2 << 0x4 & _cf2 | _3 << 0x2 & _cf3 | _4 & _cf4 | _5 >> 0x2 & _cf5 | _6 >> 0x4 & _cf6 | _7 >> 0x6 & _cf7 :
            index == 5 ? _0 << 0xa & _cf0 | _1 << 0x8 & _cf1 | _2 << 0x6 & _cf2 | _3 << 0x4 & _cf3 | _4 << 0x2 & _cf4 | _5 & _cf5 | _6 >> 0x2 & _cf6 | _7 >> 0x4 & _cf7 :
            index == 6 ? _0 << 0xc & _cf0 | _1 << 0xa & _cf1 | _2 << 0x8 & _cf2 | _3 << 0x6 & _cf3 | _4 << 0x4 & _cf4 | _5 << 0x2 & _cf5 | _6 & _cf6 | _7 >> 0x2 & _cf7 :
            index == 7 ? _0 << 0xe & _cf0 | _1 << 0xc & _cf1 | _2 << 0xa & _cf2 | _3 << 0x8 & _cf3 | _4 << 0x6 & _cf4 | _5 << 0x4 & _cf5 | _6 << 0x2 & _cf6 | _7 & _cf7 :
            throw new ArgumentOutOfRangeException(nameof(index));

        public uint GetRow(int index) =>
            index == 0 ? _0 :
            index == 1 ? _1 :
            index == 2 ? _2 :
            index == 3 ? _3 :
            index == 4 ? _4 :
            index == 5 ? _5 :
            index == 6 ? _6 :
            index == 7 ? _7 :
            throw new ArgumentOutOfRangeException(nameof(index));

        public IEnumerable<uint> GetRows()
        {
            yield return _0;
            yield return _1;
            yield return _2;
            yield return _3;
            yield return _4;
            yield return _5;
            yield return _6;
            yield return _7;
        }

        internal IEnumerable<(uint top, uint bottom)> GetRowsAsDoubled()
        {
            yield return (_0, _1);
            yield return (_2, _3);
            yield return (_4, _5);
            yield return (_6, _7);
        }

        /// <returns>row</returns>
        public uint Set(int row, int column, Reversi source) =>
            Set(row, column, (uint)source);

        /// <returns>row</returns>
        public uint Set(int row, int column, uint source) =>
            row == 0 ? _0 = _0 & _crf0 | source << 0xe & _cf0 :
            row == 1 ? _1 = _1 & _crf1 | source << 0xc & _cf1 :
            row == 2 ? _2 = _2 & _crf2 | source << 0xa & _cf2 :
            row == 3 ? _3 = _3 & _crf3 | source << 0x8 & _cf3 :
            row == 4 ? _4 = _4 & _crf4 | source << 0x6 & _cf4 :
            row == 5 ? _5 = _5 & _crf5 | source << 0x4 & _cf5 :
            row == 6 ? _6 = _6 & _crf6 | source << 0x2 & _cf6 :
            row == 7 ? _7 = _7 & _crf7 | source & _cf7 :
            throw new ArgumentOutOfRangeException(nameof(row));

        /// <remarks>A little bit slower than <see cref="SetRow" />.</remarks>
        public uint SetColumn(int index, uint source) =>
            SetColumnPrivate(index, source);

        private uint SetColumnPrivate(int index, uint source) =>
            Set(0, index, source >> 0xe & _cf7) |
            Set(1, index, source >> 0xc & _cf7) |
            Set(2, index, source >> 0xa & _cf7) |
            Set(3, index, source >> 0x8 & _cf7) |
            Set(4, index, source >> 0x6 & _cf7) |
            Set(5, index, source >> 0x4 & _cf7) |
            Set(6, index, source >> 0x2 & _cf7) |
            Set(7, index, source & _cf7);

        /// <returns>row</returns>
        public uint SetRow(int index, uint source) =>
            index == 0 ? _0 = source :
            index == 1 ? _1 = source :
            index == 2 ? _2 = source :
            index == 3 ? _3 = source :
            index == 4 ? _4 = source :
            index == 5 ? _5 = source :
            index == 6 ? _6 = source :
            index == 7 ? _7 = source :
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    public enum Reversi : uint
    {
        Empty,
        Solid,
        Black,
        White
    }

    public static class ReversiExtension
    {
        public static Char ToChar(this Reversi source)
            => ReversiBoard.ToChar(source);
    }
}
