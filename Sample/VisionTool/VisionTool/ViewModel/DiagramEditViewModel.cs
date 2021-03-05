using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VisionTool.Model;
using VisionTool.Service;

namespace VisionTool.ViewModel
{
    public class DiagramEditViewModel : ViewModelBase
    {

        private DiagramPackageService diagramPackageService;
        private SettingConfigService appConfigService;

        public DiagramEditViewModel(DiagramPackageService _diagramPackageService,
                                    SettingConfigService _appConfigService)
        {

            this.CanvasWidth = 2040;
            this.CanvasHeight = 2040;


            this.diagramPackageService = _diagramPackageService;
            this.appConfigService = _appConfigService;

            MessengerInstance.Register<NotificationMessage>(this, NotifyMessageReloadDiagramCollection);
        }

        public void NotifyMessageReloadDiagramCollection(NotificationMessage message)
        {
            if (message.Notification == "ReloadDiagramCollection")
            {

                DiagramConfigCollection = diagramPackageService.LoadAllDiagramConfig(this.appConfigService.ApplicationSetting.DiagramConfigPath,
                                                                                     this.appConfigService.ApplicationSetting.DiagramImagePath);

            }
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

        private ObservableCollection<Connector> _ConnectorCollection = null;
        public ObservableCollection<Connector> ConnectorCollection
        {
            get
            {
                _ConnectorCollection ??= new ObservableCollection<Connector>();
                return _ConnectorCollection;
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

        private ObservableCollection<DiagramConfig> _DiagramConfigCollection = null;
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get => _DiagramConfigCollection;
            set => Set(ref _DiagramConfigCollection, value);
        }

        



        public ICommand TestCommand
        {
            get => new RelayCommand(() =>
            {

                var test_function = new Function()
                {
                    Name = "TEST"
                };

                var inputSnapSpot = new InputSnapSpot()
                {
                    Name = "test",
                    Parent = test_function,
                    DataType = "Image"
                };

                inputSnapSpot.Offset.Y = 30;
                inputSnapSpot.Offset.X = 30;

                


                var outputSnapSpot = new OutputSnapSpot()
                {
                    Name = "test",
                    Parent = test_function,
                    DataType = "Image"
                };

                outputSnapSpot.Offset.Y = 50;
                outputSnapSpot.Offset.X = 30;


                test_function.Input.Add(inputSnapSpot);
                test_function.Output.Add(outputSnapSpot);



                this.FunctionCollection.Add(test_function);
                this.InputSnapSpotCollection.Add(inputSnapSpot);
                this.OutputSnapSpotCollection.Add(outputSnapSpot);


                var Object = new DiagramConfig()
                {
                    DiagramName = "test",
                    DiagramModifyDate = "2020-09-120",
                    DiagramComment = "test!!!Asdfasdfasdf",
                    DiagramVersion = 1,
                   
                };

                Object.FunctionInfo = test_function;
                Object.InputSnapSpotCollection.Add(inputSnapSpot);
                Object.OutputSnapSpotCollection.Add(outputSnapSpot);

                


                var jsonString = JsonSerializer.Serialize(Object);
                System.Console.WriteLine("test");
                File.WriteAllText("d://test.json", jsonString);

            });
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

    }
}
