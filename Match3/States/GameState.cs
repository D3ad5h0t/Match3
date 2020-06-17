using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Controls;
using Match3.Core;
using Match3.Core.Controllers;
using Match3.Core.Models;
using Match3.Elements;
using Match3.Elements.Destroyer;
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
        private SpriteFont gameFont;
        private float _timer = DefaultSettings.Timer;
        private EndWindow _gameOverWindow = null;

        public static FieldCell[,] GameField = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
        public static CurrentMove Move = null;
        public static int Score = 0;
        public static List<FieldCell> ActivatedBombs = new List<FieldCell>();
        public static List<Destroyer> Destroyers = new List<Destroyer>();


        public GameState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            GameField = GetPlayingField();

            _elements = new List<Element>()
            {
                new Background
                {
                    Texture = ContentController.GetTexture(BackgroundType.Full.SpritePath()),
                    Position = Vector2.Zero
                }
            };

            gameFont = ContentController.GetFont("Fonts/galleryFont");



            //TODO Для тестирования - потом удалить!!!
            GameField[0, 0].Gem = GemsController.GetNewGem(GameField[0, 0].Position, GemType.HorizontalLine);
            GameField[1, 1].Gem = GemsController.GetNewGem(GameField[1, 1].Position, GemType.Bomb);
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }

            foreach (var cell in GameField)
            {
                cell.Draw(gameTime, spriteBatch);
            }

            foreach (var cell in GameField)
            {
                cell.Gem?.Draw(gameTime, spriteBatch);
            }

            foreach (var destroyer in Destroyers)
            {
                destroyer.Draw(gameTime, spriteBatch);
            }

            spriteBatch.DrawString(gameFont,
                $"Score: {Score.ToString()}",
                new Vector2(3, 3),
                Color.White);

            spriteBatch.DrawString(gameFont,
                $"Time: {Math.Ceiling(_timer).ToString()}",
                new Vector2(3, 40),
                Color.White);

            _gameOverWindow?.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }


        public override void Update(GameTime gameTime)
        {
            if (CheckGameTimer(gameTime)) return;

            FieldCellsConroller.MatchAndClear(GameField);

            //TODO Вернуть возвращение элементов если не произошло удаление


            foreach (var cell in GameField)
            {
                if (cell.Gem != null && cell.Gem.WasMoved)
                {
                    switch (cell.Gem.Type)
                    {
                        case GemType.Bomb:
                            ActivatedBombs.Add((FieldCell)cell.Clone());
                            GemsController.DeleteGem(cell);
                            break;
                        case GemType.VerticalLine:
                        case GemType.HorizontalLine:
                            BonusController.LaunchDestroyers(cell);
                            GemsController.DeleteGem(cell);
                            break;
                    }
                }
            }

            foreach (var destroyer in Destroyers)
            {
                destroyer.Update(gameTime);
            }

            BonusController.BlowActivatedBombs();
            BonusController.UseDestoyers();

            if (Destroyers.Count == 0)
            {
                GemsController.MoveGems(GameField);
                GemsController.GenerateNewGems(GameField);
            }
            else
            {
                return;
            }

            foreach (var cell in GameField)
            {
                cell.Update(gameTime);
                cell.Gem?.Update(gameTime);
            }
        }

        private bool CheckGameTimer(GameTime gameTime)
        {
            _gameOverWindow?.Update(gameTime);

            if (_timer > 0)
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (_gameOverWindow == null)
                {
                    _gameOverWindow = new EndWindow(_game, EndButton_Click);
                }

                return true;
            }

            return false;
        }


        private FieldCell[,] GetPlayingField()
        {
            FieldCell[,] result = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = FieldCellsConroller.GenerateNewFieldCell(j, i, random, Cell_Click);
                    result[i, j] = cell;
                }
            }

            return result;
        }


        private void Cell_Click(object sender, EventArgs e)
        {
            FieldCell clickedCell = (FieldCell)sender;

            if (clickedCell.Gem == null) return;

            if (_prevCell != null)
            {
                GemsController.SwapGems(clickedCell, _prevCell);
                Move = new CurrentMove(clickedCell, _prevCell);
                clickedCell.Gem.WasMoved = true;
                _prevCell.Gem.WasMoved = true;

                foreach (var cell in GameField)
                {
                    if (cell.Gem != null)
                    {
                        cell.Gem.IsClicked = false;
                        FieldCellsConroller.UpdateTexture(cell.Gem);
                    }
                }

                _prevCell = null;
            }
            else
            {
                clickedCell.Gem.IsClicked = true;
                FieldCellsConroller.UpdateTexture(clickedCell.Gem);
                _prevCell = clickedCell;
            }
        }


        private void EndButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }
    }
}
