﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using VisionTool.Message;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class DiagramEditViewModel : ViewModelBase
    {

        private DiagramControlService diagramControlService;
        private SequenceControlService sequenceControlService;
        private ScriptControlService scriptControlService;
        private ProcessManagerService processManagerService;
       

        public DiagramEditViewModel(DiagramControlService _diagramControlService,
                                    SequenceControlService _sequenceControlService,
                                    ScriptControlService _scriptControlService,
                                    ProcessManagerService _processManagerService)
        {

            this.diagramControlService = _diagramControlService;
            this.sequenceControlService = _sequenceControlService;
            this.processManagerService = _processManagerService;
            this.scriptControlService = _scriptControlService;
            


            this.scriptControlService.SetCallbackRunning(data => this.IsRunningScript = data);
            this.scriptControlService.SetCallbackCurrentFPS(data => this.CurrentFPS = data);
            this.scriptControlService.SetCallbackCurrentExecutionTime(data => this.CurrentExecutionTime = data);
            this.scriptControlService.SetCallbackCurrentErrorLine(data => this.CurrentErrorLine = data);


            this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;


            this.LogCollection = this.scriptControlService.ScriptLogCollection;
            this.GlobalCollection = this.scriptControlService.GlobalCollection;
            this.GlobalNames = this.scriptControlService.GlobalNames;
            this.NativeModuleCollection = this.scriptControlService.NativeModuleCollection;
            this.ResultObjectCollection = this.scriptControlService.ResultObjectCollection;
            this.ImageFileCollection = this.scriptControlService.ImageFileCollection;
            this.DeviceObservableCollection = this.processManagerService.DeviceCollection;


            this.FunctionCollection = this.sequenceControlService.FunctionCollection;
            this.InputSnapSpotCollection = this.sequenceControlService.InputSnapSpotCollection;
            this.OutputSnapSpotCollection = this.sequenceControlService.OutputSnapSpotCollection;
            this.ConnectorCollection = this.sequenceControlService.ConnectorCollection;


            


            this.MessengerInstance.Register<AssociationModeMessage>(this, FileAssociationCallback);
        }

        private void FileAssociationCallback(AssociationModeMessage message)
        {
            if (message.AssociationMode == "Sequence")
            {
                this.sequenceControlService.LoadSequenceFromPath(message.FilePath);
                this.sequenceControlService.FunctionVersionCheck(this.diagramControlService.DiagramConfigCollection);
            }
        }




        private ObservableCollection<Function> _FunctionCollection = null;
        public ObservableCollection<Function> FunctionCollection
        {
            get => _FunctionCollection;
            set => Set(ref _FunctionCollection, value);
        }

        private ObservableCollection<Connector> _ConnectorCollection = null;
        public ObservableCollection<Connector> ConnectorCollection
        {
            get => _ConnectorCollection;
            set => Set(ref _ConnectorCollection, value);
        }

        private ObservableCollection<InputSnapSpot> _InputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> InputSnapSpotCollection
        {
            get => _InputSnapSpotCollection;
            set => Set(ref _InputSnapSpotCollection, value);
        }

        private ObservableCollection<OutputSnapSpot> _OutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> OutputSnapSpotCollection
        {
            get => _OutputSnapSpotCollection;
            set => Set(ref _OutputSnapSpotCollection, value);
        }

        private ObservableCollection<DiagramConfig> _DiagramConfigCollection = null;
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get => _DiagramConfigCollection;
            set => Set(ref _DiagramConfigCollection, value);
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



        public ICommand NewDiagramSequenceCommand
        {
            get => new RelayCommand(() =>
            {
                if (DialogHelper.ShowConfirmMessage("다이어그램 시퀀스를 초기화 하시겠습니까?") == false) return;
                this.sequenceControlService.ClearSequence();
            });
        }


        public ICommand OpenDiagramSequenceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.sequenceControlService.LoadSequenceFromPath();
                    this.sequenceControlService.FunctionVersionCheck(this.diagramControlService.DiagramConfigCollection);
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand SaveDiagramSequenceCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.sequenceControlService.SaveSequence();
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        public ICommand DeleteDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                var diagram = this.SelectedDiagramObject;

                this.sequenceControlService.DeleteDiagram(diagram);

                //command.Execute(null);
                
            });
        }





        public ICommand StartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    if (this.SelectedImageFile == null && this.IsImageLoadFromDisk == true) return;
                    this.sequenceControlService.ScriptGeneration();
                    this.FullScript = this.sequenceControlService.FullScriptContent;
                    if (this.IsImageLoadFromCamera == true)
                        this.scriptControlService.RunScript(this.FullScript, this.SelectedDevice, this.IsImageLoadGray);
                    else if (this.IsImageLoadFromDisk == true)
                        this.scriptControlService.RunScript(this.FullScript, this.SelectedImageFile.FilePath, this.IsImageLoadGray);

                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                   
                    DialogHelper.ShowToastErrorMessage("다이어그램 실행 에러", e.Message);
                }
                

            });
        }

        private string _CompileScriptCode = "";
        public string CompileScriptCode
        {
            get => _CompileScriptCode;
            set => Set(ref _CompileScriptCode, value);
        }

        public ICommand CompileDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {

                    this.sequenceControlService.ScriptGeneration();
                    this.CompileScriptCode = this.sequenceControlService.FullScriptContent;

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    DialogHelper.ShowToastErrorMessage("컴파일 에러", e.Message);
                }
            });
        }

        //public ICommand StepCompileDiagramCommand
        //{
        //    get => new RelayCommand(() =>
        //    {
        //        try
        //        {

        //            this.sequenceControlService.StepScriptGeneration(this.SelectedFunction);
        //            this.FullScript = this.sequenceControlService.FullScriptContent;

        //        }
        //        catch (Exception e)
        //        {
        //            System.Diagnostics.Debug.WriteLine(e.Message);
        //            DialogHelper.ShowToastErrorMessage("컴파일 에러", e.Message);
        //        }
        //    });
        //}


        public ICommand ContinusStartRunScriptCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    if (this.SelectedImageFile == null && this.IsImageLoadFromDisk == true) return;

                    this.sequenceControlService.ScriptGeneration();
                    this.FullScript = this.sequenceControlService.FullScriptContent;
                    if(this.IsImageLoadFromCamera == true)
                        this.scriptControlService.ContinuousRunScript(this.FullScript, this.SelectedDevice, this.IsImageLoadGray);
                    else if(this.IsImageLoadFromDisk == true)
                        this.scriptControlService.ContinuousRunScript(this.FullScript, this.SelectedImageFile.FilePath, this.IsImageLoadGray);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    DialogHelper.ShowToastErrorMessage("다이어그램 실행 에러", e.Message);
                }
                
            });
        }


        public ICommand StepRunScriptCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    if (this.SelectedImageFile == null && this.IsImageLoadFromDisk == true) return;

                    this.FullScript = this.sequenceControlService.StepScriptGeneration(this.SelectedFunction);
                    if (this.IsImageLoadFromCamera == true)
                        this.scriptControlService.RunScript(this.FullScript, this.SelectedDevice, this.IsImageLoadGray);
                    else if (this.IsImageLoadFromDisk == true)
                        this.scriptControlService.RunScript(this.FullScript, this.SelectedImageFile.FilePath, this.IsImageLoadGray);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    DialogHelper.ShowToastErrorMessage("다이어그램 실행 에러", e.Message);
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

        public ICommand DropOutputSnapSpotCommand
        {
            get => new RelayCommand<DragEventArgs>((sender) =>
            {
                string data = sender.Data.GetData(typeof(string)) as string;
                if (data == null) return;
                this.scriptControlService.AddResultObject(data);
            });
        }

        public ICommand OpenImageCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.scriptControlService.LoadImageFileFromFolder();
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        //private bool _IsOpenROIConfigurationView = false;
        //public bool IsOpenROIConfigurationView
        //{
        //    get => _IsOpenROIConfigurationView;
        //    set => Set(ref _IsOpenROIConfigurationView, value);
        //}


        private bool _IsRunningScript = false;
        public bool IsRunningScript
        {
            get => _IsRunningScript;
            set => Set(ref _IsRunningScript, value);
        }

        private string _CurrentExecutionTime = "";
        public string CurrentExecutionTime
        {
            get => _CurrentExecutionTime;
            set => Set(ref _CurrentExecutionTime, value);
        }

        private string _CurrentFPS = "";
        public string CurrentFPS
        {
            get => _CurrentFPS;
            set => Set(ref _CurrentFPS, value);
        }


        private int _CurrentErrorLine = 0;
        public int CurrentErrorLine
        {
            get => _CurrentErrorLine;
            set {

                this.sequenceControlService.SetErrorFlag(value);


                Set(ref _CurrentErrorLine, value);
            }
        }



        private double _CanvasWidth = 0;
        public double CanvasWidth
        {
            get => _CanvasWidth;
            set => Set(ref _CanvasWidth, value);
        }

        private double _CanvasHeight = 0;
        public double CanvasHeight
        {
            get => _CanvasHeight;
            set => Set(ref _CanvasHeight, value);
        }


        private string _FullScript = "";
        public string FullScript
        {
            get => _FullScript;
            set => Set(ref _FullScript, value);
        }

        private Function _SelectedFunction = null;
        public Function SelectedFunction
        {
            get => _SelectedFunction;
            set => Set(ref _SelectedFunction, value);
        }

        private DiagramObject _SelectedDiagramObject = null;
        public DiagramObject SelectedDiagramObject
        {
            get => _SelectedDiagramObject;
            set => Set(ref _SelectedDiagramObject, value);
        }

        private ObservableCollection<Log> _LogCollection = null;
        public ObservableCollection<Log> LogCollection
        {
            get => _LogCollection;
            set => Set(ref _LogCollection, value);
        }


        private ObservableCollection<HV.V1.Object> _GlobalCollection = null;
        public ObservableCollection<HV.V1.Object> GlobalCollection
        {
            get => _GlobalCollection;
            set => Set(ref _GlobalCollection, value);
        }


        private ObservableCollection<HV.V1.NativeModule> _NativeModuleCollection = null;
        public ObservableCollection<HV.V1.NativeModule> NativeModuleCollection
        {
            get => _NativeModuleCollection;
            set => Set(ref _NativeModuleCollection, value);
        }


        private ObservableCollection<string> _GlobalNames = null;
        public ObservableCollection<string> GlobalNames
        {
            get => _GlobalNames;
            set => Set(ref _GlobalNames, value);
        }



        private bool _IsImageLoadGray = true;
        public bool IsImageLoadGray
        {
            get => _IsImageLoadGray;
            set => Set(ref _IsImageLoadGray, value);
        }





        private bool _IsImageLoadFromDisk = true;
        public bool IsImageLoadFromDisk
        {
            get => _IsImageLoadFromDisk;
            set
            {
                if (value == true)
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


        private ObservableCollection<Device.Device> _DeviceObservableCollection = null;
        public ObservableCollection<Device.Device> DeviceObservableCollection
        {
            get => _DeviceObservableCollection;
            set => Set(ref _DeviceObservableCollection, value);
        }


        private Device.Device _SelectedDevice = null;
        public Device.Device SelectedDevice
        {
            get => _SelectedDevice;
            set
            {
                if (value != null)
                {
                    if (value.GetType() == typeof(Device.GigECamera))
                    {
                        Device.GigECamera camera = value as Device.GigECamera;

                        this.CurrentCameraImage = camera.ImageBuffer;
                    }
                }
                Set(ref _SelectedDevice, value);
            }
        }

        private ObservableCollection<Model.ImageFile> _ImageFileCollection = null;
        public ObservableCollection<Model.ImageFile> ImageFileCollection
        {
            get => _ImageFileCollection;
            set => Set(ref _ImageFileCollection, value);
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
                catch (Exception e)
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
    }
}
