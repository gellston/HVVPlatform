using DevExpress.Xpf.CodeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
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


        public static readonly DependencyProperty IsShowFunctionPanelProperty = DependencyProperty.Register("IsShowFunctionPanel", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsShowFunctionPanelChanged));
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

        private static void OnIsShowFunctionPanelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                bool isShowFunctionPanel = control.IsShowFunctionPanel;
                System.Console.WriteLine("IsShowFunctionPanel : " + isShowFunctionPanel);
            }
        }


        public static readonly DependencyProperty IsFunctionCreateProperty = DependencyProperty.Register("IsFunctionCreate", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsFunctionCreateChanged));
        public bool IsFunctionCreate
        {
            get
            {
                return (bool)GetValue(IsFunctionCreateProperty);
            }

            set
            {
                SetValue(IsFunctionCreateProperty, value);

            }
        }

        private static void OnIsFunctionCreateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                bool isNodeCreate = (bool)e.NewValue;

                if (isNodeCreate)
                {
                    var functionConfig = control.SelectedDiagramConfig;
                    if (functionConfig == null) return;

                    /// Function Part
                    var function = new Function()
                    {
                        Name = functionConfig.FunctionInfo.Name,
                        Color = functionConfig.FunctionInfo.Color,
                        IsNew = true,
                    };
                    function.Location.X = control.CurrentX;
                    function.Location.Y = control.CurrentY;
                    function.Size.Width = functionConfig.FunctionInfo.Size.Width;
                    function.Size.Height = functionConfig.FunctionInfo.Size.Height;
                    function.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();

                    /// InputSnapSpot Part
                    var inputSnapSpotCollection = new ObservableCollection<InputSnapSpot>();
                    foreach(var inputSnap in functionConfig.InputSnapSpotCollection)
                    {
                        var newInputSnapSpot = new InputSnapSpot()
                        {
                            Name = inputSnap.Name,
                            Color = inputSnap.Color,
                            DataType = inputSnap.DataType,
                            Parent = function,
                            IsConnected = false,
                            IsNew = true,
                            ParentFunctionHash = function.Hash

                        };
                        newInputSnapSpot.Offset.X = inputSnap.Offset.X;
                        newInputSnapSpot.Offset.Y = inputSnap.Offset.Y;
                        newInputSnapSpot.IsHighLight = false;
                        newInputSnapSpot.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();
                        inputSnapSpotCollection.Add(newInputSnapSpot);
                    }


                    var outputSnapSpotCollection = new ObservableCollection<OutputSnapSpot>();
                    foreach (var outptSnap in functionConfig.OutputSnapSpotCollection)
                    {
                        var newOutputSnapSpot = new OutputSnapSpot()
                        {
                            Name = outptSnap.Name,
                            Color = outptSnap.Color,
                            DataType = outptSnap.DataType,
                            Parent = function,
                            IsNew = true,
                            ParentFunctionHash = function.Hash

                        };
                        newOutputSnapSpot.Offset.X = outptSnap.Offset.X;
                        newOutputSnapSpot.Offset.Y = outptSnap.Offset.Y;
                        newOutputSnapSpot.IsHighLight = false;
                        newOutputSnapSpot.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();
                        outputSnapSpotCollection.Add(newOutputSnapSpot);
                    }

                    function.Input.AddRange(inputSnapSpotCollection);
                    function.Output.AddRange(outputSnapSpotCollection);
                    function.Location.ValueChanged();

                    control.FunctionCollection.Add(function);
                    control.InputSnapSpotCollection.AddRange(function.Input);
                    control.OutputSnapSpotCollection.AddRange(function.Output);

                    control.SelectedDiagram = function;

                }
                else
                {
                    control.FunctionCollection.Where(node => node.IsNew).ToList().ForEach(node =>
                    {
                        node.Input.ToList().ForEach(snap => control.InputSnapSpotCollection.Remove(snap));
                        node.Output.ToList().ForEach(snap => control.OutputSnapSpotCollection.Remove(snap));
                        control.FunctionCollection.Remove(node);
                    });

                }
            }
        }

        public static readonly DependencyProperty IsConnectorCreateProperty = DependencyProperty.Register("IsConnectorCreate", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsConnectorCreateChanged));
        public bool IsConnectorCreate
        {
            get
            {
                return (bool)GetValue(IsConnectorCreateProperty);
            }

            set
            {

                SetValue(IsConnectorCreateProperty, value);


    

            }
        }

        private static void OnIsConnectorCreateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                bool isConnectorCreate = (bool)e.NewValue;
                if(isConnectorCreate == true)
                {
                    isConnectorCreate = false;
                }
            }
        }



        public static readonly DependencyProperty SelectedDiagramConfigProperty = DependencyProperty.Register("SelectedDiagramConfig", typeof(DiagramConfig), typeof(FunctionDiagramViewer), new PropertyMetadata(OnSelectedDiagramConfigChanged));
        public DiagramConfig SelectedDiagramConfig
        {
            get
            {
                return (DiagramConfig)GetValue(SelectedDiagramConfigProperty);
            }

            set
            {
                SetValue(SelectedDiagramConfigProperty, value);
            }
        }

        private static void OnSelectedDiagramConfigChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {

                DiagramConfig oldValue = (DiagramConfig)e.OldValue;
                DiagramConfig newValue = (DiagramConfig)e.NewValue;

                if(newValue != oldValue)
                {
                    control.IsFunctionCreate = false;
                    control.IsFunctionCreate = true;
                }

            }
        }




        public static readonly DependencyProperty DiagramConfigCollectionProperty = DependencyProperty.Register("DiagramConfigCollection", typeof(ObservableCollection<DiagramConfig>), typeof(FunctionDiagramViewer));
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get
            {
                return (ObservableCollection<DiagramConfig>)GetValue(DiagramConfigCollectionProperty);
            }

            set
            {
                SetValue(DiagramConfigCollectionProperty, value);
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
        public DiagramObject SelectedDiagram
        {
            get
            {
                return (DiagramObject)GetValue(SelectedDiagramProperty);
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

        private InputSnapSpot GetInputSnapSpotUnderMouse()
        {
            var item = Mouse.DirectlyOver as ContentPresenter;
            if (item == null)
                return null;

            return item.DataContext as InputSnapSpot;
        }

        private OutputSnapSpot GetOutputSnapSpotUnderMouse()
        {
            var item = Mouse.DirectlyOver as ContentPresenter;
            if (item == null)
                return null;

            return item.DataContext as OutputSnapSpot;
        }


        private void DiagramList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;

            if (listbox == null)
                return;

            this.CurrentX = e.GetPosition(listbox).X;
            this.CurrentY = e.GetPosition(listbox).Y;


            if (SelectedDiagram != null && SelectedDiagram is Function && SelectedDiagram.IsNew)
            {
                SelectedDiagram.Location.X = e.GetPosition(listbox).X;
                SelectedDiagram.Location.Y = e.GetPosition(listbox).Y;
            }

        }



        public static readonly DependencyProperty CurrentXProperty = DependencyProperty.Register("CurrentX", typeof(double), typeof(FunctionDiagramViewer));
        public double CurrentX
        {
            get
            {
                return (double)GetValue(CurrentXProperty);
            }

            set
            {
                SetValue(CurrentXProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentYProperty = DependencyProperty.Register("CurrentY", typeof(double), typeof(FunctionDiagramViewer));
        public double CurrentY
        {
            get
            {
                return (double)GetValue(CurrentYProperty);
            }

            set
            {
                SetValue(CurrentYProperty, value);
            }
        }

        private void DiagramList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsFunctionCreate == true)
            {
                if (SelectedDiagram != null && SelectedDiagram is Function && SelectedDiagram.IsNew == true)
                {
                    var function = SelectedDiagram as Function;
                    function.Activate();
                    IsFunctionCreate = false;
                    IsFunctionCreate = true;
                    //SelectedDiagram = null;
                    e.Handled = true;
                    return;
                }
            }

            if (this.IsConnectorCreate == true && this.SelectedDiagram != null)
            {

                var inputSnapSpot = this.GetInputSnapSpotUnderMouse();
                var outputSnapSpot = this.GetOutputSnapSpotUnderMouse();




            }
        }

        private void DiagramList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsFunctionCreate = false;
            this.IsConnectorCreate = false;
        }
    }
}
