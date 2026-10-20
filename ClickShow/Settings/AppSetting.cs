using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Windows.Media;
using ClickShow.Utility;

namespace ClickShow.Settings
{
    /// <summary>
    /// 配置方案模式：跟随系统 / 始终浅色 / 始终深色。
    /// </summary>
    public enum ProfileMode
    {
        System,
        Light,
        Dark
    }

    /// <summary>
    /// 外观设置。浅色/深色各保存一套。
    /// </summary>
    public class AppearanceSetting : INotifyPropertyChanged
    {
        private bool _enableClickCircle = true;
        private bool _enableHoverDot = false;
        private Dictionary<MouseButtons, ButtonSetting> _mouseButtonSettings =
            new Dictionary<MouseButtons, ButtonSetting>()
            {
                {MouseButtons.Left, new ButtonSetting(Colors.DodgerBlue.ToArgb())},
                {MouseButtons.Middle, new ButtonSetting(Colors.Green.ToArgb())},
                {MouseButtons.Right, new ButtonSetting(Colors.OrangeRed.ToArgb())},
                {MouseButtons.XButton1, new ButtonSetting(Colors.Gray.ToArgb())},
                {MouseButtons.XButton2, new ButtonSetting(Colors.BlueViolet.ToArgb())}
            };

        private double _indicatorSize = 150;
        private double _hoverDotSize = 60;
        private string _hoverDotFill = "#60FFFF5A";

        public bool EnableClickCircle
        {
            get => _enableClickCircle;
            set { _enableClickCircle = value; OnPropertyChanged(); }
        }

        public bool EnableHoverDot
        {
            get => _enableHoverDot;
            set { _enableHoverDot = value; OnPropertyChanged(); }
        }

        public Dictionary<MouseButtons, ButtonSetting> MouseButtonSettings
        {
            get => _mouseButtonSettings;
            set { _mouseButtonSettings = value; OnPropertyChanged(); }
        }

        public double IndicatorSize
        {
            get => _indicatorSize;
            set { _indicatorSize = value; OnPropertyChanged(); }
        }

        public double HoverDotSize
        {
            get => _hoverDotSize;
            set { _hoverDotSize = value; OnPropertyChanged(); }
        }

        public string HoverDotFill
        {
            get => _hoverDotFill;
            set { _hoverDotFill = value; OnPropertyChanged(); }
        }

        public AppearanceSetting Clone()
        {
            var copy = new AppearanceSetting
            {
                EnableClickCircle = EnableClickCircle,
                EnableHoverDot = EnableHoverDot,
                IndicatorSize = IndicatorSize,
                HoverDotSize = HoverDotSize,
                HoverDotFill = HoverDotFill,
                MouseButtonSettings = new Dictionary<MouseButtons, ButtonSetting>()
            };
            foreach (var pair in MouseButtonSettings)
            {
                copy.MouseButtonSettings[pair.Key] = new ButtonSetting(pair.Value.IsEnabled, pair.Value.Color);
            }
            return copy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 设置信息 v2：外观浅色/深色两套，行为设置全局一份。
    /// </summary>
    public class AppSetting : INotifyPropertyChanged
    {
        private ProfileMode _profileMode = ProfileMode.System;
        private string _language = "";
        private bool _silenceEnabled = true;
        private bool _fullscreenHide = true;
        private List<string> _silentProcesses = new List<string>();

        /// <summary>当前配置方案</summary>
        public ProfileMode ProfileMode
        {
            get => _profileMode;
            set
            {
                _profileMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ActiveAppearance));
            }
        }

        /// <summary>
        /// 界面语言代码（空 = 首次启动按系统语言解析并固化；用户显式选择后优先于系统语言）。
        /// </summary>
        public string Language
        {
            get => _language;
            set { _language = value; OnPropertyChanged(); }
        }

        public AppearanceSetting Light { get; set; } = new AppearanceSetting();

        public AppearanceSetting Dark { get; set; } = new AppearanceSetting();

        /// <summary>当前生效的外观</summary>
        [JsonIgnore]
        public AppearanceSetting ActiveAppearance
        {
            get
            {
                switch (ProfileMode)
                {
                    case ProfileMode.Light:
                        return Light;
                    case ProfileMode.Dark:
                        return Dark;
                    default:
                        return ThemeWatcher.IsLightTheme ? Light : Dark;
                }
            }
        }

        /// <summary>启用静默功能（全屏/指定程序时隐藏特效）</summary>
        public bool SilenceEnabled
        {
            get => _silenceEnabled;
            set { _silenceEnabled = value; OnPropertyChanged(); }
        }

        /// <summary>前台窗口全屏时隐藏特效</summary>
        public bool FullscreenHide
        {
            get => _fullscreenHide;
            set { _fullscreenHide = value; OnPropertyChanged(); }
        }

        /// <summary>指定静默的程序（exe 名，前台时始终隐藏特效）</summary>
        public List<string> SilentProcesses
        {
            get => _silentProcesses;
            set { _silentProcesses = value; OnPropertyChanged(); }
        }

        /// <summary>上次提醒的最新版本，避免重复提醒。</summary>
        public string LastNotifiedVersion { get; set; }

        /// <summary>从旧版单套设置生成双套设置（两套取相同初值）。</summary>
        public static AppSetting FromSingleAppearance(AppearanceSetting appearance)
        {
            var setting = new AppSetting
            {
                Light = appearance.Clone(),
                Dark = appearance.Clone()
            };
            return setting;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
