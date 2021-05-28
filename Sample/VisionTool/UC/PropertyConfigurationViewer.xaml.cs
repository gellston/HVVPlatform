using GalaSoft.MvvmLight.Command;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// PropertyConfigurationViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PropertyConfigurationViewer : UserControl, INotifyPropertyChanged
    {
        public PropertyConfigurationViewer()
        {
            InitializeComponent();
        }

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

        public static readonly DependencyProperty InputSnapSpotCollectionProperty = DependencyProperty.Register("InputSnapSpotCollection", typeof(ObservableCollection<InputSnapSpot>), typeof(PropertyConfigurationViewer));
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

        public static readonly DependencyProperty SelectedInputSnapSpotProperty = DependencyProperty.Register("SelectedInputSnapSpot", typeof(InputSnapSpot), typeof(PropertyConfigurationViewer));
        public InputSnapSpot SelectedInputSnapSpot
        {
            get
            {
                return (InputSnapSpot)GetValue(SelectedInputSnapSpotProperty);
            }

            set
            {
                SetValue(SelectedInputSnapSpotProperty, value);
            }
        }



        public ICommand ShowROISettingWindowCommand
        {
            get => new RelayCommand(() =>
            {
                IsOpenROIConfigurationView = true;
            });
        }


        public static readonly DependencyProperty IsOpenROIConfigurationViewProperty = DependencyProperty.Register("IsOpenROIConfigurationView", typeof(bool), typeof(PropertyConfigurationViewer));
        public bool IsOpenROIConfigurationView
        {
            get
            {
                return (bool)GetValue(IsOpenROIConfigurationViewProperty);
            }

            set
            {
                SetValue(IsOpenROIConfigurationViewProperty, value);
            }
        }


 


    }
}
