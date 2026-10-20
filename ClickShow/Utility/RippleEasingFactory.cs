using System;
using System.Windows.Media.Animation;

namespace ClickShow.Utility
{
    /// <summary>
    /// 扩散样式代码 → WPF 缓动函数。Linear 返回 null（即默认匀速）。
    /// </summary>
    public static class RippleEasingFactory
    {
        public static readonly string[] Supported =
        {
            "Linear", "CubicOut", "ExponentialOut", "BackOut", "ElasticOut"
        };

        public static IEasingFunction Create(string code)
        {
            switch (code)
            {
                case "CubicOut":
                    return new CubicEase { EasingMode = EasingMode.EaseOut };
                case "ExponentialOut":
                    return new ExponentialEase { EasingMode = EasingMode.EaseOut, Exponent = 5 };
                case "BackOut":
                    return new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.35 };
                case "ElasticOut":
                    return new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 2, Springiness = 6 };
                default:
                    return null;
            }
        }
    }
}
