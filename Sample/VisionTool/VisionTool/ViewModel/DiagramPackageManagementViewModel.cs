﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Model;
using VisionTool.Service;
using Model.DiagramProperty;
using VisionTool.Message;

namespace VisionTool.ViewModel
{
    public class DiagramPackageManagementViewModel : ViewModelBase
    {

        private readonly DiagramControlService diagramControlService;
        private readonly SettingConfigService appConfigService;
        

        public DiagramPackageManagementViewModel(DiagramControlService _diagramPackageService,
                                                 SettingConfigService _appConfigService)
        {


            this.diagramControlService = _diagramPackageService;
            this.appConfigService = _appConfigService;



            this.diagramControlService.SetCallbackDiagramEditDate(data => this.DiagramEditDate = data);
            this.diagramControlService.SetCallbackDiagramComment(data => this.DiagramComment = data);
            this.diagramControlService.SetCallbackDiagramImage(data => this.DiagramImage = data);
            this.diagramControlService.SetCallbackDiagramName(data => this.DiagramName = data);
            this.diagramControlService.SetCallbackDiagramVersion(data => this.DiagramVersion = data);
            this.diagramControlService.SetCallbackDiagramColor(data => this.DiagramColor = data);
            this.diagramControlService.SetCallbackDiagramImagePath(data => this.DiagramImagePath = data);
            this.diagramControlService.SetCallbackDiagramWriter(data => this.DiagramWriter = data);
            this.diagramControlService.SetCallbackDiagramScript(data => this.DiagramScript = data);
            this.diagramControlService.SetCallbackDiagramCategory(data => this.SelectedDiagramCategory = data);


            this.DiagramDataType = this.diagramControlService.DiagramDataType;
            this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;
            this.DiagramCollection = this.diagramControlService.DiagramCollection;
            this.DiagramPropertyDataType = this.diagramControlService.DiagramPropertyDataType;
            this.DiagramCategory = this.diagramControlService.DiagramCategory;
           

            this.InputSnapSpotCollection = this.diagramControlService.InputSnapSpotCollection;
            this.OutputSnapSpotCollection = this.diagramControlService.OutputSnapSpotCollection;
            this.PreviewFunctionCollection = this.diagramControlService.PreviewFunctionCollection;
            this.PreviewInputSnapSpotCollection = this.diagramControlService.PreviewInputSnapSpotCollection;
            this.PreviewOutputSnapSpotCollection = this.diagramControlService.PreviewOutputSnapSpotCollection;
            this.PreviewFunctionPropertyCollection = this.diagramControlService.PreviewFunctionPropertyCollection;


            this.MessengerInstance.Register<AssociationModeMessage>(this, FileAssociationCallback);
        }


        private void FileAssociationCallback(AssociationModeMessage message)
        {
            if(message.AssociationMode == "Diagram")
            {

                try
                {
                    this.diagramControlService.ImportDiagram(message.FilePath);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

                
            }
        }

        public ICommand ImportDiagramCommand
        {
            get => new RelayCommand(() =>
             {
                 try
                 {
                     this.diagramControlService.ImportDiagram();
                 }catch(Exception e)
                 {
                     System.Diagnostics.Debug.WriteLine(e.Message);
                 }
             });
        }


        public ICommand ModifyRenderDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedDiagramConfig == null) return;
                
                

                try
                {
                    this.diagramControlService.LoadFunctionInfoFromConfig(this.SelectedDiagramConfig);
                    this.diagramControlService.RenderFunction(this.CanvasWidth,
                                                              this.DiagramName,
                                                              this.DiagramColor);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }

            });
        }

        public ICommand ModifyPackageDiagramCommand
        {
            get => new RelayCommand(() =>
             {

                 try
                 {

                     this.diagramControlService.RenderFunction(this.CanvasWidth,
                                                               this.DiagramName,
                                                               this.DiagramColor);

                 }
                 catch (Exception e)
                 {
                     System.Diagnostics.Debug.WriteLine(e.Message);
                     return;
                 }

                 

                 try
                 {
                     if (this.diagramControlService.CheckDiagramExists(this.DiagramName) == false) return;
                     this.diagramControlService.CreateDiagramPackage(this.DiagramName,
                                                                     this.DiagramWriter,
                                                                     this.DiagramVersion + 1,
                                                                     this.DiagramComment,
                                                                     this.DiagramScript,
                                                                     this.SelectedDiagramCategory,
                                                                     this.DiagramImagePath,
                                                                     100,
                                                                     100,
                                                                     true);
                     this.diagramControlService.UpdateDiagramInfo();
                     this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;
                     RaisePropertyChanged("DiagramConfigCollection");

                 }
                 catch (Exception e)
                 {
                     System.Diagnostics.Debug.WriteLine(e.Message);
                 }


             });

        }

        public ICommand DeleteDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.diagramControlService.DeleteDiagram(this.SelectedDiagramConfig);
                    this.diagramControlService.UpdateDiagramInfo();
                    this.diagramControlService.ClearPreviewDiagram();
                }
                catch(Exception e)
                {
                    DialogHelper.ShowToastErrorMessage("다이어그램 삭제 실패", "다이어그램 삭제에 실패 했습니다.");
                    Logger.Logger.Write(Logger.TYPE.UI, e.Message);
                }
                
            });
        }

        public ICommand ReloadDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.UpdateDiagramInfo();
            });
        }

        public ICommand ClearDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.ClearPreviewDiagram();
            });
        }


        public ICommand DiagramRenderingCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.diagramControlService.RenderFunction(this.CanvasWidth,
                                                              this.DiagramName,
                                                              this.DiagramColor);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                }

            });
        }

        public ICommand PackageDiagramCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {

                    this.diagramControlService.RenderFunction(this.CanvasWidth,
                                                              this.DiagramName,
                                                              this.DiagramColor);

                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return;
                }


                try
                {
                    this.diagramControlService.CreateDiagramPackage(this.DiagramName,
                                                                    this.DiagramWriter,
                                                                    this.DiagramVersion,
                                                                    this.DiagramComment,
                                                                    this.DiagramScript,
                                                                    this.SelectedDiagramCategory,
                                                                    this.DiagramImagePath,
                                                                    100,
                                                                    100);
                    this.diagramControlService.UpdateDiagramInfo();
                    this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;
                   
                    RaisePropertyChanged("DiagramConfigCollection");

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }

            });
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



        private ObservableCollection<BaseDiagramProperty> _PreviewFunctionPropertyCollection = null;
        public ObservableCollection<BaseDiagramProperty> PreviewFunctionPropertyCollection
        {
            get => _PreviewFunctionPropertyCollection;
            set => Set(ref _PreviewFunctionPropertyCollection, value);
        }

        private ObservableCollection<InputSnapSpot> _PreviewInputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> PreviewInputSnapSpotCollection
        {
            get => _PreviewInputSnapSpotCollection;
            set => Set(ref _PreviewInputSnapSpotCollection, value);
        }



        private ObservableCollection<Function> _PreviewFunctionCollection = null;
        public ObservableCollection<Function> PreviewFunctionCollection
        {
            get => _PreviewFunctionCollection;
            set => Set(ref _PreviewFunctionCollection, value);
        }

        private ObservableCollection<OutputSnapSpot> _PreviewOutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> PreviewOutputSnapSpotCollection
        {
            get => _PreviewOutputSnapSpotCollection;
            set => Set(ref _PreviewOutputSnapSpotCollection, value);
        }

        private ObservableCollection<DiagramConfig> _DiagramConfigCollection = null;
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get => _DiagramConfigCollection;
            set => Set(ref _DiagramConfigCollection, value);
        }


        private ObservableCollection<string> _DiagramCategory = null;
        public ObservableCollection<string> DiagramCategory
        {
            get => _DiagramCategory;
            set => Set(ref _DiagramCategory, value);
        }

        private string _SelectedDiagramCategory = null;
        public string SelectedDiagramCategory
        {
            get => _SelectedDiagramCategory;
            set => Set(ref _SelectedDiagramCategory, value);
        }


        private DiagramConfig _SelectedDiagramConfig = null;
        public DiagramConfig SelectedDiagramConfig
        {
            get => _SelectedDiagramConfig;
            set => Set(ref _SelectedDiagramConfig, value);
        }

        private Diagram _SelectedDiagram = null;
        public Diagram SelectedDiagram
        {
            get => _SelectedDiagram;
            set => Set(ref _SelectedDiagram, value);
        }

        private ObservableCollection<Diagram> _DiagramCollection = null;
        public ObservableCollection<Diagram> DiagramCollection
        {
            get => _DiagramCollection;
            set => Set(ref _DiagramCollection, value);
        }


        private string _DiagramName = "";
        public string DiagramName
        {
            get => _DiagramName;
            set => Set(ref _DiagramName, value);
        }

        private string _DiagramEditDate = "";
        public string DiagramEditDate
        {
            get => _DiagramEditDate;
            set => Set(ref _DiagramEditDate, value);

        }

        private string _DiagramWriter = "";
        public string DiagramWriter
        {
            get => _DiagramWriter;
            set => Set(ref _DiagramWriter, value);
        }

        private int _DiagramVersion = 1;
        public int DiagramVersion
        {
            get => _DiagramVersion;
            set => Set(ref _DiagramVersion, value);
        }

        private string _DiagramScript = "";
        public string DiagramScript
        {
            get => _DiagramScript;
            set => Set(ref _DiagramScript, value);
        }


        private string _DiagramComment = "";
        public string DiagramComment
        {
            get => _DiagramComment;
            set => Set(ref _DiagramComment, value);
        }

        
        
        private double _CanvasWidth = 0;
        public double CanvasWidth
        {
            get => _CanvasWidth;
            set => Set(ref _CanvasWidth, value);
        }


        private ObservableCollection<string> _DiagramDataType = null;
        public ObservableCollection<string> DiagramDataType
        {
            get => _DiagramDataType;
            set => Set(ref _DiagramDataType, value);
        }

        private string _SelectedInputDataType = "";
        public string SelectedInputDataTye
        {
            get => _SelectedInputDataType;
            set => Set(ref _SelectedInputDataType, value);
        }

        private string _SelectedOutputDataType = "";
        public string SelectedOutputDataType
        {
            get => _SelectedOutputDataType;
            set => Set(ref _SelectedOutputDataType, value);
        }

        private ObservableCollection<BaseDiagramProperty> _DiagramPropertyDataType = null;
        public ObservableCollection<BaseDiagramProperty> DiagramPropertyDataType
        {
            get => _DiagramPropertyDataType;
            set => Set(ref _DiagramPropertyDataType, value);
        }

        private BaseDiagramProperty _SelectedDiagramPropertyDataType = null;
        public BaseDiagramProperty SelectedDiagramPropertyDataType
        {
            get => _SelectedDiagramPropertyDataType;
            set => Set(ref _SelectedDiagramPropertyDataType, value);
        }


        public ICommand AddInputDataCommand
        {
            get => new RelayCommand(() =>
            {

                this.diagramControlService.AddInputSnapSpot(this.SelectedDiagramPropertyDataType);
            });
        }


        public ICommand AddOutputDataCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.AddOutputSnapSpot();
            });
        }


        public ICommand AddImageCommand
        {
            get => new RelayCommand(() =>
            {

                this.diagramControlService.AddImageFromPath();
                
            });
        }

        private string _DiagramImagePath = "";
        public string DiagramImagePath
        {
            get => _DiagramImagePath;
            set => Set(ref _DiagramImagePath, value);
        }

        private ImageSource _DiagramImage = null;
        public ImageSource DiagramImage
        {
            get => _DiagramImage;
            set => Set(ref _DiagramImage, value);
        }



        private InputSnapSpot _SelectedInputSnapSpot = null;
        public InputSnapSpot SelectedInputSnapSpot
        {
            set => Set(ref _SelectedInputSnapSpot, value);
            get => _SelectedInputSnapSpot;
        }

        public OutputSnapSpot _SelectedOutputSnapSpot = null;
        public OutputSnapSpot SelectedOutputSnapSpot
        {
            set => Set(ref _SelectedOutputSnapSpot, value);
            get => _SelectedOutputSnapSpot;
        }

        public BaseDiagramProperty _SelectedFunctionProperty = null;
        public BaseDiagramProperty SelectedFunctionProperty
        {
            set => Set(ref _SelectedFunctionProperty, value);
            get => _SelectedFunctionProperty;
        }



        public ICommand DeleteOutputCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.RemoveOutputSnapSpot(SelectedOutputSnapSpot);
            });

        }

        public ICommand DeleteInputCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.RemoveInputSnapSpot(SelectedInputSnapSpot);
            });

        }

        public ICommand DeleteFunctionPropertyCommand
        {
            get => new RelayCommand(() =>
            {
                this.diagramControlService.RemovePreviewFunctionProperty(SelectedFunctionProperty);
            });

        }


        private Color _DiagramColor = Color.FromRgb(255,255,255);
        public Color DiagramColor
        {
            get => _DiagramColor;
            set => Set(ref _DiagramColor, value);
        }


    }
}
