using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Models
{
    public class GameField
    {
        private List<FieldCell> _field = new List<FieldCell>();

        public GameField()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _field.Add(new FieldCell
                    {
                        IsFilled = false,
                        X = j,
                        Y = i
                    });
                }
            }
        }

        public List<FieldCell> GeField => _field;
    }
}
