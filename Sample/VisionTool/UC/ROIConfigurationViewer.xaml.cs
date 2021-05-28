using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// ROIConfigurationViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ROIConfigurationViewer : UserControl
    {
        public ROIConfigurationViewer()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty GlobalObjectCollectionProperty = DependencyProperty.Register("GlobalObjectCollection", typeof(ObservableCollection<HV.V1.Object>), typeof(ROIConfigurationViewer));
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

        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(HV.V1.Object), typeof(ROIConfigurationViewer));
        public HV.V1.Object SelectedImage
        {
            get
            {
                return (HV.V1.Object)GetValue(SelectedImageProperty);
            }

            set
            {
                SetValue(SelectedImageProperty, value);
            }
        }

        public static readonly DependencyProperty ROIProperty = DependencyProperty.Register("ROI", typeof(Model.DiagramProperty.BaseDiagramProperty), typeof(ROIConfigurationViewer));
        public Model.DiagramProperty.BaseDiagramProperty ROI
        {
            get
            {
                return (Model.DiagramProperty.BaseDiagramProperty)GetValue(ROIProperty);
            }

            set
            {
                SetValue(ROIProperty, value);
            }
        }
    }
}
