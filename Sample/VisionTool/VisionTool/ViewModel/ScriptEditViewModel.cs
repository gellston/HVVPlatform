using System;
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
        private readonly ProcessManagerService processManagerService;


        public ScriptEditViewModel(ScriptControlService _scriptControlService,
                                   ProcessManagerService _processManagerService)
        {


            this.scriptControlService = _scriptControlService;
            this.processManagerService = _processManagerService;

            this.LogCollection = this.scriptControlService.ScriptLogCollection;
            this.GlobalCollection = this.scriptControlService.GlobalCollection;
            this.NativeModuleCollection = this.scriptControlService.NativeModuleCollection;
            this.ScriptCollection = this.scriptControlService.ScriptCollection;
            this.ImageFileCollection = this.scriptControlService.ImageFileCollection;


            this.DeviceObservableCollection = this.processManagerService.DeviceCollection;
            

            this.scriptControlService.SetCallbackRunning(data => this.IsRunningScript = data );
            this.scriptControlService.SetCallbackCurrentExecutionTime(data => this.CurrentExecutionTime = data);
            this.scriptControlService.SetCallbackCurrentFPS(data => this.CurrentFPS = data);



            this.ResultObjectCollection = this.scriptControlService.ResultObjectCollection;

            this.ImageFileCollection = this.scriptControlService.ImageFileCollection;


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



        private WriteableBitmap _CurrentCameraImage = null;
        public WriteableBitmap CurrentCameraImage
        {
            set => Set<WriteableBitmap>(nameof(CurrentCameraImage), ref _CurrentCameraImage, value);
            get => _CurrentCameraImage;
        }

        private WriteableBitmap _CurrentFileImage = null;
        public WriteableBitmap CurrentFileImage
        {
            set => Set<WriteableBitmap>(nameof(CurrentFileImage), ref _CurrentFileImage, value);
            get => _CurrentFileImage;
        }

        //private WriteableBitmap _DetailImagePresenter = null;
        //public WriteableBitmap DetailImagePresenter
        //{
        //    set => Set<WriteableBitmap>(nameof(DetailImagePresenter), ref _DetailImagePresenter, value);
        //    get => _DetailImagePresenter;
        //}


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
                try
                {
                    this.scriptControlService.LoadImageFileFromFolder();
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand StartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    if (this.SelectedScript == null) return;
                    if (this.SelectedImageFile == null && this.IsImageLoadFromDisk == true) return;
                    if (this.IsImageLoadFromCamera == true)
                        this.scriptControlService.RunScript(this.SelectedScript.ScriptContent, this.SelectedDevice, this.IsImageLoadGray);
                    else if (this.IsImageLoadFromDisk == true)
                        this.scriptControlService.RunScript(this.SelectedScript.ScriptContent, this.SelectedImageFile.FilePath, this.IsImageLoadGray);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                
            });
        }

        public ICommand ContinusStartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    if (this.SelectedScript == null) return;
                    if (this.SelectedImageFile == null && this.IsImageLoadFromDisk == true) return;
                    if (this.IsImageLoadFromCamera == true)
                        this.scriptControlService.ContinuousRunScript(this.SelectedScript.ScriptContent, this.SelectedDevice, this.IsImageLoadGray);
                    else if (this.IsImageLoadFromDisk == true)
                        this.scriptControlService.ContinuousRunScript(this.SelectedScript.ScriptContent, this.SelectedImageFile.FilePath, this.IsImageLoadGray);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                
            });
        }

        public ICommand StopScriptCommand
        {
            get => new RelayCommand(() =>
            {
                this.scriptControlService.StopScriptRunning();
            });
        }

        //public ICommand TrackingImageCommand
        //{
        //    get => new RelayCommand(() =>
        //    {
        //        //if (this.SelectedGlobal == null) return;
        //        //if (!this.SelectedGlobal.Type.Contains("image")) return;

        //        //this._trackingType = this.SelectedGlobal.Type;
        //        //this._trackingName = this.SelectedGlobal.Name;
        //        //this._isTracking = true;

        //        //var hvImage = new HV.V1.Image(this.SelectedGlobal);
        //        //if (TrackingImagePresenter == null || TrackingImagePresenter.Width != hvImage.Width || TrackingImagePresenter.Height != hvImage.Height)
        //        //{
        //        //    TrackingImagePresenter = new WriteableBitmap(hvImage.Width, hvImage.Height, 96, 96, PixelFormats.Gray8, null);
        //        //}
        //        //TrackingImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, hvImage.Width, hvImage.Height), hvImage.Ptr(), hvImage.Size, hvImage.Stride);
        //    });
        //}

        //public ICommand ReleaseTrackingImageCommand
        //{
        //    get => new RelayCommand(() =>
        //    {
                
        //    });
        //}

        public ICommand DetailResultShowCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedGlobal == null) return;
                try
                {
                    this.scriptControlService.AddResultObject(this.SelectedGlobal.StackName);
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
        }

        private ObservableCollection<ResultObject> _ResultObjectCollection = null;
        public ObservableCollection<ResultObject> ResultObjectCollection
        {
            get => _ResultObjectCollection;
            set => Set(ref _ResultObjectCollection, value);
        }

        private Model.ResultObject _SelectedResultObject = null;
        public Model.ResultObject SelectedResultObject
        {
            get => _SelectedResultObject;
            set => Set(ref _SelectedResultObject, value);
        }



        private ObservableCollection<Script> _ScriptCollection = null;
        public ObservableCollection<Script> ScriptCollection
        {
            get => _ScriptCollection;
            set => Set(ref _ScriptCollection, value);
        }

        private ObservableCollection<Device.Device> _DeviceObservableCollection = null;
        public ObservableCollection<Device.Device> DeviceObservableCollection
        {
            get => _DeviceObservableCollection;
            set => Set(ref _DeviceObservableCollection, value);
        }

        private ObservableCollection<Model.ImageFile> _ImageFileCollection = null;
        public ObservableCollection<Model.ImageFile> ImageFileCollection
        {
            get => _ImageFileCollection;
            set => Set(ref _ImageFileCollection, value);
        }


        private Device.Device _SelectedDevice = null;
        public Device.Device SelectedDevice
        {
            get => _SelectedDevice;
            set{
                if (value != null)
                {
                    if(value.GetType() == typeof(Device.GigECamera))
                    {
                        Device.GigECamera camera = value as Device.GigECamera;

                        this.CurrentCameraImage = camera.ImageBuffer;
                    }
                }
                Set(ref _SelectedDevice, value);
            }
        }

        private Model.ImageFile _SelectedImageFile = null;
        public Model.ImageFile SelectedImageFile
        {
            get => _SelectedImageFile;
            set
            {
                Set(ref _SelectedImageFile, value);
                if (_SelectedImageFile == null) return;
                try
                {
                    WriteableBitmap writeableBmp = Helper.ImageHelper.LoadImageFromPath(_SelectedImageFile.FilePath);
                    this.CurrentFileImage = writeableBmp;
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }


        public ObservableCollection<HV.V1.Object> GlobalCollection { get; set; }


        //private ObservableCollection<HV.V1.Object> _DetailImageDrawCollection = null;
        //public ObservableCollection<HV.V1.Object> DetailImageDrawCollection
        //{
        //    get
        //    {
        //        if (_DetailImageDrawCollection == null)
        //        {
        //            _DetailImageDrawCollection = new ObservableCollection<HV.V1.Object>();
        //        }
        //        return _DetailImageDrawCollection;
        //    }
        //    set => Set<ObservableCollection<HV.V1.Object>>(nameof(DetailImageDrawCollection), ref _DetailImageDrawCollection, value);
        //}

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


        //private bool _IsShowingResult = false;
        //public bool IsShowingResult
        //{
        //    get => _IsShowingResult;
        //    set => Set<bool>(nameof(IsShowingResult), ref _IsShowingResult, value);
        //}

        private bool _IsImageLoadFromDisk = true;
        public bool IsImageLoadFromDisk
        {
            get => _IsImageLoadFromDisk;
            set {
                if(value == true)
                {
                    _IsImageLoadFromCamera = false;
                    RaisePropertyChanged(nameof(IsImageLoadFromCamera));
                }
                Set(ref _IsImageLoadFromDisk, value);
            }
        }


        private bool _IsImageLoadFromCamera = false;
        public bool IsImageLoadFromCamera
        {
            get => _IsImageLoadFromCamera;
            set
            {
                if (value == true)
                {
                    _IsImageLoadFromDisk = false;
                    RaisePropertyChanged(nameof(IsImageLoadFromDisk));
                }
                Set(ref _IsImageLoadFromCamera, value);
            }
        }

        private bool _IsImageLoadGray = true;
        public bool IsImageLoadGray
        {
            get => _IsImageLoadGray;
            set => Set(ref _IsImageLoadGray, value);
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
