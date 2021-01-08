using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WPFHVVPlatform.Model;


namespace WPFHVVPlatform.ViewModel
{
    public class ScriptEditViewModel : ViewModelBase
    {
        public ScriptEditViewModel()
        {
            //this.ScriptCollection.Add(new Script()
            //{
            //    FileName = "test",
            //    ScriptContent = "test"
            //});

        }


        private Script _SelectedScript = null;
        public Script SelectedScript
        {
            get => _SelectedScript;
            set => Set<Script>(nameof(SelectedScript), ref _SelectedScript, value);
        }


        public ICommand OpenScriptFileCommand
        {
            get => new RelayCommand(() =>
             {

             });
            
        }

        public ICommand NewScriptFileCommand
        {
            get => new RelayCommand(() =>
            {
                this.ScriptCollection.Add(new Script()
                {
                    FileName = "new.js",
                    ScriptContent = "/* Be the god of coding */",
                    FilePath = ""
                });

                //System.Console.WriteLine("test");
            });
        }

        public ICommand SaveScriptFileCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand OpenImageCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand StartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand ContinusStartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand StopScriptCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        private ObservableCollection<Script> _ScriptCollection = null;
        public ObservableCollection<Script> ScriptCollection
        {
            get
            {
                if(_ScriptCollection == null)
                {
                    _ScriptCollection = new ObservableCollection<Script>();
                }
                return _ScriptCollection;
            }
        }
    }
}
