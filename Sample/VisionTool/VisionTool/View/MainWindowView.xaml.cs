using ActiproSoftware.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisionTool.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();

            ThemeManager.CurrentTheme = ThemeNames.Office2010Black;
        }

        private void ThemedWindow_Closed(object sender, EventArgs e)
        {
            //base.OnClosed(e);
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }


}
