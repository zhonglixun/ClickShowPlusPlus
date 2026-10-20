using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickShow.Entities
{
    /// <summary>
    /// 鼠标按键状态
    /// </summary>
    class MouseButtonState
    {
        /// <summary>
        /// 按下时间
        /// </summary>
        public long? DownTimeTicks { get; set; }

        /// <summary>
        /// 按下位置
        /// </summary>
        public Point DownPosition { get; set; }

        /// <summary>
        /// 该次按下被静默（抬起时也应静默，避免用旧状态误触发）
        /// </summary>
        public bool Suppressed { get; set; }

    }
}
