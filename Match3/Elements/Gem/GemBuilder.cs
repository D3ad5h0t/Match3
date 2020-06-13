using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements.Gem
{
    public class GemBuilder
    {
        private Gem _gem = new Gem();

        public GemBuilder(GemType type)
        {
            _gem.Type = type;
        }

        public GemBuilder Texture(Texture2D texture)
        {
            _gem.Texture = texture;
            return this;
        }

        public GemBuilder Position(Vector2 position)
        {
            _gem.Position = position;
            return this;
        }

        public GemBuilder Clicked(bool isClicked)
        {
            _gem.IsClicked = isClicked;
            return this;
        }

        public GemBuilder Line(bool isLine)
        {
            _gem.IsLine = isLine;
            return this;
        }

        public Gem Build()
        {
            return _gem;
        }
    }
}
