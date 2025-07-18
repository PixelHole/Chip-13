using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chippie_Lite_WPF.UI.Windows.Development;

public partial class VideoIO : UserControl
{
    public int DisplayWidth { get; private set; } = 200;
    public int DisplayHeight { get; private set; } = 150;
    public int PixelSize { get; private set; } = 4;
    public Brush PixelDefaultColor { get; private set; }

    private WriteableBitmap PixelCanvas { get; set; }
    
    
    public VideoIO()
    {
        InitializeComponent();
        SetWindowProperties();
        FetchColors();
        CreatePixelCanvas();
        DrawPixel(2, 2, Colors.Red);
    }
    private void SetWindowProperties()
    {
        MinHeight = DisplayHeight;
        MinWidth = DisplayWidth;
    }
    private void FetchColors()
    {
        PixelDefaultColor = (Application.Current.Resources["Black"] as Brush)!;
    }
    private void CreatePixelCanvas()
    {
        PixelCanvas = new WriteableBitmap(DisplayWidth, DisplayHeight, 8, 8, PixelFormats.Bgr32, null);
        Display.Background = new ImageBrush(PixelCanvas);
    }

    public void DrawPixel(int x, int y, Color color)
    {
        try{
            PixelCanvas.Lock();

            unsafe
            {
                IntPtr pBackBuffer = PixelCanvas.BackBuffer;
                
                pBackBuffer += x * PixelCanvas.BackBufferStride;
                pBackBuffer += y * 4;
                
                int color_data = color.R << 16; // R
                color_data |= color.G << 8;   // G
                color_data |= color.B << 0;   // B
                
                *((int*) pBackBuffer) = color_data;
            }
            
            PixelCanvas.AddDirtyRect(new Int32Rect(y, x, 1, 1));
        }
        finally{
            PixelCanvas.Unlock();
        }
    }
}