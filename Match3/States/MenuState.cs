using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Core.Controllers;
using Match3.Elements;
using Match3.Elements.Background;
using Match3.Elements.Button;
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
            var background = new Background(TextureController.GetTexture(BackgroundType.Sky.SpritePath()), Vector2.Zero);

            var btnType = ButtonType.Yellow;
            var startBtn = new ButtonBuilder()
                .AddType(btnType)
                .AddTextureByType()
                .AddFontByType()
                .AddText("Play")
                .AddPenColor(Color.Black)
                .OnClick(StartButton_Click)
                .AddPosition(new Vector2(
                    game.Window.ClientBounds.Width / 2f - btnType.ButtonWidth() / 2f,
                    game.Window.ClientBounds.Height / 2f - btnType.ButtonHeight() / 2f))
                .Build();

            _elements = new List<Element>()
            {
                background,
                startBtn
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
