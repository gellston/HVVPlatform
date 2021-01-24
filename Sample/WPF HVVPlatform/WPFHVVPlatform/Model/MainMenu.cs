using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace WPFHVVPlatform.Model
{
    public class MainMenu
    {
        public MainMenu()
        {

        }
        public ImageSource Icon
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public ViewModelBase ViewModel
        {
            get;set;
        }

    }


}
