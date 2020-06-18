using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Match3.Core.DefaltConst;
using Match3.Elements;
using Match3.Elements.FieldCell;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Match3.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class GameBoardConroller
    {
        private static FieldCell _currentCell;
        private static FieldCell[,] _gameField = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
        private static List<FieldCell> _collector = new List<FieldCell>();


        public static bool IsCellsNearby(FieldCell firstCell, FieldCell secondCell)
        {
            return firstCell.Id + 1 == secondCell.Id || firstCell.Id - 1 == secondCell.Id || firstCell.Id + 8 == secondCell.Id || firstCell.Id - 8 == secondCell.Id;
        }

        public static void MatchAndClear(FieldCell[,] gameField)
        {
            int emptyCount = gameField.OfType<FieldCell>().Count(cell => cell.Gem == null);

            if (emptyCount != 0) return;

            CopyField(gameField, _gameField);
            _currentCell = null;
            _collector.Clear();

            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                for (int j = 0; j < DefaultField.BoardSize; j++)
                {
                    CheckCell(i, j);

                    ChangeGameBorderState(gameField);

                    _currentCell = null;
                    _collector.Clear();
                }
            }
        }


        private static void ChangeGameBorderState(FieldCell[,] gameField)
        {
            if (_collector.Count > 2)
            {
                var matchOnCol = _collector.GroupBy(x => x.Column)
                                           .ToDictionary(e => e.Key, e => e.ToList())
                                           .Values.Where(x => x.Count > 2)
                                           .ToList();

                var matchOnRow = _collector.GroupBy(x => x.Row)
                                           .ToDictionary(e => e.Key, e => e.ToList())
                                           .Values.Where(x => x.Count > 2)
                                           .ToList();

                SetCoincidenceResults(matchOnCol, DirectionType.Vertical);
                SetCoincidenceResults(matchOnRow, DirectionType.Horizontal);
            }
        }


        private static void SetCoincidenceResults(List<List<FieldCell>> list, DirectionType type)
        {
            foreach (var element in list)
            {
                if (element.Count > 2 && element.Count < 4)
                {
                    GemsController.DeleteGems(element);
                }
                else if (element.Count > 3 && element.Count < 5)
                {
                    var gemType = type == DirectionType.Horizontal ? GemType.HorizontalLine : GemType.VerticalLine;

                    AddNewBonus(element, gemType);
                }
                else if (element.Count > 4)
                {
                    AddNewBonus(element, GemType.Bomb);
                }
            }
        }

        private static void AddNewBonus(List<FieldCell> element, GemType gemType)
        {
            FieldCell bonus = null;

            if (GameState.Move != null && GameState.Move.FirstCell != null && element.Contains(GameState.Move.FirstCell))
            {
                bonus = GameState.Move.FirstCell;
            }

            if (GameState.Move != null && GameState.Move.SecondCell != null && element.Contains(GameState.Move.SecondCell))
            {
                bonus = GameState.Move.SecondCell;
            }

            bonus = bonus ?? element.Last();
            bonus.Gem = GemsController.GetNewGem(bonus.Position, gemType);

            element.Remove(bonus);

            GemsController.DeleteGems(element);
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

        public static FieldCell GenerateNewFieldCell(int column, int row, Random random, EventHandler onClick)
        {
            var cell = new FieldCell
            {
                Id = (column + 1) + 8 * row,
                Column = column,
                Row = row,
                Position = new Vector2(DefaultCell.Width * column, DefaultCell.Height * 2 + DefaultCell.Height * row),
                Texture = TextureController.GetTexture(BackgroundType.Ground.SpritePath())
            };

            cell.Click += onClick;

            var type = (GemType)random.Next(1, 6);
            cell.Gem = GemsController.GetNewGem(cell.Position, type);

            return cell;
        }
    }
}