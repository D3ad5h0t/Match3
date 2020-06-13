using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements
{
    public class FieldCell : Element
    {
        public int Id { get; set; }

        public bool IsEmpty { get; set; }

        public Gem.Gem Gem { get; set; }
    }
}
