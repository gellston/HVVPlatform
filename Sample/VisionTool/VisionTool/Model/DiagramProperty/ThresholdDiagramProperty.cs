using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Model.DiagramProperty
{
    public class ThresholdDiagramProperty : BaseDiagramProperty
    {

        public ThresholdDiagramProperty() { 

        }


        public double _Value = 0;
        public double Value
        {
            get => _Value;
            set => Set(ref _Value, value);
        }


        public double _MinValue = 0;
        public double MinValue
        {
            get => _MinValue;
            set => Set(ref _MinValue, value);
        }

        public double _MaxValue = 0;
        public double MaxValue
        {
            get => _MaxValue;
            set => Set(ref _MaxValue, value);
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            return string.Format("{0}", Value);
        }


        public override object Clone()
        {
            ThresholdDiagramProperty newCopy = new ThresholdDiagramProperty();
            //newCopy.Name = this.Name;
            newCopy.Hash = this.Hash;
            //newCopy.ParentFunctionHash = this.ParentFunctionHash;
            newCopy.Value = this.Value;
            newCopy.MaxValue = this.MaxValue;
            newCopy.MinValue = this.MinValue;

            return newCopy;
        }
    }
}
