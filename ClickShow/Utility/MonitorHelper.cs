using System;
using System.Runtime.InteropServices;

namespace ClickShow.Utility
{
    /// <summary>
    /// 查询指定屏幕坐标所在显示器的 DPI，用于特效窗口一次性正确落位。
    /// </summary>
    public static class MonitorHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        [DllImport("Shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, ref uint dpiX, ref uint dpiY);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        private const uint MONITOR_DEFAULTTONEAREST = 2;
        private const int MDT_EFFECTIVE_DPI = 0;

        /// <summary>返回该坐标所在显示器的缩放比例（96dpi=1.0），失败回退 1.0。</summary>
        public static double GetDpiScaleForPoint(int x, int y)
        {
            try
            {
                var mon = MonitorFromPoint(new POINT { x = x, y = y }, MONITOR_DEFAULTTONEAREST);
                if (mon != IntPtr.Zero)
                {
                    uint dpiX = 96, dpiY = 96;
                    if (GetDpiForMonitor(mon, MDT_EFFECTIVE_DPI, ref dpiX, ref dpiY) == 0 && dpiX > 0)
                    {
                        return dpiX / 96.0;
                    }
                }
            }
            catch
            {
            }

            return 1.0;
        }
    }
}
