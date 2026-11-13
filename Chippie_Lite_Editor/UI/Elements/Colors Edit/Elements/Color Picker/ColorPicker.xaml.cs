using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chippie_Lite_WPF.UI.Elements;

public partial class ColorPicker : UserControl
{
    private bool _isDraggingSv;
    private bool _isUpdating;
    private double _hue = 306; // 0..360
    private double _saturation = 0.90; // 0..1
    private double _brightness = 0.80; // 0..1

    public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor),
        typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(
            Color.FromArgb(214, 204, 21, 184), // #D6CC15B8
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public ColorPicker()
    {
        InitializeComponent();
        SyncControlsFromColor(SelectedColor);
    }

    private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var picker = (ColorPicker)d;
        if (!picker._isUpdating) picker.SyncControlsFromColor((Color)e.NewValue);
    }

    private void SyncControlsFromColor(Color color)
    {
        _isUpdating = true;

        RgbToHsv(color, ref _hue, out _saturation, out _brightness);

        HueSlider.Value = _hue;
        HueStop.Color = HsvToColor(_hue, 1, 1, 255);

        RSlider.Value = color.R;
        GSlider.Value = color.G;
        BSlider.Value = color.B;

        UpdatePreviews(color);
        UpdateThumbPosition();

        _isUpdating = false;
    }

    private void UpdateFromHsvAndAlpha()
    {
        _isUpdating = true;
        
        Color color = HsvToColor(_hue, _saturation, _brightness, 255);

        HueStop.Color = HsvToColor(_hue, 1, 1, 255);
        RSlider.Value = color.R;
        GSlider.Value = color.G;
        BSlider.Value = color.B;

        SelectedColor = color;
        UpdatePreviews(color);
        UpdateThumbPosition();

        _isUpdating = false;
    }

    private void UpdatePreviews(Color color)
    {
        var brush = new SolidColorBrush(color);
        PreviewFill.Background = brush;

        HexText.Text = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    private void UpdateThumbPosition()
    {
        if (SvCanvas.ActualWidth <= 0 || SvCanvas.ActualHeight <= 0) return;

        double x = _saturation * SvCanvas.ActualWidth;
        double y = (1.0 - _brightness) * SvCanvas.ActualHeight;

        Canvas.SetLeft(SvThumb, x - SvThumb.Width / 2);
        Canvas.SetTop(SvThumb, y - SvThumb.Width / 2);
    }

    private void SvCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => UpdateThumbPosition();

    private void SvCanvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        _isDraggingSv = true;
        SvCanvas.CaptureMouse();
        UpdateSvFromPoint(e.GetPosition(SvCanvas));
    }

    private void SvCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDraggingSv) UpdateSvFromPoint(e.GetPosition(SvCanvas));
    }

    private void SvCanvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDraggingSv)
        {
            _isDraggingSv = false;
            SvCanvas.ReleaseMouseCapture();
        }
    }

    private void UpdateSvFromPoint(Point pt)
    {
        if (SvCanvas.ActualWidth <= 0 || SvCanvas.ActualHeight <= 0) return;

        double x = Clamp(pt.X, 0, SvCanvas.ActualWidth);
        double y = Clamp(pt.Y, 0, SvCanvas.ActualHeight);

        _saturation = x / SvCanvas.ActualWidth;
        _brightness = 1.0 - (y / SvCanvas.ActualHeight);

        UpdateFromHsvAndAlpha();
    }

    private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isUpdating) return;
        _hue = HueSlider.Value;
        UpdateFromHsvAndAlpha();
    }

    private void RgbaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isUpdating) return;

        var color = Color.FromArgb(255, (byte)Math.Round(RSlider.Value),
            (byte)Math.Round(GSlider.Value), (byte)Math.Round(BSlider.Value));

        _isUpdating = true;
        RgbToHsv(color, ref _hue, out _saturation, out _brightness);
        HueSlider.Value = _hue;
        HueStop.Color = HsvToColor(_hue, 1, 1, 255);
        SelectedColor = color;
        UpdatePreviews(color);
        UpdateThumbPosition();
        _isUpdating = false;
    }

    private static double Clamp(double v, double min, double max) => v < min ? min : (v > max ? max : v);

    private static Color HsvToColor(double h, double s, double v, byte a)
    {
        double c = v * s;
        double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
        double m = v - c;

        double r1 = 0, g1 = 0, b1 = 0;
        int sector = (int)(h / 60.0) % 6;
        switch (sector)
        {
            case 0:
                r1 = c;
                g1 = x;
                break;
            case 1:
                r1 = x;
                g1 = c;
                break;
            case 2:
                g1 = c;
                b1 = x;
                break;
            case 3:
                g1 = x;
                b1 = c;
                break;
            case 4:
                r1 = x;
                b1 = c;
                break;
            case 5:
                r1 = c;
                b1 = x;
                break;
        }

        return Color.FromArgb(a, (byte)Math.Round((r1 + m) * 255), (byte)Math.Round((g1 + m) * 255),
            (byte)Math.Round((b1 + m) * 255));
    }

    private static void RgbToHsv(Color c, ref double h, out double s, out double v)
    {
        double r = c.R / 255.0;
        double g = c.G / 255.0;
        double b = c.B / 255.0;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        v = max;
        s = max == 0 ? 0 : delta / max;

        if (delta == 0) return; // Preserve existing hue on grayscale values

        if (max == r)
            h = 60 * (((g - b) / delta) % 6);
        else if (max == g)
            h = 60 * (((b - r) / delta) + 2);
        else
            h = 60 * (((r - g) / delta) + 4);

        if (h < 0) h += 360;
    }
}