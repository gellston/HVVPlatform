using System;
using System.Collections.Generic;
using System.Text;
using VisionTool.Model;
using System.Linq;

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
            get => _SequencePages;
        }


        public void ScriptGeneration(List<Function> _functions, List<Connector> _connectors)
        {

            this.FullScriptContent = "";
            this.SequencePages.Clear();
            int startIndex = 0;
            
            var funtion_index = 0;

            var funtions = _functions.Where(x => x.IsNew == false).ToList();
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
                        var connector = _connectors.Where(x => (x.End == input && x.IsNew == false)).FirstOrDefault();
                        if (connector == null) throw new Exception("Node is not connected\n" + "function name = " + function.Name + "\n" + "function index = " + funtion_index + "\n");

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
                startIndex += content.Split("\n").Length;
                sequencePage.EndRow = startIndex;
                startIndex += 1;
                this.SequencePages.Add(sequencePage);
                
            }

        }

    }
}
