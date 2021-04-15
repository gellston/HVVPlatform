﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using VisionTool.Message;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class ScriptEditViewModel : ViewModelBase
    {

        private readonly ScriptControlService scriptControlService;



        public ScriptEditViewModel(ScriptControlService _scriptControlService)
        {


            this.scriptControlService = _scriptControlService;
            this.LogCollection = this.scriptControlService.ScriptLogCollection;
            this.GlobalCollection = this.scriptControlService.GlobalCollection;
            this.NativeModuleCollection = this.scriptControlService.NativeModuleCollection;
            this.ScriptCollection = this.scriptControlService.ScriptCollection;
            

            this.scriptControlService.SetCallbackRunning(data => this.IsRunningScript = data );
            this.scriptControlService.SetCallbackCurrentExecutionTime(data => this.CurrentExecutionTime = data);
            this.scriptControlService.SetCallbackCurrentFPS(data => this.CurrentFPS = data);


            MessengerInstance.Register<AssociationModeMessage>(this, FileAssociationCallback);
        }

        ~ScriptEditViewModel()
        {
            
        }

        private void FileAssociationCallback(AssociationModeMessage message)
        {
            if(message.AssociationMode == "Script")
            {

                try
                {
                    this.scriptControlService.LoadScriptFromPath(message.FilePath);

                    this.SelectedScript = this.scriptControlService.ScriptCollection[0];
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }


            }
        }



        private WriteableBitmap _TrackingImagePresenter = null;
        public WriteableBitmap TrackingImagePresenter
        {
            set => Set<WriteableBitmap>(nameof(TrackingImagePresenter), ref _TrackingImagePresenter, value);
            get => _TrackingImagePresenter;
        }

        private WriteableBitmap _DetailImagePresenter = null;
        public WriteableBitmap DetailImagePresenter
        {
            set => Set<WriteableBitmap>(nameof(DetailImagePresenter), ref _DetailImagePresenter, value);
            get => _DetailImagePresenter;
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

                 try
                 {
                     this.scriptControlService.LoadScriptFromPath();
                 }
                 catch(Exception e)
                 {
                     System.Diagnostics.Debug.WriteLine(e.Message);
                 }
             });
            
        }

        public ICommand NewScriptFileCommand
        {
            get => new RelayCommand(() =>
            {
                this.scriptControlService.AddNewScript();
            });
        }


        public ICommand DeleteScriptItemCommand
        {
            get => new RelayCommand<Script>((data) =>
            {
                this.scriptControlService.RemoveScript(data);
            });
        }


        public ICommand SaveScriptFileCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    this.scriptControlService.SaveScript(this.SelectedScript);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                
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
                this.scriptControlService.RunScript(this.SelectedScript.ScriptContent);
            });
        }

        public ICommand ContinusStartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {
                this.scriptControlService.ContinuousRunScript(this.SelectedScript.ScriptContent);
            });
        }

        public ICommand StopScriptCommand
        {
            get => new RelayCommand(() =>
            {
                this.scriptControlService.StopScriptRunning();
            });
        }

        public ICommand TrackingImageCommand
        {
            get => new RelayCommand(() =>
            {
                //if (this.SelectedGlobal == null) return;
                //if (!this.SelectedGlobal.Type.Contains("image")) return;

                //this._trackingType = this.SelectedGlobal.Type;
                //this._trackingName = this.SelectedGlobal.Name;
                //this._isTracking = true;

                //var hvImage = new HV.V1.Image(this.SelectedGlobal);
                //if (TrackingImagePresenter == null || TrackingImagePresenter.Width != hvImage.Width || TrackingImagePresenter.Height != hvImage.Height)
                //{
                //    TrackingImagePresenter = new WriteableBitmap(hvImage.Width, hvImage.Height, 96, 96, PixelFormats.Gray8, null);
                //}
                //TrackingImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, hvImage.Width, hvImage.Height), hvImage.Ptr(), hvImage.Size, hvImage.Stride);
            });
        }

        public ICommand ReleaseTrackingImageCommand
        {
            get => new RelayCommand(() =>
            {
                
            });
        }

        public ICommand DetailImageShowCommand
        {
            get => new RelayCommand(() =>
            {
                //if (this.SelectedGlobal == null) return;
                //if (!this.SelectedGlobal.Type.Contains("image")) return;

                //var hvImage = new HV.V1.Image(this.SelectedGlobal);
                //if (DetailImagePresenter == null || DetailImagePresenter.Width != hvImage.Width || DetailImagePresenter.Height != hvImage.Height)
                //{
                //    DetailImagePresenter = new WriteableBitmap(hvImage.Width, hvImage.Height, 96, 96, PixelFormats.Gray8, null);
                //}
                //DetailImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, hvImage.Width, hvImage.Height), hvImage.Ptr(), hvImage.Size, hvImage.Stride);
                //this.DetailImageDrawCollection = new ObservableCollection<HV.V1.Object>(hvImage.DrawObjects);
            });
        }



        private ObservableCollection<Script> _ScriptCollection = null;
        public ObservableCollection<Script> ScriptCollection
        {
            get => _ScriptCollection;
            set => Set(ref _ScriptCollection, value);
        }


        public ObservableCollection<HV.V1.Object> GlobalCollection { get; set; }


        private ObservableCollection<HV.V1.Object> _DetailImageDrawCollection = null;
        public ObservableCollection<HV.V1.Object> DetailImageDrawCollection
        {
            get
            {
                if (_DetailImageDrawCollection == null)
                {
                    _DetailImageDrawCollection = new ObservableCollection<HV.V1.Object>();
                }
                return _DetailImageDrawCollection;
            }
            set => Set<ObservableCollection<HV.V1.Object>>(nameof(DetailImageDrawCollection), ref _DetailImageDrawCollection, value);
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


        private bool _IsShowingResult = false;
        public bool IsShowingResult
        {
            get => _IsShowingResult;
            set => Set<bool>(nameof(IsShowingResult), ref _IsShowingResult, value);
        }


        private string _CurrentFPS = "";
        public string CurrentFPS
        {
            get => _CurrentFPS;
            set => Set<string>(nameof(CurrentFPS), ref _CurrentFPS, value);
        }

        private string _CurrentExecutionTime = "";
        public string CurrentExecutionTime
        {
            get => _CurrentExecutionTime;
            set => Set<string>(nameof(CurrentExecutionTime), ref _CurrentExecutionTime, value);
        }

        public ObservableCollection<Log> LogCollection { get; set; }

        public ObservableCollection<HV.V1.NativeModule> NativeModuleCollection { get; set; }




       

    }
}
