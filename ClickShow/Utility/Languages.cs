using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ClickShow.Utility
{
    /// <summary>
    /// 支持的语言与系统语言解析。
    /// </summary>
    public static class Languages
    {
        public static readonly string[] Supported =
        {
            "zh-CN", "en-US", "ja-JP", "ko-KR", "es-ES",
            "fr-FR", "de-DE", "pt-BR", "ru-RU", "it-IT"
        };

        /// <summary>语言选择器显示用的原生名称（不随界面语言变化）。</summary>
        public static readonly List<KeyValuePair<string, string>> NativeNames = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("zh-CN", "简体中文"),
            new KeyValuePair<string, string>("en-US", "English"),
            new KeyValuePair<string, string>("ja-JP", "日本語"),
            new KeyValuePair<string, string>("ko-KR", "한국어"),
            new KeyValuePair<string, string>("es-ES", "Español"),
            new KeyValuePair<string, string>("fr-FR", "Français"),
            new KeyValuePair<string, string>("de-DE", "Deutsch"),
            new KeyValuePair<string, string>("pt-BR", "Português (Brasil)"),
            new KeyValuePair<string, string>("ru-RU", "Русский"),
            new KeyValuePair<string, string>("it-IT", "Italiano")
        };

        /// <summary>
        /// 解析语言代码：显式设置优先，否则按系统 UI 语言，无法匹配回退英语。
        /// </summary>
        public static string Resolve(string setting)
        {
            if (!string.IsNullOrEmpty(setting) && Supported.Contains(setting))
            {
                return setting;
            }

            var name = CultureInfo.CurrentUICulture.Name;
            if (name.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            {
                return "zh-CN";
            }

            switch (name.Length >= 2 ? name.Substring(0, 2).ToLowerInvariant() : "en")
            {
                case "ja": return "ja-JP";
                case "ko": return "ko-KR";
                case "es": return "es-ES";
                case "fr": return "fr-FR";
                case "de": return "de-DE";
                case "pt": return "pt-BR";
                case "ru": return "ru-RU";
                case "it": return "it-IT";
                default: return "en-US";
            }
        }
    }
}
