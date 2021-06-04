using Model.DiagramProperty;
using System;
using System.Collections.Generic;
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
    /// PropertySettingViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PropertySettingViewer : UserControl
    {
        public PropertySettingViewer()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty SelectedDiagramPropertyDependency = DependencyProperty.Register("SelectedDiagramProperty", typeof(BaseDiagramProperty), typeof(PropertySettingViewer));
        public BaseDiagramProperty SelectedDiagramProperty
        {
            get
            {
                return (BaseDiagramProperty)GetValue(SelectedDiagramPropertyDependency);
            }

            set
            {
                SetValue(SelectedDiagramPropertyDependency, value);
            }
        }
    }
}
