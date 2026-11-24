using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClickShow.UI
{
    /// <summary>
    /// 标准调色器：饱和度/明度方块 + 色相条 + 透明度 + HEX/RGB 输入 + 常用色。
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        private bool _sync;
        private double _hue;
        private double _sat = 1;
        private double _val = 1;
        private byte _alpha = 255;

        public string SelectedColorHex { get; private set; }

        public ColorPickerDialog(string hex)
        {
            InitializeComponent();
            var color = Parse(hex);
            _alpha = color.A;
            ToHsv(color, out _hue, out _sat, out _val);
            Loaded += (s, e) => UpdateVisuals();
            SizeChanged += (s, e) => UpdateThumbPositions();
        }

        private static Color Parse(string hex)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(hex);
            }
            catch
            {
                return Colors.DodgerBlue;
            }
        }

        private Color CurrentColor()
        {
            var c = FromHsv(_hue, _sat, _val);
            return Color.FromArgb(_alpha, c.R, c.G, c.B);
        }

        private void UpdateVisuals()
        {
            if (Preview == null)
            {
                return;
            }
            var c = CurrentColor();
            Preview.Background = new SolidColorBrush(c);
            var luma = (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B) / 255.0;
            PreviewHex.Foreground = luma > 0.6 ? Brushes.Black : Brushes.White;

            var hueColor = FromHsv(_hue, 1, 1);
            HueLayer.Background = new SolidColorBrush(hueColor);
            AlphaLayer.Background = new LinearGradientBrush(
                Color.FromArgb(0, hueColor.R, hueColor.G, hueColor.B),
                Color.FromArgb(255, hueColor.R, hueColor.G, hueColor.B), 0);

            _sync = true;
            var text = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", _alpha, c.R, c.G, c.B);
            HexBox.Text = text;
            PreviewHex.Text = text;
            RBox.Text = c.R.ToString();
            GBox.Text = c.G.ToString();
            BBox.Text = c.B.ToString();
            AlphaValue.Text = Math.Round(_alpha / 2.55) + "%";
            _sync = false;
            SelectedColorHex = text;
            UpdateThumbPositions();
        }

        private void UpdateThumbPositions()
        {
            if (SvThumb == null || SvArea.ActualWidth <= 0)
            {
                return;
            }
            Canvas.SetLeft(SvThumb, Math.Max(0, Math.Min(SvArea.ActualWidth - 14, _sat * SvArea.ActualWidth - 7)));
            Canvas.SetTop(SvThumb, Math.Max(0, Math.Min(SvArea.ActualHeight - 14, (1 - _val) * SvArea.ActualHeight - 7)));
            Canvas.SetTop(HueThumb, Math.Max(0, Math.Min(HueArea.ActualHeight - 4, _hue / 360.0 * HueArea.ActualHeight - 2)));
            Canvas.SetLeft(HueThumb, 0);
            Canvas.SetLeft(AlphaThumb, Math.Max(0, Math.Min(AlphaArea.ActualWidth - 10, _alpha / 255.0 * AlphaArea.ActualWidth - 5)));
        }

        private void Sv_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SvArea.CaptureMouse();
            UpdateFromSvPoint(e.GetPosition(SvArea));
        }

        private void Sv_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SvArea.ReleaseMouseCapture();
        }

        private void Sv_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && SvArea.IsMouseCaptured)
            {
                UpdateFromSvPoint(e.GetPosition(SvArea));
            }
        }

        private void UpdateFromSvPoint(Point p)
        {
            _sat = Math.Max(0, Math.Min(1, p.X / Math.Max(1, SvArea.ActualWidth)));
            _val = Math.Max(0, Math.Min(1, 1 - p.Y / Math.Max(1, SvArea.ActualHeight)));
            UpdateVisuals();
        }

        private void Hue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HueArea.CaptureMouse();
            UpdateHue(e.GetPosition(HueArea));
        }

        private void Hue_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HueArea.ReleaseMouseCapture();
        }

        private void Hue_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && HueArea.IsMouseCaptured)
            {
                UpdateHue(e.GetPosition(HueArea));
            }
        }

        private void UpdateHue(Point p)
        {
            _hue = Math.Max(0, Math.Min(1, p.Y / Math.Max(1, HueArea.ActualHeight))) * 360;
            UpdateVisuals();
        }

        private void Alpha_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AlphaArea.CaptureMouse();
            UpdateAlpha(e.GetPosition(AlphaArea));
        }

        private void Alpha_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AlphaArea.ReleaseMouseCapture();
        }

        private void Alpha_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && AlphaArea.IsMouseCaptured)
            {
                UpdateAlpha(e.GetPosition(AlphaArea));
            }
        }

        private void UpdateAlpha(Point p)
        {
            _alpha = (byte)Math.Round(Math.Max(0, Math.Min(1, p.X / Math.Max(1, AlphaArea.ActualWidth))) * 255);
            UpdateVisuals();
        }

        private void HexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_sync)
            {
                return;
            }
            var text = HexBox.Text.Trim();
            if (text.StartsWith("#"))
            {
                text = text.Substring(1);
            }
            if ((text.Length != 6 && text.Length != 8) || !IsHex(text))
            {
                return;
            }
            var a = text.Length == 8 ? Convert.ToByte(text.Substring(0, 2), 16) : (byte)255;
            var r = Convert.ToByte(text.Substring(text.Length - 6, 2), 16);
            var g = Convert.ToByte(text.Substring(text.Length - 4, 2), 16);
            var b = Convert.ToByte(text.Substring(text.Length - 2, 2), 16);
            _alpha = a;
            ToHsv(Color.FromRgb(r, g, b), out _hue, out _sat, out _val);
            UpdateVisuals();
        }

        private static bool IsHex(string s)
        {
            foreach (var ch in s)
            {
                if (!Uri.IsHexDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }

        private void Rgb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_sync)
            {
                return;
            }
            if (!byte.TryParse(RBox.Text.Trim(), out var r) ||
                !byte.TryParse(GBox.Text.Trim(), out var g) ||
                !byte.TryParse(BBox.Text.Trim(), out var b))
            {
                return;
            }
            ToHsv(Color.FromRgb(r, g, b), out _hue, out _sat, out _val);
            UpdateVisuals();
        }

        private void Preset_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.Tag is string hex))
            {
                return;
            }
            var c = Parse(hex);
            _alpha = c.A;
            ToHsv(c, out _hue, out _sat, out _val);
            UpdateVisuals();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SelectedColorHex = HexBox.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private static void ToHsv(Color c, out double h, out double s, out double v)
        {
            double r = c.R / 255.0, g = c.G / 255.0, b = c.B / 255.0;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var delta = max - min;
            h = 0;
            if (delta > 0)
            {
                if (max == r) h = 60 * (((g - b) / delta) % 6);
                else if (max == g) h = 60 * (((b - r) / delta) + 2);
                else h = 60 * (((r - g) / delta) + 4);
            }
            if (h < 0)
            {
                h += 360;
            }
            s = max <= 0 ? 0 : delta / max;
            v = max;
        }

        private static Color FromHsv(double h, double s, double v)
        {
            var c = v * s;
            var x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            var m = v - c;
            double r = 0, g = 0, b = 0;
            if (h < 60) { r = c; g = x; }
            else if (h < 120) { r = x; g = c; }
            else if (h < 180) { g = c; b = x; }
            else if (h < 240) { g = x; b = c; }
            else if (h < 300) { r = x; b = c; }
            else { r = c; b = x; }
            return Color.FromRgb((byte)Math.Round((r + m) * 255), (byte)Math.Round((g + m) * 255), (byte)Math.Round((b + m) * 255));
        }
    }
}

