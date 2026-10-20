using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using ClickShow.Utility;

namespace ClickShow.Settings
{
    /// <summary>
    /// 设置信息
    /// </summary>
    public class AppSetting : INotifyPropertyChanged
    {
        private bool _enableClickCircle = true;
        private bool _enableHoverDot = false;
        private Dictionary<MouseButtons, ButtonSetting> _mouseButtonSettings = new Dictionary<MouseButtons, ButtonSetting>()
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

        private bool _silenceEnabled = true;

        private bool _fullscreenHide = true;

        private List<string> _silentProcesses = new List<string>();

        /// <summary>
        /// 开启波纹特效
        /// </summary>
        public bool EnableClickCircle
        {
            get => _enableClickCircle;
            set
            {
                _enableClickCircle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 开启浮标显示
        /// </summary>
        public bool EnableHoverDot
        {
            get => _enableHoverDot;
            set
            {
                _enableHoverDot = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 鼠标按键设置
        /// </summary>
        public Dictionary<MouseButtons, ButtonSetting> MouseButtonSettings
        {
            get => _mouseButtonSettings;
            set => _mouseButtonSettings = value;
        }

        /// <summary>
        /// 波纹尺寸
        /// </summary>
        public double IndicatorSize
        {
            get => _indicatorSize;
            set
            {
                _indicatorSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 浮标尺寸
        /// </summary>
        public double HoverDotSize
        {
            get => _hoverDotSize;
            set
            {
                _hoverDotSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 浮标颜色
        /// </summary>
        public string HoverDotFill
        {
            get => _hoverDotFill;
            set
            {
                _hoverDotFill = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 启用静默功能（全屏/指定程序时隐藏特效）
        /// </summary>
        public bool SilenceEnabled
        {
            get => _silenceEnabled;
            set
            {
                _silenceEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 前台窗口全屏时隐藏特效
        /// </summary>
        public bool FullscreenHide
        {
            get => _fullscreenHide;
            set
            {
                _fullscreenHide = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 指定静默的程序（exe 名，前台时始终隐藏特效）
        /// </summary>
        public List<string> SilentProcesses
        {
            get => _silentProcesses;
            set
            {
                _silentProcesses = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 上次提醒的最新版本，避免重复提醒。
        /// </summary>
        public string LastNotifiedVersion { get; set; }
    }
}
