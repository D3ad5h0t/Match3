using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Match3.Elements;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class FieldCellsConroller
    {
        private static FieldCell[,] _gameField = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
        private static FieldCell _currentCell;
        private static List<FieldCell> _collector = new List<FieldCell>();


        public static void UpdateTexture(Gem gem)
        {
            gem.Texture = gem.IsClicked
                ? ContentController.GetTexture(gem.Type.SelectedSpritePath())
                : ContentController.GetTexture(gem.Type.SpritePath());
        }

        public static bool AreCellsNerby(FieldCell firstCell, FieldCell secondCell) =>
            firstCell.Id + 1 == secondCell.Id ||
            firstCell.Id - 1 == secondCell.Id ||
            firstCell.Id + 8 == secondCell.Id ||
            firstCell.Id - 8 == secondCell.Id;

        public static void MatchAndClear(FieldCell[,] gameField)
        {
            CopyField(gameField, _gameField);
            _currentCell = null;
            _collector.Clear();

            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                for (int j = 0; j < DefaultField.BoardSize; j++)
                {
                    CheckCell(i, j);

                    ClearGameField(gameField);

                    _currentCell = null;
                    _collector.Clear();
                }
            }
        }

        private static void ClearGameField(FieldCell[,] gameField)
        {
            if (_collector.Count > 2)
            {
                var matchOnCol = _collector.GroupBy(x => x.Column).ToDictionary(e => e.Key, e => e.ToList());
                var matchOnRow = _collector.GroupBy(x => x.Row).ToDictionary(e => e.Key, e => e.ToList());

                foreach (var element in matchOnCol)
                {
                    if (element.Value.Count > 2)
                    {
                        GemsController.DeleteGems(element.Value, gameField);
                    }
                }

                foreach (var element in matchOnRow)
                {
                    if (element.Value.Count > 2)
                    {
                        GemsController.DeleteGems(element.Value, gameField);
                    }
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

        public static FieldCell GenerateNewFieldCell(int column, int row, Random random, EventHandler Cell_Click)
        {
            var cell = new FieldCell
            {
                Id = (column + 1) + 8 * row,
                Column = column,
                Row = row,
                Position = new Vector2(DefaultCell.Width * column, DefaultCell.Height * 2 + DefaultCell.Height * row),
                Texture = ContentController.GetTexture(BackgroundType.Ground.SpritePath())
            };

            cell.Click += Cell_Click;

            var type = (GemType)random.Next(1, 6);
            cell.Gem = GemsController.GetNewGem(cell.Position, type);

            return cell;
        }
    }
}