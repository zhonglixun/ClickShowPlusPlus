using System;
using System.Threading;
using System.Threading.Tasks;
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
            ThemeWatcher.ThemeChanged += () => Dispatcher.Invoke(() => ApplyTheme(ThemeWatcher.IsLightTheme));
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
        }
    }
}
