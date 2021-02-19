using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisionTool.Model;

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


        public static readonly DependencyProperty IsShowFunctionPanelProperty = DependencyProperty.Register("IsShowFunctionPanel", typeof(bool), typeof(FunctionDiagramViewer));
        public bool IsShowFunctionPanel
        {
            get
            {
                return (bool)GetValue(IsShowFunctionPanelProperty);
            }

            set
            {
                SetValue(IsShowFunctionPanelProperty, value);
            }
        }



        public static readonly DependencyProperty CanvasWidthProperty = DependencyProperty.Register("CanvasWidth", typeof(double), typeof(FunctionDiagramViewer));
        public double CanvasWidth
        {
            get
            {
                return (double)GetValue(CanvasWidthProperty);
            }

            set
            {
                SetValue(CanvasWidthProperty, value);
            }
        }

        public static readonly DependencyProperty CanvasHeightProperty = DependencyProperty.Register("CanvasHeight", typeof(double), typeof(FunctionDiagramViewer));
        public double CanvasHeight
        {
            get
            {
                return (double)GetValue(CanvasHeightProperty);
            }

            set
            {
                SetValue(CanvasHeightProperty, value);
            }
        }

        public static readonly DependencyProperty FunctionCollectionProperty = DependencyProperty.Register("FunctionCollection", typeof(ObservableCollection<Function>), typeof(FunctionDiagramViewer));
        public ObservableCollection<Function> FunctionCollection
        {
            get
            {
                return (ObservableCollection<Function>)GetValue(FunctionCollectionProperty);
            }

            set
            {
                SetValue(FunctionCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedDiagramProperty = DependencyProperty.Register("SelectedDiagram", typeof(DiagramObject), typeof(FunctionDiagramViewer));
        public Function SelectedDiagram
        {
            get
            {
                return (Function)GetValue(SelectedDiagramProperty);
            }

            set
            {
                SetValue(SelectedDiagramProperty, value);
            }
        }

        //private static void OnFunctionCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.OldValue != null)
        //    {
        //        var coll = (INotifyCollectionChanged)e.OldValue;
        //        coll.CollectionChanged -= OnFunctionItemChanged;
        //    }

        //    if (e.NewValue != null)
        //    {
        //        var coll = (ObservableCollection<Function>)e.NewValue;
        //        coll.CollectionChanged += OnFunctionItemChanged;
        //    }
        //}
        //private static void OnFunctionItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        switch (e.Action)
        //        {
        //            case NotifyCollectionChangedAction.Add:
        //                System.Console.WriteLine("test");
        //                break;

        //            case NotifyCollectionChangedAction.Remove:
        //                System.Console.WriteLine("test");
        //                break;

        //        }

        //    }catch(Exception exception)
        //    {
        //        System.Console.WriteLine(exception.Message);
        //    }
        //}


        public static readonly DependencyProperty InputSnapSpotCollectionProperty = DependencyProperty.Register("InputSnapSpotCollection", typeof(ObservableCollection<InputSnapSpot>), typeof(FunctionDiagramViewer));
        public ObservableCollection<InputSnapSpot> InputSnapSpotCollection
        {
            get
            {
                return (ObservableCollection<InputSnapSpot>)GetValue(InputSnapSpotCollectionProperty);
            }

            set
            {
                SetValue(InputSnapSpotCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty OutputSnapSpotCollectionProperty = DependencyProperty.Register("OutputSnapSpotCollection", typeof(ObservableCollection<OutputSnapSpot>), typeof(FunctionDiagramViewer));
        public ObservableCollection<OutputSnapSpot> OutputSnapSpotCollection
        {
            get
            {
                return (ObservableCollection<OutputSnapSpot>)GetValue(OutputSnapSpotCollectionProperty);
            }

            set
            {
                SetValue(OutputSnapSpotCollectionProperty, value);
            }
        }


        public static readonly DependencyProperty ConnectorCollectionProperty = DependencyProperty.Register("ConnectorCollection", typeof(ObservableCollection<Connector>), typeof(FunctionDiagramViewer));
        public ObservableCollection<Connector> ConnectorCollection
        {
            get
            {
                return (ObservableCollection<Connector>)GetValue(ConnectorCollectionProperty);
            }

            set
            {
                SetValue(ConnectorCollectionProperty, value);
            }
        }


        private void FunctionThum_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as Function;
            if (node == null)
                return;

            node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }
    }
}
