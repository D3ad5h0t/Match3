using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Core.Controllers
{
    public static class TextureController
    {
        private static ContentManager _content;

        public static void SetContent(ContentManager content)
        {
            _content = content;
        }

        public static Texture2D GetTexture(string path) => _content.Load<Texture2D>(path);

        public static SpriteFont GetFont(string path) => _content.Load<SpriteFont>(path);
    }
}
