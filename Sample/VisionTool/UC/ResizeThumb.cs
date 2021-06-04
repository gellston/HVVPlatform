using Model.DiagramProperty;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UClib
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }





        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            BaseDiagramProperty property = this.DataContext as BaseDiagramProperty;

            if (property != null)
            {

                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:

                        double origin = property.X;
                        double width = property.Width;

                        property.X += e.HorizontalChange;
                        property.Width -= e.HorizontalChange;

                        if (property.Width <= 10)
                        {
                            property.X = origin;
                            property.Width = width;

                        }
                        break;
                    case HorizontalAlignment.Right:
                        
                        property.Width += e.HorizontalChange;
                        break;

                }

                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        double origin = property.Y;
                        double height = property.Height;

                        property.Y += e.VerticalChange;
                        property.Height -= e.VerticalChange;

                        if (property.Height <= 10)
                        {
                            property.Y = origin;
                            property.Height = height;

                        }
                        break;
                    case VerticalAlignment.Bottom:

                        property.Height += e.VerticalChange;
                        break;

                }

            }
        }
    }
}
