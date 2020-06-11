using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Models.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Controls
{
    class Crystal : Component
    {
        private Texture2D _texture;

        public CrystalType Type { get; set; }

        public Vector2 Position { get; set; }

        public bool IsClicked { get; set; }

        public Rectangle Rectangle => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            100,
            100);

        public event EventHandler Click;

        public Crystal(Texture2D texture, ContentManager content)
        {
            _texture = texture;
        }

        public Texture2D Texture
        {
            set => _texture = value;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,
                Rectangle,
                new Rectangle(0, 0, _texture.Width, _texture.Height),
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
                    IsClicked = !IsClicked;
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
