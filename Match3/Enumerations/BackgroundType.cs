using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Enumerations
{
    public enum BackgroundType
    {
        Sky,
        Grass,
        Ground,
        Full
    }

    public static class BackgroundTypeExtensions
    {
        public static string SpritePath(this BackgroundType type)
        {
            List<string> result = new List<string>()
            {
                "Backgrounds/5",
                "UI/Bg3",
                "UI/ElementBg",
                "Backgrounds/Full"
            };

            return GetComparisonResult(type, result);
        }

        private static T GetComparisonResult<T>(BackgroundType type, List<T> results)
        {
            if ((int)type >= results.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            switch (type)
            {
                case BackgroundType.Sky: return results[0];
                case BackgroundType.Grass: return results[1];
                case BackgroundType.Ground: return results[2];
                case BackgroundType.Full: return results[3];
                default: return results[0];
            }
        }
    }
}
