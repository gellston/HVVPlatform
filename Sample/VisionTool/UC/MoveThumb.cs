using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;



namespace UClib
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);

        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(MoveThumb), new PropertyMetadata(1.0));
        public double Zoom
        {
            get
            {
                return (double)GetValue(ZoomProperty);
            }

            set
            {
                SetValue(ZoomProperty, value);
            }
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            BaseDiagramProperty property = this.DataContext as BaseDiagramProperty;

            if (property != null)
            {

                Point locationUIFromWindow = this.TranslatePoint(new Point(0, 0), this);
                Point locationUIFromScreen = this.PointToScreen(locationUIFromWindow);


                Point locationMouseFromWindow = Mouse.GetPosition(this);
                Point locationMouseFromScreen = this.PointToScreen(locationMouseFromWindow);


                var xChange = locationMouseFromScreen.X - locationUIFromScreen.X;
                var yChange = locationMouseFromScreen.Y - locationUIFromScreen.Y;


                xChange = xChange / this.Zoom;
                yChange = yChange / this.Zoom;

                if ((property.X + xChange) <= 0)
                    return;

                if ((property.Y + yChange) <= 0)
                    return;

                property.X += xChange - this.Width / 2;
                property.Y += yChange - this.Height / 2;


            }
        }
    }
}
