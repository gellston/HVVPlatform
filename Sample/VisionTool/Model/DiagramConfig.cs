using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Model.DiagramProperty;

namespace Model
{
    public class DiagramConfig
    {
        public DiagramConfig()
        {

            this.FunctionInfo = null;
            this.InputSnapSpotCollection = new List<InputSnapSpot>();
            this.OutputSnapSpotCollection = new List<OutputSnapSpot>();
            

        }

        public string DiagramName { get; set; }
        public string DiagramModifyDate { get; set; }
        public string DiagramWriter { get; set; }
        public int DiagramVersion { get; set; }
        public string DiagramComment { get; set; }

        public string DiagramScript { get; set; }

        public string DiagramCategory { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ImageSource DiagramImage {
            get;set;
        }

        private string _DiagramImageName = "";
        public string DiagramImageName
        {
            get => _DiagramImageName;
            set => _DiagramImageName = value;
        }

     
        public Function FunctionInfo { get; set; }

        public List<InputSnapSpot> InputSnapSpotCollection { get; set; }

        public List<OutputSnapSpot> OutputSnapSpotCollection { get; set; }
        //public List<BaseDiagramProperty> FunctionProperties { get; set; }

    }
}
