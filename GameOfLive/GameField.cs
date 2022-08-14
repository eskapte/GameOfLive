using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GameOfLive
{
    internal class GameField : IEnumerable<string>
    {
        private const string LIVE = "\u25A0";
        private const string SPACE = " ";

        private string[,] _gameField;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public string this[int row, int col]
        {
            get
            {
                if (col < 0) col = Width - 1;
                if (row < 0) row = Height - 1;
                if (col >= Width) col = 0;
                if (row >= Height) row = 0;

                return _gameField[row, col];
            }

            private set
            {
                if (col < 0) col = Width - 1;
                if (row < 0) col = Height - 1;
                if (col >= Width) col = 0;
                if (row >= Height) row = 0;

                _gameField[row, col] = value;
            }
        }

        public string this[int row]
        {
            get
            {
                if (row < 0) row = Height - 1;
                if (row >= Height) row = 0;

                var stringBuilder = new StringBuilder(Width);

                for (int col = 0; col < Width; col++)
                {
                    stringBuilder.Append(this[row, col]);
                }

                return stringBuilder.ToString();
            }
        }

        public GameField(int height, int width)
        {
            if (height < 1)
                throw new Exception("Game field height must be greater 0");
            if (width < 1)
                throw new Exception("Game field width must be greater 0");

            Width = width;
            Height = height;

            _gameField = new string[height, width];

            InitGameField();
        }

        public void SetInitialLive(IEnumerable<Tuple<int, int>> livingCells)
        {
            foreach (var cell in livingCells)
            {
                this[cell.Item1, cell.Item2] = LIVE;
            }
        }

        public void NewTick()
        {
            var newField = new string[Height, Width];

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    var livingNear = GetNearLivingCellsCount(row, col);

                    newField[row, col] = livingNear switch
                    {
                        2 => this[row, col],
                        3 => LIVE,
                        _ => SPACE
                    };
                }
            }

            _gameField = newField;
        }

        private int GetNearLivingCellsCount(int row, int col)
        {
            var count = 0;

            for (var posY = row - 1; posY <= row + 1; posY++)
            {
                for (var posX = col - 1; posX <= col + 1; posX++)
                {
                    if (posX == col && posY == row)
                        continue;

                    if (this[posY, posX] == LIVE)
                        count++;
                }
            }

            return count;
        }

        private void InitGameField()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    this[row, col] = SPACE;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int row = 0; row < Height; row++)
            {
                yield return this[row];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
