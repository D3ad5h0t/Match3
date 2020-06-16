using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Core.Controllers;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Controls.Button
{
    public class ButtonBuilder
    {
        private Button _button = new Button();
        private bool _isTypeSet = false;

        public ButtonBuilder AddType(ButtonType type)
        {
            _button.Type = type;
            _isTypeSet = true;
            
            return this;
        }

        public ButtonBuilder AddTextureByType()
        {
            if (!_isTypeSet)
            {
                throw new NullReferenceException();
            }

            _button.Texture = ContentController.GetTexture(_button.Type.SpritePath());

            return this;
        }

        public ButtonBuilder AddFontByType()
        {
            if (!_isTypeSet)
            {
                throw new NullReferenceException();
            }

            _button.Font = ContentController.GetFont(_button.Type.FontPath());

            return this;
        }

        public ButtonBuilder AddPosition(Vector2 position)
        {
            _button.Position = position;
            
            return this;
        }

        public ButtonBuilder AddText(string text)
        {
            _button.Text = text;

            return this;
        }

        public ButtonBuilder OnClick(EventHandler click)
        {
            _button.Click += click;

            return this;
        }

        public ButtonBuilder AddPenColor(Color color)
        {
            _button.PenColor = color;

            return this;
        }

        public Button Build() => _button;
    }
}
