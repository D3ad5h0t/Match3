using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.States
{
    public class GameState : State
    {
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
                    var position = new Vector2(72 * j, 144 + 72 * i);
                    var type = (GemType)random.Next(1, 6);
                    var cell = new FieldCell
                    {
                        Position = position,
                        Texture = _content.Load<Texture2D>(BackgroundType.Ground.SpritePath()),
                        IsEmpty = false
                    };

                    var gem = new Gem
                    {
                        Texture = _content.Load<Texture2D>(type.SpritePath()),
                        Type = type,
                        Position = position,
                        IsClicked = false,
                        IsLine = false
                    };
                    gem.Click += Gem_Click;
                    cell.Gem = gem;

                    result.Add(cell);
                }
            }

            return result;
        }

        private void Gem_Click(object sender, EventArgs e)
        {
            var gem = (Gem) sender;
            var fields = _elements.Where(x => x is FieldCell).ToList();

            foreach (var element in fields)
            {
                var currentGem = ((FieldCell)element).Gem;
                if (currentGem != gem)
                {
                    currentGem.IsClicked = false;
                    currentGem.Texture = _content.Load<Texture2D>(currentGem.Type.SpritePath());
                }
            }

            gem.IsClicked = !gem.IsClicked;
            gem.Texture = gem.IsClicked
                ? _content.Load<Texture2D>(gem.Type.SelectedSpritePath())
                : _content.Load<Texture2D>(gem.Type.SpritePath());
        }
    }
}
