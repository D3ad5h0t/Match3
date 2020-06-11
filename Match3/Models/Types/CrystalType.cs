using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Models.Types
{
    public enum CrystalType
    {
        Green,
        Grey,
        Orange,
        Purple,
        Red
    }

    public static class Extensions
    {
        public static string SimpleSpritePath(this CrystalType type)
        {
            string path;

            switch (type)
            {
                case CrystalType.Green:
                    path = "Controls/Candies/1";
                    break;
                case CrystalType.Grey:
                    path = "Controls/Candies/2";
                    break;
                case CrystalType.Orange:
                    path = "Controls/Candies/3";
                    break;
                case CrystalType.Purple:
                    path = "Controls/Candies/4";
                    break;
                case CrystalType.Red:
                    path = "Controls/Candies/5";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return path;
        }

        public static string ShinySpritePath(this CrystalType type)
        {
            string path;

            switch (type)
            {
                case CrystalType.Green:
                    path = "Controls/Crystals/Shiny/Green";
                    break;
                case CrystalType.Grey:
                    path = "Controls/Crystals/Shiny/Grey";
                    break;
                case CrystalType.Orange:
                    path = "Controls/Crystals/Shiny/Orange";
                    break;
                case CrystalType.Purple:
                    path = "Controls/Crystals/Shiny/Purple";
                    break;
                case CrystalType.Red:
                    path = "Controls/Crystals/Shiny/Red";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return path;
        }
    }
}
