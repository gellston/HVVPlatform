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
using VisionTool.Model;
using VisionTool.Service;
using OpenCvSharp;

namespace VisionTool.ViewModel
{
    public class DiagramPackageManagementViewModel : ViewModelBase
    {

        private readonly DiagramPackageService diagramPackageService;
        private readonly SettingConfigService appConfigService;

        public DiagramPackageManagementViewModel(DiagramPackageService _diagramPackageService,
                                                 SettingConfigService _appConfigService)
        {

            //this.messageDialogService = _messageDialogService;
            this.diagramPackageService = _diagramPackageService;
            this.appConfigService = _appConfigService;
            //this.fileDialogService = _fileDialogService;


            this.DiagramDataType.Add("image");
            this.DiagramDataType.Add("number");
            this.DiagramDataType.Add("string");
            this.DiagramDataType.Add("boolean");


            MessengerInstance.Register<NotificationMessage>(this, NotifyMessageInitialDiagramCollection);
            MessengerInstance.Register<NotificationMessage>(this, NotifyMessageReloadDiagramCollection);
        }

        public void NotifyMessageInitialDiagramCollection(NotificationMessage message)
        {
            if (message.Notification == "InitialDiagramCollection")
            {


                this.DiagramCollection.Clear();
                this.DiagramConfigCollection.Clear();

                this.diagramPackageService.DeleteAllFiles(this.appConfigService.ApplicationSetting.DiagramConfigPath);
   


                var files = Directory.GetFiles(this.appConfigService.ApplicationSetting.DiagramPath, "*.diagram");
                foreach (var file in files)
                {
                    this.diagramPackageService.DeleteAllFiles(this.appConfigService.TempDiagramPackagePath);
                    if (this.diagramPackageService.UnzipModule(file,
                                                              appConfigService.TempDiagramPackagePath,
                                                              appConfigService.SecurityPassword) == false) continue;


                    var configFiles = Directory.GetFiles(appConfigService.TempDiagramPackagePath, "*.json");

                    foreach (var configFile in configFiles)
                    {
                        File.Copy(configFile, appConfigService.ApplicationSetting.DiagramConfigPath + Path.GetFileName(configFile), true);
                    }

                    var imageFiles = Directory.GetFiles(appConfigService.TempDiagramPackagePath, "*.jpg");

                    foreach (var imageFile in imageFiles)
                    {
                        File.Copy(imageFile, appConfigService.ApplicationSetting.DiagramImagePath + Path.GetFileName(imageFile), true);
                    }


                    this.DiagramCollection.Add(new Diagram()
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file)
                    });
                }

                
                DiagramConfigCollection = diagramPackageService.LoadAllDiagramConfig(this.appConfigService.ApplicationSetting.DiagramConfigPath,
                                                                                     this.appConfigService.ApplicationSetting.DiagramImagePath);

            }
        }


        public void NotifyMessageReloadDiagramCollection(NotificationMessage message)
        {
            if (message.Notification == "ReloadDiagramCollection")
            {

                DiagramConfigCollection = diagramPackageService.LoadAllDiagramConfig(this.appConfigService.ApplicationSetting.DiagramConfigPath,
                                                                                     this.appConfigService.ApplicationSetting.DiagramImagePath);

            }
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
                MessengerInstance.Send<NotificationMessage>(new NotificationMessage("InitialDiagramCollection"));
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
                this.FunctionCollection.Clear();
                this.InputSnapSpotCollection.Clear();
                this.OutputSnapSpotCollection.Clear();
                this.PreviewInputSnapSpotCollection.Clear();
                this.PreviewOutputSnapSpotCollection.Clear();
                
            });
        }

        private bool RenderCheck()
        {
            /*
            this.FunctionCollection.Clear();
            this.PreviewInputSnapSpotCollection.Clear();
            this.PreviewOutputSnapSpotCollection.Clear();





            bool check = this.InputSnapSpotCollection.ToList().Exists(x =>
            {
                return x.Name.Length == 0 || x.DataType.Length == 0;
            });

            if (check || this.InputSnapSpotCollection.Count == 0)
            {
                this.messageDialogService.ShowToastErrorMessage("입력 노드 에러", "입력 노드 이름이 없거나 설정되지 않았습니다.");
                return false;
            }

            check = this.OutputSnapSpotCollection.ToList().Exists(x =>
            {
                return x.Name.Length == 0 || x.DataType.Length == 0;
            });

            if (check || this.OutputSnapSpotCollection.Count == 0)
            {
                this.messageDialogService.ShowToastErrorMessage("출력 노드 에러", "출력 노드 이름이 없거나 설정되지 않았습니다.");
                return false;
            }

            check = this.DiagramConfigCollection.ToList().Exists(x =>
            {
                return x.DiagramName == this.DiagramName;
            });
            if(check == true)
            {
                this.messageDialogService.ShowToastErrorMessage("다이어그램 이름 충돌", "이미 존재하는 다이어그램입니다.");
                return false;
            }


            check = false;
            List<string> Keys = new List<string>();
            this.InputSnapSpotCollection.ToList().ForEach(x =>
            {
                Keys.Add(x.Name);
            });
            var NoDuplicates = Keys.Distinct().ToList();
            foreach (var Key in NoDuplicates)
            {
                if (Keys.Where(x => x.Contains(Key)).Count() > 1)
                    check = true;
            }
            Keys.Clear();


            this.OutputSnapSpotCollection.ToList().ForEach(x =>
            {
                Keys.Add(x.Name);
            });
            NoDuplicates = Keys.Distinct().ToList();
            foreach (var Key in Keys)
            {
                if (Keys.Where(x => x.Contains(Key)).Count() > 1)
                    check = true;
            }
            Keys.Clear();

            if (check)
            {
                this.messageDialogService.ShowToastErrorMessage("노드 중복 에러", "입출력 노드중 중복된 이름의 노드가 존재합니다. ");
                return false;
            }



            this.PreviewInputSnapSpotCollection.AddRange(this.InputSnapSpotCollection);
            this.PreviewOutputSnapSpotCollection.AddRange(this.OutputSnapSpotCollection);



            var function = new Function()
            {
                Name = this.DiagramName,
                IsNew = false,

            };

            function.Location.X = 100;
            function.Location.Y = 100;
            function.Size.Width = this.DiagramWidth;
            function.Size.Height = this.DiagramHeight;

            int count = 0;
            double lastOffsetY = 0;
            foreach (var inputSnapSpot in this.PreviewInputSnapSpotCollection)
            {
                count++;
                inputSnapSpot.Parent = function;
                inputSnapSpot.Offset.X = 15;
                lastOffsetY = inputSnapSpot.Offset.Y = 25 * count;
                function.Input.Add(inputSnapSpot);

            }
            lastOffsetY += 10;
            count = 0;
            foreach (var outputSnapSpot in this.PreviewOutputSnapSpotCollection)
            {
                count++;
                outputSnapSpot.Parent = function;
                outputSnapSpot.Offset.X = 15;
                outputSnapSpot.Offset.Y = lastOffsetY + 25 * count;
                function.Output.Add(outputSnapSpot);

            }

            function.Location.ValueChanged();
            function.Color = this.DiagramColor;


            this.FunctionCollection.Add(function);
            */
            return true;
        }

        public ICommand DiagramRenderingCommand
        {
            get => new RelayCommand(() =>
            {
                if (RenderCheck() == true)
                {
                    //this.messageDialogService.ShowToastSuccessMessage("다이어그램 랜더링 결과", "다이어그램 랜더링이 완료되었습니다.");
                }
                
            });
        }

        public ICommand PackageDiagramCommand
        {
            get => new RelayCommand(() =>
            {
                if (this.DiagramName.Length == 0)
                {
                    //this.messageDialogService.ShowToastErrorMessage("다이어그램 정보 에러", "다이어그램 이름이 입력되지 않았습니다.");
                    return;
                }

                if (this.DiagramScript.Length == 0)
                {
                    //this.messageDialogService.ShowToastErrorMessage("다이어그램 정보 에러", "다이어그램 스크립트가 입력되지 않았습니다. ");
                    return;
                }

                if(RenderCheck() == true)
                {
                    //this.messageDialogService.ShowToastSuccessMessage("다이어그램 랜더링 결과", "다이어그램 랜더링이 완료되었습니다.");
                }
                else
                {
                    //this.messageDialogService.ShowToastSuccessMessage("다이어그램 랜더링 결과", "다이어그램 랜더링이 실패했습니다.");
                    return;
                }

                this.DiagramEditDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");


                var function = this.FunctionCollection[0];
                var InputCollection = function.Input.ToList();
                var OutputCollection = function.Output.ToList();
                function.Input.Clear();
                function.Output.Clear();



                var check = this.diagramPackageService.CreateDiagramPackage(this.DiagramName,
                                                                            this.DiagramEditDate,
                                                                            this.DiagramWriter,
                                                                            this.DiagramVersion,
                                                                            this.DiagramComment,
                                                                            this.FunctionCollection[0],
                                                                            InputCollection,
                                                                            OutputCollection,
                                                                            this.appConfigService.ApplicationSetting.DiagramPath,
                                                                            this.DiagramImagePath,
                                                                            100,
                                                                            100,
                                                                            this.appConfigService.TempDiagramPackagePath,
                                                                            this.appConfigService.SecurityPassword);
                if (check == true)
                {
                    //messageDialogService.ShowToastSuccessMessage("다이어그램 패키지", "다이어그램 패키징 완료");
                    MessengerInstance.Send<NotificationMessage>(new NotificationMessage("UpdateDiagram"));

                }
                else
                {
                    //messageDialogService.ShowToastErrorMessage("다이어그램 패키지", "다이어그램 패키징 실패");
                }

            });
        }


        private ObservableCollection<Function> _FunctionCollection = null;
        public ObservableCollection<Function> FunctionCollection
        {
            get
            {
                _FunctionCollection ??= new ObservableCollection<Function>();
                return _FunctionCollection;
            }
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

        private ObservableCollection<InputSnapSpot> _PreviewInputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> PreviewInputSnapSpotCollection
        {
            get
            {
                _PreviewInputSnapSpotCollection ??= new ObservableCollection<InputSnapSpot>();
                return _PreviewInputSnapSpotCollection;
            }
        }

        private ObservableCollection<OutputSnapSpot> _PreviewOutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> PreviewOutputSnapSpotCollection
        {
            get
            {
                _PreviewOutputSnapSpotCollection ??= new ObservableCollection<OutputSnapSpot>();
                return _PreviewOutputSnapSpotCollection;
            }
        }

        private ObservableCollection<DiagramConfig> _DiagramConfigCollection = null;
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get
            {
                _DiagramConfigCollection ??= new ObservableCollection<DiagramConfig>();
                return _DiagramConfigCollection;
            }
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
            get
            {
                _DiagramCollection ??= new ObservableCollection<Diagram>();
                return _DiagramCollection;
            }
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

        private double _DiagramWidth = 250;
        public double DiagramWidth
        {
            get => _DiagramWidth;
            set => Set(ref _DiagramWidth, value);
        }

        private double _DiagramHeight = 250;
        public double DiagramHeight
        {
            get => _DiagramHeight;
            set => Set(ref _DiagramHeight, value);
        }



        private ObservableCollection<string> _DiagramDataType = null;
        public ObservableCollection<string> DiagramDataType
        {
            get
            {
                _DiagramDataType ??= new ObservableCollection<string>();
                return _DiagramDataType;
            }
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

        public ICommand AddInputDataCommand
        {
            get => new RelayCommand(() =>
            {
                

                this.InputSnapSpotCollection.Add(new InputSnapSpot()
                {
                    Name = "",
                    DataType = ""
                });

            });
        }


        public ICommand AddOutputDataCommand
        {
            get => new RelayCommand(() =>
            {
                
                this.OutputSnapSpotCollection.Add(new OutputSnapSpot()
                {
                    Name = "",
                    DataType = ""
                });

            });
        }

        public ICommand AddImageCommand
        {
            get => new RelayCommand(() =>
            {
                /*
                var path = this.fileDialogService.OpenFile("Script File (.jpg)|*.jpg");
                
                if(path.Length == 0)
                {
                    messageDialogService.ShowToastErrorMessage("파일 선택 에러","파일이 선택되지 않았습니다.");
                    return;
                }


                try
                {
                    Mat image = new Mat(path);
                    image = image.Resize(new Size(100, 100));
                    this.DiagramImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                    DiagramImagePath = path;
                }
                catch(Exception e)
                {

                }*/
                

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


        private Color _DiagramColor = Color.FromRgb(255,255,255);
        public Color DiagramColor
        {
            get => _DiagramColor;
            set => Set(ref _DiagramColor, value);
        }


    }
}
