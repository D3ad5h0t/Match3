using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Controls;
using Match3.Models.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Controllers
{
    public static class GameController
    {
        public static List<Component> GenerateComponents(ContentManager content)
        {
            List<Component> list = new List<Component>();
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var type = (CrystalType) random.Next(0, 4);

                    var crystalTexture = content.Load<Texture2D>(type.SimpleSpritePath());
                    var crystal = new Crystal(crystalTexture, content)
                    {
                        Position = new Vector2(60 + i * 80, 10 + j * 80),
                        Type = type
                    };

                    list.Add(crystal);
                }
            }

            return list;
        }
    }
}
