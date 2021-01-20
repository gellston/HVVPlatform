using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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



        private string _trackingName;
        private string _trackingType;



        public ScriptEditViewModel(FileDialogService _fileDialogService,
                                   MessageDialogService _messageDialogService,
                                   ScriptFileService _scriptFileService,
                                   HV.V1.Interpreter _interpreter)
        {

            this.fileDialogService = _fileDialogService;
            this.messageDialogService = _messageDialogService;
            this.scriptFileService = _scriptFileService;
            this.interpreter = _interpreter;


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
                this.LogCollection.Insert(0,new Log()
                {
                    Type = "스크립트",
                    Content = text
                });
            });
            Thread.Sleep(1);
        }

        private WriteableBitmap _ImagePresenter = null;
        public WriteableBitmap ImagePresenter
        {
            set => Set<WriteableBitmap>(nameof(ImagePresenter), ref _ImagePresenter, value);
            get => _ImagePresenter;
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
                 var filePath = this.fileDialogService.OpenFile("Script File (.js)|*.js");
                 if (filePath.Length == 0) return;
                 var content = this.scriptFileService.LoadScriptFile(filePath);
                 var script = new Script()
                 {
                     FilePath = filePath,
                     FileName = Path.GetFileName(filePath),
                     ScriptContent = content
                 };

                 this.ScriptCollection.Add(script);
                 this.SelectedScript = script;

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
                    this.SelectedScript.FileName = Path.GetFileName(filePath);
                    RaisePropertyChanged(nameof(SelectedScript));
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



                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.GlobalCollection.Clear();
                            this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values.ToList());

                            try
                            {
                                var image = this.interpreter.GlobalObjects.Values.ToList().Where((_object) =>
                                {
                                    return _object.Name.Contains(this._trackingName) && _object.Type.Contains(this._trackingType);
                                }).First();

                                var hvImage = new HV.V1.Image(image);
                                var width = hvImage.Width;
                                var height = hvImage.Height;
                                var stride = hvImage.Stride;
                                var size = hvImage.Size;
                                if (ImagePresenter == null || ImagePresenter.Width != width || ImagePresenter.Height != height)
                                {
                                    ImagePresenter = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
                                }
                                ImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), hvImage.Ptr(), size, stride);
                                this.TrackingImageDrawCollection.Clear();
                                this.TrackingImageDrawCollection.AddRange(hvImage.DrawObjects);
                            }
                            catch (Exception e)
                            {

                            }
                        }, DispatcherPriority.ContextIdle);
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
                if (this.IsRunningScript == true) return;
                if (this.SelectedScript == null) return;

                this.IsRunningScript = true;

                Task.Run(async () =>
                {
                    var count = 0;
                    var stacked_time = 0.0;

                    while (IsRunningScript)
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        this.interpreter.RunScript(this.SelectedScript.ScriptContent);
                        try
                        {
                            this.interpreter.RunScript(this.SelectedScript.ScriptContent);

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.GlobalCollection.Clear();
                                this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values.ToList());

                                try
                                {
                                    var image = this.interpreter.GlobalObjects.Values.ToList().Where((_object) =>
                                    {
                                        return _object.Name.Contains(this._trackingName) && _object.Type.Contains(this._trackingType);
                                    }).First();

                                    var hvImage = new HV.V1.Image(image);
                                    var width = hvImage.Width;
                                    var height = hvImage.Height;
                                    var stride = hvImage.Stride;
                                    var size = hvImage.Size;
                                    if (ImagePresenter == null || ImagePresenter.Width != width || ImagePresenter.Height != height)
                                    {
                                        ImagePresenter = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
                                    }
                                    ImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), hvImage.Ptr(), size, stride);
                                    this.TrackingImageDrawCollection.Clear();
                                    this.TrackingImageDrawCollection.AddRange(hvImage.DrawObjects);

                                }
                                catch (Exception e)
                                {

                                }
                            },DispatcherPriority.Render);
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
                            break;
                        }
                        //await Task.Delay(5);
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        stacked_time += elapsedMs;
                        count++;
                        if(stacked_time > 1000)
                        {
                            System.Console.WriteLine("fps : " + count);
                            count = 0;
                            stacked_time = 0;
                        }
                    }

                    this.IsRunningScript = false;
                });

            });
        }

        public ICommand StopScriptCommand
        {
            get => new RelayCommand(() =>
            {
                this.interpreter.Terminate();
                this.IsRunningScript = false;
            });
        }

        public ICommand TrackingImageCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedGlobal == null) return;
                if (!this.SelectedGlobal.Type.Contains("image")) return;

                this._trackingType = this.SelectedGlobal.Type;
                this._trackingName = this.SelectedGlobal.Name;

                var hvImage = new HV.V1.Image(this.SelectedGlobal);
                if (ImagePresenter == null || ImagePresenter.Width != hvImage.Width || ImagePresenter.Height != hvImage.Height)
                {
                    ImagePresenter = new WriteableBitmap(hvImage.Width, hvImage.Height, 96, 96, PixelFormats.Gray8, null);
                }
                ImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, hvImage.Width, hvImage.Height), hvImage.Ptr(), hvImage.Size, hvImage.Stride);
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

        private ObservableCollection<HV.V1.Object> _GlobalCollection = null;
        public ObservableCollection<HV.V1.Object> GlobalCollection
        {
            get
            {
                if (_GlobalCollection == null)
                {
                    _GlobalCollection = new ObservableCollection<HV.V1.Object>();
                }
                return _GlobalCollection;
            }
            set => Set<ObservableCollection<HV.V1.Object>>(nameof(GlobalCollection), ref _GlobalCollection, value);
        }

        private ObservableCollection<HV.V1.Object> _TrackingImageDrawCollection = null;
        public ObservableCollection<HV.V1.Object> TrackingImageDrawCollection
        {
            get
            {
                if (_TrackingImageDrawCollection == null)
                {
                    _TrackingImageDrawCollection = new ObservableCollection<HV.V1.Object>();
                }
                return _TrackingImageDrawCollection;
            }
            set => Set<ObservableCollection<HV.V1.Object>>(nameof(TrackingImageDrawCollection), ref _TrackingImageDrawCollection, value);
        }

        private HV.V1.Object _SelectedGlobal = null;
        public HV.V1.Object SelectedGlobal
        {
            get => _SelectedGlobal;
            set => Set<HV.V1.Object>(nameof(SelectedGlobal), ref _SelectedGlobal, value);
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
