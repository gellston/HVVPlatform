using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Model;
using Model.DiagramProperty;


namespace UClib
{
    /// <summary>
    /// FunctionDiagramViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FunctionDiagramViewer : UserControl, INotifyPropertyChanged
    {
        public FunctionDiagramViewer()
        {
            InitializeComponent();

            //this.ConnectorCollection = new ObservableCollection<Connector>();


        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            //var newWidthSize = sizeInfo.NewSize.Width;
            //var newHeightSize = sizeInfo.NewSize.Height;

            //this.CanvasWidth = newWidthSize;


            //this.UpdateFunctionDiagramLayout();
            //this.UpdateConnectorDiagramLayout();

            var newWidthSize = sizeInfo.NewSize.Width;
            var newHeightSize = sizeInfo.NewSize.Height;

            this.CanvasWidth = newWidthSize;


            this.UpdateFunctionDiagramLayout();
            this.UpdateConnectorDiagramLayout();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Set<T>(ref T _reference, T _value, [CallerMemberName] string _name = "")
        {
            if (!Equals(_reference, _value))
            {
                _reference = _value;
                OnPropertyChanged(_name);
            }
        }








        private ICommand _RefreshLayoutCommand = null;
        public ICommand RefreshLayoutCommand
        {
            get
            {
                _RefreshLayoutCommand ??= new RelayCommand(() =>
                {
                    this.UpdateConnectorDiagramLayout();
                    this.UpdateFunctionDiagramLayout();
                });

                return _RefreshLayoutCommand;
            }
        }


        private ICommand _AddNewFunctionCommand = null;
        public ICommand AddNewFunctionCommand
        {
            get
            {
                _AddNewFunctionCommand ??= new RelayCommand(() =>
                {
                    this.IsConnectorCreate = false;
                    this.IsNoAction = false;

                    var functionConfig = this.SelectedDiagramConfig;
                    if (functionConfig == null) return;

                    /// Function Part
                    var function = new Function()
                    {
                        Name = functionConfig.FunctionInfo.Name,
                        Color = functionConfig.FunctionInfo.Color,
                        IsNew = true,
                        ScriptContent = functionConfig.DiagramScript
                    };
                    function.Location.X = this.CurrentX;
                    function.Location.Y = this.CurrentY;


                    function.Hash = DateTime.Now.ToString("_yyyy_HH_mm_dd_HH_MM_ss_fff_") + Guid.NewGuid().ToString("N");

                    /// InputSnapSpot Part
                    var inputSnapSpotCollection = new ObservableCollection<InputSnapSpot>();
                    foreach (var inputSnap in functionConfig.InputSnapSpotCollection)
                    {
                        var newInputSnapSpot = new InputSnapSpot(inputSnap.Name, inputSnap.DataType)
                        {
                            Color = inputSnap.Color,
                            Parent = function,
                            IsConnected = false,
                            IsNew = true,
                            ParentFunctionHash = function.Hash

                        };
                        newInputSnapSpot.Offset.X = inputSnap.Offset.X;
                        newInputSnapSpot.Offset.Y = inputSnap.Offset.Y;
                        newInputSnapSpot.IsHighLight = false;
                        newInputSnapSpot.Hash = DateTime.Now.ToString("_yyyy_HH_mm_dd_HH_MM_ss_fff_") + Guid.NewGuid().ToString("N");
                        newInputSnapSpot.DiagramProperty = inputSnap.DiagramProperty.Clone() as BaseDiagramProperty;
                        inputSnapSpotCollection.Add(newInputSnapSpot);
                    }


                    var outputSnapSpotCollection = new ObservableCollection<OutputSnapSpot>();
                    foreach (var outptSnap in functionConfig.OutputSnapSpotCollection)
                    {
                        var newOutputSnapSpot = new OutputSnapSpot(outptSnap.Name, outptSnap.DataType)
                        {
                            Color = outptSnap.Color,
                            Parent = function,
                            IsNew = true,
                            ParentFunctionHash = function.Hash

                        };
                        newOutputSnapSpot.Offset.X = outptSnap.Offset.X;
                        newOutputSnapSpot.Offset.Y = outptSnap.Offset.Y;
                        newOutputSnapSpot.IsHighLight = false;
                        newOutputSnapSpot.Hash = DateTime.Now.ToString("_yyyy_HH_mm_dd_HH_MM_ss_fff_") + Guid.NewGuid().ToString("N");
                        outputSnapSpotCollection.Add(newOutputSnapSpot);
                    }



                    //var functionPropertyCollection = new ObservableCollection<BaseDiagramProperty>();
                    //var functionPropertyies = Helper.Extensions.Clone(functionConfig.FunctionProperties.ToList());
                    //foreach (var property in functionPropertyies)
                    //{
                    //    //property.ParentFunctionHash = function.Hash;
                    //    property.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();
                    //    //functionPropertyCollection.Add(property);
                    //}

                    function.Input.AddRange(inputSnapSpotCollection);
                    function.Output.AddRange(outputSnapSpotCollection);
                    //function.FunctionProperties.AddRange(functionPropertyCollection);

                    //var functionHeight = 30 + (function.Input.Count() + function.Output.Count()) * 30;

                    //function.Size.Height = functionHeight;


                    function.Location.ValueChanged();

                    this.FunctionCollection.Add(function);
                    this.InputSnapSpotCollection.AddRange(function.Input);
                    this.OutputSnapSpotCollection.AddRange(function.Output);

                    function.Activate();

                    //this.SelectedDiagram = function;

                    this.UpdateFunctionDiagramLayout();
                    this.UpdateConnectorDiagramLayout();
                    //this.OutterScrollViewer.UpdateLayout();


                    OnPropertyChanged("FunctionCollection");

                });

                return _AddNewFunctionCommand;
            }
        }


        private bool _ShowMidPoint = false;
        public bool ShowMidPoint
        {
            get => _ShowMidPoint;
            set => Set(ref _ShowMidPoint, value);
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
                // if((bool)e.NewValue == true)
                //    control.IsShowSequencePanel = false;
            }
        }


        public static readonly DependencyProperty IsShowSequencePanelProperty = DependencyProperty.Register("IsShowSequencePanel", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsShowSequencePanelChanged));
        public bool IsShowSequencePanel
        {
            get
            {
                return (bool)GetValue(IsShowSequencePanelProperty);
            }

            set
            {
                SetValue(IsShowSequencePanelProperty, value);
            }
        }

        private static void OnIsShowSequencePanelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                //if ((bool)e.NewValue == true)
                //     control.IsShowFunctionPanel = false;

            }
        }


        //public static readonly DependencyProperty IsShowTopPanelProperty = DependencyProperty.Register("IsShowTopPanel", typeof(bool), typeof(FunctionDiagramViewer));
        //public bool IsShowTopPanel
        //{
        //    get
        //    {
        //        return (bool)GetValue(IsShowTopPanelProperty);
        //    }

        //    set
        //    {
        //        SetValue(IsShowTopPanelProperty, value);
        //    }
        //}




        //public static readonly DependencyProperty IsFunctionCreateProperty = DependencyProperty.Register("IsFunctionCreate", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsFunctionCreateChanged));
        //public bool IsFunctionCreate
        //{
        //    get
        //    {
        //        return (bool)GetValue(IsFunctionCreateProperty);
        //    }

        //    set
        //    {
        //        SetValue(IsFunctionCreateProperty, value);

        //    }
        //}

        //private static void OnIsFunctionCreateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //FunctionDiagramViewer control = sender as FunctionDiagramViewer;
        //if (control != null)
        //{
        //    bool isNodeCreate = (bool)e.NewValue;

        //    if (isNodeCreate)
        //    {
        //        control.IsConnectorCreate = false;
        //        control.IsNoAction = false;

        //        var functionConfig = control.SelectedDiagramConfig;
        //        if (functionConfig == null) return;

        //        /// Function Part
        //        var function = new Function()
        //        {
        //            Name = functionConfig.FunctionInfo.Name,
        //            Color = functionConfig.FunctionInfo.Color,
        //            IsNew = true,
        //        };
        //        function.Location.X = control.CurrentX;
        //        function.Location.Y = control.CurrentY;


        //        // function.Size.Width = functionConfig.FunctionInfo.Size.Width;
        //        //function.Size.Height = functionConfig.FunctionInfo.Size.Height;
        //        function.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();

        //        /// InputSnapSpot Part
        //        var inputSnapSpotCollection = new ObservableCollection<InputSnapSpot>();
        //        foreach(var inputSnap in functionConfig.InputSnapSpotCollection)
        //        {
        //            var newInputSnapSpot = new InputSnapSpot(inputSnap.Name, inputSnap.DataType)
        //            {
        //                Color = inputSnap.Color,
        //                Parent = function,
        //                IsConnected = false,
        //                IsNew = true,
        //                ParentFunctionHash = function.Hash

        //            };
        //            newInputSnapSpot.Offset.X = inputSnap.Offset.X;
        //            newInputSnapSpot.Offset.Y = inputSnap.Offset.Y;
        //            newInputSnapSpot.IsHighLight = false;
        //            newInputSnapSpot.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();
        //            inputSnapSpotCollection.Add(newInputSnapSpot);
        //        }


        //        var outputSnapSpotCollection = new ObservableCollection<OutputSnapSpot>();
        //        foreach (var outptSnap in functionConfig.OutputSnapSpotCollection)
        //        {
        //            var newOutputSnapSpot = new OutputSnapSpot(outptSnap.Name, outptSnap.DataType)
        //            {
        //                Color = outptSnap.Color,
        //                Parent = function,
        //                IsNew = true,
        //                ParentFunctionHash = function.Hash

        //            };
        //            newOutputSnapSpot.Offset.X = outptSnap.Offset.X;
        //            newOutputSnapSpot.Offset.Y = outptSnap.Offset.Y;
        //            newOutputSnapSpot.IsHighLight = false;
        //            newOutputSnapSpot.Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString();
        //            outputSnapSpotCollection.Add(newOutputSnapSpot);
        //        }

        //        function.Input.AddRange(inputSnapSpotCollection);
        //        function.Output.AddRange(outputSnapSpotCollection);


        //        var functionHeight = 25 * 2 + (function.Input.Count() + function.Output.Count()) * 35;

        //        function.Size.Height = functionHeight;


        //        function.Location.ValueChanged();

        //        control.FunctionCollection.Add(function);
        //        control.InputSnapSpotCollection.AddRange(function.Input);
        //        control.OutputSnapSpotCollection.AddRange(function.Output);

        //        control.SelectedDiagram = function;

        //    }
        //    else
        //    {
        //        control.FunctionCollection.Where(node => node.IsNew).ToList().ForEach(node =>
        //        {
        //            node.Input.ToList().ForEach(snap => control.InputSnapSpotCollection.Remove(snap));
        //            node.Output.ToList().ForEach(snap => control.OutputSnapSpotCollection.Remove(snap));
        //            control.FunctionCollection.Remove(node);
        //        });

        //    }
        //}
        //}

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
                if (isConnectorCreate == true)
                {
                    //control.IsFunctionCreate = false;
                    control.IsNoAction = false;
                    var connector = new Connector()
                    {
                        Name = "Connector" + (control.ConnectorCollection.Count + 1),
                        IsNew = true,
                        Hash = DateTime.Today.ToString("yyyy-HH-mm-dd HH:MM:ss:fff ") + Guid.NewGuid().ToString()
                    };

                    control.ConnectorCollection.Add(connector);
                    control.SelectedDiagram = connector;


                }
                else
                {

                    control.ConnectorCollection.Where(connector => connector.IsNew).ToList().ForEach(connector =>
                    {
                        control.ConnectorCollection.Remove(connector);
                    });
                }
            }
        }


        public static readonly DependencyProperty IsNoActionProperty = DependencyProperty.Register("IsNoAction", typeof(bool), typeof(FunctionDiagramViewer), new PropertyMetadata(OnIsNoActionChanged));
        public bool IsNoAction
        {
            get
            {
                return (bool)GetValue(IsNoActionProperty);
            }

            set
            {
                SetValue(IsNoActionProperty, value);
            }
        }

        private static void OnIsNoActionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                bool isNoAction = (bool)e.NewValue;
                if (isNoAction == true)
                {
                    control.IsConnectorCreate = false;
                    //control.IsFunctionCreate = false;

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
            //FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            //if (control != null)
            //{

            //    DiagramConfig oldValue = (DiagramConfig)e.OldValue;
            //    DiagramConfig newValue = (DiagramConfig)e.NewValue;

            //    if(newValue != oldValue)
            //    {
            //        control.IsFunctionCreate = false;
            //        control.IsFunctionCreate = true;
            //    }

            //}
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

        public static readonly DependencyProperty CanvasHeightProperty = DependencyProperty.Register("CanvasHeight", typeof(double), typeof(FunctionDiagramViewer), new PropertyMetadata(2040.0));
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

        /*
        private static void FunctionCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           // var calendar = d as RadCalendar;

            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                // Unsubscribe from CollectionChanged on the old collection
                coll.CollectionChanged -= FunctionCollectinItemChanged;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<Function>)e.NewValue;
                //calendar.DayTemplateSelector = new SpecialDaySelector(coll, GetSpecialDayTemplate(d));
                // Subscribe to CollectionChanged on the new collection
                coll.CollectionChanged += FunctionCollectinItemChanged;
               
            }
        }


        private static void FunctionCollectinItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Function> collection = sender as ObservableCollection<Function>;
            
            //if(control != null)
            //{
            //    control.UpdateFunctionDiagramLayout();
           // }


        }*/


        public static readonly DependencyProperty SelectedDiagramProperty = DependencyProperty.Register("SelectedDiagram", typeof(DiagramObject), typeof(FunctionDiagramViewer), new PropertyMetadata(OnSelectedDiagramChanged));
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


        private static void OnSelectedDiagramChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FunctionDiagramViewer control = sender as FunctionDiagramViewer;
            if (control != null)
            {
                control.ShowMidPoint = false;

                DiagramObject newValue = (DiagramObject)e.NewValue;


                //if (newValue is InputSnapSpot) 
                //    return;
                //if (newValue is OutputSnapSpot) 
                //    return;

                if (newValue is Connector)
                {
                    var connector = newValue as Connector;
                    if (connector.Start != null && connector.End != null && connector.MidPoint != null && connector.IsNew == false)
                        control.ShowMidPoint = true;

                    control.SelectedFunction = null;
                    control.SelectedConnector = connector;
                }

                if (newValue is Function)
                {
                    var function = newValue as Function;
                    control.SelectedInputSnapSpotCollection = function.Input;
                    control.SelectedFunction = newValue as Function;
                    control.SelectedConnector = null;
                }


            }
        }


        public static readonly DependencyProperty SelectedFunctionProperty = DependencyProperty.Register("SelectedFunction", typeof(Function), typeof(FunctionDiagramViewer));
        public Function SelectedFunction
        {
            get
            {
                return (Function)GetValue(SelectedFunctionProperty);
            }

            set
            {
                SetValue(SelectedFunctionProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedConnectorProperty = DependencyProperty.Register("SelectedConnector", typeof(Connector), typeof(FunctionDiagramViewer));
        public Connector SelectedConnector
        {
            get
            {
                return (Connector)GetValue(SelectedConnectorProperty);
            }

            set
            {
                SetValue(SelectedConnectorProperty, value);
            }
        }


        private ObservableCollection<InputSnapSpot> _SelectedInputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> SelectedInputSnapSpotCollection
        {
            get => _SelectedInputSnapSpotCollection;
            set => Set(ref _SelectedInputSnapSpotCollection, value);
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
        //                System.Diagnostics.Debug.WriteLine("test");
        //                break;

        //            case NotifyCollectionChangedAction.Remove:
        //                System.Diagnostics.Debug.WriteLine("test");
        //                break;

        //        }

        //    }catch(Exception exception)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exception.Message);
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


            //if (SelectedDiagram != null && SelectedDiagram is Function && SelectedDiagram.IsNew)
            //{
            //    SelectedDiagram.Location.X = e.GetPosition(listbox).X;
            //    SelectedDiagram.Location.Y = e.GetPosition(listbox).Y;
            //}else 

            if (SelectedDiagram != null && SelectedDiagram is Connector && SelectedDiagram.IsNew)
            {
                // InputSnapSpot
                var snapSpot = GetInputSnapSpotUnderMouse();
                if (snapSpot == null)
                    return;

                var connector = SelectedDiagram as Connector;
                if (connector.Start == null) return;

                if (this.ConnectorCollection.Where(data => data.End == snapSpot && data.IsNew == false).Count() > 0)
                    return;

                var endSpotIndex = this.FunctionCollection.IndexOf(snapSpot.Parent);
                var startSpotIndex = this.FunctionCollection.IndexOf(connector.Start.Parent);

                if (startSpotIndex > endSpotIndex)
                    return;


                if (connector.Start.DataType != snapSpot.DataType)
                    return;

                if (connector.Start != null && snapSpot.Parent != connector.Start.Parent)
                {
                    connector.End = snapSpot;
                    connector.MidPoint.X = this.CanvasWidth * 0.15;
      
                }
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
            //if (this.IsFunctionCreate == true)
            //{
            //    if (SelectedDiagram != null && SelectedDiagram is Function && SelectedDiagram.IsNew == true)
            //    {
            //        var function = SelectedDiagram as Function;
            //        function.Activate();
            //        IsFunctionCreate = false;
            //        IsFunctionCreate = true;

            //        this.UpdateFunctionDiagramLayout();

            //        e.Handled = true;
            //        return;
            //    }
            //}

            if (this.IsConnectorCreate == true && this.SelectedDiagram != null)
            {

                var inputSnapSpot = this.GetInputSnapSpotUnderMouse();
                var outputSnapSpot = this.GetOutputSnapSpotUnderMouse();
                var connector = SelectedDiagram as Connector;

                if (outputSnapSpot != null && connector is Connector && connector.Start == null)
                {
                    connector.Start = outputSnapSpot;
                    connector.StartSnapHash = outputSnapSpot.Hash;

                    e.Handled = true;
                    return;
                }

                if (inputSnapSpot != null && connector is Connector && connector.Start != null && connector.End != null && connector.Start.Parent != connector.End.Parent)
                {
                    var startIndex = this.FunctionCollection.IndexOf(connector.Start.Parent);
                    var endIndex = this.FunctionCollection.IndexOf(connector.End.Parent);

                    if (startIndex >= endIndex)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (ConnectorCollection.Where(x => x.Start == connector.Start && x.End == connector.End).Count() > 1)
                    {
                        ConnectorCollection.Remove(connector);
                        IsNoAction = true;
                        return;
                    }

                    connector.MidPoint.X = this.CanvasWidth * 0.15;
                    connector.IsNew = false;
                    connector.EndSnapHash = connector.End.Hash;

                    IsConnectorCreate = false;
                    IsConnectorCreate = true;

                    //this.UpdateDiagramLayout();
                    //this.UpdateConnectorDiagramLayout();

                    e.Handled = true;
                    return;
                }


            }
        }

        private void DiagramList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.IsFunctionCreate = false;
            this.IsConnectorCreate = false;
            this.IsNoAction = true;
        }

        private void LineMidPoint_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var connector = thumb.DataContext as Connector;
            if (connector == null)
                return;

            connector.MidPoint.Value = Point.Add(connector.MidPoint.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }


        private void UpdateFunctionDiagramLayout()
        {
            try
            {
                var functionCollection = FunctionCollection;
                int functionIndex = 0;
                double yVerticalOffset = 0;
                if (functionCollection == null) return;
                double newCanvasHeight = 0;
                foreach (var function in functionCollection)
                {
                    functionIndex++;
                    function.Location.X = this.CanvasWidth * 0.3;
                    function.Location.Y = 10 + yVerticalOffset;
                    function.Size.Width = this.CanvasWidth * 0.4;

                    var functionHeight = 30 + (function.Input.Count() + function.Output.Count()) * 30;
                    function.Size.Height = functionHeight;


                    yVerticalOffset = function.Location.Y + function.Size.Height;

                    newCanvasHeight += (10 + function.Size.Height);
                }




                if (this.CanvasHeight < newCanvasHeight + 10)
                    this.CanvasHeight = this.CanvasHeight * 2;

                OnPropertyChanged("FunctionCollection");


            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
        }

        private void UpdateConnectorDiagramLayout()
        {
            try
            {

                var connectorCollection = ConnectorCollection;
                if (connectorCollection == null) return;

                foreach (var connector in ConnectorCollection)
                {
                    connector.MidPoint.X = this.CanvasWidth * 0.15;
                }


                List<Connector> functionList = new List<Connector>();
                this.ConnectorCollection.ToList().ForEach((connector) =>
                {
                    if (connector.IsNew == true) return;
                    var startIndex = this.FunctionCollection.IndexOf(connector.Start.Parent);
                    var endIndex = this.FunctionCollection.IndexOf(connector.End.Parent);

                    if (startIndex > endIndex)
                    {
                        functionList.Add(connector);
                    }
                });

                functionList.ForEach(data => this.ConnectorCollection.Remove(data));



                OnPropertyChanged("ConnectorCollection");

            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
        }

        private bool _IsDragging = false;
        private Image _dragSource = null;

        private void FunctionSequenceListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView listview = sender as ListView;

            if (listview != null)
            {
                UIElement element = listview.InputHitTest(e.GetPosition(listview)) as UIElement;
                if (element is Image)
                {
                    var dataContext = ((Image)element).DataContext as Function;
                    if (dataContext != null)
                    {
                        _dragSource = element as Image;
                        _IsDragging = true;
                        return;
                    }



                }
            }
            _IsDragging = false;
            _dragSource = null;

        }

        private void FunctionSequenceListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ListView listview = sender as ListView;
            if (listview != null && _IsDragging && _dragSource != null && (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) > 0)
            {
                // set the data to be dragged

                var dataContext = _dragSource.DataContext as Function;
                // do drag drop
                DragDrop.DoDragDrop(_dragSource, dataContext, DragDropEffects.Move);


                _IsDragging = false;
                _dragSource = null;
            }
        }

        private void FunctionSequenceListView_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //ListView listview = sender as ListView;
            //if (listview != null && _IsDragging && _dragSource != null)
            //{
            //    UIElement element = listview.InputHitTest(e.GetPosition(listview)) as UIElement;
            //    if (element is Image)
            //    {
            //        var dataContext = ((Image)element).DataContext as Function;
            //        if (dataContext != null)
            //        {
            //            _dragSource = element as Image;
            //        }



            //    }
            //}

            _IsDragging = false;
            _dragSource = null;
        }

        private void FunctionSequenceListView_PreviewDrop(object sender, DragEventArgs e)
        {
            ListView listview = sender as ListView;
            if (listview != null && _IsDragging && _dragSource != null)
            {
                UIElement element = listview.InputHitTest(e.GetPosition(listview)) as UIElement;
                if (element is Image)
                {
                    var targetContext = ((Image)element).DataContext as Function;
                    if (targetContext != null)
                    {
                        var targetElement = element as Image;
                        var targetFunction = targetElement.DataContext as Function;

                        var sourceFunction = e.Data.GetData(typeof(Function)) as Function;


                        var sourceIndex = this.FunctionCollection.IndexOf(sourceFunction);
                        var targetIndex = this.FunctionCollection.IndexOf(targetFunction);

                        if (sourceIndex != targetIndex)
                        {
                            this.FunctionCollection[targetIndex] = sourceFunction;
                            this.FunctionCollection[sourceIndex] = targetFunction;
                            this.UpdateFunctionDiagramLayout();
                            this.UpdateConnectorDiagramLayout();
                        }

                    }



                }
            }
            _IsDragging = false;
            _dragSource = null;

        }
    }
}
