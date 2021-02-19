using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VisionTool.Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class DiagramPackageManagementViewModel : ViewModelBase
    {

        private readonly MessageDialogService messageDialogService;
        private readonly DiagramPackageService diagramPackageService;

        public DiagramPackageManagementViewModel(MessageDialogService _messageDialogService,
                                                 DiagramPackageService _diagramPackageService)
        {

            this.messageDialogService = _messageDialogService;
            this.diagramPackageService = _diagramPackageService;


            this.DiagramDataType.Add("image");
            this.DiagramDataType.Add("number");
            this.DiagramDataType.Add("string");
            this.DiagramDataType.Add("boolean");
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

             });
        }

        public ICommand ClearDiagramCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public ICommand DiagramRenderingCommand
        {
            get => new RelayCommand(() =>
            {

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
                    return;
                }

                check = this.OutputSnapSpotCollection.ToList().Exists(x =>
                {
                    return x.Name.Length == 0 || x.DataType.Length == 0;
                });

                if (check ||  this.OutputSnapSpotCollection.Count == 0)
                {
                    this.messageDialogService.ShowToastErrorMessage("출력 노드 에러", "출력 노드 이름이 없거나 설정되지 않았습니다.");
                    return;
                }


                check = false;
                List<string> Keys = new List<string>();
                this.InputSnapSpotCollection.ToList().ForEach(x =>
                {
                    Keys.Add(x.Name);
                });
                var NoDuplicates = Keys.Distinct().ToList();
                foreach(var Key in Keys)
                {
                    if (NoDuplicates.Where(x => x == Key).Count() > 1)
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
                    if (NoDuplicates.Where(x => x == Key).Count() > 1)
                        check = true;
                }
                Keys.Clear();

                if (check)
                {
                    this.messageDialogService.ShowToastErrorMessage("노드 중복 에러", "입출력 노드중 중복된 이름의 노드가 존재합니다. ");
                    return;
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
                lastOffsetY += 50;
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

            });
        }

        public ICommand PackageDiagramCommand
        {
            get => new RelayCommand(() =>
            {

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

        private int _DiagramVersion = 0;
        public int DiagramVersion
        {
            get => _DiagramVersion;
            set => Set(ref _DiagramVersion, value);
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
