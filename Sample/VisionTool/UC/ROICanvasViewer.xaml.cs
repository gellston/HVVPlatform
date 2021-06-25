using Converter;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UClib
{
    /// <summary>
    /// ImageCanvasViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ROICanvasViewer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public void Set<T>(string _name, ref T _reference, T _value)
        {
            if (!Equals(_reference, _value))
            {
                _reference = _value;
                OnPropertyRaised(_name);
            }
        }


        public ROICanvasViewer()
        {
            InitializeComponent();
            //CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        ~ROICanvasViewer()
        {
            //CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        //private void CompositionTarget_Rendering(object sender, EventArgs e)
        //{

        //    ROICanvasViewer control = this;
        //    if (control != null)
        //    {

        //        try
        //        {
        //            Function function = this.SelectedFunction;
        //            if (function == null) return;
        //            ObservableCollection<InputSnapSpot> inputCollection = function.Input;
        //            foreach (var input in inputCollection)
        //            {
        //                if (input.DataType == control.TargetImageType)
        //                {
        //                    try
        //                    {
        //                        var connector = control.ConnectorCollection.ToList().Where(x => x.EndSnapHash == input.Hash).First();
        //                        var outputSnapSpotHash = connector.StartSnapHash;

        //                        for (int index = 0; index < control.GlobalNames.Count(); index++)
        //                        {
        //                            if (outputSnapSpotHash == control.GlobalNames[index])
        //                            {
        //                                var imageObject = control.GlobalObjectCollection[index];


        //                                HVObjectBitmapImageConverter converter = new HVObjectBitmapImageConverter();
        //                                var bitmap = converter.Convert(imageObject, null, null, null);
        //                                control.Image = bitmap as WriteableBitmap;
        //                                break;
        //                            }
        //                        }


        //                    }
        //                    catch (Exception exception1)
        //                    {
        //                        System.Diagnostics.Debug.WriteLine(exception1.Message);
        //                        continue;
        //                    }

        //                }
        //            }
        //        }
        //        catch (Exception exception2)
        //        {
        //            System.Diagnostics.Debug.WriteLine(exception2.Message);
        //        }


        //    }


        //    this.InvalidateVisual();

        //}

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ROICanvasViewer), new PropertyMetadata(OnCustomerChangedCallBack));
        public ImageSource Image
        {
            get
            {
                return (ImageSource)GetValue(ImageProperty);
            }

            set
            {
                SetValue(ImageProperty, value);
            }
        }

        private static void OnCustomerChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ROICanvasViewer control = sender as ROICanvasViewer;
            if (control != null)
            {
                ImageSource image = e.NewValue as ImageSource;
                if (image == null) return;

                control.CanvasWidth = image.Width;
                control.CanvasHeight = image.Height;
                //control.OnPropertyRaised("Image");

                //control.OutScrollViewer.UpdateLayout();

                //control.OutScrollViewer.ScrollToVerticalOffset(control.OutScrollViewer.ScrollableHeight / 2);
                //control.OutScrollViewer.ScrollToHorizontalOffset(control.OutScrollViewer.ScrollableWidth / 2);

            }
        }

        //private HV.V1.Object _ImageObject = null;
        //public HV.V1.Object ImageObject
        //{
        //    get => _ImageObject;

        //    set => Set(nameof(ImageObject), ref _ImageObject, value);
        //}

        private double _CanvasWidth = 0;
        public double CanvasWidth
        {
            get => _CanvasWidth;
            set => Set<double>(nameof(CanvasWidth), ref _CanvasWidth, value);
        }

        private double _CanvasHeight = 0;
        public double CanvasHeight
        {
            get => _CanvasHeight;
            set => Set<double>(nameof(CanvasHeight), ref _CanvasHeight, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(ROICanvasViewer), new PropertyMetadata(1.0));
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
        }

        public static readonly DependencyProperty ZoomMaxProperty = DependencyProperty.Register("ZoomMax", typeof(double), typeof(ROICanvasViewer), new PropertyMetadata(10.0));
        public double ZoomMax
        {
            get
            {
                return (double)GetValue(ZoomMaxProperty);
            }

            set
            {
                SetValue(ZoomMaxProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomMinProperty = DependencyProperty.Register("ZoomMin", typeof(double), typeof(ROICanvasViewer), new PropertyMetadata(0.1));
        public double ZoomMin
        {
            get
            {
                return (double)GetValue(ZoomMinProperty);
            }

            set
            {
                SetValue(ZoomMinProperty, value);
            }
        }

        public static readonly DependencyProperty ZoomStepProperty = DependencyProperty.Register("ZoomStep", typeof(double), typeof(ROICanvasViewer), new PropertyMetadata(0.505));
        public double ZoomStep
        {
            get
            {
                return (double)GetValue(ZoomStepProperty);
            }

            set
            {
                SetValue(ZoomStepProperty, value);
            }
        }

        public static readonly DependencyProperty TranslationXProperty = DependencyProperty.Register("TranslationX", typeof(double), typeof(ROICanvasViewer));
        public double TranslationX
        {
            get
            {
                return (double)GetValue(TranslationXProperty);
            }

            set
            {
                SetValue(TranslationXProperty, value);
            }
        }


        public static readonly DependencyProperty TranslationYProperty = DependencyProperty.Register("TranslationY", typeof(double), typeof(ROICanvasViewer));
        public double TranslationY
        {
            get
            {
                return (double)GetValue(TranslationYProperty);
            }

            set
            {
                SetValue(TranslationYProperty, value);
            }
        }



        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(HV.V1.Object), typeof(ROICanvasViewer), new PropertyMetadata(OnSelectedItemChangedCallBack));
        public HV.V1.Object SelectedItem
        {
            get
            {
                return (HV.V1.Object)GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }
        private static void OnSelectedItemChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ROICanvasViewer control = sender as ROICanvasViewer;
            if (control != null)
            {

            }
        }




        public static readonly DependencyProperty SelectedFunctionProperty = DependencyProperty.Register("SelectedFunction", typeof(Function), typeof(ROICanvasViewer), new PropertyMetadata(OnInputSnapSpotCollectionChanged));
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


        public static readonly DependencyProperty ConnectorCollectionProperty = DependencyProperty.Register("ConnectorCollection", typeof(ObservableCollection<Connector>), typeof(ROICanvasViewer));
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

        public static readonly DependencyProperty GlobalObjectCollectionProperty = DependencyProperty.Register("GlobalObjectCollection", typeof(ObservableCollection<HV.V1.Object>), typeof(ROICanvasViewer));
        public ObservableCollection<HV.V1.Object> GlobalObjectCollection
        {
            get
            {
                return (ObservableCollection<HV.V1.Object>)GetValue(GlobalObjectCollectionProperty);
            }

            set
            {
                SetValue(GlobalObjectCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty TargetImageTypeProperty = DependencyProperty.Register("TargetImageType", typeof(string), typeof(ROICanvasViewer));
        public string TargetImageType
        {
            get
            {
                return (string)GetValue(TargetImageTypeProperty);
            }

            set
            {
                SetValue(TargetImageTypeProperty, value);
            }
        }

        private static void OnInputSnapSpotCollectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs exception)
        {
            ROICanvasViewer control = sender as ROICanvasViewer;
            if (control != null)
            {

                try
                {
                    Function function = exception.NewValue as Function;
                    if (function == null) return;
                    ObservableCollection<InputSnapSpot> inputCollection = function.Input;
                    foreach (var input in inputCollection)
                    {
                        if (input.DataType == control.TargetImageType)
                        {
                            try
                            {
                                var connector = control.ConnectorCollection.ToList().Where(x => x.EndSnapHash == input.Hash).First();
                                var outputSnapSpotHash = connector.StartSnapHash;

                                for (int index = 0; index < control.GlobalNames.Count(); index++)
                                {
                                    if (outputSnapSpotHash == control.GlobalNames[index])
                                    {
                                        var imageObject = control.GlobalObjectCollection[index];


                                        HVObjectBitmapImageConverter converter = new HVObjectBitmapImageConverter();
                                        var bitmap = converter.Convert(imageObject, null,null,null);
                                        control.Image = bitmap as WriteableBitmap;
                                        break;
                                    }
                                }


                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message);
                                continue;
                            }

                        }
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
               

            }
        }




        public static readonly DependencyProperty GlobalNamesProperty = DependencyProperty.Register("GlobalNames", typeof(ObservableCollection<string>), typeof(ROICanvasViewer));
        public ObservableCollection<string> GlobalNames
        {
            get
            {
                return (ObservableCollection<string>)GetValue(GlobalNamesProperty);
            }
            set
            {
                SetValue(GlobalNamesProperty, value);
            }
        }





        private Point CanvasStart;
        private Point CanvasOrigin;
        //private bool IsCanvasCaptured = false;
        private void OutScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (this.IsMouseCaptured == true)
                return;

            if (Keyboard.IsKeyDown(Key.LeftShift) != true)
                return;

            if (e.Delta > 0)
            {
                this.Zoom -= ((this.ZoomMax - this.ZoomMin) / 100);
                if (this.Zoom <= this.ZoomMin)
                    this.Zoom = this.ZoomMin;
            }
            else
            {
                this.Zoom += ((this.ZoomMax - this.ZoomMin) / 100);
                if (this.Zoom >= this.ZoomMax)
                    this.Zoom = this.ZoomMax;

                
            }
            System.Diagnostics.Debug.WriteLine("zoom value = " + this.Zoom);
            OutScrollViewer.ScrollToVerticalOffset(OutScrollViewer.ScrollableHeight / 2);
            OutScrollViewer.ScrollToHorizontalOffset(OutScrollViewer.ScrollableWidth / 2);
        }

        private void ChildCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            Point cursorPosition = e.GetPosition(this);
            Canvas canvas = sender as Canvas;

            if (canvas != null)
            {
                if (canvas.IsMouseCaptured == true && canvas != null && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    Vector v = this.CanvasStart - cursorPosition;
                    this.TranslationX = CanvasOrigin.X - v.X;
                    this.TranslationY = CanvasOrigin.Y - v.Y;
                    return;
                }

            }
        }

        private void ChildCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pressedPoint = e.GetPosition(sender as IInputElement);

            HitTestResult hitTestResult = VisualTreeHelper.HitTest(sender as Visual, e.GetPosition(sender as IInputElement));
            if (hitTestResult == null) return;
            var element = hitTestResult.VisualHit;


            if (Keyboard.IsKeyDown(Key.LeftShift) == true && element.GetType() == typeof(Canvas))
            {
                this.CanvasStart = e.GetPosition(this);
                this.CanvasOrigin = new Point(this.TranslationX, this.TranslationY);

                var draggableControl = sender as Canvas;
                draggableControl.CaptureMouse();
                //IsCanvasCaptured = true;
                return;
            }

        }

        private void ChildCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.ReleaseMouseCapture();
            this.SelectedItem = null;
            //this.IsCanvasCaptured = false;
        }

    }
}
