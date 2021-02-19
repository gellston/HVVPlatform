using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace VisionTool.Model
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

            this.Location.GridStep = 25;

            this.Location.ValueChanged = RecalculateSnaps;
        }

        
        private void RecalculateSnaps()
        {
            this.Input.ToList().ForEach(x => x.Recalculate());
            this.Output.ToList().ForEach(x => x.Recalculate());
        }


     
        private string _Hash;

        public string Hash
        {
            get => _Hash;
            set => Set(ref _Hash, value);
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
        public ObservableCollection<InputSnapSpot> Input
        {
            get
            {
                _Input ??= new ObservableCollection<InputSnapSpot>();
                return _Input;
            }
        }



        private ObservableCollection<OutputSnapSpot> _Output = null;
        public ObservableCollection<OutputSnapSpot> Output
        {
            get
            {
                _Output ??= new ObservableCollection<OutputSnapSpot>();
                return _Output;
            }
        }


        private string _ScriptContent = "";
        public string ScriptContent
        {
            get => _ScriptContent;
            set => Set(ref _ScriptContent, value);
        }
    }
}
