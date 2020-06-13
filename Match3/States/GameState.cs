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
        private static FieldCell _prevField = null;

        public GameState(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var field = GetPlayingField();
            _elements = new List<Element>
            {
                new Background
                {
                    Texture = _content.Load<Texture2D>(BackgroundType.Grass.SpritePath()),
                    Position = Vector2.Zero
                }
            };
            _elements.AddRange(field);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var element in _elements)
            {
                element.Draw(gameTime, spriteBatch);
                if (element is FieldCell)
                {
                    ((FieldCell)element).Gem.Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
            {
                element.Update(gameTime);
                if (element is FieldCell)
                {
                    ((FieldCell)element).Gem.Update(gameTime);
                }
            }
        }

        private List<Element> GetPlayingField()
        {
            List<Element> result = new List<Element>();
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = GetNewFieldCell(j, i, random);
                    result.Add(cell);
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
                IsEmpty = false
            };

            var type = (GemType)random.Next(1, 6);
            cell.Gem = new GemBuilder(type)
                .Position(cell.Position)
                .Texture(_content.Load<Texture2D>(type.SpritePath()))
                .Click(Gem_Click)
                .Clicked(false)
                .Line(false)
                .Build();

            return cell;
        }

        private void Gem_Click(object sender, EventArgs e)
        {
            var field = (FieldCell)_elements.Where(x => x is FieldCell)
                                            .FirstOrDefault(x => ((FieldCell)x).Gem == (Gem)sender);
            
            field.Gem.IsClicked = !field.Gem.IsClicked;
            
            if (_prevField != null)
            {
                if (_prevField == field)
                {
                    _prevField = null;
                }
                else
                {
                    GemConroller.Swap(field, _prevField);
                    _prevField = null;

                    foreach (var element in _elements.Where(x => x is FieldCell))
                    {
                        var gem = ((FieldCell)element).Gem;
                        gem.IsClicked = false;
                        GemConroller.UpdateTexture(gem, _content);
                    }
                }
            }
            else
            {
                _prevField = field;
            }

            GemConroller.UpdateTexture(field.Gem, _content);
        }
    }
}
