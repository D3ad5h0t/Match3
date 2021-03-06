﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Enumerations
{
    public enum GemType
    {
        Bomb,
        Earth,
        Fire,
        Meat,
        Shield,
        Water,
        HorizontalLine,
        VerticalLine
    }

    public static class GemTypeExtensions
    {
        public static string SpritePath(this GemType type)
        {
            List<string> result = new List<string>()
            {
                "Elements/Bomb/7",
                "Elements/Earth/7",
                "Elements/Fire/7",
                "Elements/Meat/7",
                "Elements/Shield/7",
                "Elements/Water/7",
                "Elements/Line/horizontal",
                "Elements/Line/vertical"
            };

            return GetComparisonResult(type, result);
        }

        public static string SelectedSpritePath(this GemType type)
        {
            List<string> result = new List<string>
            {
                "Elements/Bomb/ElementSelected",
                "Elements/Earth/ElementSelected",
                "Elements/Fire/ElementSelected",
                "Elements/Meat/ElementSelected",
                "Elements/Shield/ElementSelected",
                "Elements/Water/ElementSelected",
                "Elements/Line/horizontalSelected",
                "Elements/Line/verticalSelected"
            };

            return GetComparisonResult(type, result);
        }

        private static T GetComparisonResult<T>(GemType type, List<T> results)
        {
            if ((int)type >= results.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (type)
            {
                case GemType.Bomb: return results[0];
                case GemType.Earth: return results[1];
                case GemType.Fire: return results[2];
                case GemType.Meat: return results[3];
                case GemType.Shield: return results[4];
                case GemType.Water: return results[5];
                case GemType.HorizontalLine: return results[6];
                case GemType.VerticalLine: return results[7];
                default: return results[0];
            }
        }
    }
}
