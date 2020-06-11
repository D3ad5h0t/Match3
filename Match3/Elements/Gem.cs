using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Elements
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

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            if (mouseRectangle.Intersects(Rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
