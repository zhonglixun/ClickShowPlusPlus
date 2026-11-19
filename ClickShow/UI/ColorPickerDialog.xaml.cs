using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClickShow.UI
{
    public partial class ColorPickerDialog : Window
    {
        private bool _sync;
        private byte _alpha = 255;

        public string SelectedColorHex { get; private set; }

        public ColorPickerDialog(string hex)
        {
            InitializeComponent();
            var color = Parse(hex);
            _alpha = color.A;
            var (h, s, v) = ToHsv(color);
            _sync = true;
            HueSlider.Value = h;
            SatSlider.Value = s * 100;
            ValSlider.Value = v * 100;
            AlphaSlider.Value = _alpha / 2.55;
            _sync = false;
            UpdatePreview();
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_sync || Preview == null)
            {
                return;
            }
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            var c = FromHsv(HueSlider.Value, SatSlider.Value / 100, ValSlider.Value / 100);
            _alpha = (byte)Math.Round(AlphaSlider.Value * 2.55);
            var shown = Color.FromArgb(_alpha, c.R, c.G, c.B);
            Preview.Background = new SolidColorBrush(shown);
            HueValue.Text = $"{HueSlider.Value:0}°";
            SatValue.Text = $"{SatSlider.Value:0}%";
            ValValue.Text = $"{ValSlider.Value:0}%";
            AlphaValue.Text = $"{AlphaSlider.Value:0}%";
            SatSlider.Background = new LinearGradientBrush(Color.FromRgb(128, 128, 128), c, 0);
            ValSlider.Background = new LinearGradientBrush(Colors.Black, c, 0);
            AlphaSlider.Background = new LinearGradientBrush(Color.FromArgb(0, c.R, c.G, c.B), c, 0);
            _sync = true;
            HexBox.Text = $"#{_alpha:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
            PreviewHex.Text = HexBox.Text;
            var luma = (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B) / 255.0;
            PreviewHex.Foreground = luma > 0.6 ? Brushes.Black : Brushes.White;
            _sync = false;
            SelectedColorHex = HexBox.Text;
        }

        private void HexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_sync)
            {
                return;
            }
            var text = HexBox.Text.Trim();
            if (text.Length != 9 || !text.StartsWith("#"))
            {
                return;
            }
            Color c;
            try
            {
                c = (Color)ColorConverter.ConvertFromString(text);
            }
            catch
            {
                return;
            }
            _alpha = c.A;
            var (h, s, v) = ToHsv(c);
            _sync = true;
            HueSlider.Value = h;
            SatSlider.Value = s * 100;
            ValSlider.Value = v * 100;
            AlphaSlider.Value = _alpha / 2.55;
            _sync = false;
            UpdatePreview();
        }

        private void Preset_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not string hex)
            {
                return;
            }
            var c = Parse(hex);
            _alpha = 255;
            AlphaSlider.Value = 100;
            var (h, s, v) = ToHsv(c);
            _sync = true;
            HueSlider.Value = h;
            SatSlider.Value = s * 100;
            ValSlider.Value = v * 100;
            AlphaSlider.Value = 100;
            _sync = false;
            UpdatePreview();
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

        private static (double H, double S, double V) ToHsv(Color c)
        {
            double r = c.R / 255.0, g = c.G / 255.0, b = c.B / 255.0;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var delta = max - min;
            double h = 0;
            if (delta > 0)
            {
                if (max == r) h = 60 * (((g - b) / delta) % 6);
                else if (max == g) h = 60 * (((b - r) / delta) + 2);
                else h = 60 * (((r - g) / delta) + 4);
            }
            if (h < 0) h += 360;
            return (h, max <= 0 ? 0 : delta / max, max);
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
