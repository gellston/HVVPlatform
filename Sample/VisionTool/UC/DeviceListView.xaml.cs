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
    /// DeviceListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeviceListView : UserControl
    {
        public DeviceListView()
        {
            InitializeComponent();
        }




        public static readonly DependencyProperty DeviceObservableCollectionProperty = DependencyProperty.Register("DeviceObservableCollection", typeof(ObservableCollection<Device.Device>), typeof(DeviceListView));
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

        public static readonly DependencyProperty SelectedDeviceProperty = DependencyProperty.Register("SelectedDevice", typeof(Device.Device), typeof(DeviceListView));
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

        public static readonly DependencyProperty ItemsWidthProperty = DependencyProperty.Register("ItemsWidth", typeof(double), typeof(DeviceListView));
        public double ItemsWidth
        {
            get
            {
                return (double)GetValue(ItemsWidthProperty);
            }

            set
            {
                SetValue(DeviceObservableCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty ItemsHeightProperty = DependencyProperty.Register("ItemsHeight", typeof(double), typeof(DeviceListView));
        public double ItemsHeight
        {
            get
            {
                return (double)GetValue(ItemsHeightProperty);
            }

            set
            {
                SetValue(ItemsHeightProperty, value);
            }
        }
    }
}
