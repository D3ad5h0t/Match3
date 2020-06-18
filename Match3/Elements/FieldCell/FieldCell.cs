using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3.Elements.FieldCell
{
    [Serializable]
    public class FieldCell : Element
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public event EventHandler Click;

        public int Id { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public Gem.Gem Gem { get; set; }


        public void SetGemPosition()
        {
            Gem.Position = this.Position;
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
