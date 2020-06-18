using System;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3.Elements.Button
{
    public class Button : Element
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private bool _isHovering;


        public event EventHandler Click;

        public Color PenColor { get; set; }
        public string Text { get; set; }
        public ButtonType Type { get; set; }
        public SpriteFont Font { get; set; }


        public Button()
        {
            
        }

        public Button(Texture2D texture, SpriteFont font)
        {
            Texture = texture;
            Font = font;
            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if (_isHovering)
            {
                color = Color.Gray;
            }

            spriteBatch.Draw(Texture, Rectangle, color);

            if (!string.IsNullOrWhiteSpace(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (Font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (Font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(Font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
