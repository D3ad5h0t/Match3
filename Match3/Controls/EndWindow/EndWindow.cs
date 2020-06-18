using System;
using Match3.Controls.Button;
using Match3.Core;
using Match3.Core.Controllers;
using Match3.Core.DefaltConst;
using Match3.Elements;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Controls.EndWindow
{
    public class EndWindow : Element
    {
        private Button.Button _submitBtn;
        private SpriteFont _gameFont;
        private string _titleText;

        public EndWindow(Game game, EventHandler handler)
        {
            _gameFont = ContentController.GetFont("Fonts/galleryFont");
            Texture = ContentController.GetTexture("UI/popupBox");

            Position = new Vector2(
                game.Window.ClientBounds.Width / 2f - PopUpWindow.Width / 2f,
                game.Window.ClientBounds.Height / 2f - PopUpWindow.Height / 2f);
            _titleText = "Game Over!";

            var btnType = ButtonType.Yellow;
            _submitBtn = new ButtonBuilder().AddType(btnType)
                                            .AddFontByType()
                                            .AddTextureByType()
                                            .AddPenColor(Color.Black)
                                            .AddText("Ok")
                                            .OnClick(handler)
                                            .AddPosition(new Vector2(Rectangle.Center.X / 2f + btnType.ButtonWidth() / 12f, Rectangle.Center.Y / 2f + (int)(btnType.ButtonHeight() * 1.5)))
                                            .Build();
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                new Rectangle((int)Position.X, (int)Position.Y, PopUpWindow.Width, PopUpWindow.Height),
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White);

            if (!string.IsNullOrWhiteSpace(_titleText))
            {
                var x = Rectangle.Center.X / 2f + _gameFont.MeasureString(_titleText).X / 5f;
                var y = Rectangle.Center.Y / 2f + _gameFont.MeasureString(_titleText).Y;

                spriteBatch.DrawString(_gameFont, _titleText, new Vector2(x, y), Color.White);
            }

            _submitBtn.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _submitBtn.Update(gameTime);
        }
    }
}
