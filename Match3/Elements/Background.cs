using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements
{
    public class Background : Element
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture, 
                Position, 
                new Rectangle(0, 0, 
                    DefaultWindow.Width, 
                    DefaultWindow.Height),
                Color.White);
        }
    }
}
