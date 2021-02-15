using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Behavior
{
    public class GridControlScrollTopBehavior : Behavior<GridControl>
    {
        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    this.AssociatedObject.
        //}

        //protected override void OnDetaching()
        //{
        //    this.AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
        //    base.OnDetaching();
        //}

        //private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var scrollViewer = AssociatedObject.GetChildOf<ScrollViewer>(includeSelf: true);
        //    var scrollPos = scrollViewer.ContentVerticalOffset;
        //    if ((scrollPos == scrollViewer.ScrollableHeight && e.Delta < 0) || (scrollPos == 0 && e.Delta > 0))
        //    {
        //        UIElement rerouteTo = AssociatedObject;
        //        if (ReferenceEquals(scrollViewer, AssociatedObject))
        //        {
        //            rerouteTo = (UIElement)VisualTreeHelper.GetParent(AssociatedObject);
        //        }

        //        e.Handled = true;
        //        var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
        //        e2.RoutedEvent = UIElement.MouseWheelEvent;
        //        rerouteTo.RaiseEvent(e2);
        //    }
        //}
    }
}
