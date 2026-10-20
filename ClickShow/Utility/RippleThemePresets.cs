using System.Collections.Generic;

namespace ClickShow.Utility
{
    /// <summary>
    /// 波纹颜色预设主题。颜色顺序：左键 / 中键 / 右键 / X1 / X2（#AARRGGBB 不透明）。
    /// </summary>
    public static class RippleThemePresets
    {
        public static readonly string[] Ids =
        {
            "Classic", "Neon", "Candy", "Ocean", "Sunset", "Forest", "Berry", "Macaron"
        };

        private static readonly Dictionary<string, string[]> Colors = new Dictionary<string, string[]>
        {
            { "Classic",  new[] { "#FF3B82F6", "#FF22C55E", "#FFF97316", "#FF9CA3AF", "#FF8B5CF6" } },
            { "Neon",     new[] { "#FF39FF14", "#FFFF2D95", "#FF00E5FF", "#FFFFE600", "#FFB026FF" } },
            { "Candy",    new[] { "#FFFF6B9D", "#FF4ECDC4", "#FFFFD166", "#FF7C83FD", "#FF9BF6FF" } },
            { "Ocean",    new[] { "#FF0077B6", "#FF00B4D8", "#FF48CAE4", "#FF06D6A0", "#FF8ECAE6" } },
            { "Sunset",   new[] { "#FFFF5E5B", "#FFFF9F1C", "#FFFFD166", "#FFE76F51", "#FFF72585" } },
            { "Forest",   new[] { "#FF38B000", "#FFFFD60A", "#FF1B998B", "#FF95D5B2", "#FFD9822B" } },
            { "Berry",    new[] { "#FF7B2CBF", "#FF9D4EDD", "#FFC77DFF", "#FFFF6BD6", "#FFE56399" } },
            { "Macaron",  new[] { "#FFFF8FA3", "#FFFFC971", "#FF8EE3EF", "#FFB9FBC0", "#FFCDB4DB" } }
        };

        public static bool TryGetColors(string id, out string[] colors)
        {
            return Colors.TryGetValue(id, out colors);
        }
    }
}
