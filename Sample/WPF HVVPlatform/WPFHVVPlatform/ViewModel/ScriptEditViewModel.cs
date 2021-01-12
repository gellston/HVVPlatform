using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WPFHVVPlatform.Model;
using WPFHVVPlatform.Service;

namespace WPFHVVPlatform.ViewModel
{
    public class ScriptEditViewModel : ViewModelBase
    {

        private readonly FileDialogService fileDialogService;
        private readonly MessageDialogService messageDialogService;
        private readonly ScriptFileService scriptFileService;


       
        private readonly HV.V1.Interpreter interpreter;
       

        public ScriptEditViewModel(FileDialogService _fileDialogService,
                                   MessageDialogService _messageDialogService,
                                   ScriptFileService _scriptFileService,
                                   HV.V1.Interpreter _interpreter)
        {

            this.fileDialogService = _fileDialogService;
            this.messageDialogService = _messageDialogService;
            this.scriptFileService = _scriptFileService;
            this.interpreter = _interpreter;


            this.ScriptCollection.Add(new Script()
            {
                FileName = "new.js",
                ScriptContent = "/* Be the god of coding */",
                FilePath = ""
            });


            this.SelectedScript = ScriptCollection[0];


            this.interpreter.TraceEvent += Trace;
        }

        ~ScriptEditViewModel()
        {
            this.interpreter.TraceEvent -= Trace;
        }

        private void Trace(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.LogCollection.Add(new Log()
                {
                    Type = "스크립트",
                    Content = text
                });
            });
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
                if (this.SelectedScript == null)
                    return;

                if(this.SelectedScript.FilePath.Length == 0)
                {
                    var filePath = this.fileDialogService.SaveFile("Script File (.js)|*.js");
                    if (filePath.Length == 0) return;
                    this.SelectedScript.FilePath = filePath;
                }

                this.scriptFileService.SaveScriptFile(this.SelectedScript.FilePath, this.SelectedScript.ScriptContent);
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
            get => new RelayCommand(async () =>
            {
                if (this.IsRunningScript == true) return;
                if (this.SelectedScript == null) return;
                await Task.Run(() =>
                {
                    this.IsRunningScript = true;
                    try
                    {
                        this.interpreter.RunScript(this.SelectedScript.ScriptContent);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.LogCollection.Add(new Log()
                            {
                                Type = "Error",
                                Content = e.Message
                            });
                        });
                    }
                    this.IsRunningScript = false;
                });
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
                //this.interpreter.Terminate();
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

        private bool _IsRunningScript = false;
        public bool IsRunningScript
        {
            get => _IsRunningScript;
            set => Set<bool>(nameof(IsRunningScript), ref _IsRunningScript, value);
        }


        private ObservableCollection<Log> _LogCollection = null;
        public ObservableCollection<Log> LogCollection
        {
            get
            {
                if (_LogCollection == null)
                {
                    _LogCollection = new ObservableCollection<Log>();
                }
                return _LogCollection;
            }
        }

    }
}
