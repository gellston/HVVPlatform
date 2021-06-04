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
    public class RotationThumb : Thumb
    {
        public RotationThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }


        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            BaseDiagramProperty property = this.DataContext as BaseDiagramProperty;

            if (property != null)
            {
                string typeString = property.GetType().Name;

                //You can use this values when RotateTransform is null
                double deltaHorizontal = e.HorizontalChange;
                double deltaVertical = e.VerticalChange;

                switch (typeString)
                {
                    case "LineROIDiagramProperty":

                        var lineProperty = property as LineFitROIDiagramProperty;

                        UIElement container = VisualTreeHelper.GetParent(this) as UIElement;
                        Point relativeLocation = this.TranslatePoint(new Point(0, 0), container);
                        Point mousePosition = Mouse.GetPosition(container);
                        Vector origin = new Vector(relativeLocation.X, relativeLocation.Y);
                        Vector thumbLocation = new Vector(mousePosition.X, mousePosition.Y);

                        break;
                }
            }
        }
    }
}
