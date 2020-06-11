using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Models.Types
{
    public enum Background
    {
        Green,
        Blue,
        Red
    }

    public static class Extentions
    {
        public static string SpritePath(this Background background)
        {
            string path;

            switch (background)
            {
                case Background.Green:
                    path = "Backgrounds/Green";
                    break;
                case Background.Blue:
                    path = "Backgrounds/Blue";
                    break;
                case Background.Red:
                    path = "Backgrounds/Red";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(background), background, null);
            }

            return path;
        }
    }
}
