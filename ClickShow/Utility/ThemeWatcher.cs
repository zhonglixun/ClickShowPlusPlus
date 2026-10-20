using System;
using Microsoft.Win32;

namespace ClickShow.Utility
{
    /// <summary>
    /// 监听系统应用主题（浅色/深色）。
    /// </summary>
    public static class ThemeWatcher
    {
        public static bool IsLightTheme
        {
            get
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                    if (key?.GetValue("AppsUseLightTheme") is int value)
                    {
                        return value != 0;
                    }
                }
                catch
                {
                }

                return true;
            }
        }

        public static event Action ThemeChanged;

        static ThemeWatcher()
        {
            SystemEvents.UserPreferenceChanged += (sender, e) =>
            {
                if (e.Category == UserPreferenceCategory.General)
                {
                    ThemeChanged?.Invoke();
                }
            };
        }
    }
}
