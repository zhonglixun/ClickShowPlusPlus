using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ClickShow.Utility;
using Microsoft.Win32;

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
            // 单实例保护：双开会导致双重波纹与配置文件写入竞态
            _instanceMutex = new Mutex(true, "Local\\ClickShow.SingleInstance", out var createdNew);
            if (!createdNew)
            {
                // 已有实例在运行：通知它显示设置窗口，然后本进程退出
                try
                {
                    EventWaitHandle.OpenExisting("Local\\ClickShow.ShowSettings").Set();
                }
                catch (Exception)
                {
                }
                Shutdown();
                return;
            }
            // 语言按设置（未设置时按系统语言）解析；槽位 0 = 语言，槽位 1 = 主题
            ApplyLanguage(Languages.Resolve(SettingsStore.ReadLanguageCode()));
            ApplyTheme(ThemeWatcher.IsLightTheme);
            ThemeWatcher.ThemeChanged += () => Dispatcher.Invoke(() =>
            {
                ApplyTheme(ThemeWatcher.IsLightTheme);
                ApplySystemAccent(ThemeWatcher.IsLightTheme);
            });
            // 静默启动：只进托盘不弹窗；托盘左键或第二个实例可唤出设置
            var showSettings = new EventWaitHandle(false, EventResetMode.AutoReset, "Local\\ClickShow.ShowSettings");
            Task.Run(() =>
            {
                while (showSettings.WaitOne())
                {
                    Dispatcher.Invoke(() => (MainWindow as MainWindow)?.ShowSettingsWindow());
                }
            });
            var mainWindow = new MainWindow();
            mainWindow.StartBackend();
            MainWindow = mainWindow;
        }

        private Mutex _instanceMutex;

        protected override void OnExit(ExitEventArgs e)
        {
            _instanceMutex?.ReleaseMutex();
            _instanceMutex?.Dispose();
            base.OnExit(e);
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
            ApplySystemAccent(light);
        }

        /// <summary>跟随 Windows 强调色；读不到时回落到 Windows 11 默认蓝。</summary>
        private static void ApplySystemAccent(bool light)
        {
            var accent = ReadAccentColor(light);
            var hover = Blend(accent, light ? Colors.Black : Colors.White, 0.14);
            var soft = Blend(accent, (Current.Resources["Bg"] as SolidColorBrush)?.Color ?? Colors.White, light ? 0.88 : 0.82);
            var fg = Luminance(accent) > 0.62 ? Colors.Black : Colors.White;

            Current.Resources["Accent"] = Frozen(accent);
            Current.Resources["AccentHover"] = Frozen(hover);
            Current.Resources["AccentSoft"] = Frozen(soft);
            Current.Resources["AccentForeground"] = Frozen(fg);
        }

        private static Color ReadAccentColor(bool light)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"SoftwareMicrosoftWindowsDWM");
                if (key?.GetValue("AccentColor") is int v)
                {
                    return Color.FromRgb((byte)(v & 0xFF), (byte)((v >> 8) & 0xFF), (byte)((v >> 16) & 0xFF));
                }
            }
            catch
            {
            }
            return light ? Color.FromRgb(0x00, 0x67, 0xC0) : Color.FromRgb(0x4C, 0xC2, 0xFF);
        }

        private static Color Blend(Color a, Color b, double amountOfB)
        {
            byte Mix(byte x, byte y) => (byte)Math.Round(x * (1 - amountOfB) + y * amountOfB);
            return Color.FromRgb(Mix(a.R, b.R), Mix(a.G, b.G), Mix(a.B, b.B));
        }

        private static double Luminance(Color c)
        {
            return (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B) / 255.0;
        }

        private static SolidColorBrush Frozen(Color c)
        {
            var b = new SolidColorBrush(c);
            b.Freeze();
            return b;
        }
    }
}
