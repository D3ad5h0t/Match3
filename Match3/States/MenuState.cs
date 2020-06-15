using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Controls;
using Match3.Core.Controllers;
using Match3.Elements;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.States
{
    public class MenuState : State
    {
        public MenuState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var background = new Background()
            {
                Texture = _content.Load<Texture2D>(BackgroundType.Sky.SpritePath()),
                Position = new Vector2(0, 0)
            };

            var buttonType = ButtonType.Yellow;
            var buttonTexture = _content.Load<Texture2D>(buttonType.SpritePath());
            var buttonFont = _content.Load<SpriteFont>(buttonType.FontPath());

            var startButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Play",
                Position = new Vector2(
                    game.Window.ClientBounds.Width / 2f - buttonType.ButtonWidth() / 2f,
                    game.Window.ClientBounds.Height / 2f - buttonType.ButtonHeight() / 2f)
            };

            startButton.Click += StartButton_Click;

            _elements = new List<Element>()
            {
                background,
                startButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
            {
                element.Update(gameTime);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
    }
}
