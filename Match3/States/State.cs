using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.States
{
    public abstract class State
    {
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Match3Game _game;
        protected List<Element> _elements;


        public State(Match3Game game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _game = game;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
