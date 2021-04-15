using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using DevExpress.Xpf.CodeView;

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


        private List<SequencePage> _SequencePages = new List<SequencePage>();
        public List<SequencePage> SequencePages
        {
            get
            {
                return _SequencePages;
            }
        }


        private ObservableCollection<Function> _FunctionCollection = new ObservableCollection<Function>();
        public ObservableCollection<Function> FunctionCollection
        {
            get
            {
                return _FunctionCollection;
            }
        }

        private ObservableCollection<Connector> _ConnectorCollection = new ObservableCollection<Connector>();
        public ObservableCollection<Connector> ConnectorCollection
        {
            get
            {
                return _ConnectorCollection;
            }
        }

        private ObservableCollection<InputSnapSpot> _InputSnapSpotCollection = new ObservableCollection<InputSnapSpot>();
        public ObservableCollection<InputSnapSpot> InputSnapSpotCollection
        {
            get
            {
                return _InputSnapSpotCollection;
            }
        }

        private ObservableCollection<OutputSnapSpot> _OutputSnapSpotCollection = new ObservableCollection<OutputSnapSpot>();
        public ObservableCollection<OutputSnapSpot> OutputSnapSpotCollection
        {
            get
            {
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
            {
                this.ConnectorCollection.Where(data =>
                {
                    if (data.Start == null) return false;
                    if (data.End == null) return false;

                    if (data.Start.Parent == function || data.End.Parent == function) return true;
                    else return false;

                }).ToList().ForEach(data =>
                {
                    if (data.IsNew == true) return;
                    this.ConnectorCollection.Remove(data);
                });


                this.InputSnapSpotCollection.Where(data => data.Parent == function).ToList().ForEach(data =>
                {
                    this.InputSnapSpotCollection.Remove(data);
                });
                this.OutputSnapSpotCollection.Where(data => data.Parent == function).ToList().ForEach(data =>
                {
                    this.OutputSnapSpotCollection.Remove(data);
                });

                this.FunctionCollection.Remove(function);
            }
            else if (connector != null)
            {
                this.ConnectorCollection.Remove(connector);
            }
                
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


        public void ClearSequence() {

            this.FunctionCollection.Clear();
            this.InputSnapSpotCollection.Clear();
            this.OutputSnapSpotCollection.Clear();
            this.ConnectorCollection.Clear();
        }

        public void LoadSequenceFromPath(string _path)
        {
            try
            {
                string filePath = _path;




                var jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
                var sequence = JsonConvert.DeserializeObject<DiagramSequence>(jsonContent);

                this.InputSnapSpotCollection.Clear();
                this.OutputSnapSpotCollection.Clear();
                this.FunctionCollection.Clear();
                this.ConnectorCollection.Clear();





                var inputSnapSpotCollection = sequence.InputSnapSpotCollection;
                var outputSnapSpotcollection = sequence.OutputSnapSpotCollection;
                var functionCollection = sequence.FunctionCollection;
                var connectorCollection = sequence.ConnectorCollection;



                functionCollection.ToList().ForEach(function =>
                {
                    var inputSnapSpot = inputSnapSpotCollection.Where(input => input.ParentFunctionHash == function.Hash).ToList();
                    var outputSnapSpot = outputSnapSpotcollection.Where(output => output.ParentFunctionHash == function.Hash).ToList();

                    inputSnapSpot.ForEach(input => input.Parent = function);
                    outputSnapSpot.ForEach(output => output.Parent = function);

                    function.Input.AddRange(inputSnapSpot);
                    function.Output.AddRange(outputSnapSpot);

                });



                outputSnapSpotcollection.ToList().ForEach(output =>
                {
                    var connector = connectorCollection.Where(connector => connector.StartSnapHash == output.Hash).ToList();
                    connector.ForEach(connector => connector.Start = output);
                });




                inputSnapSpotCollection.ToList().ForEach(input =>
                {
                    var connector = connectorCollection.Where(connector => connector.EndSnapHash == input.Hash).ToList();
                    connector.ForEach(connector => connector.End = input);
                });



                functionCollection.ToList().ForEach(function =>
                {

                    function.UpdateValueChangeAction();
                    function.Location.ValueChanged();
                });


                connectorCollection.ToList().ForEach(connector =>
                {
                    connector.MidPoint.Y = (connector.Start.Location.Y + connector.End.Location.Y) / 2;

                });


                this.InputSnapSpotCollection.AddRange(inputSnapSpotCollection);
                this.OutputSnapSpotCollection.AddRange(outputSnapSpotcollection);
                this.FunctionCollection.AddRange(functionCollection);
                this.ConnectorCollection.AddRange(connectorCollection);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FunctionVersionCheck(ObservableCollection<DiagramConfig> config) 
        {

            foreach(var function in this.FunctionCollection)
            {
                var selectedFunction = config.ToList().Where(data => data.DiagramName == function.Name).FirstOrDefault();

                if (selectedFunction == null) continue;


                var inputSnapSpot = function.Input.ToList();
                var outputSnapSpot = function.Output.ToList();
                foreach(var functionInput in inputSnapSpot)
                {
                    if (function.IsNodeError == true) break;
                    var count = selectedFunction.InputSnapSpotCollection.Where(data => data.Name == functionInput.Name).Count();
                    if (count == 0) function.IsNodeError = true;
                }

                foreach (var functionOutput in outputSnapSpot)
                {
                    if (function.IsNodeError == true) break;
                    var count = selectedFunction.OutputSnapSpotCollection.Where(data => data.Name == functionOutput.Name).Count();
                    if (count == 0) function.IsNodeError = true;
                }


                if (selectedFunction.InputSnapSpotCollection.Count != function.Input.Count)
                    function.IsNodeError = true;

                if (selectedFunction.OutputSnapSpotCollection.Count != function.Output.Count)
                    function.IsNodeError = true;

                
                if(function.IsNodeError == true) 
                    continue;


                if (selectedFunction.DiagramScript != function.ScriptContent)
                {
                    function.IsCodeError = true;
                    continue;
                }

            }

        }

        public void LoadSequenceFromPath()
        {
            try
            {
                string filePath = DialogHelper.OpenFile("Sequence File (.vsseq)|*.vsseq");


   

                var jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
                var sequence = JsonConvert.DeserializeObject<DiagramSequence>(jsonContent);

                this.InputSnapSpotCollection.Clear();
                this.OutputSnapSpotCollection.Clear();
                this.FunctionCollection.Clear();
                this.ConnectorCollection.Clear();


           


                var inputSnapSpotCollection = sequence.InputSnapSpotCollection;
                var outputSnapSpotcollection = sequence.OutputSnapSpotCollection;
                var functionCollection = sequence.FunctionCollection;
                var connectorCollection = sequence.ConnectorCollection;



                functionCollection.ToList().ForEach(function =>
                {
                    var inputSnapSpot = inputSnapSpotCollection.Where(input => input.ParentFunctionHash == function.Hash).ToList();
                    var outputSnapSpot = outputSnapSpotcollection.Where(output => output.ParentFunctionHash == function.Hash).ToList();

                    inputSnapSpot.ForEach(input => input.Parent = function);
                    outputSnapSpot.ForEach(output => output.Parent = function);

                    function.Input.AddRange(inputSnapSpot);
                    function.Output.AddRange(outputSnapSpot);

                });



                outputSnapSpotcollection.ToList().ForEach(output =>
                {
                    var connector = connectorCollection.Where(connector => connector.StartSnapHash == output.Hash).ToList();
                    connector.ForEach(connector => connector.Start = output);
                });




                inputSnapSpotCollection.ToList().ForEach(input =>
                {
                    var connector = connectorCollection.Where(connector => connector.EndSnapHash == input.Hash).ToList();
                    connector.ForEach(connector => connector.End = input);
                });



                functionCollection.ToList().ForEach(function =>
                {

                    function.UpdateValueChangeAction();
                    function.Location.ValueChanged();
                });


                connectorCollection.ToList().ForEach(connector =>
                {
                    connector.MidPoint.Y = (connector.Start.Location.Y + connector.End.Location.Y)/2;

                });


                this.InputSnapSpotCollection.AddRange(inputSnapSpotCollection);
                this.OutputSnapSpotCollection.AddRange(outputSnapSpotcollection);
                this.FunctionCollection.AddRange(functionCollection);
                this.ConnectorCollection.AddRange(connectorCollection);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SaveSequence()
        {
            try
            {
                string filePath = DialogHelper.SaveFile("Sequence File (.vsseq)|*.vsseq");


                var sequence = new DiagramSequence();

                var functions = this.FunctionCollection.Where(data => data.IsNew == false).ToList();
                var connectors = this.ConnectorCollection.Where(data => data.IsNew == false).ToList();
                var inputSpot = this.InputSnapSpotCollection.Where(data => data.IsNew == false).ToList();
                var outputSpot = this.OutputSnapSpotCollection.Where(data => data.IsNew == false).ToList();


                sequence.ConnectorCollection = connectors;
                sequence.InputSnapSpotCollection = inputSpot;
                sequence.OutputSnapSpotCollection = outputSpot;
                sequence.FunctionCollection = functions;


                var targetConfigPath = filePath;
                string jsonString = JsonConvert.SerializeObject(sequence, Formatting.Indented);
                File.WriteAllText(targetConfigPath, jsonString,Encoding.UTF8);


            }
            catch (Exception e)
            {
                throw e;
            }
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
