using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        private DiagramControlService diagramControlService;
        private SequenceControlService sequenceControlService;
       

        public DiagramEditViewModel(DiagramControlService _diagramControlService,
                                    SequenceControlService _sequenceControlService)
        {

            this.diagramControlService = _diagramControlService;
            this.sequenceControlService = _sequenceControlService;
            this.DiagramConfigCollection = this.diagramControlService.DiagramConfigCollection;
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
                try
                {
                    this.sequenceControlService.ScriptGeneration(this.FunctionCollection.ToList(), this.ConnectorCollection.ToList());
                    this.FullScript = this.sequenceControlService.FullScriptContent;
                    var list = this.sequenceControlService.SequencePages;
                }catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
                

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


        private string _FullScript = "";
        public string FullScript
        {
            get => _FullScript;
            set => Set(ref _FullScript, value);
        }

    }
}
