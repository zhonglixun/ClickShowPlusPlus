using System;
using System.Windows;
using ClickShow.Utility;

namespace ClickShow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 语言按设置（未设置时按系统语言）解析；槽位 0 = 语言，槽位 1 = 主题
            ApplyLanguage(Languages.Resolve(SettingsStore.ReadLanguageCode()));
            ApplyTheme(ThemeWatcher.IsLightTheme);
            ThemeWatcher.ThemeChanged += () => Dispatcher.Invoke(() => ApplyTheme(ThemeWatcher.IsLightTheme));
        }

        public static void ApplyLanguage(string code)
        {
            var lang = new ResourceDictionary { Source = new Uri($"Languages/{code}.xaml", UriKind.Relative) };
            var dicts = Current.Resources.MergedDictionaries;
            if (dicts.Count > 0)
            {
                dicts[0] = lang;
            }
            else
            {
                dicts.Insert(0, lang);
            }
        }

        public static void ApplyTheme(bool light)
        {
            var theme = new ResourceDictionary { Source = new Uri($"Themes/{(light ? "Light" : "Dark")}.xaml", UriKind.Relative) };
            var dicts = Current.Resources.MergedDictionaries;
            if (dicts.Count >= 2)
            {
                dicts[1] = theme;
            }
            else
            {
                dicts.Add(theme);
            }
        }
    }
}
