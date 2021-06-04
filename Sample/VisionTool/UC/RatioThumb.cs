using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UClib
{
    public class RatioThumb : Thumb
    {
        public RatioThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(RatioThumb));
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

        public static readonly DependencyProperty RatioProperty = DependencyProperty.Register("Ratio", typeof(double), typeof(RatioThumb));
        public double Ratio
        {
            get
            {
                return (double)GetValue(RatioProperty);
            }

            set
            {
                SetValue(RatioProperty, value);
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
                        var ChangedRadius1 = (this.Radius * this.Ratio) - e.HorizontalChange;
                        var ratio1 = ChangedRadius1 / this.Radius;
                        this.Ratio = ratio1;
                        break;
                    case HorizontalAlignment.Right:
                        var ChangedRadius2 = (this.Radius * this.Ratio) + e.HorizontalChange;
                        var ratio2 = ChangedRadius2 / this.Radius;
                        this.Ratio = ratio2;
                        break;

                }

                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        var ChangedRadius3 = (this.Radius * this.Ratio) - e.VerticalChange;
                        var ratio3 = ChangedRadius3 / this.Radius;
                        this.Ratio = ratio3;
                        break;
                    case VerticalAlignment.Bottom:
                        var ChangedRadius4 = (this.Radius * this.Ratio) + e.VerticalChange;
                        var ratio4 = ChangedRadius4 / this.Radius;
                        this.Ratio = ratio4;
                        break;

                }

            }
        }
    }
}
