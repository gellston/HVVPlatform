
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Model;
using VisionTool.Message;
using VisionTool.Service;


namespace VisionTool.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ScriptControlService scriptContorlService;
        public MainWindowViewModel(ScriptControlService _scriptContorlService,
                                   ScriptEditViewModel _scriptEditViewModel)
        {
            this.scriptContorlService = _scriptContorlService;



            this.MessengerInstance.Register<AssociationModeMessage>(this, FileAssocationCallback);
        }

        private void FileAssocationCallback(AssociationModeMessage message)
        {
            switch (message.AssociationMode)
            {
                case "Script":
                    this.CurrentContentViewModel = message.CurrentViewModel;
                    break;

                case "Sequence":
                    this.CurrentContentViewModel = message.CurrentViewModel;
                    break;
            }
        }


        private ViewModelBase _CurrentContentViewModel = null;
        public ViewModelBase CurrentContentViewModel
        {
            get => _CurrentContentViewModel;
            set => Set<ViewModelBase>(nameof(CurrentContentViewModel), ref _CurrentContentViewModel, value);

        }



        public ICommand SelectCurrentMenuCommand
        {
            get => new RelayCommand<ViewModelBase>((menu) =>
            {
                this.CurrentContentViewModel = menu;
            });
        }

    }
}
