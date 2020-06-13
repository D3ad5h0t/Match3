using System;
using System.Runtime.Remoting.Messaging;
using Match3.Elements;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class GemConroller
    {
        public static void Swap(FieldCell firstCell, FieldCell secondCell)
        {
            bool areNerby = AreCellsNerby(firstCell, secondCell);
            if (areNerby)
            {
                var middleField = (FieldCell)firstCell.Clone();

                firstCell.Gem = (Gem)secondCell.Gem.Clone();
                firstCell.Gem.Position = firstCell.Position;

                secondCell.Gem = (Gem)middleField.Gem;
                secondCell.Gem.Position = secondCell.Position;
            }
        }

        public static void UpdateTexture(Gem gem, ContentManager content)
        {
            gem.Texture = gem.IsClicked
               ? content.Load<Texture2D>(gem.Type.SelectedSpritePath())
               : content.Load<Texture2D>(gem.Type.SpritePath());
        }

        private static bool AreCellsNerby(FieldCell firstCell, FieldCell secondCell) => 
            firstCell.Id + 1 == secondCell.Id || 
            firstCell.Id - 1 == secondCell.Id || 
            firstCell.Id + 8 == secondCell.Id || 
            firstCell.Id - 8 == secondCell.Id;
        
    }
}
