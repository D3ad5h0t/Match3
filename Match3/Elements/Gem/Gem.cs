using System;
using Match3.Core;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Elements.Gem
{
    [Serializable]
    public class Gem : Element
    {
        public GemType Type;

        public bool IsClicked { get; set; }

        public bool IsLine { get; set; }

        public bool IsBomb { get; set;}

        public int ScorePrice { get; set; }


        public override Rectangle Rectangle => new Rectangle(
            (int) Position.X + DefaultGem.DeltaWidth, 
            (int) Position.Y + DefaultGem.DeltaHeight, 
            DefaultGem.Width, 
            DefaultGem.Height);


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Rectangle((int)Position.X, (int)Position.Y, DefaultGem.Width, DefaultGem.Height),
                new Rectangle(-DefaultGem.DeltaWidth, -DefaultGem.DeltaHeight, Texture.Width, Texture.Height),
                Color.White);
        }
    }
}
