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

        private CurrentMove _move = null;


        public GameState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _gameField = GetPlayingField();

            _elements = new List<Element>()
            {
                new Background
                {
                    Texture = ContentController.GetTexture(BackgroundType.Full.SpritePath()),
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

            foreach (var cell in _gameField)
            {
                cell.Draw(gameTime, spriteBatch);
            }

            foreach (var cell in _gameField)
            {
                cell.Gem?.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            FieldCellsConroller.MatchAndClear(_gameField);
            GemsController.MoveGems(_gameField);


            if (_move != null)
            {
                if (_move.FirstCell?.Gem == null || _move.SecondCell?.Gem == null)
                {
                    ;
                }
                else
                {
                    GemsController.SwapGems(_move.FirstCell, _move.SecondCell);
                    _move = null;
                }
            }

            GemsController.GenerateNewGems(_gameField);

            foreach (var cell in _gameField)
            {
                cell.Update(gameTime);
                cell.Gem?.Update(gameTime);
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
                    var cell = FieldCellsConroller.GenerateNewFieldCell(j, i, random, Cell_Click);
                    result[i,j] = cell;
                }
            }

            return result;
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            FieldCell clickedCell = (FieldCell) sender;

            if (clickedCell.Gem == null) return;

            if (_prevCell != null)
            {
                GemsController.SwapGems(clickedCell, _prevCell);
                _move = new CurrentMove(clickedCell, _prevCell);

                foreach (var cell in _gameField)
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
    }


    public class CurrentMove
    {
        public FieldCell FirstCell { get; set; }

        public FieldCell SecondCell { get; set; }


        public CurrentMove(FieldCell first, FieldCell second)
        {
            FirstCell = first;
            SecondCell = second;
        }
    }
}
