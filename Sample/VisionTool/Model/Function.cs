using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Model.DiagramProperty;

namespace Model
{
    public class Function : DiagramObject
    {


        public Function()
        {
            
            ///

            this.Location.X = 0;
            this.Location.Y = 0;
            this.Size.Width = 200;
            this.Size.Height = 200;

            this.Location.GridStep = 10;

            this.Location.ValueChanged = RecalculateSnaps;
        }

        
        private void RecalculateSnaps()
        {
            this.Input.ToList().ForEach(x => x.Recalculate());
            this.Output.ToList().ForEach(x => x.Recalculate());
        }


        private bool _IsError = false;
        public bool IsError
        {
            get => _IsError;
            set => Set(ref _IsError, value);
        }

        private BindableSize _Size = null;
        public BindableSize Size
        {
            get {
                _Size ??= new BindableSize();
                
                return _Size;
            }
        }

        
        private ObservableCollection<InputSnapSpot> _Input = null;
        [JsonIgnore]
        [IgnoreDataMember]
        public ObservableCollection<InputSnapSpot> Input
        {
            get
            {
                _Input ??= new ObservableCollection<InputSnapSpot>();
                return _Input;
            }
        }


        
        private ObservableCollection<OutputSnapSpot> _Output = null;
        [JsonIgnore]
        [IgnoreDataMember]
        public ObservableCollection<OutputSnapSpot> Output
        {
            get
            {
                _Output ??= new ObservableCollection<OutputSnapSpot>();
                return _Output;
            }
        }

        private ObservableCollection<BaseDiagramProperty> _FunctionProperties = null;
        [JsonIgnore]
        [IgnoreDataMember]
        public ObservableCollection<BaseDiagramProperty> FunctionProperties
        {
            get
            {
                _FunctionProperties ??= new ObservableCollection<BaseDiagramProperty>();
                return _FunctionProperties;
            }
        }


        private string _ScriptContent = "";
        public string ScriptContent
        {
            get => _ScriptContent;
            set => Set(ref _ScriptContent, value);
        }
        /*
        public string GenerateFullScript()
        {
            string fullScript = this.ScriptContent;
            int index = 0;
            foreach(var input in this.Input)
            {
                index++;
                fullScript = fullScript.Replace("###input_name" + index, input.Hash);
                var check = "";
                if (input.IsProperty == true)
                    check = "true";
                else check = "false";

                fullScript = fullScript.Replace("###input_check" + index, check);
                fullScript = fullScript.Replace("###input_value" + index, input.DiagramProperty.ToCode());
            }


            index = 0;
            foreach (var output in this.Output)
            {
                index++;
                fullScript = fullScript.Replace("###output_name" + index, output.Hash);
            }
            return fullScript;
        }
        */

        public void Activate()
        {
            this.IsNew = false;
            this.Input.ToList().ForEach(x => x.IsNew = false);
            this.Output.ToList().ForEach(x => x.IsNew = false);
        }

    }
}
