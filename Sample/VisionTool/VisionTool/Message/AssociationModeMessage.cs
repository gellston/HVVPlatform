using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Message
{
    public class AssociationModeMessage
    {

        public AssociationModeMessage()
        {

        }

        public string AssociationMode { get; set; }

        public string FilePath { get; set; }

        public ViewModelBase CurrentViewModel { get; set; }
    }
}
