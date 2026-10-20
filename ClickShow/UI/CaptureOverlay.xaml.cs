using System;
using System.Windows;
using System.Windows.Input;

namespace ClickShow
{
    /// <summary>
    /// 待捕捉全屏遮罩：点击任意位置 = 捕捉该处程序；右键或 ESC = 取消。
    /// </summary>
    public partial class CaptureOverlay : Window
    {
        /// <summary>捕捉成功，参数为点击点的屏幕坐标。</summary>
        public event Action<Point> Captured;

        /// <summary>用户取消捕捉。</summary>
        public event Action Cancelled;

        public CaptureOverlay()
        {
            InitializeComponent();
            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
        }

        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                e.Handled = true;
                Cancelled?.Invoke();
                Close();
                return;
            }
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            e.Handled = true;
            var screen = PointToScreen(e.GetPosition(this));
            // 先隐藏遮罩再查询，否则 WindowFromPoint 会命中遮罩自身
            Hide();
            Captured?.Invoke(screen);
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Cancelled?.Invoke();
                Close();
            }
        }
    }
}
