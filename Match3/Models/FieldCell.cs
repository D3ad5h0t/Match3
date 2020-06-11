using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Models
{
    public class FieldCell
    {
        public bool IsFilled { get; set; }

        public Texture2D Texture2D { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
