using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisionTool.UC
{
    /// <summary>
    /// Direct2DCanvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Direct2DCanvas : UserControl
    {
        public Direct2DCanvas()
        {
            InitializeComponent();


            D3DImage test = new D3DImage();
            test.SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
        }
    }
}
