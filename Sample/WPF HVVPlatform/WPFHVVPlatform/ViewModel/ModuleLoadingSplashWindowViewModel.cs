using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace WPFHVVPlatform.ViewModel
{
    public class ModuleLoadingSplashWindowViewModel : ViewModelBase
    {

        public ModuleLoadingSplashWindowViewModel()
        {



        }


        private string _Title = "";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private double _MainProgress = 0;
        public double MainProgress
        {
            get => _MainProgress;
            set => Set(ref _MainProgress, value);
        }

        public string _CurrentFile = "";
        public string CurrentFile
        {
            get => _CurrentFile;
            set => Set(ref _CurrentFile, value);
        }

    }
}
