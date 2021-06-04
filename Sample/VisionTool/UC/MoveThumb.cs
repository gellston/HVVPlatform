<<<<<<< HEAD
﻿using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
=======
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
>>>>>>> a57f593ead8c8ac5c8981bedbfa63d7d3abbc5ac

namespace UClib
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
<<<<<<< HEAD

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
=======
>>>>>>> a57f593ead8c8ac5c8981bedbfa63d7d3abbc5ac
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
<<<<<<< HEAD
            BaseDiagramProperty property = this.DataContext as BaseDiagramProperty;

            if (property !=  null)
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

                property.X += xChange - this.Width/2;
                property.Y += yChange - this.Height/2;

=======
            Control item = this.DataContext as Control;

            if (item != null)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                Canvas.SetLeft(item, left + e.HorizontalChange);
                Canvas.SetTop(item, top + e.VerticalChange);
>>>>>>> a57f593ead8c8ac5c8981bedbfa63d7d3abbc5ac
            }
        }
    }
}
