using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace VisionTool.Model
{
    public class DiagramConfig
    {
        public DiagramConfig()
        {

            this.FunctionCollection = new List<Function>();
            this.InputSnapSpotCollection = new List<InputSnapSpot>();
            this.OutputSnapSpotCollection = new List<OutputSnapSpot>();

        }

        public string DiagramName { get; set; }
        public string DiagramModifyDate { get; set; }
        public int DiagramVersion { get; set; }
        public string DiagramComment { get; set; }

     
        public List<Function> FunctionCollection { get; set; }

        public List<InputSnapSpot> InputSnapSpotCollection { get; set; }

        public List<OutputSnapSpot> OutputSnapSpotCollection { get; set; }
    }
}
