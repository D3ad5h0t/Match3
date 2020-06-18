using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3.Core;
using Match3.Core.Controllers;
using Match3.Core.DefaltConst;
using Match3.Enumerations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3.Elements.Destroyer
{
    public class Destroyer : Element
    {
        private DirectionType _direction;
        private CoordinateDirection _coordinate;

        public bool IsOutOfScreen;

        public Destroyer(Vector2 position, DirectionType direction, CoordinateDirection coordinate)
        {
            Position = position;
            _direction = direction;
            _coordinate = coordinate;

            Texture = direction == DirectionType.Horizontal
                ? ContentController.GetTexture(GemType.HorizontalLine.SelectedSpritePath())
                : ContentController.GetTexture(GemType.VerticalLine.SelectedSpritePath());
        }


        public override void Update(GameTime gameTime)
        {
            switch (_coordinate)
            {
                case CoordinateDirection.Right:
                    Position = new Vector2(Position.X + 2 * DefaultSettings.Speed, Position.Y);
                    break;
                case CoordinateDirection.Left:
                    Position = new Vector2(Position.X - 2 * DefaultSettings.Speed, Position.Y);
                    break;
                case CoordinateDirection.Up:
                    Position = new Vector2(Position.X, Position.Y + 2 * DefaultSettings.Speed);
                    break;
                case CoordinateDirection.Down:
                    Position = new Vector2(Position.X, Position.Y - 2 * DefaultSettings.Speed);
                    break;
            }
        }


        public override Rectangle Rectangle => new Rectangle(
            (int)Position.X + DefaultGem.DeltaWidth,
            (int)Position.Y + DefaultGem.DeltaHeight,
            DefaultGem.Width,
            DefaultGem.Height);


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                new Rectangle((int)Position.X, (int)Position.Y, DefaultGem.Width, DefaultGem.Height),
                new Rectangle(-DefaultGem.DeltaWidth, -DefaultGem.DeltaHeight, Texture.Width, Texture.Height),
                Color.White);
        }
    }
}
