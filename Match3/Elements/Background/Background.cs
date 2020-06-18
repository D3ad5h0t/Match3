using Match3.Core.DefaltConst;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements.Background
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
