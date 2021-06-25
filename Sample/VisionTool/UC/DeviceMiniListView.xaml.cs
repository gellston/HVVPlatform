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
    /// DeviceMiniListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeviceMiniListView : UserControl
    {
        public DeviceMiniListView()
        {
            InitializeComponent();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        ~DeviceMiniListView()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;

        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        public static readonly DependencyProperty DeviceObservableCollectionProperty = DependencyProperty.Register("DeviceObservableCollection", typeof(ObservableCollection<Device.Device>), typeof(DeviceMiniListView));
        public ObservableCollection<Device.Device> DeviceObservableCollection
        {
            get
            {
                return (ObservableCollection<Device.Device>)GetValue(DeviceObservableCollectionProperty);
            }

            set
            {
                SetValue(DeviceObservableCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedDeviceProperty = DependencyProperty.Register("SelectedDevice", typeof(Device.Device), typeof(DeviceMiniListView));
        public Device.Device SelectedDevice
        {
            get
            {
                return (Device.Device)GetValue(SelectedDeviceProperty);
            }

            set
            {
                SetValue(SelectedDeviceProperty, value);
            }
        }

    }
}
