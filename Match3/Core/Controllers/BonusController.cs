using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Match3.Enumerations;
using Match3.States;

namespace Match3.Core.Controllers
{
    public static class BonusController
    {
        public static void BlowBomb(FieldCell cell, FieldCell[,] gameField)
        {
            foreach (var fieldCell in gameField)
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
                    if (fieldCell.Gem != null && fieldCell.Gem.Type == GemType.Bomb)
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
