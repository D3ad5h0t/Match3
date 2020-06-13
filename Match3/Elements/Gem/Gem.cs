using System;
using Match3.Core;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Elements.Gem
{
    public class Gem : Element
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public GemType Type;

        public event EventHandler Click;

        public bool IsClicked { get; set; }

        public bool IsLine { get; set; }

        public override Rectangle Rectangle => new Rectangle(
            (int) Position.X + DefaultGem.DeltaWidth, 
            (int) Position.Y + DefaultGem.DeltaHeight, 
            DefaultGem.GameWidth, 
            DefaultGem.GameHeight);


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Rectangle((int)Position.X, (int)Position.Y, DefaultGem.GameWidth, DefaultGem.GameHeight),
                new Rectangle(-DefaultGem.DeltaWidth, -DefaultGem.DeltaHeight, Texture.Width, Texture.Height),
                Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mousePosition = new Point(_currentMouse.X, _currentMouse.Y);

            if (Rectangle.Contains(mousePosition))
            {
                if (_previousMouse.LeftButton == ButtonState.Released && _currentMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
