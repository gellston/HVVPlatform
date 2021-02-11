using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiagramViewerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            this.thumb.Background = Brushes.Orange;

        }

        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double newCanvasWidth = this.canvas.Width + e.HorizontalChange;

            double newCanvasHeight = this.canvas.Height + e.VerticalChange;



            if ((newCanvasWidth >= 0) && (newCanvasHeight >= 0))

            {

                this.canvas.Width = newCanvasWidth;

                this.canvas.Height = newCanvasHeight;



                Canvas.SetLeft(this.thumb, Canvas.GetLeft(this.thumb) + e.HorizontalChange);

                Canvas.SetTop(this.thumb, Canvas.GetTop(this.thumb) + e.VerticalChange);

            }




        }

        private void thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            this.thumb.Background = Brushes.Blue;

        }

        private double _zoomValue = 1.0;

        private void ThemedWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                _zoomValue += 0.1;
            }
            else
            {
                _zoomValue -= 0.1;
            }

            ScaleTransform scale = new ScaleTransform(_zoomValue, _zoomValue);
            canvas.LayoutTransform = scale;
            e.Handled = true;
        }
    }
}
