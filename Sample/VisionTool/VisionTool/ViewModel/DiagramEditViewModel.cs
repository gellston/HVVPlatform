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
using VisionTool.Model;

namespace VisionTool.ViewModel
{
    public class DiagramEditViewModel : ViewModelBase
    {

        public DiagramEditViewModel()
        {

            this.CanvasWidth = 2040;
            this.CanvasHeight = 2040;



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

        private ObservableCollection<InputSnapSpot> _OutputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> OutputSnapSpotCollection
        {
            get
            {
                _OutputSnapSpotCollection ??= new ObservableCollection<InputSnapSpot>();
                return _OutputSnapSpotCollection;
            }
        }



        public ICommand TestCommand
        {
            get => new RelayCommand(() =>
            {


                this.FunctionCollection.Add(new Function()
                {
                    Name = "test",
                    
                });

                var Object = new DiagramConfig()
                {
                    DiagramName = "test",
                    DiagramModifyDate = "2020-09-120",
                    DiagramComment = "test!!!Asdfasdfasdf",
                    DiagramVersion = 1,
                   
                };

                Object.FunctionCollection.Add(new Function()
                {
                    Name = "test"
                });

                Object.InputSnapSpotCollection.Add(new InputSnapSpot()
                {
                    Color = Color.FromRgb(255, 255, 255),
                    Name = "test",
                    IsConnected = false,
                    IsNew = false,
                   
                });


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
