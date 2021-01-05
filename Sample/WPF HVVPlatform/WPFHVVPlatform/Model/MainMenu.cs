using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

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

        public ICommand MenuAction
        {
            get; set;
        }

    }


}
