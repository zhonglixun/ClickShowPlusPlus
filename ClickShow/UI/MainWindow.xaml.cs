using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ClickShow.Settings;
using ClickShow.Utility;
using Microsoft.Win32;
using System.Windows.Forms;
using Brush = System.Windows.Media.Brush;
using Exception = System.Exception;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseButtonState = ClickShow.Entities.MouseButtonState;
using Point = System.Drawing.Point;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using RadioButton = System.Windows.Controls.RadioButton;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

namespace ClickShow
{
    /// <summary>
    /// 单窗口设置界面 + 特效调度。
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const long UP_SHOW_DISTANCE = 200;  //移动超过多少像素后显示抬起特效
        private const long UP_TICKS_DELTA = 500;   //长按多久后抬起显示抬起特效

        private MouseHook.MouseHook _mouseHook;
        private readonly IList<ClickIndicator> _clickIndicators = new List<ClickIndicator>();
        private HoverDot _hoverDot = null;
        private System.Windows.Forms.NotifyIcon _notifyIcon = null;
        private bool _forceClose = false;
        private DebounceDispatcher _settingDebounceDispatcher;
        private bool _suppressRadioEvents;
        private bool _suppressHexSync;
        private volatile bool _captureArmed;
        private CaptureOverlay _captureOverlay;
        private bool _trayPauseRipples;
        private System.Windows.Forms.ToolStripMenuItem _pauseMenuItem;
        private Version _updateAvailable;
        private string _updateUrl;
        private string _updateAssetUrl;
        private string _updateAssetName;
        private string _updateAssetDigest;
        private long _updateAssetSize;
        private bool _isDownloading;
        private int _lastHoverX = int.MinValue;
        private int _lastHoverY = int.MinValue;
        private static readonly HttpClient UpdateHttpClient = new();
        private const string ReleasesPageUrl = "https://github.com/zhonglixun/ClickShowPlusPlus/releases";
        private static readonly string OwnProcessExe = (Process.GetCurrentProcess().ProcessName + ".exe").ToLowerInvariant();

        private readonly IDictionary<MouseButtons, Brush> _buttonBrushes =
            new Dictionary<MouseButtons, Brush>()
            {
                {MouseButtons.None, System.Windows.Media.Brushes.Black},
                {MouseButtons.Left, System.Windows.Media.Brushes.DodgerBlue},
                {MouseButtons.Middle, System.Windows.Media.Brushes.Green},
                {MouseButtons.Right, System.Windows.Media.Brushes.OrangeRed},
                {MouseButtons.XButton1, System.Windows.Media.Brushes.Gray},
                {MouseButtons.XButton2, System.Windows.Media.Brushes.BlueViolet},
            };

        private readonly IDictionary<MouseButtons, MouseButtonState> _buttonStates =
            new Dictionary<MouseButtons, MouseButtonState>()
            {
                {MouseButtons.Left, new MouseButtonState()},
                {MouseButtons.Middle, new MouseButtonState()},
                {MouseButtons.Right, new MouseButtonState()},
                {MouseButtons.XButton1, new MouseButtonState()},
                {MouseButtons.XButton2, new MouseButtonState()},
            };

        private AppSetting _setting;
        private AppearanceSetting _editing;
        private string _editingKey = "Light";
        private ObservableCollection<string> _processList = new ObservableCollection<string>();
        private bool _autoStart;

        public AppSetting Setting
        {
            get => _setting;
            private set { _setting = value; OnPropertyChanged(nameof(Setting)); OnPropertyChanged(nameof(Editing)); }
        }

        /// <summary>当前正在编辑的外观套（浅色/深色）。</summary>
        public AppearanceSetting Editing
        {
            get => _editing;
            private set { _editing = value; OnPropertyChanged(nameof(Editing)); }
        }

        public ObservableCollection<string> ProcessList
        {
            get => _processList;
            private set { _processList = value; OnPropertyChanged(nameof(ProcessList)); }
        }

        /// <summary>开机自启（注册表）。</summary>
        public bool AutoStart
        {
            get => _autoStart;
            set
            {
                _autoStart = value;
                SaveAutoStart();
                OnPropertyChanged(nameof(AutoStart));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = Math.Max(620, SystemParameters.WorkArea.Height - 40);
            MaxWidth = Math.Max(900, SystemParameters.WorkArea.Width - 40);
            DataContext = this;
            SourceInitialized += OnSourceInitialized;
            Closing += OnClosing;
            Closed += OnClosed;
            StateChanged += OnStateChanged;
            CreateNotifyIcon();
        }

        /// <summary>取当前语言的文案，找不到键时回退键名。</summary>
        private string L(string key)
        {
            return TryFindResource(key) as string ?? key;
        }

        #region 加载/保存

        /// <summary>启动后端（配置/托盘/钩子）。窗口保持隐藏，仅托盘常驻。</summary>
        public void StartBackend()
        {
            LoadSetting();
            LoadAutoStartStatus();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Title = $"ClickShow++ {L("settingsTitle")} {version.ToString(3)}";
            VersionText.Text = $"v{version.ToString(3)}";
            VersionPill.Text = "ClickShow++ " + version.ToString(3);
            SidebarVersion.Text = "v" + version.ToString(3);
            ApplySettings();

            _mouseHook = new MouseHook.MouseHook();
            _mouseHook.MouseDown += MouseHookOnMouseDown;
            _mouseHook.MouseMove += MouseHookOnMouseMove;
            _mouseHook.MouseUp += MouseHookOnMouseUp;
            _mouseHook.Start();
            if (!_mouseHook.IsStarted)
            {
                _notifyIcon.ShowBalloonTip(4000, "ClickShow", L("hookFailed"), System.Windows.Forms.ToolTipIcon.Error);
            }

            Task.Run(async () =>
            {
                await Task.Delay(1000 * 10);
                CheckUpdate();
            });
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            ApplyTitleBarTheme();
            ThemeWatcher.ThemeChanged += OnSystemThemeChanged;
        }

        private void OnSystemThemeChanged()
        {
            Dispatcher.Invoke(() => ApplyTitleBarTheme());
        }

        private void ApplyTitleBarTheme()
        {
            DwmHelper.ApplyDarkTitleBar(this, !ThemeWatcher.IsLightTheme);
        }

        private void LoadSetting()
        {
            var setting = SettingsStore.Load(out var notice);
            if (!string.IsNullOrEmpty(notice))
            {
                _notifyIcon.ShowBalloonTip(3000, "ClickShow", notice, System.Windows.Forms.ToolTipIcon.Warning);
            }
            if (string.IsNullOrEmpty(setting.Language))
            {
                // 首次启动：按系统语言解析并固化
                setting.Language = Languages.Resolve(null);
            }
            Setting = setting;
            HookSettingEvents(setting);
            BindSetting();
        }

        private void HookSettingEvents(AppSetting setting)
        {
            setting.PropertyChanged += (s, e) => OnSettingPropertyChanged();
            foreach (var appearance in new[] { setting.Light, setting.Dark })
            {
                appearance.PropertyChanged += (s, e) => OnSettingPropertyChanged();
                foreach (var bs in appearance.MouseButtonSettings.Values)
                {
                    bs.PropertyChanged += (s, e) => OnSettingPropertyChanged();
                }
            }
        }

        private void OnSettingPropertyChanged()
        {
            if (_settingDebounceDispatcher == null)
            {
                _settingDebounceDispatcher = new DebounceDispatcher();
            }
            _settingDebounceDispatcher.Debounce(100, o =>
            {
                SaveSettings();
                ApplySettings();
            });
        }

        /// <summary>把当前 Setting 绑定到界面（切套/导入/重置后调用）。</summary>
        private void BindSetting()
        {
            _suppressRadioEvents = true;
            ModeSystemRadio.IsChecked = Setting.ProfileMode == ProfileMode.System;
            ModeLightRadio.IsChecked = Setting.ProfileMode == ProfileMode.Light;
            ModeDarkRadio.IsChecked = Setting.ProfileMode == ProfileMode.Dark;
            _suppressRadioEvents = false;

            ProcessList = new ObservableCollection<string>(Setting.SilentProcesses);
            Editing = _editingKey == "Dark" ? Setting.Dark : Setting.Light;
            SyncAllHexBoxes();
            SetEasingSelection();
            SetPresetSelection();
            RipplesSwitch.IsChecked = !_trayPauseRipples;
            LanguageCombo.ItemsSource = Languages.NativeNames.Select(pair => pair.Value).ToList();
            LanguageCombo.SelectedIndex = Math.Max(0, Languages.NativeNames.FindIndex(pair => pair.Key == Setting.Language));
            OnPropertyChanged(nameof(Setting));
        }

        private void LanguageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Setting == null)
            {
                return;
            }
            if (LanguageCombo.SelectedIndex < 0 || LanguageCombo.SelectedIndex >= Languages.NativeNames.Count)
            {
                return;
            }
            var code = Languages.NativeNames[LanguageCombo.SelectedIndex].Key;
            if (code == Setting.Language)
            {
                return;
            }
            Setting.Language = code;
            App.ApplyLanguage(code);
            SetEasingSelection();
            SetPresetSelection();
            SaveSettings();
        }

        private void SetEasingSelection()
        {
            if (Editing == null)
            {
                return;
            }
            var map = new Dictionary<string, RadioButton>
            {
                { "Linear", EasingLinear },
                { "CubicOut", EasingCubic },
                { "ExponentialOut", EasingExpo },
                { "BackOut", EasingBack },
                { "ElasticOut", EasingElastic }
            };
            if (map.TryGetValue(Editing.RippleEasing ?? "Linear", out var radio))
            {
                radio.IsChecked = true;
            }
        }

        private void EasingRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (Editing == null || sender is not RadioButton radio || radio.Tag is not string code)
            {
                return;
            }
            if (Editing.RippleEasing != code)
            {
                Editing.RippleEasing = code;
            }
        }

        private void SetPresetSelection()
        {
            if (Editing == null)
            {
                return;
            }
            var map = new Dictionary<string, RadioButton>
            {
                { "Classic", PresetClassic },
                { "Neon", PresetNeon },
                { "Candy", PresetCandy },
                { "Ocean", PresetOcean },
                { "Sunset", PresetSunset },
                { "Custom", PresetCustom }
            };
            if (map.TryGetValue(Editing.RipplePreset ?? "Custom", out var radio))
            {
                radio.IsChecked = true;
            }
            else
            {
                PresetCustom.IsChecked = true;
            }
        }

        private void PresetRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (Editing == null || sender is not RadioButton radio || radio.Tag is not string code)
            {
                return;
            }
            if (code == Editing.RipplePreset)
            {
                return;
            }
            if (code != "Custom" && RippleThemePresets.TryGetColors(code, out var colors))
            {
                var order = new[] { MouseButtons.Left, MouseButtons.Middle, MouseButtons.Right, MouseButtons.XButton1, MouseButtons.XButton2 };
                for (var i = 0; i < order.Length; i++)
                {
                    if (Editing.MouseButtonSettings.TryGetValue(order[i], out var bs))
                    {
                        bs.Color = colors[i];
                    }
                }
            }
            Editing.RipplePreset = code;
        }

        private void SaveSettings()
        {
            try
            {
                SettingsStore.Save(Setting);
            }
            catch (Exception ex)
            {
                _notifyIcon.ShowBalloonTip(2000, "ClickShow", L("saveError") + ex.Message, System.Windows.Forms.ToolTipIcon.Warning);
            }
        }

        #endregion

        #region 配置方案/编辑切换

        private void ModeRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (_suppressRadioEvents || Setting == null)
            {
                return;
            }
            Setting.ProfileMode = (ProfileMode)Enum.Parse(typeof(ProfileMode), (string)((RadioButton)sender).Tag);
            ApplySettings();
            SaveSettings();
        }

        private void EditRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (Setting == null)
            {
                return;
            }
            _editingKey = (string)((RadioButton)sender).Tag;
            Editing = _editingKey == "Dark" ? Setting.Dark : Setting.Light;
            SyncAllHexBoxes();
            SetEasingSelection();
            SetPresetSelection();
        }

        #endregion

        #region 鼠标位置浮标

        private void UpdateHoverDot()
        {
            var appearance = Setting.ActiveAppearance;
            if (appearance.EnableHoverDot)
            {
                if (_hoverDot == null)
                {
                    _hoverDot = new HoverDot();
                }

                _hoverDot.Width = _hoverDot.Height = appearance.HoverDotSize;
                _hoverDot.SetDotBrush(appearance.HoverDotFill.ToBrush());
                _hoverDot.Show();
            }
            else
            {
                if (_hoverDot != null)
                {
                    _hoverDot.Close();
                    _hoverDot = null;
                }
            }
        }

        #endregion

        #region 点击波纹效果

        private ClickIndicator GetClickIndicatorWindow(double size)
        {
            var indicator = _clickIndicators.FirstOrDefault(x => x.IsIdle
                                                                 && x.Width > size - 1
                                                                 && x.Width < size + 1);
            if (indicator != null)
            {
                indicator.Prepare();
                KillDeadWindow();
                return indicator;
            }

            indicator = new ClickIndicator(size)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Topmost = true,
                ShowActivated = false
            };
            _clickIndicators.Add(indicator);
            KillDeadWindow();
            return indicator;
        }

        private void ShowIndicator(MouseButtons button, Point point, bool isDown)
        {
            try
            {
                var appearance = Setting.ActiveAppearance;
                // 先查目标显示器 DPI，一次落位，避免跨屏时尺寸/位置错乱
                var dpi = MonitorHelper.GetDpiScaleForPoint(point.X, point.Y);
                var indicator = GetClickIndicatorWindow(appearance.IndicatorSize);

                Brush brush = _buttonBrushes[button];
                indicator.Play(brush, isDown, appearance.RippleDurationMs / 1000.0, RippleEasingFactory.Create(appearance.RippleEasing));

                var size = (int)Math.Round(appearance.IndicatorSize * dpi);
                MoveWindowWrapper(indicator,
                    point.X - (int)(size / 2),
                    point.Y - (int)(size / 2), size, size);
            }
            catch
            {
            }
        }

        /// <summary>关闭长时间未使用的indicator</summary>
        private void KillDeadWindow()
        {
            var deadIndicators = _clickIndicators
                .Where(x => x.IsIdle && (Environment.TickCount - x.LastLiveTime) / 1000 > 5)
                .ToList();
            foreach (var i in deadIndicators)
            {
                i.Close();
                _clickIndicators.Remove(i);
            }
        }

        #endregion

        #region 波纹预览

        private void PreviewArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseButtons button;
            switch (e.ChangedButton)
            {
                case MouseButton.Left: button = MouseButtons.Left; break;
                case MouseButton.Middle: button = MouseButtons.Middle; break;
                case MouseButton.Right: button = MouseButtons.Right; break;
                case MouseButton.XButton1: button = MouseButtons.XButton1; break;
                case MouseButton.XButton2: button = MouseButtons.XButton2; break;
                default: return;
            }

            e.Handled = true;
            if (Editing == null || !Editing.MouseButtonSettings.TryGetValue(button, out var bs) || !bs.IsEnabled)
            {
                return;
            }

            SpawnPreviewRipple(e.GetPosition(PreviewArea), bs.Color.ToBrush());
        }

        private void SpawnPreviewRipple(System.Windows.Point center, Brush stroke)
        {
            var size = Editing.IndicatorSize;
            var circle = new System.Windows.Shapes.Ellipse
            {
                Width = size,
                Height = size,
                Stroke = stroke,
                StrokeThickness = 3,
                RenderTransformOrigin = new System.Windows.Point(0.5, 0.5)
            };
            var scale = new ScaleTransform(0.2, 0.2);
            circle.RenderTransform = scale;
            PreviewCanvas.Children.Add(circle);
            System.Windows.Controls.Canvas.SetLeft(circle, center.X - size / 2);
            System.Windows.Controls.Canvas.SetTop(circle, center.Y - size / 2);

            var duration = new Duration(TimeSpan.FromSeconds(Editing.RippleDurationMs / 1000.0));
            var fade = new DoubleAnimation(0, duration);
            var grow = new DoubleAnimation(1, duration);
            grow.EasingFunction = RippleEasingFactory.Create(Editing.RippleEasing);
            circle.BeginAnimation(OpacityProperty, fade);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, grow);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, grow);
            fade.Completed += (s, e) => PreviewCanvas.Children.Remove(circle);
        }

        /// <summary>色块按钮：打开系统取色对话框修改对应颜色。</summary>
        private void Swatch_Click(object sender, RoutedEventArgs e)
        {
            if (Editing == null)
            {
                return;
            }

            var key = (string)((Button)sender).Tag;
            ButtonSetting buttonSetting = null;
            string current;
            if (key == "HoverDotFill")
            {
                current = Editing.HoverDotFill;
            }
            else
            {
                var mb = (MouseButtons)Enum.Parse(typeof(MouseButtons), key);
                if (!Editing.MouseButtonSettings.TryGetValue(mb, out buttonSetting))
                {
                    return;
                }
                current = buttonSetting.Color;
            }

            var dlg = new ClickShow.UI.ColorPickerDialog(current) { Owner = this };
            if (dlg.ShowDialog() != true)
            {
                return;
            }

            var argb = dlg.SelectedColorHex;
            if (buttonSetting != null)
            {
                buttonSetting.Color = argb;
            }
            else
            {
                Editing.HoverDotFill = argb;
            }
        }

        private static System.Drawing.Color ToDrawingColor(string hex)
        {
            try
            {
                var m = (System.Windows.Media.Color)ColorConverter.ConvertFromString(hex);
                return System.Drawing.Color.FromArgb(m.A, m.R, m.G, m.B);
            }
            catch
            {
                return System.Drawing.Color.White;
            }
        }

        #region 十六进制颜色输入

        private void HexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressHexSync || Editing == null)
            {
                return;
            }
            var box = (TextBox)sender;
            var text = box.Text.Trim();
            if (IsValidHexColor(text))
            {
                SetColorByKey((string)box.Tag, NormalizeHex(text));
            }
        }

        private void HexBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SyncHexBox((TextBox)sender);
        }

        private void HexBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            e.Handled = true;
            SyncHexBox((TextBox)sender);
        }

        private void SyncHexBox(TextBox box)
        {
            _suppressHexSync = true;
            box.Text = GetColorByKey((string)box.Tag);
            box.CaretIndex = box.Text.Length;
            _suppressHexSync = false;
        }

        private void SyncAllHexBoxes()
        {
            foreach (var box in new[] { HexLeft, HexMiddle, HexRight, HexX1, HexX2, HexDot })
            {
                SyncHexBox(box);
            }
        }

        private static bool IsValidHexColor(string text)
        {
            if (text == null)
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(text, "^#?([0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");
        }

        private static string NormalizeHex(string text)
        {
            var hex = text.Trim().TrimStart('#').ToUpperInvariant();
            if (hex.Length == 6)
            {
                hex = "FF" + hex;
            }
            return "#" + hex;
        }

        private string GetColorByKey(string key)
        {
            if (Editing == null)
            {
                return "#FF808080";
            }
            if (key == "HoverDotFill")
            {
                return Editing.HoverDotFill;
            }
            return Editing.MouseButtonSettings.TryGetValue((MouseButtons)Enum.Parse(typeof(MouseButtons), key), out var bs)
                ? bs.Color
                : "#FF808080";
        }

        private void SetColorByKey(string key, string value)
        {
            if (Editing == null)
            {
                return;
            }
            if (key == "HoverDotFill")
            {
                Editing.HoverDotFill = value;
                return;
            }
            if (Editing.MouseButtonSettings.TryGetValue((MouseButtons)Enum.Parse(typeof(MouseButtons), key), out var bs))
            {
                bs.Color = value;
                // 手动改色后不再匹配预设
                Editing.RipplePreset = "Custom";
            }
        }

        #endregion

        #endregion

        #region 鼠标hook处理

        private void MouseHookOnMouseDown(object sender, MouseEventArgs e)
        {
            if (_captureArmed)
            {
                return;
            }

            var point = e.Location;
            var button = e.Button;
            var suppressed = _trayPauseRipples || SilenceChecker.ShouldSuppress(Setting);

            _buttonStates[button].DownPosition = point;
            _buttonStates[button].DownTimeTicks = Environment.TickCount;
            _buttonStates[button].Suppressed = suppressed;
            if (suppressed)
            {
                return;
            }

            var appearance = Setting.ActiveAppearance;
            if (!appearance.EnableClickCircle || !appearance.MouseButtonSettings[e.Button].IsEnabled)
            {
                return;
            }

            Dispatcher.InvokeAsync(() => { ShowIndicator(button, point, true); });
        }

        private void MouseHookOnMouseUp(object sender, MouseEventArgs e)
        {
            if (_captureArmed)
            {
                return;
            }

            var downState = _buttonStates[e.Button];
            var suppressed = downState.Suppressed || _trayPauseRipples || SilenceChecker.ShouldSuppress(Setting);
            downState.Suppressed = false;
            if (suppressed)
            {
                return;
            }

            var appearance = Setting.ActiveAppearance;
            if (!appearance.EnableClickCircle || !appearance.MouseButtonSettings[e.Button].IsEnabled)
            {
                return;
            }

            var point = e.Location;
            if (((point.X - downState.DownPosition.X) * (point.X - downState.DownPosition.X)
                 + (point.Y - downState.DownPosition.Y) * (point.Y - downState.DownPosition.Y)) > UP_SHOW_DISTANCE * UP_SHOW_DISTANCE
                || (Environment.TickCount > (downState.DownTimeTicks + UP_TICKS_DELTA)))
            {
                Dispatcher.InvokeAsync(() => { ShowIndicator(e.Button, point, false); });
            }
        }

        private void MouseHookOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X == _lastHoverX && e.Location.Y == _lastHoverY)
            {
                return;
            }
            _lastHoverX = e.Location.X;
            _lastHoverY = e.Location.Y;
            var appearance = Setting.ActiveAppearance;
            if (appearance.EnableHoverDot && _hoverDot != null)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    var size = (int)(appearance.HoverDotSize * _hoverDot.GetDpiScale());
                    MoveWindowWrapper(_hoverDot,
                        e.Location.X - (int)(size / 2),
                        e.Location.Y - (int)(size / 2), size, size);
                });
            }
        }

        #endregion

        #region 指定程序静默

        private void AddProcess(string name)
        {
            name = (name ?? "").Trim().ToLowerInvariant();
            if (name.Length == 0)
            {
                return;
            }
            if (!name.EndsWith(".exe"))
            {
                name += ".exe";
            }
            if (Setting.SilentProcesses.Contains(name))
            {
                return;
            }
            Setting.SilentProcesses.Add(name);
            ProcessList.Add(name);
            SaveSettings();
        }

        private void AddProcess_Click(object sender, RoutedEventArgs e)
        {
            AddProcess(AddProcessBox.Text);
            AddProcessBox.Text = "";
        }

        private void AddProcessBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddProcess(AddProcessBox.Text);
                AddProcessBox.Text = "";
            }
        }

        private void BrowseProcess_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "程序 (*.exe)|*.exe"
            };
            if (dlg.ShowDialog(this) == true)
            {
                AddProcess(System.IO.Path.GetFileName(dlg.FileName));
            }
        }

        private void CaptureForeground_Click(object sender, RoutedEventArgs e)
        {
            if (_captureArmed)
            {
                DisarmCapture();
                return;
            }

            _captureArmed = true;
            CaptureHint.Visibility = Visibility.Visible;
            // 保持动态资源引用，语言切换时按钮文字跟随更新
            CaptureButton.SetResourceReference(ContentProperty, "captureCancel");

            _captureOverlay = new CaptureOverlay();
            _captureOverlay.Captured += OnCaptureOverlayCaptured;
            _captureOverlay.Cancelled += DisarmCapture;
            _captureOverlay.Show();
        }

        private void DisarmCapture()
        {
            _captureArmed = false;
            if (_captureOverlay != null)
            {
                _captureOverlay.Close();
                _captureOverlay = null;
            }
            CaptureHint.Visibility = Visibility.Collapsed;
            CaptureButton.SetResourceReference(ContentProperty, "captureStart");
        }

        private void OnCaptureOverlayCaptured(System.Windows.Point screenPoint)
        {
            if (_captureOverlay != null)
            {
                _captureOverlay.Close();
                _captureOverlay = null;
            }
            DisarmCapture();
            var exe = SilenceChecker.GetProcessExeAtPoint((int)screenPoint.X, (int)screenPoint.Y);
            if (string.IsNullOrEmpty(exe) || exe == OwnProcessExe)
            {
                _notifyIcon.ShowBalloonTip(2500, "ClickShow", L("captureNone"), System.Windows.Forms.ToolTipIcon.Info);
                return;
            }
            AddProcess(exe);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _captureArmed)
            {
                DisarmCapture();
                e.Handled = true;
            }
        }

        private void ChipRemove_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is string name)
            {
                Setting.SilentProcesses.Remove(name);
                ProcessList.Remove(name);
                SaveSettings();
            }
        }

        #endregion

        #region 通用

        // The path to the key where Windows looks for startup applications
        private readonly RegistryKey _runKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private const string AppName = "ClickShow";

        private void LoadAutoStartStatus()
        {
            try
            {
                var existing = _runKey.GetValue(AppName) as string;
                if (string.IsNullOrEmpty(existing))
                {
                    _autoStart = false;
                }
                else
                {
                    // 旧安装路径已删除时，把自启指向修复为当前程序
                    var target = existing.Trim('"');
                    if (!File.Exists(target))
                    {
                        var current = Process.GetCurrentProcess().MainModule?.FileName;
                        if (!string.IsNullOrEmpty(current))
                        {
                            _runKey.SetValue(AppName, current);
                        }
                    }
                    _autoStart = true;
                }
                OnPropertyChanged(nameof(AutoStart));
            }
            catch (Exception ex)
            {
                MessageBox.Show(L("autoStartLoadError") + ex.Message);
            }
        }

        private void SaveAutoStart()
        {
            try
            {
                if (_autoStart)
                {
                    _runKey.SetValue(AppName, Process.GetCurrentProcess().MainModule?.FileName ?? AppName);
                }
                else
                {
                    _runKey.DeleteValue(AppName, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(L("autoStartSaveError") + ex.Message);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "ClickShow 配置 (*.json)|*.json",
                FileName = "clickshow-setting.json"
            };
            if (dlg.ShowDialog(this) == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, MyJsonConverter.Serialize(Setting));
                    _notifyIcon.ShowBalloonTip(2000, "ClickShow", L("exportDone") + dlg.FileName, System.Windows.Forms.ToolTipIcon.Info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(L("exportFail") + ex.Message);
                }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { Filter = "ClickShow 配置 (*.json)|*.json|旧版配置 (*.setting)|*.setting" };
            if (dlg.ShowDialog(this) != true)
            {
                return;
            }
            try
            {
                var setting = SettingsStore.ImportFrom(dlg.FileName);
                var keepLanguage = Setting.Language;
                Setting = setting;
                Setting.Language = keepLanguage;
                HookSettingEvents(setting);
                BindSetting();
                ApplySettings();
                SaveSettings();
                _notifyIcon.ShowBalloonTip(2000, "ClickShow", L("importDone"), System.Windows.Forms.ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(L("importFail") + ex.Message);
            }
        }

        private void ResetDefault_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(L("resetConfirm"), "ClickShow", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }
            var keepLanguage = Setting.Language;
            Setting = new AppSetting();
            Setting.Language = keepLanguage;
            HookSettingEvents(Setting);
            BindSetting();
            ApplySettings();
            SaveSettings();
        }

        #endregion

        #region 版本更新

        private void CheckUpdate()
        {
            try
            {
                var json = UpdateHttpClient.GetStringAsync(
                    "https://api.github.com/repos/zhonglixun/ClickShowPlusPlus/releases/latest").Result;
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var tag = doc.RootElement.GetProperty("tag_name").GetString() ?? "";
                var url = doc.RootElement.GetProperty("html_url").GetString() ?? ReleasesPageUrl;
                var match = System.Text.RegularExpressions.Regex.Match(tag, @"d+(?:.d+){1,3}");
                if (!match.Success)
                {
                    throw new FormatException("unrecognized version tag: " + tag);
                }
                var newVersion = new Version(match.Value);
                var current = Assembly.GetExecutingAssembly().GetName().Version;

                string assetUrl = null, assetName = null;
                string assetDigest = null;
                long assetSize = 0;
                if (doc.RootElement.TryGetProperty("assets", out var assets))
                {
                    var lite = new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Length < 10L * 1024 * 1024;
                    foreach (var asset in assets.EnumerateArray())
                    {
                        var name = asset.GetProperty("name").GetString() ?? "";
                        if (!name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || !name.Contains("Setup"))
                        {
                            continue;
                        }
                        var isLite = name.Contains("-lite");
                        if (assetUrl == null || isLite == lite)
                        {
                            assetUrl = asset.GetProperty("browser_download_url").GetString();
                            assetName = name;
                            assetDigest = asset.TryGetProperty("digest", out var digest) ? digest.GetString() : null;
                            assetSize = asset.GetProperty("size").GetInt64();
                            if (isLite == lite)
                            {
                                break;
                            }
                        }
                    }
                }

                Dispatcher.InvokeAsync(() =>
                {
                    _updateUrl = url;
                    _updateAssetUrl = assetUrl;
                    _updateAssetName = assetName;
                    _updateAssetDigest = assetDigest;
                    _updateAssetSize = assetSize;
                    if (newVersion > current)
                    {
                        _updateAvailable = newVersion;
                        UpdateTitle.Text = string.Format(L("updateFound"), tag);
                        UpdateSub.Text = L("updateAvailable");
                        UpdateButton.Content = L("updateDownload");
                    }
                    else
                    {
                        _updateAvailable = null;
                        UpdateTitle.Text = L("updateLatest");
                        UpdateSub.Text = "ClickShow++ " + current.ToString(3);
                        UpdateButton.Content = L("updateCheck");
                    }
                });
            }
            catch (Exception)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    UpdateTitle.Text = L("updateCheck");
                    UpdateSub.Text = L("checkFailed");
                    UpdateButton.Content = L("updateCheck");
                });
            }
        }

        private void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_updateAvailable != null && !string.IsNullOrEmpty(_updateAssetUrl))
            {
                _ = DownloadAndInstallAsync();
                return;
            }
            if (_updateAvailable != null && !string.IsNullOrEmpty(_updateUrl))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(_updateUrl) { UseShellExecute = true });
                }
                catch
                {
                }
                return;
            }
            Task.Run(() => CheckUpdate());
        }

        private async Task DownloadAndInstallAsync()
        {
            if (_isDownloading)
            {
                return;
            }
            _isDownloading = true;
            UpdateButton.IsEnabled = false;
            UpdateProgress.Visibility = Visibility.Visible;
            UpdateProgress.Value = 0;

            var target = Path.Combine(Path.GetTempPath(), _updateAssetName ?? "ClickShow-Setup.exe");
            try
            {
                using var response = await UpdateHttpClient.GetAsync(_updateAssetUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var total = response.Content.Headers.ContentLength ?? _updateAssetSize;

                await using var source = await response.Content.ReadAsStreamAsync();
                await using var destination = File.Create(target);
                var buffer = new byte[81920];
                long received = 0;
                var lastPercent = -1;
                int count;
                while ((count = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await destination.WriteAsync(buffer, 0, count);
                    received += count;
                    var percent = total > 0 ? (int)(received * 100 / total) : 0;
                    if (percent != lastPercent)
                    {
                        lastPercent = percent;
                        var done = received / 1048576.0;
                        var all = total / 1048576.0;
                        _ = Dispatcher.InvokeAsync(() =>
                        {
                            UpdateProgress.Value = percent;
                            UpdateSub.Text = $"{percent}% · {done:0.0}/{all:0.0} MB";
                        });
                    }
                }

                if (!string.IsNullOrEmpty(_updateAssetDigest))
                {
                    var expected = _updateAssetDigest.StartsWith("sha256:")
                        ? _updateAssetDigest.Substring(7)
                        : _updateAssetDigest;
                    using var sha = SHA256.Create();
                    await using var check = File.OpenRead(target);
                    var actual = Convert.ToHexString(await sha.ComputeHashAsync(check));
                    if (!actual.Equals(expected, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Delete(target);
                        throw new InvalidDataException("checksum mismatch");
                    }
                }

                _ = Dispatcher.InvokeAsync(() =>
                {
                    UpdateSub.Text = L("updateDownloading");
                    try
                    {
                        Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
                    }
                    catch
                    {
                        UpdateSub.Text = L("updateFailed");
                        UpdateButton.IsEnabled = true;
                        return;
                    }
                    // 安装器需要覆盖文件，先退出托盘实例
                    _forceClose = true;
                    Close();
                });
            }
            catch (InvalidDataException)
            {
                _ = Dispatcher.InvokeAsync(() =>
                {
                    UpdateSub.Text = L("updateVerifyFailed");
                    UpdateButton.IsEnabled = true;
                    UpdateProgress.Visibility = Visibility.Collapsed;
                });
            }
            catch
            {
                _ = Dispatcher.InvokeAsync(() =>
                {
                    UpdateSub.Text = L("updateFailed");
                    UpdateButton.IsEnabled = true;
                    UpdateProgress.Visibility = Visibility.Collapsed;
                });
            }
            finally
            {
                _isDownloading = false;
            }
        }

        #endregion

        #region 托盘图标

        private void CreateNotifyIcon()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            using (Stream iconStream = System.Windows.Application
                       .GetResourceStream(new Uri(
                           $"pack://application:,,,/{Assembly.GetEntryAssembly().GetName().Name};component/clickshow.ico"))
                       .Stream)
            {
                _notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            }

            _notifyIcon.BalloonTipText = "ClickShow++\n" + L("trayBalloon");
            _notifyIcon.MouseClick += NotifyIconOnClick;
            _notifyIcon.Visible = true;

            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            var openItem = new System.Windows.Forms.ToolStripMenuItem(L("trayOpenSettings"), null, (sender, args) => ShowSettingsWindow());
            var pauseItem = new System.Windows.Forms.ToolStripMenuItem(L("showRipples"))
            {
                CheckOnClick = true,
                Checked = true
            };
            _pauseMenuItem = pauseItem;
            pauseItem.CheckedChanged += (sender, args) =>
            {
                var enabled = pauseItem.Checked;
                _trayPauseRipples = !enabled;
                if (RipplesSwitch != null && RipplesSwitch.IsChecked != enabled)
                {
                    RipplesSwitch.IsChecked = enabled;
                }
            };
            contextMenu.Items.Add(openItem);
            contextMenu.Items.Add(pauseItem);
            contextMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            var menuItem = new System.Windows.Forms.ToolStripMenuItem(L("trayExit"), null, (sender, args) =>
            {
                _forceClose = true;
                this.Close();
            });
            contextMenu.Items.Add(menuItem);
            _notifyIcon.ContextMenuStrip = contextMenu;
            // 钩子死亡时“暂停波纹”没有意义，置灰
            contextMenu.Opening += (sender, args) =>
            {
                pauseItem.Enabled = _mouseHook != null && _mouseHook.IsStarted;
            };
        }

        private void NotifyIconOnClick(object sender, EventArgs e)
        {
            if (e is System.Windows.Forms.MouseEventArgs me && me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ShowSettingsWindow();
            }
        }

        private void RipplesSwitch_Changed(object sender, RoutedEventArgs e)
        {
            var enabled = RipplesSwitch.IsChecked == true;
            _trayPauseRipples = !enabled;
            if (_pauseMenuItem != null && _pauseMenuItem.Checked != enabled)
            {
                _pauseMenuItem.Checked = enabled;
            }
        }

        private void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (AppearancePage == null || SilencePage == null || GeneralPage == null)
            {
                return;
            }
            var tag = (sender as RadioButton)?.Tag as string;
            AppearancePage.Visibility = tag == "Appearance" ? Visibility.Visible : Visibility.Collapsed;
            SilencePage.Visibility = tag == "Silence" ? Visibility.Visible : Visibility.Collapsed;
            GeneralPage.Visibility = tag == "General" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenRepo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://github.com/zhonglixun/ClickShowPlusPlus") { UseShellExecute = true });
            }
            catch
            {
            }
        }

        /// <summary>从托盘或第二个实例唤出设置窗口。</summary>
        public void ShowSettingsWindow()
        {
            this.Show();
            this.Activate();
            this.WindowState = System.Windows.WindowState.Normal;
        }

        #endregion

        #region 窗口事件

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!_forceClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            SaveSettings();
            if (_mouseHook != null)
            {
                _mouseHook.MouseMove -= MouseHookOnMouseMove;
                _mouseHook.MouseDown -= MouseHookOnMouseDown;
                _mouseHook.MouseUp -= MouseHookOnMouseUp;
                _mouseHook.Stop();
            }

            _captureArmed = false;

            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }

            foreach (var x in _clickIndicators)
            {
                x.Close();
            }

            _hoverDot?.Close();
            _captureOverlay?.Close();
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        public void MoveWindowWrapper(Window window, int X, int Y, int nWidth, int nHeight)
        {
            var handle = new WindowInteropHelper(window).Handle;
            WindowHelper.SetWindowPos(handle,
                (IntPtr)WindowHelper.SpecialWindowHandles.HWND_TOPMOST,
                X, Y,
                nWidth, nHeight,
                WindowHelper.SetWindowPosFlags.SWP_NOACTIVATE
            );
        }

        #endregion

        #region 设置应用

        private void ApplySettings()
        {
            try
            {
                var appearance = Setting.ActiveAppearance;
                foreach (var pair in appearance.MouseButtonSettings)
                {
                    _buttonBrushes[pair.Key] = pair.Value.Color.ToBrush();
                }
                UpdateHoverDot();
            }
            catch (Exception ex)
            {
                _notifyIcon.ShowBalloonTip(2000, "ClickShow", L("applyError") + ex.Message, System.Windows.Forms.ToolTipIcon.Warning);
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
