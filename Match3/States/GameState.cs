using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Core.Controllers;
using Match3.Core.DefaltConst;
using Match3.Core.Models;
using Match3.Elements;
using Match3.Elements.Background;
using Match3.Elements.Destroyer;
using Match3.Elements.EndWindow;
using Match3.Elements.FieldCell;
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
        public static CurrentMove Move = new CurrentMove();
        public static int Score = 0;
        public static List<FieldCell> ActivatedBombs = new List<FieldCell>();
        public static List<Destroyer> Destroyers = new List<Destroyer>();


        public GameState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            GameField = GetPlayingField();

            _elements = new List<Element>();
            _elements.Add(new Background(TextureController.GetTexture(BackgroundType.Full.SpritePath()), Vector2.Zero));

            gameFont = TextureController.GetFont("Fonts/galleryFont");
            _gameOverWindow = new EndWindow(_game, EndButton_Click);



            //TODO Для тестирования - потом удалить!!!
            GameField[0, 0].Gem = GemsController.GetNewGem(GameField[0, 0].Position, GemType.HorizontalLine);
            GameField[1, 1].Gem = GemsController.GetNewGem(GameField[1, 1].Position, GemType.Bomb);
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            DrawElements(gameTime, spriteBatch);

            DrawGameField(gameTime, spriteBatch);

            DrawGems(gameTime, spriteBatch);

            DrawScoreText(spriteBatch);

            DrawTimerValue(spriteBatch);

            DrawDestroeyers(gameTime, spriteBatch);

            if (IsTimerEnded(gameTime))
            {
                DrawGameOverWindow(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawElements(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }

        private void DrawGameOverWindow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _gameOverWindow?.Draw(gameTime, spriteBatch);
        }

        private void DrawTimerValue(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(gameFont, $"Time: {Math.Ceiling(_timer).ToString()}", new Vector2(3, 40), Color.White);
        }

        private void DrawScoreText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(gameFont, $"Score: {Score.ToString()}", new Vector2(3, 3), Color.White);
        }

        private static void DrawDestroeyers(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var destroyer in Destroyers)
            {
                destroyer.Draw(gameTime, spriteBatch);
            }
        }

        private static void DrawGems(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cell in GameField)
            {
                cell.Gem?.Draw(gameTime, spriteBatch);
            }
        }

        private static void DrawGameField(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var cell in GameField)
            {
                cell.Draw(gameTime, spriteBatch);
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (IsTimerEnded(gameTime))
            {
                _gameOverWindow?.Update(gameTime);

                return;
            }

            GameBoardConroller.MatchAndClear(GameField);

            UpdateActivatedBonuses();

            UpdateDestroyers(gameTime);

            BonusController.BlowActivatedBombs();

            BonusController.UseDestoyers();

            if (Destroyers.Count == 0)
            {
                GemsController.MoveGems(GameField);

                GemsController.GenerateNewGems(GameField);
            }

            if (IsCurrentMoveImposible()) return;

            foreach (var cell in GameField)
            {
                cell.Update(gameTime);
                cell.Gem?.Update(gameTime);
            }
        }

        private static bool IsCurrentMoveImposible()
        {
            if (Move.FirstCell != null && Move.SecondCell != null)
            {
                if (Move.FirstCell.Gem == null || Move.SecondCell.Gem == null)
                {
                    return true;
                }

                if (Move.FirstCell.Gem.WasMoved && Move.SecondCell.Gem.WasMoved)
                {
                    GemsController.SwapGems(Move.FirstCell, Move.SecondCell);
                    Move.FirstCell = null;
                    Move.SecondCell = null;
                }
            }

            return false;
        }

        private static void UpdateDestroyers(GameTime gameTime)
        {
            foreach (var destroyer in Destroyers)
            {
                destroyer.Update(gameTime);
            }
        }

        private static void UpdateActivatedBonuses()
        {
            foreach (var cell in GameField)
            {
                if (cell.Gem != null && cell.Gem.WasMoved)
                {
                    CheckGemAndActivateBonus(cell);
                }
            }
        }

        private static void CheckGemAndActivateBonus(FieldCell cell)
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

        private bool IsTimerEnded(GameTime gameTime)
        {
            bool isTimerEnded = true;

            if (_timer > 0)
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                isTimerEnded = false;
            }

            return isTimerEnded;
        }


        private FieldCell[,] GetPlayingField()
        {
            FieldCell[,] result = new FieldCell[DefaultField.BoardSize, DefaultField.BoardSize];
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = GameBoardConroller.GenerateNewFieldCell(j, i, random, Cell_Click);
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
                clickedCell.Gem.WasMoved = true;
                _prevCell.Gem.WasMoved = true;
                Move = new CurrentMove(clickedCell, _prevCell);
                GemsController.SwapGems(Move.FirstCell, Move.SecondCell);

                foreach (var cell in GameField)
                {
                    if (cell.Gem != null) cell.Gem.IsClicked = false;
                    GemsController.UpdateGemTexture(cell.Gem);
                }

                _prevCell = null;
            }
            else
            {
                clickedCell.Gem.IsClicked = true;
                GemsController.UpdateGemTexture(clickedCell.Gem);
                _prevCell = clickedCell;
            }
        }


        private void EndButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }
    }
}
