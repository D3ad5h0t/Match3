using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Match3.States;
using Microsoft.Xna.Framework;

namespace Match3.Core.Controllers
{
    public static class GemsController
    {
        public static void DeleteGems(List<FieldCell> list)
        {
            foreach (var cell in list)
            {
                cell.Gem = null;
            }
        }

        public static void SwapGems(FieldCell firstCell, FieldCell secondCell)
        {
            var areNervy = FieldCellsConroller.AreCellsNerby(firstCell, secondCell);
            
            if (areNervy)
            {
                var gem = (Gem)firstCell.Gem.Clone();

                firstCell.Gem = (Gem)secondCell.Gem.Clone();
                firstCell.Gem.Position = firstCell.Position;

                secondCell.Gem = gem;
                secondCell.Gem.Position = secondCell.Position;
            }
        }

        public static void MoveGems(FieldCell[,] gameField)
        {
            for (int y = DefaultField.BoardSize - 1; y > 0; y--)
            {
                for (int x = 0; x < DefaultField.BoardSize; x++)
                {
                    if (gameField[y, x].Gem != null) continue;
                    if (gameField[y - 1, x].Gem == null) continue;

                    if (gameField[y - 1, x].Gem.Position != gameField[y, x].Position)
                    {
                        gameField[y - 1, x].Gem.Position = new Vector2(gameField[y - 1, x].Gem.Position.X, gameField[y - 1, x].Gem.Position.Y + DefaultSettings.Speed);
                    }
                    else
                    {
                        gameField[y, x].Gem = (Gem)gameField[y - 1, x].Gem.Clone();
                        gameField[y, x].SetGemPosition();
                        gameField[y - 1, x].Gem = null;
                    }
                }
            }
        }

        public static Gem GetNewGem(Vector2 position, GemType type)
        {
            Gem newGem = new Gem()
            {
                Position = position,
                Texture = ContentController.GetTexture(type.SpritePath()),
                IsClicked = false,
                ScorePrice = DefaultSettings.GemScore,
                Type = type
            };

            return newGem;
        }

        public static void GenerateNewGems(FieldCell[,] gameField)
        {
            Random random = new Random();

            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                if (gameField[0, i].Gem == null)
                {
                    gameField[0, i].Gem = GetNewGem(gameField[0, i].Position, (GemType) random.Next(1, 6));
                }
            }
        }
    }
}
