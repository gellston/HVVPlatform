using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisionTool.UC
{
    /// <summary>
    /// FunctionDiagramViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FunctionDiagramViewer : UserControl
    {
        public FunctionDiagramViewer()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(FunctionDiagramViewer));
        public ImageSource Image
        {
            get
            {
                return (ImageSource)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

    }
}
