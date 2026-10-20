using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ClickShow.Utility
{
    public static class DwmHelper
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        /// <summary>
        /// 深色标题栏（Win10 2004+/Win11）。老系统属性不存在，静默跳过（标题栏保持亮色）。
        /// </summary>
        public static void ApplyDarkTitleBar(Window window, bool dark)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
            {
                return;
            }
            var value = dark ? 1 : 0;
            if (DwmSetWindowAttribute(hwnd, 20, ref value, sizeof(int)) != 0)
            {
                DwmSetWindowAttribute(hwnd, 19, ref value, sizeof(int));
            }
        }
    }
}
