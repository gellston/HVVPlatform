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
    public class DiagramSequence
    {
        public DiagramSequence()
        {

            this.FunctionCollection =  new List<Function>();
            this.InputSnapSpotCollection = new List<InputSnapSpot>();
            this.OutputSnapSpotCollection = new List<OutputSnapSpot>();
            this.ConnectorCollection = new List<Connector>();
        }



        public List<Function> FunctionCollection { get; set; }

        public List<InputSnapSpot> InputSnapSpotCollection { get; set; }

        public List<OutputSnapSpot> OutputSnapSpotCollection { get; set; }

        public List<Connector> ConnectorCollection { get; set; }

    }
}
