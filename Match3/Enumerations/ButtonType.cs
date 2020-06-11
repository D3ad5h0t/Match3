using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Enumerations
{
    public enum ButtonType
    {
        Yellow,
        Green,
        Orange
    }

    public static class ButtonTypeExtensions
    {
        public static string SpritePath(this ButtonType type)
        {
            List<string> results = new List<string>()
            {
                "UI/Button1",
                "UI/Button2",
                "UI/Button6"
            };

            return GetComparisonResult(type, results);
        }

        public static string FontPath(this ButtonType type)
        {
            List<string> results = new List<string>()
            {
                "Fonts/galleryFont"
            };

            return GetComparisonResult(type, results);
        }

        public static float ButtonHeight(this ButtonType type)
        {
            List<float> results = new List<float>()
            {
                69f
            };

            return GetComparisonResult(type, results);
        }

        public static float ButtonWidth(this ButtonType type)
        {
            List<float> results = new List<float>()
            {
                214f
            };

            return GetComparisonResult(type, results);
        }

        private static T GetComparisonResult<T>(ButtonType type, List<T> results)
        {
            if ((int) type >= results.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (type)
            {
                case ButtonType.Yellow: return results[0];
                case ButtonType.Green: return results[1];
                case ButtonType.Orange: return results[2];
                default: return results[0];
            }
        }
    }
}
