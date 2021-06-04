using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UClib
{
    public class RadiusThumb : Thumb
    {
        public RadiusThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(RadiusThumb));
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }

            set
            {
                SetValue(RadiusProperty, value);
            }
        }


        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            BaseDiagramProperty property = this.DataContext as BaseDiagramProperty;


            if (property != null)
            {


                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        Radius -= e.HorizontalChange;
                        break;
                    case HorizontalAlignment.Right:
                        Radius += e.HorizontalChange;
                        break;

                }

                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        Radius -= e.VerticalChange;
                        break;
                    case VerticalAlignment.Bottom:
                        Radius += e.VerticalChange;
                        break;

                }

            }
        }
    }
}
