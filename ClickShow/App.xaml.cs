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
            ApplyTheme(ThemeWatcher.IsLightTheme);
            ThemeWatcher.ThemeChanged += () => Dispatcher.Invoke(() => ApplyTheme(ThemeWatcher.IsLightTheme));
        }

        public static void ApplyTheme(bool light)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"Themes/{(light ? "Light" : "Dark")}.xaml", UriKind.Relative)
            };
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
