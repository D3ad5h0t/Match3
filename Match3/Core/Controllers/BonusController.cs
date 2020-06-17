using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Match3.Elements.Destroyer;
using Match3.Enumerations;
using Match3.States;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class BonusController
    {
        public static void BlowActivatedBombs()
        {
            if (GameState.ActivatedBombs.Count > 0)
            {
                foreach (var bomb in GameState.ActivatedBombs)
                {
                    if (bomb.Gem.Timer > 0)
                    {
                        bomb.Gem.Timer -= DefaultSettings.BombTick;
                    }
                    else
                    {
                        BlowBomb(bomb);
                        bomb.Gem.WasMoved = false;
                    }
                }

                GameState.ActivatedBombs.RemoveAll(x => !x.Gem.WasMoved);
            }
        }

        public static void BlowBomb(FieldCell cell)
        {
            foreach (var fieldCell in GameState.GameField)
            {
                if ((cell.Row + 1 == fieldCell.Row && cell.Column == fieldCell.Column) ||
                    (cell.Row - 1 == fieldCell.Row && cell.Column == fieldCell.Column) ||
                    (cell.Column + 1 == fieldCell.Column && cell.Row == fieldCell.Row) ||
                    (cell.Column - 1 == fieldCell.Column && cell.Row == fieldCell.Row) ||
                    (cell.Row + 1 == fieldCell.Row && cell.Column + 1 == fieldCell.Column) ||
                    (cell.Row - 1 == fieldCell.Row && cell.Column + 1 == fieldCell.Column) ||
                    (cell.Row + 1 == fieldCell.Row && cell.Column - 1 == fieldCell.Column) ||
                    (cell.Row - 1 == fieldCell.Row && cell.Column - 1 == fieldCell.Column))
                {
                    if (fieldCell.Gem != null && (fieldCell.Gem.Type == GemType.Bomb || 
                                                  fieldCell.Gem.Type == GemType.HorizontalLine ||
                                                  fieldCell.Gem.Type == GemType.VerticalLine))
                    {
                        fieldCell.Gem.WasMoved = true;
                    }
                    else
                    {
                        GemsController.DeleteGem(fieldCell);
                    }
                }
            }
        }

        public static void LaunchDestroyers(FieldCell cell)
        {
            switch (cell.Gem.Type)
            {
                case GemType.HorizontalLine:
                    GameState.Destroyers.Add(new Destroyer(cell.Position, DirectionType.Horizontal, CoordinateDirection.Right));
                    GameState.Destroyers.Add(new Destroyer(cell.Position, DirectionType.Horizontal, CoordinateDirection.Left));
                    break;
                case GemType.VerticalLine:
                    GameState.Destroyers.Add(new Destroyer(cell.Position, DirectionType.Vertical, CoordinateDirection.Up));
                    GameState.Destroyers.Add(new Destroyer(cell.Position, DirectionType.Vertical, CoordinateDirection.Down));
                    break;
            }
        }

        public static void UseDestoyers()
        {
            foreach (var destroyer in GameState.Destroyers)
            {
                if (destroyer.Position.X >= DefaultWindow.Width ||
                    destroyer.Position.X <= 0 ||
                    destroyer.Position.Y >= DefaultWindow.Height ||
                    destroyer.Position.Y <= 0)
                {
                    destroyer.IsOutOfScreen = true;
                }

                RemoveCrossedGems(destroyer);
            }

            GameState.Destroyers.RemoveAll(x => x.IsOutOfScreen);
        }

        private static void RemoveCrossedGems(Destroyer destroyer)
        {
            foreach (var fieldCell in GameState.GameField)
            {
                if (fieldCell.Rectangle.Intersects(destroyer.Rectangle))
                {
                    if (fieldCell.Gem != null &&
                        (fieldCell.Gem.Type == GemType.Bomb ||
                         fieldCell.Gem.Type == GemType.HorizontalLine ||
                         fieldCell.Gem.Type == GemType.VerticalLine))
                    {
                        fieldCell.Gem.WasMoved = true;
                    }
                    else
                    {
                        GemsController.DeleteGem(fieldCell);
                    }
                }
            }
        }
    }
}
