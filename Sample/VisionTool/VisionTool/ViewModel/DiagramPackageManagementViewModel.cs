using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Model;
using VisionTool.Service;
using OpenCvSharp;
using Model.DiagramProperty;

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



            this.diagramControlService.SetCheckDiagramEditDate(data => this.DiagramEditDate = data);




            this.DiagramDataType = this.diagramControlService.DiagramDataType;
            this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;
            this.DiagramCollection = this.diagramControlService.DiagramCollection;
            this.DiagramPropertyDataType = this.diagramControlService.DiagramPropertyDataType;
            this.SelectedDiagramPropertyDataType = this.DiagramPropertyDataType[0];

            this.PreviewFunctionCollection = this.diagramControlService.PreviewFunctionCollection;
            this.PreviewInputSnapSpotCollection = this.diagramControlService.PreviewInputSnapSpotCollection;
            this.PreviewOutputSnapSpotCollection = this.diagramControlService.PreviewOutputSnapSpotCollection;
            this.PreviewFunctionPropertyCollection = this.diagramControlService.PreviewFunctionPropertyCollection;
        }

        public ICommand ImportDiagramCommand
        {
            get => new RelayCommand(() =>
             {

             });
        }

        public ICommand ModifyDiagramCommand
        {
            get => new RelayCommand(() =>
             {

             });

        }

        public ICommand DeleteDiagramCommand
        {
            get => new RelayCommand(() =>
            {

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
                this.DiagramName = "";
                this.DiagramComment = "";
                this.DiagramColor = Color.FromRgb(255, 255, 255);
                this.DiagramEditDate = "";
                this.DiagramVersion = 1;
                
                this.InputSnapSpotCollection.Clear();
                this.OutputSnapSpotCollection.Clear();

            });
        }


        public ICommand DiagramRenderingCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    this.diagramControlService.RenderFunction(this.InputSnapSpotCollection,
                                                              this.OutputSnapSpotCollection,
                                                              this.CanvasWidth,
                                                              this.CanvasHeight,
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

                    this.diagramControlService.RenderFunction(this.InputSnapSpotCollection,
                                                              this.OutputSnapSpotCollection,
                                                              this.CanvasWidth,
                                                              this.CanvasHeight,
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
                    this.PreviewFunctionCollection[0].ScriptContent = this.DiagramScript;
                    this.diagramControlService.CreateDiagramPackage(this.DiagramName,
                                                                    this.DiagramWriter,
                                                                    this.DiagramVersion,
                                                                    this.DiagramComment,
                                                                    this.DiagramScript,
                                                                    this.PreviewFunctionCollection[0],
                                                                    this.PreviewInputSnapSpotCollection,
                                                                    this.PreviewOutputSnapSpotCollection,
                                                                    this.PreviewFunctionPropertyCollection,
                                                                    this.DiagramImagePath,
                                                                    100,
                                                                    100);
                    this.diagramControlService.UpdateDiagramInfo();
                    //this.diagramControlService.UpdateDiagramInfo();
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
            get
            {
                _InputSnapSpotCollection ??= new ObservableCollection<InputSnapSpot>();
                return _InputSnapSpotCollection;
            }
        }

        private ObservableCollection<OutputSnapSpot> _OutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> OutputSnapSpotCollection
        {
            get
            {
                _OutputSnapSpotCollection ??= new ObservableCollection<OutputSnapSpot>();
                return _OutputSnapSpotCollection;
            }
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


        private double _CanvasHeight = 0;
        public double CanvasHeight
        {
            get => _CanvasHeight;
            set => Set(ref _CanvasHeight, value);
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


        //private string _SelectedDiagramPropertyDataType = "";
        //public string SelectedDiagramPropertyDataType
        //{
        //    get => _SelectedDiagramPropertyDataType;
        //    set => Set(ref _SelectedDiagramPropertyDataType, value);
        //}

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
                var cloneProperty = (BaseDiagramProperty)this.SelectedDiagramPropertyDataType.Clone();
                this.InputSnapSpotCollection.Add(this.diagramControlService.CreateInputSnapSpot(cloneProperty));
            });
        }


        public ICommand AddOutputDataCommand
        {
            get => new RelayCommand(() =>
            {
                this.OutputSnapSpotCollection.Add(this.diagramControlService.CreateOutputSnapSpot());
            });
        }

        public ICommand AddFunctionPropertyCommand
        {
            get => new RelayCommand(() =>
            {
                try
                {
                    var functionProperty = this.diagramControlService.CreateFunctionProperty("VisionTool.Model.FunctionProperty." + this.SelectedDiagramPropertyDataType);
                    this.PreviewFunctionPropertyCollection.Add(functionProperty);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }



        public ICommand AddImageCommand
        {
            get => new RelayCommand(() =>
            {

                try
                {
                    var path = DialogHelper.OpenFile("Image File (.jpg)|*.jpg");
                    var image = Helper.ImageHelper.LoadImageFromPath(path, 100, 100);
                    this.DiagramImage = image;
                    this.DiagramImagePath = path;
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                
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
                if (this.SelectedOutputSnapSpot != null)
                    this.OutputSnapSpotCollection.Remove(SelectedOutputSnapSpot);

            });

        }

        public ICommand DeleteInputCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.SelectedInputSnapSpot != null)
                    this.InputSnapSpotCollection.Remove(SelectedInputSnapSpot);
            });

        }

        public ICommand DeleteFunctionPropertyCommand
        {
            get => new RelayCommand(() =>
            {
                var result = this.SelectedFunctionProperty.ToString();
                if (this.SelectedFunctionProperty != null)
                    this.PreviewFunctionPropertyCollection.Remove(SelectedFunctionProperty);


                
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
