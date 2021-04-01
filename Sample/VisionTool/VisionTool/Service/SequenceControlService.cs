using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Linq;
using System.Collections.ObjectModel;

namespace VisionTool.Service
{
    public class SequenceControlService
    {

        public SequenceControlService()
        {

        }


        public string _FullScriptContent = "";
        public string FullScriptContent
        {
            get => _FullScriptContent;
            set => _FullScriptContent = value;
        }


        private List<SequencePage> _SequencePages = null;
        public List<SequencePage> SequencePages
        {
            get
            {
                _SequencePages ??= new List<SequencePage>();
                return _SequencePages;
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

        public void SetErrorFlag(int _errorLine)
        {
            int index = 0;
            this.SequencePages.ForEach(data =>
            {
                if (data.StartRow <= _errorLine && data.EndRow >= _errorLine)
                {
                    this.FunctionCollection[index].IsError = true;
                    
                }
                index++;
            });
        }

        public void DeleteDiagram(DiagramObject _diagram)
        {
            if (_diagram == null) return;
            if (_diagram.IsNew == true) return;
            var function = _diagram as Function;
            var connector = _diagram as Connector;

            if (function != null)
                this.FunctionCollection.Remove(function);
            else if (connector != null)
                this.ConnectorCollection.Remove(connector);
        }

        public string StepScriptGeneration(Function _diagram)
        {

            if (_diagram == null)
                throw new Exception("there is no selected function");


            if (this.SequencePages.Count == 0)
                throw new Exception("there are no sequence pages");



            var targetIndex = this.FunctionCollection.IndexOf(_diagram);



            string stepScript = "";


            for(var index =0; index < targetIndex + 1; index++)
            {
                stepScript += this.SequencePages[index].Content;
            }


            return stepScript;
        }

        public void ScriptGeneration()
        {

            this.FullScriptContent = "";
            this.SequencePages.Clear();
            int startIndex = 0;
            
            var funtion_index = 0;

            foreach (var function in this.FunctionCollection)
                function.IsError = false;


            var funtions = this.FunctionCollection.Where(x => x.IsNew == false);
            foreach(var function in funtions)
            {
                
                var sequencePage = new SequencePage();
                funtion_index++;
                var content = "";
                content += "// \n";
                content += "// function name = " + function.Name + "\n";
                content += "// funtion index = " + funtion_index + "\n";
                //content += function.GenerateFullScript();



                var filled_code = function.ScriptContent;
                for (var index = 0; index < function.Input.Count; index++)
                {
                    var input = function.Input[index];
                    var check = "";
                    if (input.IsProperty == true)
                        check = "true";
                    else check = "false";

                    filled_code = filled_code.Replace("###input_check" + (index + 1), check);
                    if (input.IsProperty)
                    {
                        filled_code = filled_code.Replace("###input" + (index + 1), input.DiagramProperty.ToCode());
                    }
                    else
                    {
                        var connector = this.ConnectorCollection.Where(x => (x.End == input && x.IsNew == false)).FirstOrDefault();
                        if (connector == null)
                        {
                            function.IsError = true;

                            throw new Exception("Node is not connected\n" + "function name = " + function.Name + "\n" + "function index = " + (funtion_index) + "\n");
                        }
                        

                        filled_code = filled_code.Replace("###input" + (index + 1), connector.Start.Hash);
                    }

                }
                
                for (var index = 0; index < function.Output.Count; index++)
                {
                    var output = function.Output[index];
                    filled_code = filled_code.Replace("###output" + (index + 1), output.Hash);
                }
                content += filled_code;

                content += "\n//\n";

                this.FullScriptContent += content;

                sequencePage.Content = content;
                sequencePage.StartRow = startIndex;
                startIndex += content.Split("\n").Length - 1;
                sequencePage.EndRow = startIndex;
                startIndex += 1;
                this.SequencePages.Add(sequencePage);
                
            }

        }

    }
}
