using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Match3.Elements.FieldCell;

namespace Match3.Core.Models
{
    public class CurrentMove
    {
        public FieldCell FirstCell { get; set; }

        public FieldCell SecondCell { get; set; }


        public CurrentMove(FieldCell first, FieldCell second)
        {
            FirstCell = first;
            SecondCell = second;
        }
    }
}
