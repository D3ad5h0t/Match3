using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Messaging;
using Match3.Elements;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class GemConroller
    {
        private static FieldCell[,] _gameField = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
        private static bool _clearedCells = false;
        private static FieldCell _currentCell;
        private static List<FieldCell> _collector = new List<FieldCell>();


        public static void SwapGems(FieldCell firstCell, FieldCell secondCell)
        {
            bool areNerby = AreCellsNerby(firstCell, secondCell);
            if (areNerby)
            {
                var middleField = (FieldCell) firstCell.Clone();

                firstCell.Gem = (Gem) secondCell.Gem.Clone();
                firstCell.Gem.Position = firstCell.Position;

                secondCell.Gem = (Gem) middleField.Gem;
                secondCell.Gem.Position = secondCell.Position;
            }
        }

        public static void UpdateTexture(Gem gem, ContentManager content)
        {
            gem.Texture = gem.IsClicked
                ? content.Load<Texture2D>(gem.Type.SelectedSpritePath())
                : content.Load<Texture2D>(gem.Type.SpritePath());
        }

        private static bool AreCellsNerby(FieldCell firstCell, FieldCell secondCell) =>
            firstCell.Id + 1 == secondCell.Id ||
            firstCell.Id - 1 == secondCell.Id ||
            firstCell.Id + 8 == secondCell.Id ||
            firstCell.Id - 8 == secondCell.Id;


        public static void MatchAndClear(FieldCell[,] gameField)
        {
            _clearedCells = false;
            CopyField(gameField, _gameField);
            _currentCell = null;
            _collector.Clear();

            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                for (int j = 0; j < DefaultField.BoardSize; j++)
                {
                    CheckCell(j, i);

                    if (_collector.Count >= 3)
                    {
                        foreach (var cell in _collector)
                        {
                            gameField[cell.Row, cell.Column].Gem = null;
                            _clearedCells = true;
                        }



                    }

                    _currentCell = null;
                    _collector.Clear();
                }
            }
        }

        private static void CheckCell(int x, int y)
        {
            if (_gameField[x, y] == null || _gameField[x, y].Gem == null) return;

        if (_currentCell == null)
            {
                _currentCell = _gameField[x, y];
                _gameField[x, y] = null;
                _collector.Add(_currentCell);
            }
            else if (_currentCell.Gem.Type != _gameField[x, y].Gem.Type)
            {
                return;
            }
            else
            {
                _collector.Add(_gameField[x, y]);
                _gameField[x, y] = null;
            }

            if (x > 0) CheckCell(x - 1, y);
            if (y > 0) CheckCell(x, y - 1);
            if (x < DefaultField.BoardSize - 1) CheckCell(x + 1, y);
            if (y < DefaultField.BoardSize - 1) CheckCell(x, y + 1);
        }

        private static void CopyField(FieldCell[,] source, FieldCell[,] destination)
        {
            for (int y = 0; y < DefaultField.BoardSize; y++)
            {
                for (int x = 0; x < DefaultField.BoardSize; x++)
                {
                    destination[x, y] = source[x, y];
                }
            }
        }
    }
}
