using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{
    public class BoolDiagramProperty : BaseDiagramProperty
    {
        
        public BoolDiagramProperty()
        {

        }

        public bool _Value = false;
        public bool Value
        {
            get => _Value;
            set => Set(ref _Value, value);
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            if (this.Value == true)
                return "true";
            else return "false";
        }

        public override object Clone()
        {
            BoolDiagramProperty newCopy = new BoolDiagramProperty();
            newCopy.Hash = this.Hash;
            newCopy.Value = this.Value;
            return newCopy;
        }
    }
}
