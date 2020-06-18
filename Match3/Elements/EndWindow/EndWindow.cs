using System;
using Match3.Core.Controllers;
using Match3.Core.DefaltConst;
using Match3.Elements.Button;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements.EndWindow
{
    public class EndWindow : Element
    {
        private Button.Button _submitBtn;
        private SpriteFont _gameFont;
        private string _titleText;

        public EndWindow(Game game, EventHandler handler)
        {
            Texture = TextureController.GetTexture(PopUpWindow.TexturePath);
            Position = GetPosition(game);

            _gameFont = TextureController.GetFont(DefaultSettings.FontPath);
            _titleText = "Game Over!";

            var btnType = ButtonType.Yellow;
            _submitBtn = new ButtonBuilder().AddType(btnType)
                                            .AddFontByType()
                                            .AddTextureByType()
                                            .AddPenColor(Color.Black)
                                            .AddText("Ok")
                                            .OnClick(handler)
                                            .AddPosition(GetCenterPositionToButton(btnType))
                                            .Build();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, GetDefaultSize(), GetTextureResize(), Color.White);

            DrawTitleText(spriteBatch);

            _submitBtn.Draw(gameTime, spriteBatch);
        }

        private void DrawTitleText(SpriteBatch spriteBatch)
        {
            if (!string.IsNullOrWhiteSpace(_titleText))
            {
                var x = Rectangle.Center.X / 2f + _gameFont.MeasureString(_titleText).X / 5f;
                var y = Rectangle.Center.Y / 2f + _gameFont.MeasureString(_titleText).Y;

                spriteBatch.DrawString(_gameFont, _titleText, new Vector2(x, y), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _submitBtn.Update(gameTime);
        }

        private Vector2 GetPosition(Game game)
        {
            var x = game.Window.ClientBounds.Width / 2f - PopUpWindow.Width / 2f;
            var y = game.Window.ClientBounds.Height / 2f - PopUpWindow.Height / 2f;

            return new Vector2(x, y);
        }

        private Vector2 GetCenterPositionToButton(ButtonType type)
        {
            var x = Rectangle.Center.X / 2f + type.ButtonWidth() / 12f;
            var y = Rectangle.Center.Y / 2f + (int) (type.ButtonHeight() * 1.5);

            return new Vector2(x, y);
        }

        private Rectangle GetDefaultSize()
        {
            return new Rectangle((int) Position.X, (int) Position.Y, PopUpWindow.Width, PopUpWindow.Height);
        }

        private Rectangle GetTextureResize()
        {
            return new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
    }
}
