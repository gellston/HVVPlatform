using GalaSoft.MvvmLight.Command;
using Model;
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
    /// GridResultViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ListResultViewer : UserControl
    {
        public ListResultViewer()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ResultObjectCollectionProperty = DependencyProperty.Register("ResultObjectCollection", typeof(ObservableCollection<ResultObject>), typeof(ListResultViewer));
        public ObservableCollection<ResultObject> ResultObjectCollection
        {
            get
            {
                return (ObservableCollection<ResultObject>)GetValue(ResultObjectCollectionProperty);
            }

            set
            {
                SetValue(ResultObjectCollectionProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedResultObjectProperty = DependencyProperty.Register("SelectedResultObject", typeof(ResultObject), typeof(ListResultViewer));
        public ResultObject SelectedResultObject
        {
            get
            {
                return (ResultObject)GetValue(SelectedResultObjectProperty);
            }

            set
            {
                SetValue(SelectedResultObjectProperty, value);
            }
        }

        public ICommand DeleteScriptItemCommand
        {
            get => new RelayCommand<ResultObject>((data) =>
            {
                this.ResultObjectCollection.Remove(data);
            });
        }


    }
}
