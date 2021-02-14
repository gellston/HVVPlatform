using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using DevExpress.Xpf.CodeView;
using DiagramViewerTest.Model;

namespace DiagramViewerTest.UC
{
    /// <summary>
    /// DiagramViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DiagramViewer : UserControl, INotifyPropertyChanged
    {
        public DiagramViewer()
        {
            InitializeComponent();


            IsNoAction = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }




        private ObservableCollection<Node> _FunctionCollection = null;
        public ObservableCollection<Node> FunctionCollection
        {
            get
            {
                _FunctionCollection ??= new ObservableCollection<Node>();
                return _FunctionCollection;
            }
        }

        private ObservableCollection<SnapSpot> _SnapsCollection = null;
        public ObservableCollection<SnapSpot> SnapsCollection
        {
            get
            {
                _SnapsCollection ??= new ObservableCollection<SnapSpot>();
                return _SnapsCollection;
            }
        }

        private ObservableCollection<Connector> _ConnectorCollection = null;
        public ObservableCollection<Connector> ConnectorCollection
        {
            get
            {
                _ConnectorCollection ??= new ObservableCollection<Connector>();
                return _ConnectorCollection;
            }
        }



        private DiagramObject _SelectedDiagram = null;
        public DiagramObject SelectedDiagram
        {
            get => _SelectedDiagram;
            set {

                ShowMidPoint = false;

                if (value is SnapSpot)
                    return;


                _SelectedDiagram = value;
                OnPropertyChanged(nameof(SelectedDiagram));


                
                if (value is Connector)
                {
                    var connector = value as Connector;
                    if (connector.Start != null && connector.MidPoint != null && connector.IsNew == false)
                        ShowMidPoint = true;
                }
          
            }
        }


        private bool _IsConnectorCreate = false;
        public bool IsConnectorCreate
        {
            get => _IsConnectorCreate;
            set
            {

                _IsConnectorCreate = value;
                OnPropertyChanged(nameof(IsConnectorCreate));

                if (value)
                {
                    var connector = new Connector()
                    {
                        Name = "Connector" + (ConnectorCollection.Count + 1),
                        IsNew = true,
                    };

                    ConnectorCollection.Add(connector);
                    SelectedDiagram = connector;

                }
                else
                {
                   
                    ConnectorCollection.Where(x => x.IsNew).ToList().ForEach(x => ConnectorCollection.Remove(x));
                }

                
            }
        }

        private bool _IsNodeCreate = true;
        public bool IsNodeCreate
        {
            get => _IsNodeCreate;
            set
            {
                _IsNodeCreate = value;
                OnPropertyChanged(nameof(IsNodeCreate));


                if (value)
                {
                    var node = new Node()
                    {
                        Name = "FucntionA",
                    };
                    node.Location.X = this.CurrentX;
                    node.Location.Y = this.CurrentY;
                    node.IsNew = true;
                    this.FunctionCollection.Add(node);
                    this.SnapsCollection.AddRange(node.InputCollection);
                    this.SelectedDiagram = node;
                }
                else
                {
                    FunctionCollection.Where(node => node.IsNew).ToList().ForEach(node =>
                    {
                        node.InputCollection.ToList().ForEach(snap => SnapsCollection.Remove(snap));
                        FunctionCollection.Remove(node);
                    });

                }
            }
        }

        private bool _IsNoAction = true;
        public bool IsNoAction
        {
            get => _IsNoAction;
            set
            {
                _IsNoAction = value;
                OnPropertyChanged(nameof(IsNoAction));

                if (value)
                {
                    FunctionCollection.Where(node => node.IsNew).ToList().ForEach(node =>
                    {
                        node.InputCollection.ToList().ForEach(snap => SnapsCollection.Remove(snap));
                        FunctionCollection.Remove(node);
                    });

                    ConnectorCollection.Where(x => x.IsNew).ToList().ForEach(x => ConnectorCollection.Remove(x));
                }
            }
        }

        private double _CurrentX = 0;
        public double CurrentX
        {
            get => _CurrentX;
            set {
                _CurrentX = value;
                OnPropertyChanged(nameof(CurrentX));
            }
        }

        private double _CurrentY = 0;
        public double CurrentY
        {
            get => _CurrentY;
            set
            {
                _CurrentY = value;
                OnPropertyChanged(nameof(CurrentY));
            }
        }


        private bool _ShowMidPoint = false;
        public bool ShowMidPoint
        {
            get => _ShowMidPoint;
            set
            {
                _ShowMidPoint = value;
                OnPropertyChanged(nameof(ShowMidPoint));
            }
        }



        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var node = thumb.DataContext as Node;
            if (node == null)
                return;

            node.Location.Value = Point.Add(node.Location.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var node = new Node()
            {
                Name = "FucntionA",
            };
            node.Location.X = new Random().NextDouble()*100;
            node.Location.Y = new Random().NextDouble() * 100;
            this.FunctionCollection.Add(node);
            this.SnapsCollection.AddRange(node.InputCollection);
            node.Activate();
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
           
            if (IsNodeCreate)
            {
                if (SelectedDiagram != null && SelectedDiagram is Node && SelectedDiagram.IsNew == true)
                {
                    var node = SelectedDiagram as Node;
                    node.Activate();
                    IsNodeCreate = true;
                    e.Handled = true;
                    return;
                }

            }else if (IsConnectorCreate)
            {
                var snapSpot = GetSnapSpotUnderMouse();
                var connector = SelectedDiagram as Connector;


                if (snapSpot != null && connector is Connector && connector.Start != null && connector.End != null && connector.Start != connector.End)
                {

                    //ConnectorCollection.Where(x => x.Start == x.End).ToList().ForEach(x => ConnectorCollection.Remove(x));
                    if (ConnectorCollection.Where(x => x.Start == connector.Start && x.End == connector.End).Count() > 1)
                    {
                        ConnectorCollection.Remove(connector);
                        IsNoAction = true;
                        return;
                    }
                    

                    connector.IsNew = false;


                    IsConnectorCreate = true;
                    e.Handled = true;
                    return;
                }



                if (snapSpot != null && connector is Connector && connector.Start == null)
                {
                    connector.Start = snapSpot;
                    //snapSpot.IsHighlighted = true;
                    e.Handled = true;
                    return;
                }


            }


        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;

            if (listbox == null)
                return;

            this.CurrentX = e.GetPosition(listbox).X;
            this.CurrentY = e.GetPosition(listbox).Y;

            if (SelectedDiagram != null && SelectedDiagram is Node && SelectedDiagram.IsNew)
            {
                SelectedDiagram.Location.X = e.GetPosition(listbox).X;
                SelectedDiagram.Location.Y = e.GetPosition(listbox).Y;
            }
            else if (SelectedDiagram != null && SelectedDiagram is Connector && SelectedDiagram.IsNew)
            {
                var snap = GetSnapSpotUnderMouse();
                if (snap == null)
                   return;

                var connector = SelectedDiagram as Connector;

                if (connector.Start != null && snap != connector.Start && snap.Parent != connector.Start.Parent)
                {
                    connector.End = snap;
                }
                    
            }

        }

        private SnapSpot GetSnapSpotUnderMouse()
        {
            var item = Mouse.DirectlyOver as ContentPresenter;
            if (item == null)
                return null;

            return item.DataContext as SnapSpot;
        }

        private void Mid_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var connector = thumb.DataContext as Connector;
            if (connector == null)
                return;

            connector.MidPoint.Value = Point.Add(connector.MidPoint.Value, new Vector(e.HorizontalChange, e.VerticalChange));
        }
    }
}
