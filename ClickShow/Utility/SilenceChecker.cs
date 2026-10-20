using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ClickShow.Settings;

namespace ClickShow.Utility
{
    /// <summary>
    /// 判断一次点击是否应当静默（不显示特效）。
    /// </summary>
    public static class SilenceChecker
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO info);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint pid);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder sb, int max);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT point);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        private static readonly int OwnProcessId = Process.GetCurrentProcess().Id;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        private const uint MONITOR_DEFAULTTONEAREST = 2;

        /// <summary>前台窗口句柄（供“抓取前台程序”等功能复用）。</summary>
        public static IntPtr GetForegroundWindowHandle()
        {
            return GetForegroundWindow();
        }

        /// <summary>屏幕坐标下窗口所属进程的 exe 名（小写，含 .exe），失败返回 null。</summary>
        public static string GetProcessExeAtPoint(int x, int y)
        {
            try
            {
                var hwnd = WindowFromPoint(new POINT { x = x, y = y });
                if (hwnd == IntPtr.Zero)
                {
                    return null;
                }
                GetWindowThreadProcessId(hwnd, out var pid);
                if (pid == 0)
                {
                    return null;
                }
                var name = Process.GetProcessById((int)pid).ProcessName;
                return string.IsNullOrEmpty(name) ? null : (name + ".exe").ToLowerInvariant();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>前台窗口进程的 exe 名（小写，含 .exe），失败返回 null。</summary>
        public static string GetForegroundProcessExe()
        {
            try
            {
                var hwnd = GetForegroundWindow();
                if (hwnd == IntPtr.Zero)
                {
                    return null;
                }
                GetWindowThreadProcessId(hwnd, out var pid);
                if (pid == 0)
                {
                    return null;
                }
                var name = Process.GetProcessById((int)pid).ProcessName;
                return string.IsNullOrEmpty(name) ? null : (name + ".exe").ToLowerInvariant();
            }
            catch
            {
                return null;
            }
        }

        public static bool ShouldSuppress(AppSetting setting)
        {
            if (setting == null || !setting.SilenceEnabled)
            {
                return false;
            }

            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
            {
                return false;
            }

            GetWindowThreadProcessId(hwnd, out var foregroundPid);

            // 本程序自己的界面不显示特效（预览区负责演示，避免双重触发）
            if (foregroundPid == OwnProcessId)
            {
                return true;
            }

            // 指定程序静默：前台进程在列表中即静默（不限全屏）
            if (setting.SilentProcesses != null && setting.SilentProcesses.Count > 0)
            {
                try
                {
                    if (foregroundPid != 0)
                    {
                        var processName = Process.GetProcessById((int)foregroundPid).ProcessName;
                        if (!string.IsNullOrEmpty(processName))
                        {
                            var exe = (processName + ".exe").ToLowerInvariant();
                            var plain = processName.ToLowerInvariant();
                            if (setting.SilentProcesses.Any(item =>
                                (item ?? "").Trim().ToLowerInvariant() is { Length: > 0 } p && (p == exe || p == plain)))
                            {
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    // 拿不到前台进程信息时忽略该项
                }
            }

            // 全屏自动隐藏：前台窗口矩形铺满所在显示器
            if (!setting.FullscreenHide)
            {
                return false;
            }

            try
            {
                // 桌面（Progman/WorkerW）铺满显示器但不是"全屏应用"，桌面点击要正常显示特效
                var className = new StringBuilder(64);
                GetClassName(hwnd, className, 64);
                if (className.ToString() is "Progman" or "WorkerW")
                {
                    return false;
                }

                if (!GetWindowRect(hwnd, out var wr))
                {
                    return false;
                }

                var mon = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
                var mi = new MONITORINFO { cbSize = Marshal.SizeOf<MONITORINFO>() };
                if (!GetMonitorInfo(mon, ref mi))
                {
                    return false;
                }

                return wr.Left == mi.rcMonitor.Left && wr.Top == mi.rcMonitor.Top
                    && wr.Right == mi.rcMonitor.Right && wr.Bottom == mi.rcMonitor.Bottom;
            }
            catch
            {
                return false;
            }
        }
    }
}
