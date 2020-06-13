using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements
{
    public abstract class Element
    {
        public Texture2D Texture;

        public Vector2 Position { get; set; }

        public virtual Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {

        }


        public Element Clone() => (Element)this.MemberwiseClone();
    }
}
