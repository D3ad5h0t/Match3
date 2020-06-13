using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Core;
using Match3.Core.Controllers;
using Match3.Elements;
using Match3.Elements.Gem;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.States
{
    public class GameState : State
    {
        private static FieldCell _prevCell = null;
        private static FieldCell[,] _gameField = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];

        public GameState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _gameField = GetPlayingField();
            _elements = new List<Element>
            {
                new Background
                {
                    Texture = _content.Load<Texture2D>(BackgroundType.Grass.SpritePath()),
                    Position = Vector2.Zero
                }
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                for (int j = 0; j < DefaultField.BoardSize; j++)
                {
                    _gameField[i, j].Draw(gameTime, spriteBatch);
                    _gameField[i, j].Gem.Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < DefaultField.BoardSize; i++)
            {
                for (int j = 0; j < DefaultField.BoardSize; j++)
                {
                    _gameField[i, j].Update(gameTime);
                    _gameField[i, j].Gem.Update(gameTime);
                }
            }
        }

        private FieldCell[,] GetPlayingField()
        {
            FieldCell[,] result = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = GetNewFieldCell(j, i, random);
                    result[i,j] = cell;
                }
            }

            return result;
        }

        private FieldCell GetNewFieldCell(int j, int i, Random random)
        {
            var cell = new FieldCell
            {
                Id = (j + 1) + 8 * i,
                Position = new Vector2(DefaultCell.Width * j, DefaultCell.Height * 2 + DefaultCell.Height * i),
                Texture = _content.Load<Texture2D>(BackgroundType.Ground.SpritePath()),
                IsEmpty = false,
            };

            cell.Click += Cell_Click;

            var type = (GemType)random.Next(1, 6);
            cell.Gem = new GemBuilder(type)
                .Position(cell.Position)
                .Texture(_content.Load<Texture2D>(type.SpritePath()))
                .Clicked(false)
                .Line(false)
                .Build();

            return cell;
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            FieldCell clickedCell = (FieldCell) sender;

            if (_prevCell != null)
            {
                GemConroller.Swap(clickedCell, _prevCell);

                foreach (var cell in _gameField)
                {
                    cell.Gem.IsClicked = false;
                    GemConroller.UpdateTexture(cell.Gem, _content);
                }

                _prevCell = null;
            }
            else
            {
                clickedCell.Gem.IsClicked = true;
                GemConroller.UpdateTexture(clickedCell.Gem, _content);
                _prevCell = clickedCell;
            }
        }
    }
}
