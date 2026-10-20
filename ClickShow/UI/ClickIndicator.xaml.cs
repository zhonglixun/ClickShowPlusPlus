using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClickShow
{
    /// <summary>
    /// 波纹特效窗口。动画驱动缩放与透明度（渲染线程），不触发布局重算。
    /// </summary>
    public partial class ClickIndicator : Window
    {
        // 高刷屏上限意愿值；实际帧率由显示器刷新率封顶
        private const int DesiredFrameRate = 120;

        private Storyboard _mouseDownStoryBoard;
        private Storyboard _mouseUpStoryBoard;

        public int LastLiveTime { get; set; } = Environment.TickCount;

        public ClickIndicator(double size)
        {
            ShowActivated = false;
            InitializeComponent();

            this.Width = size;
            this.Height = size;

            SourceInitialized += OnSourceInitialized;

            _mouseDownStoryBoard = CreateStoryboard(0.4);
            _mouseUpStoryBoard = CreateStoryboard(0.3);
        }

        private Storyboard CreateStoryboard(double seconds)
        {
            var sb = new Storyboard { FillBehavior = FillBehavior.Stop };

            sb.Children.Add(CreateScaleAnimation("ScaleX", seconds));
            sb.Children.Add(CreateScaleAnimation("ScaleY", seconds));

            var fade = new DoubleAnimation(toValue: 0, new Duration(TimeSpan.FromSeconds(seconds)))
                ;
            Timeline.SetDesiredFrameRate(fade, DesiredFrameRate);
            Storyboard.SetTarget(fade, TheCircle);
            Storyboard.SetTargetProperty(fade, new PropertyPath("Opacity"));
            sb.Children.Add(fade);

            sb.Completed += (s, e) =>
            {
                CircleScale.ScaleX = CircleScale.ScaleY = StartScale;
                this.Opacity = 0;
                IsIdle = true;
            };

            if (sb.CanFreeze)
            {
                sb.Freeze();
            }

            return sb;
        }

        private DoubleAnimation CreateScaleAnimation(string property, double seconds)
        {
            var anim = new DoubleAnimation(toValue: 1, new Duration(TimeSpan.FromSeconds(seconds)))
                ;
            Timeline.SetDesiredFrameRate(anim, DesiredFrameRate);
            Storyboard.SetTargetName(anim, "CircleScale");
            Storyboard.SetTargetProperty(anim, new PropertyPath(property));
            return anim;
        }

        private const double StartScale = 0.2;

        public bool IsIdle { get; private set; } = false;

        public void Prepare()
        {
            IsIdle = false;
        }

        public void Play(Brush circleBrush, bool isDown)
        {
            this.LastLiveTime = Environment.TickCount;
            IsIdle = false;

            TheCircle.Stroke = circleBrush;
            this.Opacity = isDown ? 0.95 : 0.7;
            CircleScale.ScaleX = CircleScale.ScaleY = StartScale;

            (isDown ? _mouseDownStoryBoard : _mouseUpStoryBoard).Begin(this);

            this.Show();
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            WindowHelper.SetWindowExTransparent(new WindowInteropHelper(this).Handle);
        }
    }
}
